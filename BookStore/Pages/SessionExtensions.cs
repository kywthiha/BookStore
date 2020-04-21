using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Pages
{
    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonConvert.DeserializeObject<T>(value);
        }
        public static int GetCounts(this ISession session) { 
            var dict = Get<IDictionary<string, int>>(session, "bookCats");
            
            if (dict != null)
            {
                int counts = 0;
                foreach (var kvp in dict)
                    counts += kvp.Value;
                return counts;
            }
            else
                return 0;
        }
    }
}
