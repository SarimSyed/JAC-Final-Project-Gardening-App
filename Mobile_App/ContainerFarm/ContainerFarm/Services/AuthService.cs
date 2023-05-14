using ContainerFarm.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Auth.Providers;

namespace ContainerFarm.Services
{
    public static class AuthService
    {
        private static FirebaseAuthConfig config = new FirebaseAuthConfig
        {
            ApiKey = ResourceStrings.Firebase_ApiKey,
            AuthDomain = ResourceStrings.AuthorizedDomain,
            Providers = new FirebaseAuthProvider[]
    {
                new EmailProvider()
    },
        };

        public static FirebaseAuthClient Client { get; } = new FirebaseAuthClient(config);
        public static UserCredential UserCreds { get; set; }
    }
}
