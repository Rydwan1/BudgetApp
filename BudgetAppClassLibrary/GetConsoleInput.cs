using System;

namespace BudgetAppClassLibrary
{
    public class GetConsoleInput
    {
        public static DateTimeOffset ChooseDateOfTransaction()
        {
            Console.WriteLine("Jeśli transakcja jest z dzisiaj zostaw puste pole, w innym wypadku wprowadź datę w formacie DD-MM-RRRR");
            while (true)
            {
                string consoleInput = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(consoleInput))
                {
                    return DateTimeOffset.Now;
                }
                DateTimeOffset returnDate = DateTimeOffset.MinValue;
                if (DateTimeOffset.TryParseExact(consoleInput.Trim(), "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out returnDate))
                {
                    return returnDate;
                }
                Console.WriteLine($"Nieprawidłowy format daty! ma być w formacie DD-MM-RRRR, przykład - dzisiaj jest {DateTimeOffset.Now.ToString("dd-MM-yyyy")}");
            }
        }

        public static double UserInputTransactionAmmount(bool allowEmpty)
        {
            Console.WriteLine("Wprowadź kwotę PLN");
            double transactionAmmount = -1;
            while (true)
            {
                string consoleInput = Console.ReadLine();
                if (allowEmpty && string.IsNullOrWhiteSpace(consoleInput))
                {
                    return -1;
                }
                if (double.TryParse(consoleInput, out transactionAmmount))
                {
                    if (transactionAmmount >= 0)
                    {
                        return transactionAmmount;
                    }
                    Console.WriteLine("transakcja nie może być ujemna, jeśli chcesz odjąć wybierz kategorię wydatek");
                }
                Console.WriteLine("w tym miejscu wpisujemy wyłącznie liczbę");
            }
        }
    }

}
