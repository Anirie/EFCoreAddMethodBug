using System;
using System.Collections.Generic;
using System.Text;

namespace devMathOpt.Interfaces
{
    public interface IScenario
    {
        String ScenarioID { get; }
        String ShortDescription { get; set; }
    }
}
