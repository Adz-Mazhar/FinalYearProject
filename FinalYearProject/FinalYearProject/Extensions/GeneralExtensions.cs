using System.Reflection;

namespace FinalYearProject.Extensions
{
    public static class GeneralExtensions
    {
        public static TDerived ConvertToDerived<TBase, TDerived>(this TBase baseObject) where TDerived : TBase, new()
        {
            TDerived derivedObject = new();

            var baseType = typeof(TBase);
            PropertyInfo[] properties = baseType.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                MethodInfo getMethod = property.GetGetMethod();
                MethodInfo setMethod = property.GetSetMethod();

                object val = getMethod?.Invoke(baseObject, null);
                object[] param = new object[] { val };
                setMethod?.Invoke(derivedObject, param);
            }

            return derivedObject;
        }
    }
}
