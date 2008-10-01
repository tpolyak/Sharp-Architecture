using System;
using System.Reflection;

namespace SharpArch.Core
{
    public class EnumDescription
    {
        public class TextRepresentationAttribute : Attribute
        {
            public readonly string Text;
            public readonly string Description = "";

            public TextRepresentationAttribute(string text) {
                Text = text;
                Description = "";
            }

            public TextRepresentationAttribute(string text, string description) {
                Text = text;
                Description = description;
            }
        }

        public class TextRepresentation
        {
            public static string GetTextRepresentationOf(Enum enumType) {
                TextRepresentationAttribute textRepresentation = GetTextRepresentationAttributeFor(enumType);

                return textRepresentation != null 
                    ? textRepresentation.Text 
                    : enumType.ToString();
            }

            public static string GetDescriptionOf(Enum enumType) {
                TextRepresentationAttribute textRepresentation = GetTextRepresentationAttributeFor(enumType);

                return textRepresentation != null 
                    ? textRepresentation.Description 
                    : String.Empty;
            }

            private static TextRepresentationAttribute GetTextRepresentationAttributeFor(Enum enumType) {
                MemberInfo[] memberInfo = enumType.GetType().GetMember(enumType.ToString());

                if (memberInfo != null && memberInfo.Length == 1) {
                    object[] customAttributes =
                        memberInfo[0].GetCustomAttributes(typeof(TextRepresentationAttribute), false);

                    if (customAttributes.Length == 1) {
                        return (TextRepresentationAttribute) customAttributes[0];
                    }
                }

                return null;
            }
        }
    }
}
