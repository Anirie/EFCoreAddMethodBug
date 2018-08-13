using devMathOpt.Implementations;
using devMathOpt.TestModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TestSupport.EfHelpers;

namespace devMathOpt
{
    public abstract class TestModelContextBase : IDisposable
    {
        protected DbContextOptions<TestModelContext> Options { get; }
        protected TestModelContext Context { get; set; }
        protected GenericRepository GenericRepository { get; set; }

        public TestModelContextBase()
        {
            Options = SqliteInMemory.CreateOptions<TestModelContext>();
            this.ResetConnectionToDatabase();
        }
        public void Dispose()
        {
            this.Context.Dispose();
        }
        protected virtual void ResetConnectionToDatabase()
        {
            if (this.Context != null)
            {
                this.Context.Dispose();
            }
            this.Context = new TestModelContext(this.Options);
            this.GenericRepository = new GenericRepository(this.Context);
        }
    }
}
