namespace IsThereAnyNews.Dtos
{
    using System.ComponentModel.DataAnnotations;

    public class AddChannelDto
    {
        [Required]
        public string RssChannelName { get; set; }

        [Required]
        public string RssChannelLink { get; set; }
    }
}