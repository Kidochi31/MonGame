using MonGame.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MonGame.ECS
{
    public class EventConstructorCache<T> where T: Delegate
    {
        Dictionary<Type, T> CachedEventConstructors = [];

        public T this[Type type]
        {
            get
            {
                // Create a constructor if there isn't one already
                if(!CachedEventConstructors.ContainsKey(type))
                    CachedEventConstructors[type] = CreateConstructor(type);
                return CachedEventConstructors[type];
            }
        }

        static T CreateConstructor(Type type)
        {
            //T must be of Func<X, Y, ..., base type>
            // params will be the type arguments except the last
            Type[] typeArgs = typeof(T).GetGenericArguments()[..^1];
            ParameterExpression[] parameters = (from t in typeArgs select Expression.Parameter(t)).ToArray();

            var ctor = type.GetConstructor(typeArgs);

            // from Ecs to type
            T constructor = (T)Expression.Lambda(Expression.New(ctor, parameters), parameters).Compile();
            return constructor;
        }
    }
}
