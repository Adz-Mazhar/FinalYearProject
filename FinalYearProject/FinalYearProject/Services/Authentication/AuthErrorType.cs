namespace FinalYearProject.Services.Authentication
{
    public enum AuthErrorType
    {
        None,
        NotRegistered,
        InvalidEmail,
        UserCollision,
        WeakPassword,
        WrongPassword,
    }
}