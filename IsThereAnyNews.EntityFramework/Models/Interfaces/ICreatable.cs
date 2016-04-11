using System;

namespace IsThereAnyNews.EntityFramework.Models.Interfaces
{
    public interface ICreatable
    {
        DateTime Created { get; set; }
    }
}