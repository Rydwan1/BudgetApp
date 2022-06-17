using System;
using System.Collections.Generic;

namespace BudgetApp
{
    public class Menu : Budget, IMenu
    {
        private static bool _isProgramOpen = true;

        private static readonly Dictionary<string, string> _programOptions = new()
        {
            { "[w]", "Wyświetl listę domowników" },
            { "[d]", "Wyświetl transakcje" },
            { "[f]", "Wyświetl listę kategorii" },
            { "[c]", "Wyświetl transakcje wg kategorii" },
            { "[u]", "Wyświetl transakcje wg użytkownika" },
            { "[s]", "Wyświetl podsumowanie" }
        };

        public bool IsProgramOpen { get => _isProgramOpen; set => _isProgramOpen = value; }
        public Dictionary<string, string> ProgramOptions { get => _programOptions; }

        private static void PrintMenuHeader()
        {
            Console.Clear();
            Console.WriteLine("Witamy w aplikacji budżetowej. Aby przejść dalej, wybierz opcję z listy poniżej:");

            foreach (KeyValuePair<string, string> option in _programOptions)
            {
                Console.WriteLine($" {option.Key} - {option.Value}");
            }
        }

        public static void ManageProgramWorking()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Czy chcesz wyjść z programu? \n Naciśnij [t] - tak, [n] - nie");

            if (Console.ReadKey().Key == ConsoleKey.T)
            {
                Console.WriteLine("\n Dziękujemy za skorzystanie z aplikacji budżetowej");
                SaveTransactionList(transactionsList, fileNames["Transactions"]);
                SaveCategoryList(categoriesList, fileNames["Categories"]);
                SaveUserList(usersList, fileNames["Users"]);
                _isProgramOpen = !_isProgramOpen;
            }
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public void HandleMenu()
        {
            do
            {
                PrintMenuHeader();

                ConsoleKeyInfo keyInfo = Console.ReadKey();

                Console.Clear();

                switch (keyInfo.Key)
                {
                    case ConsoleKey.W:
                        User.ManageUsers(usersList);
                        break;

                    case ConsoleKey.D:
                        Transaction.ManageTransactions(transactionsList, categoriesList, usersList);
                        break;

                    case ConsoleKey.F:
                        Category.ManageCategories(categoriesList);
                        break;

                    case ConsoleKey.U:
                        User.PrintUsers(false, usersList);
                        int selectedUserID = GetConsoleInput<User>.GetUserInputID(usersList, false);
                        if (selectedUserID == -1)
                            break;
                        Transaction.GetTransactionByUser(selectedUserID, transactionsList, categoriesList, usersList);
                        break;

                    case ConsoleKey.C:
                        Category.PrintCategories(false, categoriesList);
                        int selectedConsoleID = GetConsoleInput<Category>.GetUserInputID(categoriesList, false);
                        if (selectedConsoleID == -1)
                            break;
                        Transaction.GetTransactionByCategory(selectedConsoleID, transactionsList, categoriesList, usersList);
                        break;

                    case ConsoleKey.S:
                        EstablishBudgetStructure();
                        break;

                    default:
                        ManageProgramWorking();
                        break;
                }
            } while (_isProgramOpen);
            Console.ReadKey();
        }
    }
}