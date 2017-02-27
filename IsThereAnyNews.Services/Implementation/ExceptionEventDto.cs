using System;

namespace IsThereAnyNews.Services.Implementation
{
    public class ExceptionEventDto
    {
        public long Id { get; set; }
        public DateTime Occured { get; set; }
        public Guid ErrorId { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public string Source { get; set; }
        public string Typeof { get; set; }
    }
}