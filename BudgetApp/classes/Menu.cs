﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace BudgetApp
{
    public class Menu : IMenu
    {
        private bool _isProgramOpen = true;
        private readonly Dictionary<string, string> _programOptions = new()
        {
            { "[w]", "Wyświetl listę domowników" },
            { "[s]", "Dodaj nowe transakcje" },
            { "[a]", "Edytuj i usuwaj istniejące transakcje" },
            { "[d]", "Wyświetl transakcje" },
            { "[f]", "Wyświetl listę kategorii"},
            { "[c]", "Sprawdź stan konta"}
        };
        private Budget _budget;

        public bool IsProgramOpen { get => _isProgramOpen; set => _isProgramOpen = value; }
        public Dictionary<string, string> ProgramOptions { get => _programOptions; }

        public Menu(Budget budget)
        {
            _budget = budget;
        }

        public void ShowUsersList(Dictionary<int, User> usersList)
        {
            Console.WriteLine("Lista wszystkich domowników:");
            Console.ForegroundColor = ConsoleColor.Yellow;

            foreach (KeyValuePair<int, User> record in usersList)
            {
                Console.WriteLine(
                    $" + {record.Key}: " +
                    $"{record.Value.UserFirstName} {record.Value.UserLastName} " +
                    $"{(record.Value.UserIsActive ? "AKTYWNY" : "NIEAKTYWNY")} " +
                    $"{(record.Value.UserIsAdmin ? "ADMINISTRATOR" : "USER")} ");
            }
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("\n");
        }

        public void ShowCategoriesList(Dictionary<int,Category> categoriesList)
        {
            Console.WriteLine("Lista wszystkich kategorii:");
            Console.ForegroundColor = ConsoleColor.Yellow;

            foreach (KeyValuePair<int, Category> record in categoriesList)
            {
                if (record.Value.CategoryType == "expense") Console.ForegroundColor = ConsoleColor.Red;
                else if (record.Value.CategoryType == "income") Console.ForegroundColor = ConsoleColor.Green;
                else Console.ForegroundColor = ConsoleColor.Yellow;

                Console.WriteLine(
                        $" + {record.Key}: " +
                        $"{record.Value.CategoryName} ({record.Value.CategoryType})");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("\n");
        }

        public void AddTransactions(Dictionary<int, Transaction> transactionsList, Dictionary<int, Category> categoriesList, User user)
        {
            int transactionID = transactionsList.Keys.Max() + 1;

            Transaction addingTransaction = Transaction.CreateNewTransaction(transactionID, categoriesList, user);

            transactionsList.Add(transactionID, addingTransaction);

            Console.WriteLine("Transakcja pomyślnie dodana!");
            addingTransaction.PrintProperties();

            _budget.UpdateBudget(transactionsList);
        }

        public void EditTransactions(Dictionary<int, Transaction> transactionsList, Dictionary<int, Category> categoriesList, User user)
        {
            Console.Write("Czy chcesz wyświetlić listę transakcji przed edycją? [T/N]");
            string showingTransactionsList = Console.ReadLine();
            if (showingTransactionsList.ToUpper() == "T") ShowTransactions(transactionsList, categoriesList, user);

            Console.Write("Wpisz ID poszukiwanej transakcji: ");
            int selectedTransactionID = int.Parse(Console.ReadLine());

            Transaction transactionToEdit = Transaction.FindTransactionByID(transactionsList);

                Console.WriteLine("Co zamierzasz zrobić z wybraną transakcją? [e] - edycja, [d] - usuwanie");
                ConsoleKeyInfo keyInfo = Console.ReadKey();

                switch (keyInfo.Key)
                {
                    case ConsoleKey.E:
                        // EDYCJA DANYCH TRANSAKCJI (można uwzględnić uprawnienia - pole isAdmin)                        
                        Transaction newTransactionData = Transaction.ModifySelectedTransaction(transactionToEdit, categoriesList);
                        transactionsList[selectedTransactionID] = newTransactionData;
                        Console.WriteLine("Edycja zakończona!");
                        newTransactionData.PrintProperties();
                        _budget.UpdateBudget(transactionsList);
                    break;

                    case ConsoleKey.D:
                        // USUWANIE DANYCH TRANSAKCJI (można uwzględnić uprawnienia - pole isAdmin)
                        transactionsList = Transaction.RemoveSelectedTransaction(selectedTransactionID, transactionsList);
                        Console.WriteLine("Usuwanie zakończone!");
                        _budget.UpdateBudget(transactionsList);
                    break;

                    default:
                        Console.WriteLine("Nieprawidłowy wybór");
                        ManageProgramWorking();
                        break;
                }

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("\n");
        }
        private static void ShowFilterOptions(bool isAdmin)
        {

            string adminWelcome = "Uprawnienia administracyjne - dostęp do pełnej listy transakcji";
            string nonAdminWelcome = "Brak uprawnień administracyjnych - dostęp do listy własnych transakcji";

            Console.WriteLine($" {(isAdmin ? adminWelcome : nonAdminWelcome)} ");
            Dictionary<string, string> filterOptions = new()
            {
                { "[a]", "Wyświetl wszystkie (dostępne) transakcje" },
                { "[m]", "Wyświetl transakcje dla wybranego miesiąca" },
                { "[u]", "Wyświetl transakcje dla wybranego użytkownika" },
                { "[c]", "Wyświetl transakcje według kategorii" },
                { "[p]", "Wyświetl kwoty w przedziale" },
            };

            foreach (KeyValuePair<string, string> option in filterOptions)
            {
                Console.WriteLine($" {option.Key} - {option.Value}");
            }
        }
        public void ShowTransactions(Dictionary<int, Transaction> transactionsList, Dictionary<int, Category> categoriesList, User user)
        {
            // do implementacji (można uwzględnić uprawnienia - pole isAdmin)
            ShowFilterOptions(user.UserIsAdmin);

            // Krok 1. Wybór kryterium filtrowania (switch <-> klawisz wg filterOptions)
            // Krok 2. Wygenerowanie nowej listy transakcji, powstałej w rezultacie przefiltrowania głównej listy.
            // Krok 3. Wyświetlenie w konsoli tabeli zawierającej dane z przefiltrowanej listy transakcji.
            // Krok 4. Obsłużenie możliwości wyczyszczenia okna i zastosowania nowego kryterium filtrowania.
            // Krok 5. Obsłużenie możliwości powrotu do głównego menu lub sekcji modyfikacji transakcji.

         // Przykład filtrowania (do potencjalnego wykorzystania)
         //   Dictionary<string, int> dict = new Dictionary<string, int>() {
         //   {"A", 1}, {"B", 2}, {"C", 3}, {"D", 4}, {"E", 5}
         //   };

         //   Dictionary<string, int> filtered = dict.Where(x => x.Value % 2 == 0)
         //                       .ToDictionary(x => x.Key, x => x.Value);

         //   Console.WriteLine(String.Join(", ", filtered));
        }
        private void PrintMenuHeader(User user)
        {
            Console.WriteLine($"Witamy {user.UserFirstName} {user.UserLastName} w aplikacji budżetowej. Aby przejść dalej, wybierz opcję z listy poniżej:");

            foreach (KeyValuePair<string, string> option in _programOptions)
            {
                Console.WriteLine($" {option.Key} - {option.Value}");
            }
        }

        public void ManageProgramWorking()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Czy chcesz wyjść z programu? \n Naciśnij [t] - tak, [n] - nie");

            if (Console.ReadKey().Key == ConsoleKey.T)
            {
                Console.WriteLine("\n Dziękujemy za skorzystanie z aplikacji budżetowej");
                Budget.SaveTransactionList(_budget._budget, Budget.fileNames["Transactions"]);
                _isProgramOpen = !_isProgramOpen;
            }
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public void HandleMenu(Dictionary<int, User> usersList, Dictionary<int, Transaction> transactionsList, Dictionary<int, Category> categoriesList, User user)
        {
            if (user.UserIsActive)
            {
                do
                {
                    PrintMenuHeader(user);

                    ConsoleKeyInfo keyInfo = Console.ReadKey();

                    Console.Clear();

                    switch (keyInfo.Key)
                    {
                        case ConsoleKey.W:
                            ShowUsersList(usersList);
                            break;

                        case ConsoleKey.S:
                            AddTransactions(transactionsList, categoriesList, user);
                            break;

                        case ConsoleKey.A:
                            EditTransactions(transactionsList, categoriesList, user);
                            break;

                        case ConsoleKey.D:
                            ShowTransactions(transactionsList, categoriesList, user);
                            break;

                        case ConsoleKey.F:
                            ShowCategoriesList(categoriesList);
                            break;

                        case ConsoleKey.C:
                            _budget.CalculateBalance();
                            break;

                        default:
                            ManageProgramWorking();
                            break;
                    }

                } while (_isProgramOpen);
            } else
            {
                Console.WriteLine("Konto nieaktywne - brak uprawnień");
                ManageProgramWorking();
            }
                Console.ReadKey();
        }
    }
}
