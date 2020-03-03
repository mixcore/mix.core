using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Mix.Cms.Lib.Helpers
{
    public class ReflectionHelper
    {
        public static Type GetPropertyType(Type type, string name)
        {
            Type fieldPropertyType;
            FieldInfo fieldInfo = type.GetField(name);

            if (fieldInfo == null)
            {
                PropertyInfo propertyInfo = type.GetProperty(name);

                //if (propertyInfo == null)
                //{
                //    throw new Exception();
                //}

                fieldPropertyType = propertyInfo?.PropertyType;
            }
            else
            {
                fieldPropertyType = fieldInfo.FieldType;
            }
            return fieldPropertyType;
        }
    }
}
