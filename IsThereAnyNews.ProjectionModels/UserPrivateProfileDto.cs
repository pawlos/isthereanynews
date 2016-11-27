namespace IsThereAnyNews.ProjectionModels
{
    using System;
    using System.Collections.Generic;

    public class UserPrivateProfileDto
    {
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public DateTime Registered { get; set; }
        public List<SocialLoginDTO> SocialLogins { get; set; }
    }
}