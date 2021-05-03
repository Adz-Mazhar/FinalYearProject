using System;

namespace FinalYearProject.Services.Database.Transactions
{
    class TransactionTaskWithReturn<TDocument, TOut> : ITransactionTask
    {
        public Func<TDocument, TOut> Function { get; set; }

        public object Invoke(object[] parameters)
        {
            if (parameters.Length != 1
                || parameters[0] is not TDocument)
            {
                return null;
            }

            TOut returnObj = Function is null ? default : Function.Invoke((TDocument)parameters[0]);
            return returnObj;
        }
    }

    class TransactionTaskWithReturn<TDocument, T1, TOut> : ITransactionTask
    {
        public Func<TDocument, T1, TOut> Function { get; set; }

        public object Invoke(object[] parameters)
        {
            if (parameters.Length != 2
                || parameters[0] is not TDocument
                || parameters[1] is not T1)
            {
                return null;
            }

            TOut returnObj = Function is null ? default : Function.Invoke((TDocument)parameters[0], (T1)parameters[1]);
            return returnObj;
        }
    }
}
