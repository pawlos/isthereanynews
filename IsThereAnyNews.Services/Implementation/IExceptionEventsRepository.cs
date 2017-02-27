using System.Collections.Generic;

namespace IsThereAnyNews.Services.Implementation
{
    public interface IExceptionEventsRepository
    {
        List<ExceptionEventDto> LoadLatest(int eventCount);
    }
}