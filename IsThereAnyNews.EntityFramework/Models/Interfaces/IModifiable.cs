using System;

namespace IsThereAnyNews.EntityFramework.Models.Interfaces
{
    public interface IModifiable
    {
        DateTime Updated { get; set; }
    }
}