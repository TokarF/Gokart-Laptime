using Gokart_Laptime.Models;

namespace Gokart_Laptime.Services
{
    public interface IUserDAO
    {
        bool RegisteredEmail(string email);
        bool UsernameAlreadyInUse(string userName);
        void RegisterUser(UserRegisterViewModel userRegistrationViewModel);
        UserModel GetUserByEmail(string email);

    }
}
