using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MediaManager
{
    class Utils
    {
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
    }
}
