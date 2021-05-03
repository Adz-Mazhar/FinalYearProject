using FinalYearProject.Extensions;
using Plugin.FirebaseAuth;
using System;
using System.Threading.Tasks;

namespace FinalYearProject.Services.Authentication
{
    public class AuthService : IAuthService
    {
        public async Task ChangePasswordAsync(string newPassword)
        {
            try
            {
                await CrossFirebaseAuth
                    .Current
                    .Instance
                    .CurrentUser
                    ?.UpdatePasswordAsync(newPassword);
            }
            catch (FirebaseAuthException e) when (e.ErrorType == ErrorType.WeakPassword)
            {
                throw new AuthException(AuthErrorType.WeakPassword);
            }
        }

        public string GetUserId()
        {
            return CrossFirebaseAuth.Current.Instance.CurrentUser?.Uid;
        }

        public bool IsSignedIn()
        {
            return CrossFirebaseAuth.Current.Instance.CurrentUser is not null;
        }

        public async Task<string> SignInAsync(string email, string password)
        {
            email.ThrowIfNull(nameof(email));
            password.ThrowIfNull(nameof(password));

            try
            {
                var result = await CrossFirebaseAuth
                    .Current
                    .Instance
                    .SignInWithEmailAndPasswordAsync(email, password);

                return result.User.Uid;
            }
            catch (FirebaseAuthException e) when (e.ErrorType is ErrorType.InvalidUser)
            {
                throw new AuthException(AuthErrorType.NotRegistered);
            }
            catch (FirebaseAuthException e) when (e.ErrorType is ErrorType.InvalidCredentials)
            {
                return e.ErrorCode switch
                {
                    "ERROR_INVALID_EMAIL" => throw new AuthException(AuthErrorType.InvalidEmail),
                    "ERROR_WRONG_PASSWORD" => throw new AuthException(AuthErrorType.WrongPassword),
                    _ => null,
                };
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool SignOut()
        {
            try
            {
                CrossFirebaseAuth.Current.Instance.SignOut();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<string> SignUpAsync(string email, string password)
        {
            email.ThrowIfNull(nameof(email));
            password.ThrowIfNull(nameof(password));

            try
            {
                var result = await CrossFirebaseAuth.Current
                                                    .Instance
                                                    .CreateUserWithEmailAndPasswordAsync(email, password);

                return result.User.Uid;
            }
            catch (FirebaseAuthException e) when (e.ErrorType == ErrorType.InvalidCredentials)
            {
                throw new AuthException(AuthErrorType.InvalidEmail);
            }
            catch (FirebaseAuthException e) when (e.ErrorType == ErrorType.UserCollision)
            {
                throw new AuthException(AuthErrorType.UserCollision);
            }
            catch (FirebaseAuthException e) when (e.ErrorType == ErrorType.WeakPassword)
            {
                throw new AuthException(AuthErrorType.WeakPassword);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}