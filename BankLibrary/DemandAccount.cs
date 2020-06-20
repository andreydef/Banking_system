namespace BankLibrary
{
    public class DemandAccount : Account
    {
        public DemandAccount(decimal sum, int percentage)
            : base(sum, percentage)
        {
        }

        // Override the method Open() for DemandAccount
        protected internal override void Open()
        {
            base.OnOpened(new AccountEventArgs($"Відкрито новий дебетовий рахунок! Id рахунка: {this.Id} ", this.Sum));
        }
    }
}