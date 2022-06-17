using System;

namespace BudgetApp
{
    public class BudgetElement
    {
        public int _id { get; set; }

        public virtual void PrintProperties() => Console.WriteLine($"id: {_id}");
    }
}