using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DBMapper.MapperFormat
{
    public interface IFormatMapper<T>
    {
        /// <summary>
        /// asigna un mapa de formato o cambio de tipo a un valor para asignarlo a una propiedad
        /// </summary>
        /// <param name="destinationMember"></param>
        /// <param name="origenMember"></param>
        /// <returns></returns>
        IFormatMapper<T> FormatMember(Expression<Func<T, object>> destinationMember, Func<object, object> origenMember);
        /// <summary>
        /// asigna un mapa de formato o cambio de tipo a un valor para asignarlo a una propiedad
        /// </summary>
        /// <param name="destinationMember"></param>
        /// <param name="origenMember"></param>
        /// <returns></returns>
        IFormatMapper<T> FormatMember(string columnName, Func<T, object, object> origenMember);
        /// <summary>
        /// asigna un mapa de formato o cambio de tipo a un valor para asignarlo a una propiedad
        /// </summary>
        /// <param name="destinationMember"></param>
        /// <param name="origenMember"></param>
        /// <returns></returns>
        IFormatMapper<T> FormatMember(string columnName, Action<T, object> origenMember);
    }
}
