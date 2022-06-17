using System;
using System.Collections.Generic;
using System.Linq;

namespace BudgetApp
{
    public class Budget : BudgetService, IBudget
    {
        private Dictionary<int, Transaction> _budget;
        private double _balance = 0;

        internal static Dictionary<int, Transaction> transactionsList;
        internal static Dictionary<int, User> usersList;
        internal static Dictionary<int, Category> categoriesList;

        public Dictionary<int, Transaction> BudgetData { get => _budget; set => _budget = value; }
        public string BudgetSelector { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double BudgetBalance { get => _balance; set => _balance = value; }

        public Budget()
        {
            _budget = transactionsList = LoadTransactionList(fileNames["Transactions"]);
            usersList = LoadUserList(fileNames["Users"]);
            categoriesList = LoadCategoryList(fileNames["Categories"]);
        }

        public void UpdateBudget(Dictionary<int, Transaction> newData) => _budget = newData;

        public void CalculateBalance()
        {
            foreach (KeyValuePair<int, Transaction> record in _budget) _balance += record.Value.TransactionAmount;

            Console.WriteLine($"Stan konta: {_balance}");
            _balance = 0;
        }

        public void EstablishBudgetStructure()
        {
            var incomeCategories = new Dictionary<string, valueAndPercentage>();
            var expenseCategories = new Dictionary<string, valueAndPercentage>();
            //populate with categories
            foreach (var category in categoriesList.Values)
            {
                if (category.CategoryType.Equals("income"))
                    incomeCategories.Add(category.CategoryName,new valueAndPercentage(0,0));
                else if (category.CategoryType.Equals("expense"))
                    expenseCategories.Add(category.CategoryName, new valueAndPercentage(0,0));
            }

            //sumup 
            double sumExpense = 0;
            double sumIncome = 0;
            foreach (var transaction in transactionsList.Values)
            {
                if (transaction.TransactionCategory.CategoryType.Equals("income"))
                {
                    incomeCategories[transaction.TransactionCategory.CategoryName].value += transaction.TransactionAmount;
                    sumIncome += transaction.TransactionAmount;
                }
                else if (transaction.TransactionCategory.CategoryType.Equals("expense"))
                {
                    expenseCategories[transaction.TransactionCategory.CategoryName].value += transaction.TransactionAmount;
                    sumExpense += transaction.TransactionAmount;
                }
            }

            //calculate %
            foreach (var incomeTrans in incomeCategories.Values)
            {
                incomeTrans.percentage = incomeTrans.value / sumIncome; 
            }
            foreach (var expenseTrans in expenseCategories.Values)
            {
                expenseTrans.percentage = expenseTrans.value / sumExpense;
            }

            //display
            Console.ForegroundColor = ConsoleColor.Green;
            foreach (KeyValuePair<string, valueAndPercentage> record in incomeCategories)
            {
                Console.WriteLine($" + {record.Key}: {record.Value.value} PLN ({(record.Value.percentage * 100).ToString("#.##")}%)");
            }
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("--------------------------------------------------");
            Console.ForegroundColor = ConsoleColor.Red;
            foreach (KeyValuePair<string, valueAndPercentage> record in expenseCategories)
            {
                Console.WriteLine($" + {record.Key}: {record.Value.value} PLN ({(record.Value.percentage * 100).ToString("#.##")}%)");
            }
            Console.ForegroundColor = ConsoleColor.Gray;

            Console.WriteLine($"\n\t saldo: {(sumIncome - sumExpense).ToString("#.##")}");
            Console.ReadLine();
        }
        private class valueAndPercentage
        {
            public valueAndPercentage(double value, double percentage)
            {
                this.value = value;
                this.percentage = percentage;
            }

            public double value { get; set; }
            public double percentage { get; set; }
        }
    }
}