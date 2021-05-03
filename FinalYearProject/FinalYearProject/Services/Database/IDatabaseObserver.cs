namespace FinalYearProject.Services.Database
{
    public interface IDatabaseObserver
    {
        bool IsObserving { get; }

        void BeginObserving(string documentId);
        void StopObserving();
    }
}
