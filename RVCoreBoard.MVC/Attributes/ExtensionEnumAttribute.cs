namespace RVCoreBoard.MVC.Attributes
{
    using System;
    using System.ComponentModel;
    using System.Reflection;

    public class ExtensionEnumAttribute : Attribute
    {
        private object[] _objs;

        public ExtensionEnumAttribute(params object[] objs)
        {
            _objs = objs;
        }

        public object[] Objs
        {
            get
            {
                return _objs;
            }
        }
    }

    public static class ExtensionEnumAttributeHelper
    {
        public static T GetEnumAttribute<T>(this Enum pEnum)
        {
            Type tType = pEnum.GetType();
            FieldInfo fi = tType.GetField(pEnum.ToString());
            object[] objCustomAttributes = fi.GetCustomAttributes(false);

            if (objCustomAttributes != null && objCustomAttributes.Length <= 0) return default(T);
            foreach (object Obj in objCustomAttributes)
            {
                if (Obj is T)
                {
                    return (T)Obj;
                }
            }
            return default(T);
        }

        public static T GetEnumAttributeValue<T>(this Enum pEnum, int valIdx)
        {
            Type tType = pEnum.GetType();
            FieldInfo fi = tType.GetField(pEnum.ToString());

            ExtensionEnumAttribute tObj = GetEnumAttribute<ExtensionEnumAttribute>(pEnum);
            if (tObj == null)
            {
                return default(T);
            }

            if (tObj.Objs != null &&
                tObj.Objs.Length > 0 &&
                tObj.Objs.Length > valIdx)
            {
                if (tObj.Objs[valIdx] is IConvertible)
                {
                    return (T)Convert.ChangeType(tObj.Objs[valIdx], typeof(T));
                }
                else
                {
                    return (T)tObj.Objs[valIdx];
                }
            }
            else
            {
                return default(T);
            }
        }

        public static bool CheckValIdx(this Enum pEnum, int valIdx)
        {
            Type tType = pEnum.GetType();
            FieldInfo fi = tType.GetField(pEnum.ToString());

            ExtensionEnumAttribute tObj = GetEnumAttribute<ExtensionEnumAttribute>(pEnum);
            if (tObj == null)
            {
                return false;
            }

            if (tObj.Objs != null && tObj.Objs.Length > valIdx)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
