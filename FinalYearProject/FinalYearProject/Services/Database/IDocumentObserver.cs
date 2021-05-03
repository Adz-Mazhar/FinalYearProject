namespace FinalYearProject.Services.Database
{
    public interface IDocumentObserver<T> : IDatabaseObserver
    {
        T Document { get; }
    }
}
