using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace WerServer
{
    public class RequestData
    {
        /// <summary>
        /// Get Values from a body
        /// </summary>
        /// <param name="contextBody"></param>
        /// <returns></returns>
        public static List<DataFormat> GetValues(string contextBody)
        {
            List<DataFormat> result = new List<DataFormat>();
            string[] elements = contextBody.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            List<string> items = new List<string>();
            int c = 0;
            int t = elements.Length;
            do
            {
                if (elements[c].Substring(0, 1) != "-")
                {
                    if (elements[c].IndexOf("\"") >= 0)
                    {
                        string variable = GetSubStrings(elements[c]);
                        c++;
                        if (elements[c].Substring(0, 1) == "-") result.Add(new DataFormat() { Variable = variable, Valor = string.Empty });
                        else result.Add(new DataFormat() { Variable = variable, Valor = elements[c] });
                    }
                }
                c++;
            } while (c < t);
            return result;
        }

        /// <summary>
        /// Get Values from the query String
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        public static List<DataFormat> GetValues(NameValueCollection queryString)
        {
            List<DataFormat> result = new List<DataFormat>();
            foreach (string variable in queryString)
            {
                result.Add(new DataFormat()
                {
                    Variable = variable,
                    Valor = queryString[variable]
                });
            }
            return result;
        }

        private static string GetSubStrings(string input)
        {   
            string word = Regex.Match(input, "\"(\\w+)\"").Groups[1].Value;            
            return word;
        }

    }
}
