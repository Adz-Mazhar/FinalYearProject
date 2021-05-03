using FinalYearProject.Extensions;
using FinalYearProject.Services.Database.Transactions;
using Plugin.CloudFirestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalYearProject.Services.Database
{
    public abstract class BaseDBService
    {
        private readonly Dictionary<string, ITransactionTask> transactionTasks = new();

        protected IFirestore Firestore => CrossCloudFirestore.Current.Instance;

        protected async Task<IDocumentReference> AddAsync<T>(T newItem,
                                                             string documentId = null,
                                                             ICollectionReference collectionReference = null)
        {
            collectionReference ??= GetBaseCollectionReference<T>();

            if (documentId is null)
            {
                return await collectionReference.AddAsync(newItem);
            }
            else
            {
                await collectionReference.Document(documentId)
                                         .SetAsync(newItem);
                return null;
            }
        }

        protected async Task DeleteAsync<T>(string documentId,
                                                    ICollectionReference collectionReference = null)
        {
            collectionReference ??= GetBaseCollectionReference<T>();

            await collectionReference.Document(documentId)
                                     .DeleteAsync();
        }

        //protected async Task<IList<T>> GetAllAsync<T>(ICollectionReference collectionReference = null)
        //{
        //    collectionReference ??= GetBaseCollectionReference<T>();

        //    IQuerySnapshot collection = await collectionReference.GetAsync();
        //    return collection.ToObjects<T>().ToList();
        //}

        protected async Task<IList<T>> GetAllAsync<T>(IQuery query = null)
        {
            query ??= GetBaseCollectionReference<T>();

            var querySnapshot = await query.GetAsync();
            return querySnapshot.ToObjects<T>().ToList();
        }

        protected async Task<T> GetAsync<T>(string id,
                                            ICollectionReference collectionReference = null)
        {
            collectionReference ??= GetBaseCollectionReference<T>();

            return await GetAsync<T>(collectionReference.Document(id));
        }

        protected async Task<T> GetAsync<T>(IDocumentReference documentReference)
        {
            var document = await documentReference.GetAsync();
            return document.ToObject<T>();
        }

        protected ICollectionReference GetBaseCollectionReference<T>()
        {
            string collectionName = typeof(T).Name + "s";
            return GetCollectionReference(collectionName);
        }

        protected ICollectionReference GetCollectionReference(string collectionPath)
        {
            return Firestore.Collection(collectionPath);
        }

        protected async Task<IList<T>> GetMultipleAsync<T>(IEnumerable<string> ids,
                                                           IQuery query = null)
        {
            var items = new List<T>();

            if (ids.Count() is 0)
                return items;

            query ??= GetBaseCollectionReference<T>();

            foreach (var idBatch in ids.Split(10))
            {
                IQuerySnapshot querySnapshot = await query.WhereIn(FieldPath.DocumentId, idBatch).GetAsync();
                items.AddRange(querySnapshot.ToObjects<T>());
            }

            return items;
        }

        protected async Task OverwriteAsync<T>(string id,
                                               T item,
                                               ICollectionReference collectionReference = null)
        {
            collectionReference ??= GetBaseCollectionReference<T>();

            await collectionReference.Document(id)
                                     .UpdateAsync(item);
        }

        protected void RegisterMultipleTransactionTasks(List<(string, ITransactionTask)> tasksAndKeys)
        {
            foreach (var item in tasksAndKeys)
            {
                RegisterTransactionTask(item.Item1, item.Item2);
            }
        }

        protected void RegisterTransactionTask(string key, ITransactionTask task)
        {
            if (key is null || task is null)
            {
                return;
            }

            transactionTasks.Add(key, task);
        }

        // When invoking the transaction task, the first parameter passed in will always be the object 
        // from the database of type TDocument
        protected async Task RunTransactionAsync<TDocument>(IDocumentReference documentReference,
                                                            string key,
                                                            object[] parameters)
        {
            transactionTasks.TryGetValue(key, out ITransactionTask task);
            task.ThrowIfNull(nameof(key), "Key does not correspond to a transaction task.");

            await Firestore.RunTransactionAsync(transaction =>
            {
                var snapshot = transaction.Get(documentReference);

                if (!snapshot.Exists)
                {
                    throw new DatabaseException(DatabaseErrorType.NotFound);
                }

                var obj = snapshot.ToObject<TDocument>();
                obj.ThrowIfNull(nameof(documentReference), "Document type is not of type 'TDocument'");

                if (parameters is null)
                {
                    parameters = new object[] { obj };
                }
                else
                {
                    var list = parameters.ToList();
                    list.Insert(0, obj);
                    parameters = list.ToArray();
                }

                task.Invoke(parameters);

                transaction.Update(documentReference, obj);
            });
        }

        // When invoking the transaction task, the first parameter passed in will always be the object 
        // from the database of type TDocument
        protected async Task<TOut> RunTransactionAsync<TDocument, TOut>(IDocumentReference documentReference,
                                                                        string key,
                                                                        object[] parameters)
        {
            transactionTasks.TryGetValue(key, out ITransactionTask task);
            task.ThrowIfNull(nameof(key), "Key does not correspond to a transaction task.");

            TOut returnObj = await Firestore.RunTransactionAsync(transaction =>
            {
                var snapshot = transaction.Get(documentReference);

                if (!snapshot.Exists)
                {
                    throw new DatabaseException(DatabaseErrorType.NotFound);
                }

                var obj = snapshot.ToObject<TDocument>();
                obj.ThrowIfNull(nameof(documentReference), "Document type is not of type 'TDocument'");

                if (parameters is null)
                {
                    parameters = new object[] { obj };
                }
                else
                {
                    var list = parameters.ToList();
                    list.Insert(0, obj);
                    parameters = list.ToArray();
                }

                object returnObj = task.Invoke(parameters);

                TOut @out = returnObj is TOut out1 ? out1 : throw new ArgumentException(nameof(TOut), "Transaction task return type does not match TOut");

                transaction.Update(documentReference, obj);

                return @out;
            });

            return returnObj;
        }

        protected async Task UpdateAsync<T>(string id,
                                            string fieldName,
                                            object value,
                                            ICollectionReference collectionReference = null)
        {
            collectionReference ??= GetBaseCollectionReference<T>();

            await collectionReference.Document(id)
                                     .UpdateAsync(fieldName, value);
        }
    }
}