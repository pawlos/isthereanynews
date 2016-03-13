using System;

namespace IsThereAnyNews.EntityFramework.Models
{
    public interface IModel
    {
        long Id { get; set; }
        DateTime Created { get; set; }
        DateTime Updated { get; set; }
    }
}