using System;
using System.Linq.Expressions;
using System.Windows;

namespace Padstone.Xaml
{
    public static class PropertyName
    {
        private static readonly string DependencyPropertyNameSuffix = "Property";
        private static string ExtractMemberName(LambdaExpression memberSelector)
        {
            MemberExpression expression =
                memberSelector.Body.NodeType == ExpressionType.Convert ?
                    (memberSelector.Body as UnaryExpression).Operand as MemberExpression
                    :
                    memberSelector.Body as MemberExpression;
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

        public static string Get(Expression<Func<object>> memberSelector)
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
