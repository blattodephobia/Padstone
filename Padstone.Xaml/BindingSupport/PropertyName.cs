using System;
using System.Linq.Expressions;
using System.Windows;
using Expression = System.Linq.Expressions.Expression;

namespace Padstone.Xaml
{
    public static class PropertyName
    {
        private static Expression RemoveConvert(Expression expression)
        {
            // Expressions using constants use their values rather names after compilation. Therefore,
            // any information regarding the constant's name is stripped from the Expression object.
            if (expression is ConstantExpression) throw new InvalidOperationException("Constant member access expressions are not supported.");

            Expression result = expression;
            while (result.NodeType == ExpressionType.Convert ||
				   result.NodeType == ExpressionType.ConvertChecked)
            {
                result = ((UnaryExpression)result).Operand;
            }

            return result;
        }

        private static readonly string DependencyPropertyNameSuffix = "Property";
        private static string ExtractMemberName(LambdaExpression memberSelector)
        {
            MemberExpression expression = RemoveConvert(memberSelector.Body) as MemberExpression;
            string result = expression.Member.Name;
            return result;
        }

        public static string Get<T>(Expression<Func<T, object>> memberSelector)
        {
            string memberName = ExtractMemberName(memberSelector);
            return memberName;
        }

        public static string Get<T>(Expression<Func<T>> memberSelector)
        {
            string memberName = ExtractMemberName(memberSelector);
            return memberName;
        }

        public static string FromDependencyProperty(Expression<Func<DependencyProperty>> dependencyPropertySelector)
        {
            string memberName = ExtractMemberName(dependencyPropertySelector);
            if (memberName.Length <= DependencyPropertyNameSuffix.Length || !memberName.EndsWith(DependencyPropertyNameSuffix))
            {
                throw new ArgumentException(
                    string.Format("Selected property name must be longer than {0} characters and end with the suffix {1}.", DependencyPropertyNameSuffix.Length, DependencyPropertyNameSuffix),
                    PropertyName.Get(() => dependencyPropertySelector));
            }

            memberName = memberName.Remove(memberName.LastIndexOf("Property"));
            return memberName;
        }
    }
}
