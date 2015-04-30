using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using DBMapper.MapperFormat;
using Entities.DBMapper;

namespace DBMapper
{
    delegate IEnumerable<T> DelMap<T>(IDataReader idr);
    public class ORM
    {
        private static ORM instance;
        private static ORM GetInstance()
        {
            if (instance != null)
                return instance;
            else
                return (instance = new ORM());
        }

        private IEnumerable<T> MapResolved<T>(IDataReader idr) where T : class, new()
        {
            var arrProp = typeof(T).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

            if (arrProp.Length == 0)
                throw new MapperException("La clase debe contener al menos una propiedad a mapear.");

            var listProp = new Dictionary<string, PropertyInfo>();
            try
            {
                foreach (var p in arrProp)
                {
                    listProp.Add(p.Name.ToLower(), p);
                    var custAttr = p.GetCustomAttributes(false).OfType<NamePropertyDB>().ToArray();
                    if (custAttr.Length > 0)
                        listProp.Add(custAttr.ElementAt(0).Name.ToLower(), p);
                }
            }
            catch (ArgumentNullException ane)
            {
                throw new MapperException("No se puede asignar un valor nulo.", ane);
            }
            catch (ArgumentException ae)
            {
                throw new MapperException("No se puede repetir el nombre de propiedades y atributos ni entre estos.", ae);
            }


            while (idr.Read())
            {
                T ob = new T();

                for (int i = 0; i < idr.FieldCount; i++)
                {
                    var name = idr.GetName(i).ToLower();
                    PropertyInfo pI;
                    Delegate delegado;
                    if (listProp.TryGetValue(name, out pI))
                    {
                        object valorBase = idr.GetValue(i);

                        if (valorBase is DBNull)
                            valorBase = null;
                        else if (valorBase is string)
                            valorBase = ((string)valorBase).Trim();
                        else
                            valorBase = Convert.ChangeType(valorBase
                                , (Nullable.GetUnderlyingType(pI.PropertyType) ?? pI.PropertyType));


                        if (FormatMapper<T>.GetDiccionarioMappero.TryGetValue(pI.Name, out delegado))
                            if (delegado is Func<object, object>)
                                valorBase = delegado.DynamicInvoke(valorBase);
                            else if (delegado is Func<T, object, object>)
                                valorBase = delegado.DynamicInvoke(ob, valorBase);

                        pI.SetValue(ob, valorBase, null);
                    }//Para propiedades que representen listas o algo distinto al campo en base
                    else if (FormatMapper<T>.GetDiccionarioMappero.TryGetValue(name, out delegado))
                    {
                        object valorBase = idr.GetValue(i);
                        if (delegado is Func<object, object>)
                            valorBase = delegado.DynamicInvoke(valorBase);
                        else if (delegado is Func<T, object, object>)
                            valorBase = delegado.DynamicInvoke(ob, valorBase);
                        pI.SetValue(ob, valorBase, null);
                    }
                }
                yield return ob;
            }



        }
        public static IEnumerable<T> Map<T>(IDataReader idr) where T : class, new()
        {
            return ORM.GetInstance().MapResolved<T>(idr);
        }

    }
    public static class ORMExtensions
    {
        public static IEnumerable<T> Map<T>(this IDataReader idr) where T : class, new()
        {
            return ORM.Map<T>(idr);
        }
    }
}
