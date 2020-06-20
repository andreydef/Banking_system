namespace BankLibrary
{
    public abstract class Account : IAccount
    {
        // Event that occurs when take the money
        protected internal event AccountStateHandler Take;

        // Event that occurs when put the money
        protected internal event AccountStateHandler Added;

        // Event that occurs when open  an account 
        protected internal event AccountStateHandler Opened;

        // Event that occurs when close an account 
        protected internal event AccountStateHandler Closed;

        // Event that occurs when adding a percent
        protected internal event AccountStateHandler Calculated;

        static int counter = 0;
        protected int _days = 0; // time from the moment of account opening

        public Account(decimal sum, int percentage)
        {
            Sum = sum;
            Percentage = percentage;
            Id = ++counter;
        }

        // Total sum on account 
        public decimal Sum { get; private set; }

        // Percentage of charges
        public int Percentage { get; private set; }

        // Unique id of account 
        public int Id { get; private set; }

        // Event call
        private void CallEvent(AccountEventArgs e, AccountStateHandler handler)
        {
            if (e != null)
                handler?.Invoke(this, e);
        }

        // Call individual events
        protected virtual void OnOpened(AccountEventArgs e)
        {
            CallEvent(e, Opened);
        }

        protected virtual void OnTake(AccountEventArgs e)
        {
            CallEvent(e, Take);
        }

        protected virtual void OnAdded(AccountEventArgs e)
        {
            CallEvent(e, Added);
        }

        protected virtual void OnClosed(AccountEventArgs e)
        {
            CallEvent(e, Closed);
        }

        protected virtual void OnCalculated(AccountEventArgs e)
        {
            CallEvent(e, Calculated);
        }

        // method of putting a money from account
        public virtual void Put(decimal sum)
        {
            Sum += sum;
            OnAdded(new AccountEventArgs("На рахунок було додано кошти: " + sum, sum)); 
        }

        // method of taking the money from account, return how much is taken from account
        public virtual decimal Take_money(decimal sum)
        {
            decimal result = 0;

            if (Sum >= sum)
            {
                Sum -= sum;
                result = sum;
                OnTake(new AccountEventArgs($"Сума {sum} була знята з рахунку {Id} ", sum));
            }
            else
            {
                OnTake(new AccountEventArgs($"Недостатньо грошей на рахунку {Id} ", 0));
            }
            return result;
        }

        // method of opening the account
        protected internal virtual void Open()
        {
            OnOpened(new AccountEventArgs($"Відкрито новий рахунок! Id рахунку: {Id}", Sum));
        }

        // method of closing the account
        protected internal virtual void Close()
        {
            OnClosed(new AccountEventArgs($"Рахунок {Id} було закрито. Підсумкова сума: {Sum}", Sum));
        }

        // increment the days 
        protected internal void IncrementDays()
        {
            _days++;
        }

        // method for charges of percentage
        protected internal virtual void Calculate()
        {
            decimal increment = Sum * Percentage / 100;
            Sum = Sum + increment;
            OnCalculated(new AccountEventArgs($"Нараховано відсотки в розмірі: {increment}", increment));
        }
    }
}