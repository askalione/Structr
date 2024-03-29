using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Structr.Abstractions
{
    // Taken from https://gist.github.com/syaifulnizamyahya/e9b7d935f14940b236445508aab0320f
    // http://stackoverflow.com/questions/852181/c-printing-all-properties-of-an-object
    // https://github.com/mcshaz/BlowTrial/blob/master/GenericToDataFile/ObjectDumper.cs
    internal class ObjectDumper
    {
        private readonly int _depth;
        private readonly Dictionary<object, int> _hashListOfFoundElements;
        private readonly char _indentChar;
        private readonly int _indentSize;
        private readonly StringBuilder _stringBuilder;
        private int _currentIndent;
        private int _currentLine;

        public ObjectDumper(int depth, int indentSize, char indentChar)
        {
            _depth = depth;
            _indentSize = indentSize;
            _indentChar = indentChar;
            _stringBuilder = new StringBuilder();
            _hashListOfFoundElements = new Dictionary<object, int>();
        }

        private bool AlreadyDumped(object value)
        {
            if (value == null)
                return false;
            int lineNo;
            if (_hashListOfFoundElements.TryGetValue(value, out lineNo))
            {
                Write("(reference already dumped - line:{0})", lineNo);
                return true;
            }
            _hashListOfFoundElements.Add(value, _currentLine);
            return false;
        }

        public string Dump(object element, bool isTopOfTree = false)
        {
            if (_currentIndent > _depth) { return null; }
            if (element == null || element is string)
            {
                Write(FormatValue(element));
            }
            else if (element is ValueType)
            {
                Type objectType = element.GetType();
                bool isWritten = false;
                if (objectType.GetTypeInfo().IsGenericType)
                {
                    Type baseType = objectType.GetGenericTypeDefinition();
                    if (baseType == typeof(KeyValuePair<,>))
                    {
                        isWritten = true;
                        Write("Key:");
                        _currentIndent++;
                        Dump(objectType.GetProperty("Key").GetValue(element, null));
                        _currentIndent--;
                        Write("Value:");
                        _currentIndent++;
                        Dump(objectType.GetProperty("Value").GetValue(element, null));
                        _currentIndent--;
                    }
                }
                if (!isWritten)
                {
                    Write(FormatValue(element));
                }
            }
            else
            {
                var enumerableElement = element as IEnumerable;
                if (enumerableElement != null)
                {
                    foreach (object item in enumerableElement)
                    {
                        if (item is IEnumerable && !(item is string))
                        {
                            _currentIndent++;
                            Dump(item);
                            _currentIndent--;
                        }
                        else
                        {
                            Dump(item);
                        }
                    }
                }
                else
                {
                    Type objectType = element.GetType();
                    Write("{{{0}(HashCode:{1})}}", objectType.FullName, element.GetHashCode());
                    if (!AlreadyDumped(element))
                    {
                        _currentIndent++;
                        MemberInfo[] members = objectType.GetMembers(BindingFlags.Public | BindingFlags.Instance);
                        foreach (var memberInfo in members)
                        {
                            var fieldInfo = memberInfo as FieldInfo;
                            var propertyInfo = memberInfo as PropertyInfo;

                            if (fieldInfo == null && (propertyInfo == null || !propertyInfo.CanRead || propertyInfo.GetIndexParameters().Length > 0))
                            {
                                continue;
                            }

                            var type = fieldInfo != null ? fieldInfo.FieldType : propertyInfo.PropertyType;
                            object value;
                            try
                            {
                                value = fieldInfo != null
                                                   ? fieldInfo.GetValue(element)
                                                   : propertyInfo.GetValue(element, null);
                            }
                            catch (Exception e)
                            {
                                Write("{0} failed with:{1}", memberInfo.Name, (e.GetBaseException() ?? e).Message);
                                continue;
                            }

                            if (type.GetTypeInfo().IsValueType || type == typeof(string))
                            {
                                Write("{0}: {1}", memberInfo.Name, FormatValue(value));
                            }
                            else
                            {
                                var isEnumerable = typeof(IEnumerable).IsAssignableFrom(type);
                                Write("{0}: {1}", memberInfo.Name, isEnumerable ? "..." : "{ }");

                                _currentIndent++;
                                Dump(value);
                                _currentIndent--;
                            }
                        }
                        _currentIndent--;
                    }
                }
            }

            return isTopOfTree ? _stringBuilder.ToString() : null;
        }
        private string FormatValue(object o)
        {
            if (o == null)
                return ("null");

            if (o is DateTime)
                return (((DateTime)o).ToString());

            if (o is string)
                return "\"" + (string)o + "\"";

            if (o is char)
            {
                if (o.Equals('\0'))
                {
                    return "''";
                }
                else
                {
                    return "'" + (char)o + "'";
                }
            }

            if (o is ValueType)
                return (o.ToString());

            if (o is IEnumerable)
                return ("...");

            return ("{ }");
        }

        private void Write(string value, params object[] args)
        {
            var space = new string(_indentChar, _currentIndent * _indentSize);

            if (args != null)
                value = string.Format(value, args);

            _stringBuilder.AppendLine(space + value);
            _currentLine++;
        }
    }
}
