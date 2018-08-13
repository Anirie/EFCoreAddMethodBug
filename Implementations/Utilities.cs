using DBCommunicatorInterfaces.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Reflection;

namespace devMathOpt.Implementations
{
        public static class Utilities
        {
            public static object CreateUntrackedEntity(object entity, Type entityType)
            {
                var rowString = JsonConvert.SerializeObject(entity);
                return JsonConvert.DeserializeObject(rowString, entityType);
            }

            public static DbContext CreateNewConnection(this IDbToolContext context)
            {
                DbContextOptions options = context.ContextOptions;
                ((DbContext)context).Dispose();
                Type contextType = context.GetType();
                ConstructorInfo ctor = contextType.GetConstructor(new[] { options.GetType() });
                var instance = ctor.Invoke(new object[] { options }) as DbContext;
                return instance;

            }
    }
}
