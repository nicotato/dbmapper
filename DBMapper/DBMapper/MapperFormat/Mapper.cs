using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBMapper.MapperFormat
{
    public class Mapper
    {
        /// <summary>
        /// Crea un mapa para dar formato a las propiedades de los objetos a cargar.
        /// </summary>
        /// <typeparam name="T" value="Clase a la que se asignaran sus instancias."></typeparam>
        /// <returns></returns>
        public static IFormatMapper<T> CrearMap<T>()
        {
            return new FormatMapper<T>();                     
        }

    }
}
