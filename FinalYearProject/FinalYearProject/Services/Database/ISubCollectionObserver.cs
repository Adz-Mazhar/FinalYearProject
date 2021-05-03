using System.Collections.Generic;

namespace FinalYearProject.Services.Database
{
    public interface ISubCollectionObserver<T> : IDatabaseObserver
    {
        List<T> Collection { get; }
    }
}
