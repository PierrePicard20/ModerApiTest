using ModerApiTest.DAL.Collections;
using ModerApiTest.Models;
using ModerApiTest.Models.Responses;
using System;

namespace ModerApiTest.Managers
{
    public interface IAuthManager
    {
        (string, DateTime)? Login(LoginModel login);

        LoginResponseModel GetLoggedInResponse((string, DateTime) jwt);

        void Logout(string jwtToken);

        UserDocument GetRegisteredUser(UserModel user);

        RegisterResponseModel GetAlreadyRegisteredResponse(string existingUserId, UserModel user);

        RegisterResponseModel Register(UserModel user);
    }
}
