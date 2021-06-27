using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalFantasu.Utils
{
    public class SessionUtils
    {
        public class SESSION
        {
            public static string UserInfo = "UserInfo";
            public static string Cart = "Cart";            
        }

        public class TEMPDATA
        {
            public static string Message = "Message";
            public static string RequireLogin = "RequireLogin";
        }

        public class DROPDOWN_SORT
        {
            public static string NEWEST = "Mới nhất";
            public static string OLDEST = "Cũ nhất";
            public static string HIGHEST_PRICE = "Giá cao nhất";
            public static string LOWEST_PRICE = "Giá thấp nhất";
        }
        public static int ID_ADMIN = 1;
    }
}
