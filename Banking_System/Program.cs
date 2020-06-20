using System;
using System.Text;
using BankLibrary;

namespace Banking_System
{
    class Program
    {
        static void Main(string[] args)
        {
            // Implementation of ensuring the correct output of the Cyrillic alphabet in the console
            Console.OutputEncoding = Encoding.Default;

            Bank<Account> bank = new Bank<Account>("MyBank");

            Console.WriteLine();
            ConsoleColor color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Вас вітає банк {bank.Name}");
            Console.WriteLine();
            Console.WriteLine("Виберіть, будь ласка, один із пунктів нижче");
            Console.WriteLine();
            Console.ForegroundColor = color;

            bool alive = true;
            while (alive)
            {
                ConsoleColor color1 = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.DarkGreen; // display the list of commands with green color
                Console.WriteLine("1. Відкрити рахунок \t 2. Вивести кошти  \t 3. Додати рахунок");
                Console.WriteLine("4. закрити рахунок \t 5. пропустити день  \t 6. Вийти з програми");
                Console.WriteLine("Введіть номер пункту:");
                Console.ForegroundColor = color1;

                try
                {
                    int command = Convert.ToInt32(Console.ReadLine());

                    switch (command)
                    {
                        case 1:
                            OpenAccount(bank);
                            break;
                        case 2:
                            Take_money(bank);
                            break;
                        case 3:
                            Put(bank);
                            break;
                        case 4:
                            CloseAccount(bank);
                            break;
                        case 5:
                            IncrementDays(bank);
                            break;
                        case 6:
                            alive = false;
                            continue;
                    }
                    bank.CalculatePercentage();
                }
                catch (Exception ex)
                {
                    // display the message about error with red color
                    color = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex.Message);
                    Console.ForegroundColor = color;
                }
            }
        }

        // the event handlers for the class Account
        #region Events

        // event handler to opened an account
        private static void OpenAccountHandler(object sender, AccountEventArgs e)
        {
            Console.WriteLine(e.Message);
        }

        // event handler to put the money to account
        private static void AddSumHandler(object sender, AccountEventArgs e)
        {
            Console.WriteLine(e.Message);
        }

        // event handler to take the money from account
        private static void TakeMoneyHandler(object sender, AccountEventArgs e)
        {
            Console.WriteLine(e.Message);
            if (e.Sum > 0)
                Console.WriteLine("Йдемо витрачати гроші!");
        }

        // event handler to close the account
        private static void CloseAccountHandler(object sender, AccountEventArgs e)
        {
            Console.WriteLine(e.Message);
        }

        #endregion

        #region Methods

        // method to open the account
        private static void OpenAccount(Bank<Account> bank)
        {
            Console.WriteLine("Вкажіть суму для створення рахунку:");

            decimal sum = Convert.ToDecimal(Console.ReadLine());
            Console.WriteLine("Виберіть тип рахунку: 1. Дебетовий 2. Депозит");
            AccountType accountType;

            int type = Convert.ToInt32(Console.ReadLine());

            if (type == 2)
                accountType = AccountType.Deposit;
            else
                accountType = AccountType.Ordinary;

            bank.Open(accountType,
                sum,
                AddSumHandler,                          // event handler for put the money to account
                TakeMoneyHandler,                       // event handler to take the money from account
                (o, e) => Console.WriteLine(e.Message), // event handler to charges the percentage for accounts in the form of lambda expression
                CloseAccountHandler,                    // event handler to open the account
                OpenAccountHandler);                    // event handler to close the account
        }

        // method to take money from account
        private static void Take_money(Bank<Account> bank)
        {
            Console.WriteLine("Вкажіть суму для виводу із рахунку:");

            decimal sum = Convert.ToDecimal(Console.ReadLine());
            Console.WriteLine("Введіть Id рахунку:");
            int id = Convert.ToInt32(Console.ReadLine());

            bank.Take_money(sum, id);
        }

        // method for put money to account
        private static void Put(Bank<Account> bank)
        {
            Console.WriteLine("Вкажіть суму, яку хочете покласти на рахунок:");

            decimal sum = Convert.ToDecimal(Console.ReadLine());
            Console.WriteLine("Введіть Id рахунку:");
            int id = Convert.ToInt32(Console.ReadLine());
            bank.Put(sum, id);
        }

        // method to close the account
        private static void CloseAccount(Bank<Account> bank)
        {
            Console.WriteLine("Введіть Id рахунку, який треба закрити:");
            int id = Convert.ToInt32(Console.ReadLine());
            bank.Close(id);
        }

        // increment days
        private static void IncrementDays(Bank<Account> bank)
        {
            Console.WriteLine("Пропускаємо 1 день....");
            bank.IncrementDays();
            Console.WriteLine("Ви успішно пропустили 1 день!");
        }

        #endregion
    }
}