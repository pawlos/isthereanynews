namespace IsThereAnyNews.ProjectionModels
{
    using System;

    using IsThereAnyNews.SharedData;

    public class ApplicationConfigurationDTO
    {
        public DateTime Created { get; set; }
        public long Id { get; set; }
        public RegistrationSupported RegistrationSupported { get; set; }
        public DateTime Updated { get; set; }
        public long UsersLimit { get; set; }
    }
}
