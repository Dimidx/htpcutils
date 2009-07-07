using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace MediaManager.Library
{
    public class Utils
    {
        #region MD5HashFile
        /// <summary>
        /// Computes the MD5 hash of a given file.
        /// </summary>
        /// <param name="filename">The path of the file.</param>
        /// <returns>The MD5 checksum in string format.</returns>
        /// <exception>Throws an FileNotFoundException if the given file doesn't exist (or is not accessible).</exception>
        public static string MD5HashFile(string filename)
        {
            if (File.Exists(filename))
            {

                System.Security.Cryptography.MD5CryptoServiceProvider md5_summer = new System.Security.Cryptography.MD5CryptoServiceProvider();

                string result = string.Empty;
                using (FileStream fs = File.OpenRead(filename))
                    result = BitConverter.ToString(md5_summer.ComputeHash(fs));
                return result;
            }
            else
            {
                return "";
            }

        } 
        #endregion

        #region GetSourceHTML
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
        
        #endregion

        #region GetStreamImage
        /// <summary>
        /// Télécharge une image dans un stream
        /// </summary>
        /// <param name="url">URL de l'image a télécharger</param>
        /// <returns></returns>
        public static MemoryStream GetStreamImage(string url)
        {
            try
            {
                WebClient client = new WebClient();

                //#region Proxy
                WebProxy wProxy = new WebProxy("10.126.71.12", 80);
                wProxy.Credentials = new NetworkCredential("rfraftp", "Siberbo2000", "fr");
                client.Proxy = wProxy;
                //#endregion

                byte[] _result = client.DownloadData(new Uri(url));
                client.Dispose();
                MemoryStream ms = new MemoryStream(_result);
                _result = null;
                return ms;

            }
            catch (WebException ex)
            {
                Console.WriteLine("Impossible de télécharger l'image " + url + Environment.NewLine + ex.Message);
                return null;
            }

        } 
        #endregion

        #region RemoveUnwantedChars
        /// <summary>
        /// Supprime les caractères html
        /// </summary>
        /// <param name="tInput"></param>
        /// <returns></returns>
        public static string RemoveUnwantedChars(string tInput)
        {
            tInput = Regex.Replace(tInput, "<[^<]*>", "");
            tInput = Regex.Replace(tInput, "Plus.*?...", "");
            tInput = tInput.Replace("\\s{2,}", " ");
            tInput = tInput.Replace("&nbsp;", "").Trim();
            return tInput;
        } 
        #endregion

        #region GetStringBetweenTwoStrings
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
        #endregion

        #region Hash
        public static string Hash(string input)
        {
            byte[] bytes;
            uint m_crc = 0xffffffff;
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            bytes = encoding.GetBytes(input.ToLower());
            foreach (byte myByte in bytes)
            {
                m_crc ^= ((uint)(myByte) << 24);
                for (int i = 0; i < 8; i++)
                {
                    if ((System.Convert.ToUInt32(m_crc) & 0x80000000) == 0x80000000)
                    {
                        m_crc = (m_crc << 1) ^ 0x04C11DB7;
                    }
                    else
                    {
                        m_crc <<= 1;
                    }
                }
            }
            return String.Format("{0:x8}", m_crc);
        } 
        #endregion
    }
}
