using devMathOpt.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using MathTec.Lib.Metadata;
using static MathTec.Lib.Metadata.MethodReflector;

namespace devMathOpt.Implementations
{
    public class GenericRepository : IGenericRepository
    {
        public DbContext Context { get; set; }

        public GenericRepository(DbContext dbContext)
        {
            this.Context = dbContext;
        }

        public Object Create(object entity)
        {
            return CreateBatch(new object[] { entity }).First();
        }

        public IEnumerable<Object> CreateBatch(IEnumerable<object> entities)
        {
            foreach (var entity in entities)
            {
                Type type = entity.GetType();
                InvokeDbSetMethod(type, "Add", new object[] { entity });
            }
            Context.SaveChanges();
            return entities;
        }

        // "deep copy" 
        public Object InsertEntityWithDeletedIds(object entity)
        {
         //   ResetConnection();
            Context.SetKeyPropertiesToZero(entity);
            Context.SetKeyPropertiesOfChildrenToZero(entity);
            Create(entity);
            Context.SaveChanges();
            return entity;
        }

        public void DeleteById(Type entityType, params object[] ids)
        {
            var entity = GetById(entityType, ids);
            InvokeDbSetMethod(entityType, "Remove", new object[] { entity });
            Context.SaveChanges();
        }

        public void Delete(object entity)
        {
            DeleteBatch(new object[] { entity });
        }

        public void DeleteBatch(IEnumerable<object> entities)
        {
            foreach(var entity in entities)
            {
                Type type = entity.GetType();
                InvokeDbSetMethod(type, "Remove", new object[] { entity });
            }
            Context.SaveChanges();
        }

        public IQueryable<Object> GetAll(Type entityType)
        {
            var set = GetDbSetForType(entityType); 
            return (IQueryable<object>)set;
        }

        public Object GetById(Type entityType, params object[] ids)
        {
            object[] convertedIds = ConvertIdsToExpectedKeyPropertyTypes(ids, entityType);
            object[] parameters = new object[] { convertedIds };

            var entity = InvokeDbSetMethod(entityType, "Find", parameters);
            return entity;
        }

        public IQueryable<Object> GetAllIncludingNavigationProperties(Type entityType)
        {
            IQueryable<Object> queryable = (IQueryable<Object>)GetDbSetForType(entityType);
            
            foreach(var nav in Context.GetNavigations(entityType))
            {
                queryable = Include(queryable, entityType, nav.Name);
            }

            return queryable;
        }

        private IQueryable<Object> Include(IQueryable<Object> queryable, Type queriedEntityType ,string navigationPropertyName)
        {
            var parameterTypes = new Type[] { typeof(IQueryable<>).MakeGenericType(typeof(T)), typeof(string) };
            var includingQueryable = typeof(EntityFrameworkQueryableExtensions).GetMethodExt("Include", parameterTypes).MakeGenericMethod(queriedEntityType).Invoke(null, new object[] { queryable, navigationPropertyName });
            return (IQueryable<Object>)includingQueryable;
        }

        private Object Find(Type entityType, params object[] ids)
        {
            return GetById(entityType, ids);
        }

        private object[] ConvertIdsToExpectedKeyPropertyTypes(object[] ids, Type entityType)
        {
            var keys = this.Context.GetKeyPropertiesOf(entityType);
            var keyPropertiesTypes = keys.Properties.Select(x => x.PropertyInfo.PropertyType).ToList();
            var convertedIds = ConvertObjectsToTypes(ids.ToList(), keyPropertiesTypes);
            return convertedIds.ToArray();
        }

        private List<object> ConvertObjectsToTypes(List<object> objects, List<Type> types)
        {
            ThrowExceptionIfNotOfEqualLength(objects, types);

            var convertedObjects = new List<object>();

            for (int i = 0; i < types.Count; i++)
            {
                convertedObjects.Add(ConvertObjectToType(objects[i], types[i]));
            }
            return convertedObjects;
        }

        private void ThrowExceptionIfNotOfEqualLength(List<object> objects, List<Type> types)
        {
            if (objects.Count != types.Count)
            {
                throw new InvalidOperationException("The list of objects and the list of types must be of equal length!");
            }
        }

        private object ConvertObjectToType(object element, Type type)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(type);
            var canConvertFrom = converter.CanConvertFrom(element.GetType());
            var canConvertTo = converter.CanConvertTo(type);
            if (converter != null && canConvertFrom && canConvertTo)
            {
                return converter.ConvertTo(element, type);
            }
            else
            {
                return element;
            }
        }

        public Object Update(object entity)
        {
            Type type = entity.GetType();
            Context.DeleteNavigationPropertiesOfChildren(entity);

            InvokeDbSetMethod(type, "Update", new object[] { entity });
            Context.SaveChanges();
            return entity;
        }

        private object GetDbSetForType(Type entityType)
        {
            return typeof(DbContext).GetMethod("Set").MakeGenericMethod(entityType).Invoke(Context, null);
        }
        
        private Object InvokeDbSetMethod(Type entityType, String methodName, object[] methodParameters)
        {
            return GetDbSetMethodInfoFor(entityType, methodName).Invoke(GetDbSetForType(entityType), methodParameters);
        }

        private System.Reflection.MethodInfo GetDbSetMethodInfoFor(Type entityType, String methodName)
        {
            return typeof(DbSet<>).MakeGenericType(entityType).GetMethod(methodName);
        }
        


        private void AddInstance(Type entityType, object instance)
        {
            typeof(DbContext).GetMethodExt("Add", new Type[] { typeof(T)}).MakeGenericMethod(entityType).Invoke(Context, new object[] { instance });
        }

        public Object ShallowCopy(Type entityType, params object[] idsOfEntityToCopy)
        {
            var instanceToCopy = this.Find(entityType, idsOfEntityToCopy);

            //copy without children 
            var newInstance = CreateUntrackedCopyOfEntity(instanceToCopy);

            Context.SetKeyPropertiesToZero(newInstance);

            AddInstance(entityType, newInstance);
            Context.SaveChanges();

            return newInstance;
        }

        private object CreateUntrackedCopyOfEntity(object entity)
        {
            return this.Context.Entry(entity).CurrentValues.ToObject();
        }

        //public void ResetConnection()
        //{
        //    this.Context = ((IDbToolContext) Context).CreateNewConnection();
        //}

    }

}
