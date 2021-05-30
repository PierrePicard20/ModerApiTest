using Microsoft.Extensions.Configuration;
using ModerApiTest.Authentication;
using ModerApiTest.DAL.Collections;
using ModerApiTest.DAL.Services;
using ModerApiTest.Models;
using ModerApiTest.Models.Responses;
using ModerApiTest.Utils;
using System;

namespace ModerApiTest.Managers
{
    public class AuthManager : IAuthManager
    {
        private ICollectionService<UserDocument> _userService;
        private IConfiguration _config;

        /// <summary>
        /// Constructor called only by the framework
        /// </summary>
        /// <param name="userService">built by dependency injection</param>
        /// <param name="config">built by dependency injection</param>
        public AuthManager(ICollectionService<UserDocument> userService, IConfiguration config)
        {
            _userService = userService;
            _config = config;
        }

        /// <summary>
        /// Login perform a login by returning a JWT token information
        /// </summary>
        /// <param name="login">the login informations</param>
        /// <returns>the token and its expiration date</returns>
        public (string,DateTime)? Login(LoginModel login)
        {
            var userDocument = _userService.FirstOrDefault(_ => _.Email == login.email);
            if (userDocument != null && BCrypt.Net.BCrypt.Verify(login.password, userDocument.Password))
            {
                return JWTAuthenticationStrategy.GenerateToken(login, userDocument.UserId, _config);
            }
            return null;
        }

        /// <summary>
        /// GetLoggedInResponse build the response when a login succeded
        /// </summary>
        /// <param name="jwt">the token and its expiration date</param>
        /// <returns>the response for the client</returns>
        public LoginResponseModel GetLoggedInResponse((string,DateTime) jwt)
        {
            return new LoginResponseModel( message: "User logged in"
                                         , token: jwt.Item1
                                         , expirationUtcDate: jwt.Item2);
        }

        /// <summary>
        /// Logout performs logout by black listing the token
        /// </summary>
        /// <param name="jwtToken">the token to be black listed</param>
        public void Logout(string jwtToken)
        {
            JWTTokenBlackList.Instance.AddToBlackList(jwtToken);
        }

        /// <summary>
        /// GetRegisteredUser check user registration
        /// </summary>
        /// <param name="user">the user informations</param>
        /// <returns>the user document when it exists, null otherwise</returns>
        public UserDocument GetRegisteredUser(UserModel user)
        {
            return _userService.FirstOrDefault(_ => _.Email == user.email);
        }

        /// <summary>
        /// GetAlreadyRegisteredResponse build the response when a user is already registered
        /// </summary>
        /// <param name="existingUserId">the id of the already registered user</param>
        /// <param name="user">the user informations</param>
        /// <returns>the response</returns>
        public RegisterResponseModel GetAlreadyRegisteredResponse(string existingUserId, UserModel user)
        {
            return new RegisterResponseModel( message: "User already registered"
                                            , id: existingUserId
                                            , user: user);
        }

        /// <summary>
        /// Register actually performs the registration work
        /// </summary>
        /// <param name="user">the user informations</param>
        /// <returns></returns>
        public RegisterResponseModel Register(UserModel user)
        {
            // save to the DB
            var userDocument = user.ToDocument();
            _userService.Add(userDocument);

            // build the response
            return new RegisterResponseModel( message: "User registered"
                                            , id: string.Empty
                                            , user: user);
        }
    }
}
