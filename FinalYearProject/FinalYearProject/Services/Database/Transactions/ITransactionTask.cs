namespace FinalYearProject.Services.Database.Transactions
{
    public interface ITransactionTask
    {
        object Invoke(object[] parameters);
    }
}