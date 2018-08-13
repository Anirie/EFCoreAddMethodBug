using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using devMathOpt.Implementations;

namespace devMathOpt.Interfaces
{
    public interface IDbToolContext
    {
       // Task<List<TableInfo>> GetEntriesForScenarioAsync(String scenarioId);

        Type ScenarioType { get; }
        Type ScenarioIdType { get; }

        DbContextOptions ContextOptions { get; }
    }
}
