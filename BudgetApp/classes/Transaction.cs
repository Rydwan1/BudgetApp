﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace BudgetApp
{
    public class Transaction : BudgetElement, ITransaction
    {
        private Category _category;
        private double _amount;
        private string _description;
        private User _user;
        private DateTimeOffset _date;

        public int TransactionID { get => _id; set => _id = value; }
        public Category TransactionCategory { get => _category; set => _category = value; }
        public double TransactionAmount { get => _amount; set => _amount = value; }
        public string TransactionDescription { get => _description; set => _description = value; }
        public User TransactionUser { get => _user; set => _user = value; }
        public DateTimeOffset TransactionDate { get => _date; set => _date = value; }

        public Transaction(int id, Category category, double amount, string description, User user, DateTimeOffset date)
        {
            _id = id;
            _category = category;
            _amount = amount;
            _description = description;
            _user = user;
            _date = date;
        }

        public override void PrintProperties()
        {
            Console.WriteLine($"Kategoria: {_category.CategoryName} \n" +
                $"Ilość: {_amount} \n" +
                $"Opis: {_description} \n" +
                $"Domownik: {_user.UserFirstName} {_user.UserLastName} \n" +
                $"Data: {_date.ToString("dd-MM-yyyy")}");
        }

        public static Dictionary<int, Transaction> GetTransactionByCategory(int selectedCategoryID, Dictionary<int, Transaction> transactionsList, Dictionary<int, Category> categoriesList, Dictionary<int, User> usersList)
        {
            Dictionary<int, Transaction> selectedCategoryTransaciton = new();
            foreach (KeyValuePair<int, Transaction> transaction in transactionsList)
            {
                if (selectedCategoryID == (transaction.Value.TransactionCategory.CategoryID))
                {
                    selectedCategoryTransaciton.Add(transaction.Key, transaction.Value);
                }
            }
            Console.Clear();
            PrintTransactionList(selectedCategoryTransaciton);
            Console.ReadLine();
            return selectedCategoryTransaciton;
        }

        public static Dictionary<int, Transaction> GetTransactionByUser(int selectedUserID, Dictionary<int, Transaction> transactionsList, Dictionary<int, Category> categoriesList, Dictionary<int, User> usersList)
        {
            Dictionary<int, Transaction> selectedUserTransaciton = new();
            foreach (KeyValuePair<int, Transaction> transaction in transactionsList)
            {
                if (selectedUserID == (transaction.Value.TransactionUser.UserID))
                {
                    selectedUserTransaciton.Add(transaction.Key, transaction.Value);
                }
            }
            Console.Clear();
            PrintTransactionList(selectedUserTransaciton);
            Console.ReadLine();
            return selectedUserTransaciton;
        }

        public static void AddTransactionReworked(Dictionary<int, Transaction> transactionsList, Dictionary<int, Category> categoriesList, Dictionary<int, User> usersList)
        {
            Console.Clear();
            int transactionID = transactionsList.Count == 0 ? 1 : (transactionsList.Keys.Max() + 1);

            Console.WriteLine("Wybierz kategorię transakcji z listy poniżej, wpisując jej numer: ");
            Category.PrintCategories(true, categoriesList);
            int selectedCategoryID = GetConsoleInput<Category>.GetUserInputID(categoriesList, true);
            Console.Clear();
            double transactionAmmount = GetConsoleInput.UserInputTransactionAmmount(false);
            Console.Clear();
            Console.Write("Wprowadź opis transakcji (pole opcjonalne): ");
            string description = Console.ReadLine();
            Console.Clear();
            Console.WriteLine("Do którego domownika należy ta transakcja?");
            User.PrintUsers(false, usersList);
            int selectedUserID = GetConsoleInput<User>.GetUserInputID(usersList, true);
            Console.Clear();
            DateTimeOffset date = GetConsoleInput.ChooseDateOfTransaction();

            transactionsList.Add(transactionID, new Transaction(transactionID, categoriesList[selectedCategoryID], transactionAmmount, description, usersList[selectedUserID], date));
            Console.Clear();
        }

        public static void EditTransactionReworked(int selectedTransactionID, Dictionary<int, Transaction> transactionsList, Dictionary<int, Category> categoriesList, Dictionary<int, User> usersList)
        {
            Console.Clear();
            Console.WriteLine("Co zamierzasz zrobić z wybraną transakcją? [e] - edycja, [d] - usuwanie, [jakikolwiek inny klawisz] - wróć do menu");
            transactionsList[selectedTransactionID].PrintProperties();
            ConsoleKeyInfo keyInfo = Console.ReadKey();
            switch (keyInfo.Key)
            {
                case ConsoleKey.E:
                    var oldTransaction = transactionsList[selectedTransactionID];

                    Console.WriteLine($"Wybierz nową kategorie ({oldTransaction.TransactionCategory.CategoryName}), zostaw puste żeby nie zmieniać");
                    Category.PrintCategories(true, categoriesList);
                    int selectedNewCategoryID = GetConsoleInput<Category>.GetUserInputID(categoriesList, true);
                    transactionsList[selectedTransactionID].TransactionCategory = selectedNewCategoryID == -1 ? transactionsList[selectedTransactionID].TransactionCategory : categoriesList[selectedNewCategoryID];
                    Console.Clear();
                    Console.WriteLine($"Wpisz nową kwotę ({oldTransaction.TransactionAmount}), zostaw puste żeby nie zmieniać");
                    double newAmmount = GetConsoleInput.UserInputTransactionAmmount(true);
                    transactionsList[selectedTransactionID].TransactionAmount = newAmmount == -1 ? transactionsList[selectedTransactionID].TransactionAmount : newAmmount;
                    Console.Clear();
                    Console.WriteLine($"Wpisz nowy opis transakcji {oldTransaction.TransactionDescription}, zostaw puste żeby nie zmieniać"); //ogarnąć żeby wyświetlało to estetycznie
                    string newDescription = Console.ReadLine();
                    transactionsList[selectedTransactionID].TransactionDescription = string.IsNullOrWhiteSpace(newDescription) ? transactionsList[selectedTransactionID].TransactionDescription : newDescription;
                    Console.Clear();
                    Console.WriteLine($"Przypisz tą transakcje do innego domownika ({oldTransaction.TransactionUser.UserFirstName} {oldTransaction.TransactionUser.UserLastName}), zostaw puste żeby nie zmieniać");
                    User.PrintUsers(true, usersList);
                    int selectedNewUserID = GetConsoleInput<User>.GetUserInputID(usersList, true);
                    transactionsList[selectedTransactionID].TransactionUser = selectedNewUserID == -1 ? transactionsList[selectedTransactionID].TransactionUser : usersList[selectedNewUserID];
                    Console.Clear();
                    Console.WriteLine($"Zmienić datę tej transakcji? {oldTransaction.TransactionDate.ToString("dd-MM-yyyy")} (t/n)");
                    if (Console.ReadLine().ToUpper().Equals("T"))
                    {
                        transactionsList[selectedTransactionID].TransactionDate = GetConsoleInput.ChooseDateOfTransaction();
                    }
                    break;

                case ConsoleKey.D:
                    transactionsList.Remove(selectedTransactionID);
                    Console.WriteLine("Usuwanie zakończone!");
                    break;

                default:
                    Console.WriteLine("Nieprawidłowy wybór");
                    Menu.ManageProgramWorking();
                    break;
            }
        }

        public static void ManageTransactions(Dictionary<int, Transaction> transactionsList, Dictionary<int, Category> categoriesList, Dictionary<int, User> usersList)
        {
            Console.Clear();
            PrintTransactionList(transactionsList);
            Console.WriteLine("\n -> Wybierz 0, aby dodać nową transakcję. \n -> Jeżeli chcesz zmodyfikować dane istniejącej transakacji, wypisz jego numer ID. \n -> Aby wrócić do głównego menu, naciśnij ENTER, pozostawiając pole puste. "); //help
            while (true)
            {
                string consoleInput = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(consoleInput))
                {
                    Console.Clear();
                    return;
                }
                if (consoleInput.Equals("0"))
                {
                    AddTransactionReworked(transactionsList, categoriesList, usersList);
                    return;
                }
                int selectedID = -1;
                if (int.TryParse(consoleInput, out selectedID) && transactionsList.ContainsKey(selectedID))
                {
                    EditTransactionReworked(selectedID, transactionsList, categoriesList, usersList);
                    return;
                }
                Console.WriteLine("podanego id nie ma na liscie transakcji");
            }
        }

        private static void PrintTransactionList(Dictionary<int, Transaction> transactionsList)
        {
            bool colorChanger = false;
            foreach (KeyValuePair<int, Transaction> transaction in transactionsList)
            {
                if (colorChanger)
                {
                    if (transaction.Value.TransactionCategory.CategoryType.Equals("income"))
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                    }
                    if (transaction.Value.TransactionCategory.CategoryType.Equals("expense"))
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                    }
                }
                else if (!colorChanger)
                {
                    if (transaction.Value.TransactionCategory.CategoryType.Equals("income"))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    if (transaction.Value.TransactionCategory.CategoryType.Equals("expense"))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                }
                Console.WriteLine($"[{transaction.Key}] : ");
                transaction.Value.PrintProperties();
                colorChanger = !colorChanger;
            }
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}