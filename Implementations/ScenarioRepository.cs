using System;
using System.Collections.Generic;
using System.Linq;
using devMathOpt.Interfaces;
using System.Diagnostics;

namespace devMathOpt.Implementations
{
    public class ScenarioRepository : IScenarioRepository
    {
        private readonly Type scenarioType;
        private readonly IGenericRepository repository;

        public ScenarioRepository(Type scenarioType, IGenericRepository repository)
        {
            this.scenarioType = scenarioType;
            this.repository = repository;
        }

        public ScenarioRepository(IDbToolContext dbContext, IGenericRepository repository) : this(dbContext.ScenarioType, repository) { }

        public IScenario CopyScenario(string scenarioId)
        {
            IScenario scenario = GetScenarioIncludingNavigationProps(scenarioId);
            scenario.ShortDescription = "Copy of " + scenario.ShortDescription;
            IScenario newScenario = CreateScenarioWithValues(scenario);
            return newScenario;
        }

        public IScenario CreateEmptyScenario()
        {
            IScenario scenario = (IScenario) Activator.CreateInstance(scenarioType);
            scenario.ShortDescription = "New Scenario";
            repository.Create(scenario);
            return scenario;
        }

        public IScenario CreateScenarioWithValues(IScenario scenario)
        {
            IScenario newScenario=(IScenario)repository.InsertEntityWithDeletedIds(scenario);
            return newScenario;
        }

        public void DeleteScenario(string scenarioId)
        {
            repository.DeleteById(scenarioType, scenarioId);
        }

        public IQueryable<IScenario> GetAllScenarios()
        {
            return (IQueryable<IScenario>) this.repository.GetAll(scenarioType);
        }

        public IScenario GetScenarioIncludingNavigationProps(string scenarioId)
        {
            foreach (IScenario scenario in this.repository.GetAllIncludingNavigationProperties(scenarioType))
            {
                if (scenario.ScenarioID == scenarioId)
                    return scenario;
            }
            throw new KeyNotFoundException("Did not find a Scenario with id: " + scenarioId);
        }

        public IScenario RenameScenario(string scenarioId, string shortDescription)
        {
            IScenario scenario = (IScenario)repository.GetById(scenarioType, scenarioId);
            scenario.ShortDescription = shortDescription;
            scenario = (IScenario) repository.Update(scenario);
            return scenario;
        }

        //public void ReplaceTables(string scenarioId, IList<TableInfo> newTables)
        //{
        //    IScenario scenario = GetScenarioIncludingNavigationProps(scenarioId);

        //    List<Object> entitiesToDelete = new List<Object>();
        //    List<Object> entitiesToCreate = new List<Object>();

        //    foreach(var newTable in newTables)
        //    {
        //        var oldEntities = GetCurrentEntities(scenario, newTable.EntityType);
        //        entitiesToDelete.AddRange(oldEntities);
        //        var newEntities = newTable.Entries;
        //        entitiesToCreate.AddRange(newEntities);
        //    }

        //    repository.DeleteBatch(entitiesToDelete);
        //    repository.CreateBatch(entitiesToCreate);
        //}

        private IEnumerable<Object> GetCurrentEntities(IScenario scenario, Type entityType)
        {
            var setType = typeof(ICollection<>).MakeGenericType(entityType);
            Debug.Assert(scenarioType.GetProperties().Where(p => p.PropertyType == setType).Count() == 1);
            var setPropertyInScenario = scenarioType.GetProperties().Where(p => p.PropertyType == setType).First();
            var currentEntities = (IEnumerable<Object>)setPropertyInScenario.GetValue(scenario);

            return currentEntities;
        }        

        public void UpdateScenario(IScenario scenario)
        {
            repository.Update(scenario);
        }

    }
}
