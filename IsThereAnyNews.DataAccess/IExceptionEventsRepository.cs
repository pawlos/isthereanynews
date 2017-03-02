namespace IsThereAnyNews.DataAccess
{
    using System.Collections.Generic;
    using Dtos;

    public interface IExceptionEventsRepository
    {
        List<ExceptionEventDto> LoadLatest(int eventCount);
    }
}