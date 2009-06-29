using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;

namespace MediaManager.Library
{
    public class Utils
    {
        /// <summary>
        /// Récupère la source HTML d'une page Web
        /// </summary>
        /// <param name="url">URL de la page à récupérer</param>
        /// <returns></returns>
        public static string GetSourceHTML(string url)
        {
            string sourceHTML = string.Empty;

            try
            {
                WebClient m_webClient = new WebClient();

                #region Proxy
                WebProxy wProxy = new WebProxy("10.126.71.12", 80);
                wProxy.Credentials = new NetworkCredential("rfraftp", "Siberbo2000", "fr");
                m_webClient.Proxy = wProxy;
                #endregion

                //m_webClient.Encoding = Encoding.UTF8;
                sourceHTML = m_webClient.DownloadString(url);

            }
            catch (Exception e)
            {
                throw new Exception("An exception occured when you tried to download the file: " + e.Message);
                sourceHTML = "";
            }

            return sourceHTML;

        }


        /// <summary>
        /// Supprime les caractères html
        /// </summary>
        /// <param name="tInput"></param>
        /// <returns></returns>
        public static string removeUnwantedChars(string tInput)
        {
            tInput = Regex.Replace(tInput, "<[^<]*>", "");
            tInput = Regex.Replace(tInput, "Plus.*?...", "");
            tInput = tInput.Replace("\\s{2,}", " ");
            tInput = tInput.Replace("&nbsp;", "").Trim();
            return tInput;
        }

        /// <summary>
        /// Method used to extract the content of a string from a 2 strings.
        /// </summary>
        private static string GetStringBetweenTwoStrings(string src, string start, string end)
        {
            string retVal = string.Empty;

            int idxStart = src.IndexOf(start);

            if (idxStart != -1)
            {
                idxStart++;

                int idxEnd = src.IndexOf(end, idxStart);

                if (idxEnd != -1)
                {
                    retVal = src.Substring(idxStart, idxEnd - idxStart);
                }
            }

            return retVal;
        }

      
    }
}
