using devMathOpt.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace devMathOpt.Interfaces
{
    public interface IScenarioRepository
    {
        IQueryable<IScenario> GetAllScenarios();
        IScenario GetScenarioIncludingNavigationProps(String scenarioId);
        IScenario CreateEmptyScenario();
        IScenario CreateScenarioWithValues(IScenario scenario);
        IScenario CopyScenario(String scenarioId);
        IScenario RenameScenario(String scenarioId, String shortDescription);
        void UpdateScenario(IScenario scenario);
        //void ReplaceTables(String scenarioId, IList<TableInfo> newTables);
        void DeleteScenario(String scenarioId);
    }
}
