using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Structr.Abstractions.Extensions
{
    public static class ExpressionExtensions
    {
        /// <summary>
        /// Gets property name by expression.
        /// </summary>
        /// <typeparam name="TObject">Type of object.</typeparam>
        /// <typeparam name="TProperty">Type of property.</typeparam>
        /// <param name="propertyExpression">Lambda expression for property.</param>
        /// <returns>
        /// Property name.
        /// </returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static string GetPropertyName<TObject, TProperty>(this Expression<Func<TObject, TProperty>> propertyExpression)
        {
            Ensure.NotNull(propertyExpression, nameof(propertyExpression));

            LambdaExpression lambda = (LambdaExpression)propertyExpression;
            MemberExpression memberExpression;

            if (lambda.Body is UnaryExpression)
            {
                UnaryExpression unaryExpression = (UnaryExpression)(lambda.Body);
                memberExpression = (MemberExpression)(unaryExpression.Operand);
            }
            else
            {
                memberExpression = (MemberExpression)(lambda.Body);
            }

            string memberName = "";
            MemberExpression tempMemberExpression = memberExpression;
            while (tempMemberExpression.Expression.NodeType == ExpressionType.MemberAccess)
            {
                var propInfo = tempMemberExpression.Expression.GetType().GetProperty("Member");
                var propValue = propInfo.GetValue(tempMemberExpression.Expression, null) as PropertyInfo;
                if (propValue != null)
                {
                    memberName = propValue.Name + "." + memberName;
                }
                tempMemberExpression = tempMemberExpression.Expression as MemberExpression;
            }

            if (memberName.IndexOf('.') > -1)
            {
                memberName += memberExpression.Member.Name;
            }
            if (string.IsNullOrWhiteSpace(memberName))
            {
                memberName = memberExpression.Member.Name;
            }

            PropertyInfo propertyInfo = typeof(TObject).GetPropertyInfo(memberName);
            if (propertyInfo == null)
            {
                throw new ArgumentException("Expression is not a property", propertyExpression.ToString());
            }

            return memberName;
        }

        /// <summary>
        /// Gets <see cref="MemberInfo"/> for property provided via lambda expression.
        /// </summary>
        /// <param name="expression">Lambda expression for property.</param>
        /// <returns>
        /// Instance of <see cref="MemberInfo"/> type describing provided property.
        /// </returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static MemberInfo GetMember(this LambdaExpression expression)
        {
            Ensure.NotNull(expression, nameof(expression));

            var memberExp = RemoveUnary(expression.Body) as MemberExpression;
            if (memberExp == null)
            {
                return null;
            }

            return memberExp.Member;
        }

        /// <summary>
        /// Gets <see cref="MemberInfo"/> for property provided via expression.
        /// </summary>
        /// <typeparam name="TObject">Type of object.</typeparam>
        /// <typeparam name="TProperty">Type of property.</typeparam>
        /// <param name="expression">Expression.</param>
        /// <returns>
        /// Instance of <see cref="MemberInfo"/> type describing provided property.
        /// </returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static MemberInfo GetMember<TObject, TProperty>(this Expression<Func<TObject, TProperty>> expression)
        {
            Ensure.NotNull(expression, nameof(expression));

            var memberExp = RemoveUnary(expression.Body) as MemberExpression;
            if (memberExp == null)
            {
                return null;
            }

            Expression currentExpr = memberExp.Expression;

            while (true)
            {
                currentExpr = RemoveUnary(currentExpr);

                if (currentExpr != null && currentExpr.NodeType == ExpressionType.MemberAccess)
                {
                    currentExpr = ((MemberExpression)currentExpr).Expression;
                }
                else
                {
                    break;
                }
            }

            if (currentExpr == null || currentExpr.NodeType != ExpressionType.Parameter)
            {
                return null;
            }

            return memberExp.Member;
        }

        private static Expression RemoveUnary(Expression toUnwrap)
        {
            if (toUnwrap is UnaryExpression)
            {
                return ((UnaryExpression)toUnwrap).Operand;
            }

            return toUnwrap;
        }

        /// <summary>
        /// Make non generic function from generic.
        /// </summary>
        /// <typeparam name="TObject">Type of object.</typeparam>
        /// <typeparam name="TProperty">Type of property.</typeparam>
        /// <param name="func"></param>
        /// <returns>
        /// Non generic function.
        /// </returns>
        public static Func<object, object> MakeNonGeneric<TObject, TProperty>(this Func<TObject, TProperty> func)
        {
            Ensure.NotNull(func, nameof(func));

            return x => func((TObject)x);
        }

        /// <summary>
        /// Make non generic function from generic.
        /// </summary>
        /// <typeparam name="TObject">Type of object.</typeparam>
        /// <param name="func">Function.</param>
        /// <returns>
        /// Non generic function.
        /// </returns>
        public static Func<object, bool> MakeNonGeneric<TObject>(this Func<TObject, bool> func)
        {
            Ensure.NotNull(func, nameof(func));

            return x => func((TObject)x);
        }

        /// <summary>
        /// Make non generic function from generic.
        /// </summary>
        /// <typeparam name="T1">Type of object1.</typeparam>
        /// <typeparam name="T2">Type of object2.</typeparam>
        /// <param name="func">Function.</param>
        /// <returns>
        /// Non generic function.
        /// </returns>
        public static Func<object, object, bool> MakeNonGeneric<T1, T2>(this Func<T1, T2, bool> func)
        {
            Ensure.NotNull(func, nameof(func));

            return (x, y) => func((T1)x, (T2)y);
        }

        /// <summary>
        /// Make non generic function from generic.
        /// </summary>
        /// <typeparam name="T1">Type of object1.</typeparam>
        /// <typeparam name="T2">Type of object2.</typeparam>
        /// <typeparam name="T3">Type of object3.</typeparam>
        /// <param name="func">Function.</param>
        /// <returns>
        /// Non generic function.
        /// </returns>
        public static Func<object, object, object, bool> MakeNonGeneric<T1, T2, T3>(this Func<T1, T2, T3, bool> func)
        {
            Ensure.NotNull(func, nameof(func));

            return (x, y, z) => func((T1)x, (T2)y, (T3)z);
        }

        /// <summary>
        /// Make non generic function from generic.
        /// </summary>
        /// <typeparam name="T">Type of object.</typeparam>
        /// <param name="func">Function.</param>
        /// <returns>
        /// Non generic function.
        /// </returns>
        public static Func<object, Task<bool>> MakeNonGeneric<T>(this Func<T, Task<bool>> func)
        {
            Ensure.NotNull(func, nameof(func));

            return x => func((T)x);
        }

        /// <summary>
        /// Make non generic function from generic.
        /// </summary>
        /// <typeparam name="T">Type of object.</typeparam>
        /// <param name="func">Function.</param>
        /// <returns>
        /// Non generic function.
        /// </returns>
        public static Func<object, int> MakeNonGeneric<T>(this Func<T, int> func)
        {
            Ensure.NotNull(func, nameof(func));

            return x => func((T)x);
        }

        /// <summary>
        /// Make non generic function from generic.
        /// </summary>
        /// <typeparam name="T">Type of object.</typeparam>
        /// <param name="func">Function.</param>
        /// <returns>
        /// Non generic function.
        /// </returns>
        public static Func<object, long> MakeNonGeneric<T>(this Func<T, long> func)
        {
            Ensure.NotNull(func, nameof(func));

            return x => func((T)x);
        }

        /// <summary>
        /// Make non generic function from generic.
        /// </summary>
        /// <typeparam name="T">Type of object.</typeparam>
        /// <param name="func">Function.</param>
        /// <returns>
        /// Non generic function.
        /// </returns>
        public static Func<object, string> MakeNonGeneric<T>(this Func<T, string> func)
        {
            Ensure.NotNull(func, nameof(func));

            return x => func((T)x);
        }

        /// <summary>
        /// Make non generic function from generic.
        /// </summary>
        /// <typeparam name="T">Type of object.</typeparam>
        /// <param name="func">Function.</param>
        /// <returns>
        /// Non generic function.
        /// </returns>
        public static Func<object, Regex> MakeNonGeneric<T>(this Func<T, Regex> func)
        {
            Ensure.NotNull(func, nameof(func));

            return x => func((T)x);
        }

        /// <summary>
        /// Make non generic action from generic.
        /// </summary>
        /// <typeparam name="T">Type of object.</typeparam>
        /// <param name="action">Action.</param>
        /// <returns>
        /// Non generic action.
        /// </returns>
        public static Action<object> CoerceToNonGeneric<T>(this Action<T> action)
        {
            Ensure.NotNull(action, nameof(action));

            return x => action((T)x);
        }
    }
}
