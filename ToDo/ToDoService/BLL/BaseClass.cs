using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Objects.DataClasses;

namespace ToDoService.BLL
{
    public abstract class BaseClass
    {
        private static Dictionary<Type, Dictionary<Guid, BaseClass>> _cache = new Dictionary<Type, Dictionary<Guid, BaseClass>>();
        protected EntityObject _DalObject;

        protected static ToDoService.VantodoEntities GetContext()
        {
            return new ToDoService.VantodoEntities();
        }

        public static Dictionary<Guid, BaseClass> TypeCache<T>() where T : BaseClass
        {
            // Find the cache for the class
            if (!_cache.ContainsKey(typeof(T)))
            {
                _cache.Add(typeof(T), new Dictionary<Guid, BaseClass>());
            }
            return _cache[typeof(T)];
        }

        public static T Get<T>(Guid id, bool notFoundIsOk = false) where T : BaseClass
        {
            var cache = TypeCache<T>();

            // Find the item
            if (!cache.ContainsKey(id))
            {
                if (notFoundIsOk) return null;
                throw new ArgumentException(String.Format(
                    "Could not find id {0} for {1}.", id, typeof(T)));
            }

            // Return the item
            return cache[id] as T;
        }

        public static T Get<T>(EntityObject dalObject) where T : BaseClass, new()
        {
            var cache = TypeCache<T>(); 
            Guid id = new Guid(dalObject.EntityKey.ToString());

            // Find or create the item
            if (!cache.ContainsKey(id))
            {
                var t = new T();
                t._DalObject = dalObject;
                cache.Add(id, t);
            }

            // Return the item
            return cache[id] as T;
        }
    }
}