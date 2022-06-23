using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Structr.Specifications.Internal
{
    internal class FetchExpressionVisitor : ExpressionVisitor
    {
        public string Property { get; private set; }

        public FetchExpressionVisitor(Expression expr)
        {
            base.Visit(expr);
        }

        protected override Expression VisitMember(MemberExpression m)
        {
            PropertyInfo pinfo = m.Member as PropertyInfo;

            if (pinfo == null)
            {
                throw new Exception("You can only include Properties");
            }

            if (m.Expression.NodeType != ExpressionType.Parameter)
            {
                throw new Exception("You can only include Properties of the Expression Parameter");
            }

            Property = pinfo.Name;

            return m;
        }
    }
}
