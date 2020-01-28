using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    using System.Reflection;
    using BE;
    /// <summary>
    /// static class for deep copy.
    /// Copy source fields to new instance
    /// </summary>
    /// <typeparam name="T">The type argument must have a public parameterless c'tor.</typeparam>
    /// <returns>returns a new instance of T with the same values as source</returns>
    public static class Cloning
    {
        /// <summary>
        /// template function for deep copy.
        /// Copy source fields to new instance
        /// </summary>
        /// <typeparam name="T">The type argument must have a public parameterless c'tor.</typeparam>
        /// <returns>returns a new instance of T with the same values as source</returns>
        public static T clone<T>(this T source) where T : new()
        {
            // create new type and save his type
            T target = new T();
            Type type = typeof(T);

            // copy all properties from source to target
            foreach (PropertyInfo sourceProperty in type.GetProperties())
            {
                PropertyInfo targetProperty = type.GetProperty(sourceProperty.Name); // get property name
                targetProperty.SetValue(target, sourceProperty.GetValue(source, null), null);
            }

            // copy all field (the data) from source to target
            foreach (FieldInfo sourceField in type.GetFields())
            {
                FieldInfo targetField = type.GetField(sourceField.Name); // get field name
                targetField.SetValue(target, sourceField.GetValue(source));
            }

            // return clonning object
            return target;
        }
    }
}
