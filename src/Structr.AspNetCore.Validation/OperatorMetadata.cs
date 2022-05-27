using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Structr.AspNetCore.Validation
{
    public class OperatorMetadata
    {
        public string ErrorMessage { get; set; }
        public Func<object, object, bool> IsValid { get; set; }

        static OperatorMetadata()
        {
            CreateOperatorMetadata();
        }

        private static Dictionary<Operator, OperatorMetadata> _operatorMetadata;

        public static OperatorMetadata Get(Operator @operator)
        {
            return _operatorMetadata[@operator];
        }

        private static void CreateOperatorMetadata()
        {
            _operatorMetadata = new Dictionary<Operator, OperatorMetadata>()
            {
                {
                    Operator.EqualTo, new OperatorMetadata()
                    {
                        ErrorMessage = "equal to",
                        IsValid = (value, relatedPropertyValue) => {
                            if (value == null && relatedPropertyValue == null)
                            {
                                return true;
                            }
                            else if (value == null && relatedPropertyValue != null)
                            {
                                return false;
                            }
                            return value.Equals(relatedPropertyValue);
                        }
                    }
                },
                {
                    Operator.NotEqualTo, new OperatorMetadata()
                    {
                        ErrorMessage = "not equal to",
                        IsValid = (value, relatedPropertyValue) => {
                            if (value == null && relatedPropertyValue != null)
                            {
                                return true;
                            }
                            else if (value == null && relatedPropertyValue == null)
                            {
                                return false;
                            }
                            return value.Equals(relatedPropertyValue) == false;
                        }
                    }
                },
                {
                    Operator.GreaterThan, new OperatorMetadata()
                    {
                        ErrorMessage = "greater than",
                        IsValid = (value, relatedPropertyValue) => {
                            if (value == null || relatedPropertyValue == null)
                            {
                                return false;
                            }
                            return Comparer<object>.Default.Compare(value, relatedPropertyValue) >= 1;
                        }
                    }
                },
                {
                    Operator.LessThan, new OperatorMetadata()
                    {
                        ErrorMessage = "less than",
                        IsValid = (value, relatedPropertyValue) => {
                            if (value == null || relatedPropertyValue == null)
                            {
                                return false;
                            }
                            return Comparer<object>.Default.Compare(value, relatedPropertyValue) <= -1;
                        }
                    }
                },
                {
                    Operator.GreaterThanOrEqualTo, new OperatorMetadata()
                    {
                        ErrorMessage = "greater than or equal to",
                        IsValid = (value, relatedPropertyValue) => {
                            if (value == null && relatedPropertyValue == null)
                            {
                                return true;
                            }
                            if (value == null || relatedPropertyValue == null)
                            {
                                return false;
                            }
                            return Get(Operator.EqualTo).IsValid(value, relatedPropertyValue) || Comparer<object>.Default.Compare(value, relatedPropertyValue) >= 1;
                        }
                    }
                },
                {
                    Operator.LessThanOrEqualTo, new OperatorMetadata()
                    {
                        ErrorMessage = "less than or equal to",
                        IsValid = (value, relatedPropertyValue) => {
                            if (value == null && relatedPropertyValue == null)
                            {
                                return true;
                            }
                            if (value == null || relatedPropertyValue == null)
                            {
                                return false;
                            }
                            return Get(Operator.EqualTo).IsValid(value, relatedPropertyValue) || Comparer<object>.Default.Compare(value, relatedPropertyValue) <= -1;
                        }
                    }
                },
                {
                    Operator.RegExMatch, new OperatorMetadata()
                    {
                        ErrorMessage = "a match to",
                        IsValid = (value, relatedPropertyValue) => {
                            return Regex.Match((value ?? "").ToString(), relatedPropertyValue.ToString()).Success;
                        }
                    }
                },
                {
                    Operator.NotRegExMatch, new OperatorMetadata()
                    {
                        ErrorMessage = "not a match to",
                        IsValid = (value, relatedPropertyValue) => {
                            return Regex.Match((value ?? "").ToString(), relatedPropertyValue.ToString()).Success == false;
                        }
                    }
                },
                {
                    Operator.In, new OperatorMetadata()
                    {
                        ErrorMessage = "in",
                        IsValid = (value, relatedPropertyValue) => {
                            var eqOperMtd = Get(Operator.EqualTo);
                            if(relatedPropertyValue is IEnumerable valueList)
                            {
                                foreach (var val in valueList)
                                {
                                    if (eqOperMtd.IsValid(value, val))
                                    {
                                        return true;
                                    }
                                }
                                return false;
                            }
                            return eqOperMtd.IsValid(value, relatedPropertyValue);
                        }
                    }
                },
                {
                    Operator.NotIn, new OperatorMetadata()
                    {
                        ErrorMessage = "not in",
                        IsValid = (value, relatedPropertyValue) => {
                            var eqOperMtd = Get(Operator.EqualTo);
                            if(relatedPropertyValue is IEnumerable valueList)
                            {
                                foreach (var val in valueList)
                                {
                                    if (eqOperMtd.IsValid(value, val))
                                    {
                                        return false;
                                    }
                                }
                                return true;
                            }
                            return eqOperMtd.IsValid(value, relatedPropertyValue) == false;
                        }
                    }
                }
            };
        }
    }
}
