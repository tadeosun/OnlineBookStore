using EBookStore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EBookStore.Common
{
    public static class Helper
    {

        public static string GetClientIpAddress(this HttpRequest request)
        {
            if (request.Headers.ContainsKey("MS_HttpContext"))
            {
                dynamic val = request.Headers["MS_HttpContext"];
                if (val != null)
                {
                    return val.Request.UserHostAddress;
                }
            }
            if (request.Headers.ContainsKey("System.ServiceModel.Channels.RemoteEndpointMessageProperty"))
            {
                dynamic val2 = request.Headers["System.ServiceModel.Channels.RemoteEndpointMessageProperty"];
                if (val2 != null)
                {
                    return val2.Address;
                }
            }
            if (request.Headers.ContainsKey("MS_OwinContext"))
            {
                dynamic val3 = request.Headers["MS_OwinContext"];
                if (val3 != null)
                {
                    return val3.Request.RemoteIpAddress;
                }
            }

            if (request.HttpContext.Connection.RemoteIpAddress != null)
            {
                dynamic val3 = request.HttpContext.Connection.RemoteIpAddress;
                if (val3 != null)
                {
                    return val3.ToString();
                }
            }
            return null;
        }

        public static string GetInfo<T>(T obj)
        {
            var builder = new StringBuilder();
            foreach (PropertyInfo p in obj.GetType().GetProperties())
            {
                var s = p.GetValue(obj, null);
                builder.AppendLine(string.Format("{0}: {1}", p.Name, s?.ToString()));
            }
            return builder.ToString();
        }

        public static bool IsItemExistInList(string[] list, string item)
        {

            if ((list == null | string.IsNullOrEmpty(item)))
                return false;

            string foundItem = (from l in list where l.ToLower() == item.ToLower() select l).FirstOrDefault();

            return !string.IsNullOrEmpty(foundItem);

        }

        public static HashSet<string> ToHashSet(string input)
        {
            if (string.IsNullOrEmpty(input)) return new HashSet<string>();

            return new HashSet<string>(input.Split(",".ToCharArray()));
        }

        public static bool IsValidTitle(string title)
        {
            string regexPattern = @"^[a-zA-Z0-9]+$";

            return Regex.IsMatch(title, regexPattern);
        }

        public static bool IsValidISBN(string title)
        {
            string regexPattern = @"^[0-9-]*$";

            return Regex.IsMatch(title, regexPattern);

        }

        //public static string GetLoggedInUser()
        //{
        //    var c = HttpContext.Current;
        //    if (c == null) return "anonymous";
        //    if (c.User.Identity as Identity != null)
        //        return ((Identity)c.User.Identity).FullName;
        //    else
        //        return "Anonymous User";
        //}
        public static string GetLoggedInUser(IHttpContextAccessor _httpContextAccessor)
        {
            HttpContext context = _httpContextAccessor.HttpContext;

            if (context == null)
                return "anonymous";

            var identity = context.User.Identity.Name;
            if (identity == null)
            {
                return "Anonymous User";
            }
            else
            {
                return identity.ToString();
            }
        }

        public static bool IsValidEmail(string email)
        {
            string regexPattern = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
                    + "@"
                    + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$";

            return Regex.IsMatch(email, regexPattern);
        }

        public static bool IsValidPassword(string password)
        {
            /****Must include at least 1 lower-case letter.
                Must include at least 1 upper-case letter.
                Must include at least 1 number.
                Must include at least 1 special character (only the following special characters are allowed: !#%).
                Must NOT include any other characters then A-Za-z0-9!#% (must not include ; for example).
                Must be from 8 to 32 characters long.****/
            string regexPattern = @"^(?=[^a-z]*[a-z])(?=[^A-Z]*[A-Z])(?=\D*\d)(?=[^!#%]*[!#%])[A-Za-z0-9!#%]{8,32}$";
            return Regex.IsMatch(password, regexPattern);
        }

        public static DataTable ConvertToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }

            foreach (T item in items)
            {
                var values = new object[Props.Length];

                for (int i = 0; i < Props.Length; i++)
                {

                    //inserting property values to datatable rows

                    values[i] = Props[i].GetValue(item, null);

                }
                dataTable.Rows.Add(values);
            }

            //put a breakpoint here and check datatable
            return dataTable;
        }
    }
}
