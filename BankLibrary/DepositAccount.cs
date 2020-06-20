namespace BankLibrary
{
    public class DepositAccount : Account
    {
        public DepositAccount(decimal sum, int percentage)
            : base(sum, percentage)
        {
        }

        // Override the method Open() for DepositAccount
        protected internal override void Open()
        {
            base.OnOpened(new AccountEventArgs($"Відкрито новий депозитний рахунок! Id рахунку: {this.Id}", this.Sum));
        }

        // Override the method Put() for DepositAccount
        public override void Put(decimal sum)
        {
            if (_days % 30 == 0)
                base.Put(sum);
            else
                base.OnAdded(new AccountEventArgs("На рахунок можна покласти гроші тільки після 30-ти денного періоду!", 0));
        }

        // Override the method Take_money() for DepositAccount
        public override decimal Take_money(decimal sum)
        {
            if (_days % 30 == 0)
                return base.Take_money(sum);
            else
                base.OnTake(new AccountEventArgs("Вивести кошти з рахунку можна тільки після 30-ти денного періоду!", 0));
            return 0;
        }

        // Override the method Calculate() for DepositAccount
        protected internal override void Calculate()
        {
            if (_days % 30 == 0)
                base.Calculate();
        }
    }
}