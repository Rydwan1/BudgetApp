using System;
using System.Collections.Generic;
using System.Linq;

namespace BudgetApp
{
    public class Budget : BudgetService, IBudget
    {
        private Dictionary<int, Transaction> _budget;
        private double _balance = 0;
        private Dictionary<string, (double, double)> _budgetStructure;

        internal static Dictionary<int, Transaction> transactionsList;
        internal static Dictionary<int, User> usersList;
        internal static Dictionary<int, Category> categoriesList;

        public Dictionary<int, Transaction> BudgetData { get => _budget; set => _budget = value; }
        public string BudgetSelector { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double BudgetBalance { get => _balance; set => _balance = value; }
        public Dictionary<string, (double CategoryAmount, double CategoryPercentage)> BudgetStructure { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

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
            // Krok 1. Zsumować kwoty w poszczególnych kategoriach.
            //         - dla każdej kategorii w liście kategorii przefiltrować BudgetData,
            //         - z pozyskanych transakcji zsumować wartości z Amount.
            // Krok 2. Wyliczyć procentową wartość z wykorzystaniem BudgetBalance.
            // Krok 3. Wygenerować słownik:
            //         - nazwy transakcji jako klucze,
            //         - krotka zawierająca sumy i procenty z ww. kroków jako wartości.
            // Krok 4. Wyświetlić uporządkowaną tabelę w konsoli.

            var transactionByCategories = new Dictionary<string, valueAndPercentage>();

            //populate with categories
            foreach (var category in categoriesList)
            {
                transactionByCategories.Add(category.Value.CategoryName,new valueAndPercentage(0,0));
            }

            //sumup 
            double sum = 0;
            foreach (var transaction in transactionsList.Values)
            {
                transactionByCategories[transaction.TransactionCategory.CategoryName].value += transaction.TransactionAmount;
                sum+= transaction.TransactionAmount;
            }

            //calculate %
            foreach (var transByCat in transactionByCategories.Values)
            {
                transByCat.percentage = transByCat.value / sum; 
            }

            foreach (KeyValuePair<string, (double amount, double percentage)> record in _budgetStructure)
            {
                Console.WriteLine($" + {record.Key}: {record.Value.amount} PLN ({record.Value.percentage}%)");
            }
        }
        private class valueAndPercentage //musi być klasa structy też są immutable albo value type albo cośtam nie wiem
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