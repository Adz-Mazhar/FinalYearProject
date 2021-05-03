using System;

namespace FinalYearProject.Services.Database.Transactions
{
    public class TransactionTask<TDocument> : ITransactionTask
    {
        public Action<TDocument> Action { get; set; }

        public object Invoke(object[] parameters)
        {
            if (parameters.Length != 1
                || parameters[0] is not TDocument)
            {
                throw new ArgumentException("Invalid parameters to transaction task.", nameof(parameters));
            }

            Action?.Invoke((TDocument)parameters[0]);
            return null;
        }
    }

    public class TransactionTask<TDocument, T1> : ITransactionTask
    {
        public Action<TDocument, T1> Action { get; set; }

        public object Invoke(object[] parameters)
        {
            if (parameters.Length != 2
                || parameters[0] is not TDocument
                || parameters[1] is not T1)
            {
                throw new ArgumentException("Invalid parameters to transaction task.", nameof(parameters));
            }

            Action?.Invoke((TDocument)parameters[0], (T1)parameters[1]);
            return null;
        }
    }
}
