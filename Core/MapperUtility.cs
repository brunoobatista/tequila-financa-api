using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;

namespace Tequila.Core
{
    class NameType
    {
        public string Name { get; set; }
        public Type Type { get; set; }
    }
    public static class MapperUtility
    {
        public static TSource atualizarDados<TSource, TTarget>(this TSource aSource, TTarget aTarget)
        {
            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic;
            Type srcType = aSource.GetType();
            PropertyInfo[] srcProperties = srcType.GetProperties(flags);

            var srcFields = (from PropertyInfo aProp in srcProperties
                where aProp.CanWrite
                select new
                {
                    Name = aProp.Name,
                    Type = Nullable.GetUnderlyingType(aProp.PropertyType) ?? aProp.PropertyType
                }).ToList();

            Type trgType = aTarget.GetType();
            PropertyInfo[] trgProperties = trgType.GetProperties(flags);

            var trgFields = (from PropertyInfo aProp in trgProperties
                where aProp.CanRead
                select new
                {
                    Name = aProp.Name,
                    Type = Nullable.GetUnderlyingType(aProp.PropertyType) ?? aProp.PropertyType
                }).ToList();

            var commonFields = srcFields.Intersect(trgFields).ToList();
            TSource newSource;
            foreach (var aField in commonFields)
            {
                var srcValue = aSource.GetType().GetProperty(aField.Name).GetValue(aSource, null);
                var trgValue = aTarget.GetType().GetProperty(aField.Name).GetValue(aTarget, null);
                if (trgValue != null && !srcValue.Equals(trgValue))
                {
                    PropertyInfo propertyInfo = aSource.GetType().GetProperty(aField.Name);
                    propertyInfo.SetValue(aSource, trgValue, null);
                }
            }

            return aSource;
        }
    }
}