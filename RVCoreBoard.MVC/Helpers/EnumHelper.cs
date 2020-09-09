using System;
using System.ComponentModel;

namespace RVCoreBoard.MVC.Helpers
{
    public class EnumHelper
    {
        ///<summary>
        ///</summary>
        ///<param name="enumeratedType"></param>
        ///<typeparam name="T"></typeparam>
        ///<returns></returns>
        ///<exception cref="ArgumentException"></exception>
        public static string GetEnumDescription(Enum value)
        {
            var description = value.ToString();

            var fieldInfo = value.GetType().GetField(value.ToString());

            if (fieldInfo != null)
            {
                var attributes = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attributes != null && attributes.Length > 0)
                {
                    description = ((DescriptionAttribute)attributes[0]).Description;
                }
            }
            return description;
        }
    }
}
