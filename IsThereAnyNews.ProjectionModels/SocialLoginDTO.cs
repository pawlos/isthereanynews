namespace IsThereAnyNews.ProjectionModels
{
    using System;

    using IsThereAnyNews.SharedData;

    public class SocialLoginDTO
    {
        public AuthenticationTypeProvider AuthenticationTypeProvider { get; set; }

        public DateTime Registered { get; set; }
    }
}