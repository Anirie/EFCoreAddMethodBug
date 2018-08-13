using System;
using Microsoft.EntityFrameworkCore;
namespace devMathOpt.Interfaces
{
    public interface IDbToolContext
    {

        Type ScenarioType { get; }
        Type ScenarioIdType { get; }

        DbContextOptions ContextOptions { get; }
    }
}
