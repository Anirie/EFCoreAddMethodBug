using System;
using System.Collections.Generic;
using System.Text;

namespace devMathOpt
{
    public class ScenarioRepositoryTest : TestModelContextBase
    {
        private Implementations.ScenarioRepository ScenarioRepository { get; set; }

        protected override void ResetConnectionToDatabase()
        {
            base.ResetConnectionToDatabase();
            this.ScenarioRepository = new Implementations.ScenarioRepository(this.Context, this.GenericRepository);
        }

    }
}
