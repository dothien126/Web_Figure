using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;

namespace FinalFantasu.Utils
{
    public class HelpFunction
    {
        public static int[] getNumPage(int currentPage, int maxPageSize)
        {
            var list = new int[] { };

            if (currentPage <= 3)
                list = new int[] { 1, 2, 3, 4, 5 };
            else if (maxPageSize - currentPage <= 2)
                list = new int[] { maxPageSize - 4, maxPageSize - 3, maxPageSize - 2, maxPageSize - 1, maxPageSize };
            else
                list = new int[] { currentPage - 2, currentPage - 1, currentPage, currentPage + 1, currentPage + 2 };

            if (maxPageSize < 5)
            {
                List<int> l = new List<int>(list);
                l.RemoveRange(maxPageSize, 5 - maxPageSize);
                list = l.ToArray();
            }

            return list;
        }

        public static string RemoveUnicode(string text)
        {
            string[] arr1 = new string[]
            {
                "á", "à", "ả", "ã", "ạ", "â", "ấ", "ầ", "ẩ", "ẫ", "ậ", "ă", "ắ", "ằ", "ẳ", "ẵ", "ặ",
                "đ",
                "é", "è", "ẻ", "ẽ", "ẹ", "ê", "ế", "ề", "ể", "ễ", "ệ",
                "í", "ì", "ỉ", "ĩ", "ị",
                "ó", "ò", "ỏ", "õ", "ọ", "ô", "ố", "ồ", "ổ", "ỗ", "ộ", "ơ", "ớ", "ờ", "ở", "ỡ", "ợ",
                "ú", "ù", "ủ", "ũ", "ụ", "ư", "ứ", "ừ", "ử", "ữ", "ự",
                "ý", "ỳ", "ỷ", "ỹ", "ỵ",
            };
            string[] arr2 = new string[]
            {
                "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a",
                "d",
                "e", "e", "e", "e", "e", "e", "e", "e", "e", "e", "e",
                "i", "i", "i", "i", "i",
                "o", "o", "o", "o", "o", "o", "o", "o", "o", "o", "o", "o", "o", "o", "o", "o", "o",
                "u", "u", "u", "u", "u", "u", "u", "u", "u", "u", "u",
                "y", "y", "y", "y", "y",
            };
            for (int i = 0; i < arr1.Length; i++)
            {
                text = text.Replace(arr1[i], arr2[i]);
                text = text.Replace(arr1[i].ToUpper(), arr2[i]);
            }

            return text.ToLower().Trim();
        }

        public static string sha256(string randomString)
        {
            var crypt = new SHA256Managed();
            string hash = String.Empty;
            byte[] crypto = crypt.ComputeHash( Encoding.ASCII.GetBytes(randomString));
            foreach (byte theByte in crypto)
            {
                hash += theByte.ToString("x2");
            }
            return hash;
        }
    }
}
