namespace IsThereAnyNews.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class Extensions
    {
        public static T Random<T>(this List<T> collection)
        {
            var r = new Random(DateTime.Now.Millisecond);
            var element = collection.ElementAt(r.Next(collection.Count - 1));
            return element;
        }
    }
}