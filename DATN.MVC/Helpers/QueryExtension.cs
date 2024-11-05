using System.Web;

namespace DATN.MVC.Helpers
{
    public static class QueryExtension
    {
        public static string ToQueryString(this object obj)
        {
            var properties = from p in obj.GetType().GetProperties()
                             where p.GetValue(obj, null) != null
                             select p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(obj, null).ToString());

            return string.Join("&", properties.ToArray());
        }
        public static string ToQueryString(this IDictionary<string, object> dict)
        {
            var list = new List<string>();
            foreach (var item in dict)
            {
                if (item.Value != null)
                    list.Add(item.Key + "=" + item.Value);
            }
            return string.Join("&", list);
        }
    }
}
