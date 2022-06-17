using System.Collections.Generic;

namespace BudgetApp
{
    internal interface IMenu
    {
        bool IsProgramOpen { get; set; }
        Dictionary<string, string> ProgramOptions { get; }

        void HandleMenu();

        static void ManageProgramWorking() { }
    }
}