using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace devMathOpt.Interfaces
{
    public interface IGenericRepository
    {
        DbContext Context { get; }

        IQueryable<Object> GetAll(Type entityType);

        IQueryable<Object> GetAllIncludingNavigationProperties(Type entityType);

        Object GetById(Type entityType, params object[] ids);
       
        Object Create(Object entity);

        Object InsertEntityWithDeletedIds(Object etity);

        IEnumerable<Object> CreateBatch(IEnumerable<Object> entities);

        Object Update(Object entity);
        
        void DeleteById(Type entityType, params object[] ids);

        void Delete(Object entity);

        void DeleteBatch(IEnumerable<Object> entities);

        Object ShallowCopy(Type entityType, params object[] ids);
        
    }
}
