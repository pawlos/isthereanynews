using System;
using System.Collections.Generic;
using System.Linq;

namespace IsThereAnyNews.Extensions
{
    public static class Extensions
    {
        public static T Random<T>(this List<T> collection)
        {
            Random r = new Random(DateTime.Now.Millisecond);
            var element = collection.ElementAt(r.Next(collection.Count - 1));
            return element;
        }
    }
}