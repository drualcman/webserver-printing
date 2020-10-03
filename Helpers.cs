using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WerServer
{
    static class Helpers
    {
        public static string ToJson(bool retul)
        {
            return "{\"Result\":" + retul.ToString().ToLower() + "}";
        }

        public static string ToJson(bool retul, string msg)
        {
            return "{\"Result\":" + retul.ToString().ToLower() + ", \"Message\":\"" + msg + "\"}";
        }

        public static string ToJson(List<DataFormat> valores)
        {
            string result;
            if (valores.Count > 1)
            {
                result = "[";
                foreach (DataFormat item in valores)
                {
                    result += "{\"" + item.Variable + "\":\"" + item.Valor + "\"},";
                }
                result = result.Remove(result.Length - 1, 1);
                result += "]";
            }
            else if (valores.Count > 0)
            {
                result = "{\"" + valores[0].Variable + "\":\"" + valores[0].Valor + "\"}";
            }
            else
            {
                result = string.Empty;
            }
            return result;
        }

        public static string ToJson(List<string> valores)
        {
            string result;
            if (valores.Count > 1)
            {
                result = "[";
                foreach (string item in valores)
                {
                    result += "{\"Printer\":\"" + item + "\"},";
                }
                result = result.Remove(result.Length - 1, 1);
                result += "]";
            }
            else if (valores.Count > 0)
            {
                result = "{\"Printer\":\"" + valores[0] + "\"}";
            }
            else
            {
                result = string.Empty;
            }
            return result;
        }
    }
}
