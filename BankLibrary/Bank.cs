using System;

namespace BankLibrary
{
    // the type of account
    public enum AccountType
    {
        Ordinary,
        Deposit
    }

    public class Bank<T> where T : Account
    {
        T[] accounts;

        public string Name { get; private set; }

        public Bank(string name)
        {
            this.Name = name;
        }

        // the method for creating of account
        public void Open(AccountType accountType, decimal sum, AccountStateHandler addSumHandler,
            AccountStateHandler takeSumHandler, AccountStateHandler calculationHandler,
            AccountStateHandler closeAcccountHandler, AccountStateHandler openAccountHandler
            )
        {
            T newAccount = null;

            switch (accountType)
            {
                case AccountType.Ordinary:
                    newAccount = new DemandAccount(sum, 1) as T;
                    break;
                case AccountType.Deposit:
                    newAccount = new DepositAccount(sum, 40) as T;
                    break;
            }

            if (newAccount == null)
                throw new Exception("Помилка створення рахунку");

            // add the new account to massif of accounts
            if (accounts == null)
                accounts = new T[] { newAccount };
            else
            {
                T[] tempAccounts = new T[accounts.Length + 1];
                for (int i = 0; i < accounts.Length; i++)
                    tempAccounts[i] = accounts[i];
                tempAccounts[tempAccounts.Length - 1] = newAccount;
                accounts = tempAccounts;
            }

            // setting up an account event handler
            newAccount.Added += addSumHandler;
            newAccount.Take += takeSumHandler;
            newAccount.Closed += closeAcccountHandler;
            newAccount.Opened += openAccountHandler;
            newAccount.Calculated += calculationHandler;

            newAccount.Open();
        }

        // search the account for id
        public T FindAccount(int id)
        {
            for (int i = 0; i < accounts.Length; i++)
            {
                if (accounts[i].Id == id)
                    return accounts[i];
            }
            return null;
        }

        // rebooted version for search the account for id
        public T FindAccount(int id, out int index)
        {
            for (int i = 0; i < accounts.Length; i++)
            {
                if (accounts[i].Id == id)
                {
                    index = i;
                    return accounts[i];
                }
            }
            index = -1;
            return null;
        }

        // put the money to account
        public void Put(decimal sum, int id)
        {
            T account = FindAccount(id);
            if (account == null)
            {
                Console.WriteLine("Будь ласка, спочатку створіть рахунок!");
                return;
            }
            account.Put(sum);
        }

        // take the money to account
        public void Take_money(decimal sum, int id)
        {
            T account = FindAccount(id);
            if (account == null)
                throw new Exception("Рахунок не знайдено!");
            account.Take_money(sum);
        }

        // close the account
        public void Close(int id)
        {
            int index;
            T account = FindAccount(id, out index);
            if (account == null)
                throw new Exception("Рахунок не знайдено!");

            account.Close();

            if (accounts.Length <= 1)
                accounts = null;
            else
            {
                // reduce the array of accounts, deleting from it a closed account
                T[] tempAccounts = new T[accounts.Length - 1];
                for (int i = 0, j = 0; i < accounts.Length; i++)
                {
                    if (i != index)
                        tempAccounts[j++] = accounts[i];
                }
                accounts = tempAccounts;
            }

        }

        // charges the percentage for accounts
        public void CalculatePercentage()
        {
            if (accounts == null) // if massif don't created, return from method
                return;
            for (int i = 0; i < accounts.Length; i++)
            {
                accounts[i].IncrementDays();
                accounts[i].Calculate();
            }
        }

        // increment days
        public void IncrementDays()
        {
            if (accounts == null) // if massif don't created, return from method
                return;
            for (int i = 0; i < accounts.Length; i++)
            {
                accounts[i].IncrementDays();
            }
        }
    }
}