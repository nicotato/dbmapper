using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace DBMapper.MapperFormat
{
    public class FormatMapper<T> : IFormatMapper<T>
    {

        private static Dictionary<String, Delegate> dicMap = new Dictionary<String, Delegate>();

        public static Dictionary<String, Delegate> GetDiccionarioMappero
        {
            get { return dicMap; }
        }

        private static MemberInfo FindProperty(LambdaExpression lambdaExpression)
        {
            Expression expressionToCheck = lambdaExpression;

            bool done = false;

            while (!done)
            {
                switch (expressionToCheck.NodeType)
                {
                    case ExpressionType.Convert:
                        expressionToCheck = ((UnaryExpression)expressionToCheck).Operand;
                        break;
                    case ExpressionType.Lambda:
                        expressionToCheck = ((LambdaExpression)expressionToCheck).Body;
                        break;
                    case ExpressionType.MemberAccess:
                        var memberExpression = ((MemberExpression)expressionToCheck);

                        if (memberExpression.Expression.NodeType != ExpressionType.Parameter &&
                            memberExpression.Expression.NodeType != ExpressionType.Convert)
                        {
                            throw new MapperException(string.Format("Expression '{0}' resultado debe ser miembro de nivel superior y no las propiedades de cualquier objeto secundario.", lambdaExpression)
                                , new ArgumentException(string.Format("Expression '{0}' resultado debe ser miembro de nivel superior y no las propiedades de cualquier objeto secundario.", lambdaExpression, "lambdaExpression")));
                        }

                        MemberInfo member = memberExpression.Member;

                        return member;
                    default:
                        done = true;
                        break;
                }
            }

            throw new MapperException("Configuración personalizada para los miembros sólo se admite para los miembros individuales de alto nivel sobre un tipo.");
        }


        public IFormatMapper<T> FormatMember(Expression<Func<T, object>> destinationMember, Func<object, object> origenMember)
        {
            var propName = FindProperty(destinationMember);
            AddMap(propName.Name, origenMember);
            return this;
        }
        public IFormatMapper<T> FormatMember(string propName, Func<T, object, object> origenMember)
        {
            AddMap(propName, origenMember);
            return this;
        }
        public IFormatMapper<T> FormatMember(string propName, Action<T, object> origenMember)
        {
            AddMap(propName, origenMember);
            return this;
        }

        private void AddMap(string name, Delegate accion)
        {
            try
            {
                dicMap.Add(name, accion);
            }
            catch (ArgumentNullException ex)
            {
                throw new MapperException("No se puede Mapear clave nula: " + name, ex);
            }
            catch (ArgumentException ex)
            {
                throw new MapperException("No se puede Mapear clave repetida: " + name, ex);
            }
        }
    }
}
