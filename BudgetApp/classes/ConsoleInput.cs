using System;
using System.Collections.Generic;

namespace BudgetApp
{
    public class GetConsoleInput<T> where T : ITransactionObject
    {
        internal static int GetUserInputID(Dictionary<int, T> transactionObjectDictionary, bool chooseOnlyActive)
        {
            Console.WriteLine("Wpisz id które chcesz wybrać:");
            int returnID = -1;
            while (true)
            {
                string selectedID = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(selectedID) && !chooseOnlyActive)
                {
                    return -1;
                }
                if (selectedID.Equals("0") && !chooseOnlyActive) //zwracanie 0 ma tylko sens w metodach które mają chooseOnlyActive = false
                {
                    return 0;
                }
                if (int.TryParse(selectedID, out returnID))
                {
                    if (transactionObjectDictionary.ContainsKey(returnID) && (chooseOnlyActive ? transactionObjectDictionary[returnID].IsActive : true))
                    {
                        return returnID;
                    }
                    Console.WriteLine($"na liście nie istnieje podane id: {selectedID}");
                }
                else
                {
                    Console.WriteLine($"podana wartość {selectedID} jest niepoprawna, wpisz wartość numeryczną");
                }
            }
        }
    }
}