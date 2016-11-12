namespace IsThereAnyNews.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Security.Principal;

    using IsThereAnyNews.SharedData;

    public static class Extensions
    {
        public static T Random<T>(this List<T> collection)
        {
            var r = new Random(DateTime.Now.Millisecond);
            var element = collection.ElementAt(r.Next(collection.Count - 1));
            return element;
        }


        public static long GetItanUserId(this IIdentity principal)
        {
            var ci = principal as ClaimsIdentity;
            var userid = long.Parse(ci.Claims.Single(c => c.Type == ItanClaimTypes.ApplicationIdentifier).Value);
            return userid;
        }
    }
}