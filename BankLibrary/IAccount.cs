namespace BankLibrary
{
    interface IAccount
    {
        // put the money to account
        void Put(decimal sum);

        // take money from the account 
        decimal Take_money(decimal sum);
    }
}