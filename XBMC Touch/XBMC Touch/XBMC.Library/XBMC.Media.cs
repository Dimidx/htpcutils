// ------------------------------------------------------------------------
//    XBMControl - A compact remote controller for XBMC (.NET 3.5)
//    Copyright (C) 2008  Bram van Oploo (bramvano@gmail.com)
//                        Mike Thiels (Mike.Thiels@gmail.com)
//
//    This program is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    This program is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with this program.  If not, see <http://www.gnu.org/licenses/>.
// ------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace XBMC
{
    public class XBMC_Media
    {
        XBMC_Communicator parent = null;

        public XBMC_Media(XBMC_Communicator p)
        {
            parent = p;
        }

        public string[] GetShares(string type, bool path)
        {
            string[] aMediaShares = parent.Request("GetShares(" + type + ")");

            if (aMediaShares != null)
            {
                string[] aShareNames = new string[aMediaShares.Length];
                string[] aSharePaths = new string[aMediaShares.Length];

                for (int x = 0; x < aMediaShares.Length; x++)
                {
                    string[] aTmpShare = aMediaShares[x].Split(';');

                    if (aTmpShare != null)
                    {
                        aShareNames[x] = aTmpShare[0];
                        aSharePaths[x] = aTmpShare[1];
                    }
                }

                return (path) ? aSharePaths : aShareNames;
            }
            else
                return null;
        }

        public string[] GetShares(string type)
        {
            return GetShares(type, false);
        }

        public string[] GetDirectoryContentPaths(string directory, string mask)
        {
            mask = (mask == null) ? "" : ";" + mask;
          
            string[] aDirectoryContent = parent.Request("GetDirectory(" + directory + mask + ")");

            if (aDirectoryContent != null)
            {
                string[] aContentPaths = new string[aDirectoryContent.Length];

                for (int x = 0; x < aDirectoryContent.Length; x++)
                {
                    aDirectoryContent[x] = aDirectoryContent[x].Replace("/", "\\");
                    aContentPaths[x] = (aDirectoryContent[x] == "Error:Not folder" || aDirectoryContent[x] == "Error") ? null : aDirectoryContent[x];
                }
                return aContentPaths;
            }
            else
                return null;
        }

        public string[] GetDirectoryContentPaths(string directory)
        {
            return GetDirectoryContentPaths(directory, null);
        }

        public string[] GetDirectoryContentNames(string directory, string mask)
        {
            string[] aContentPaths = this.GetDirectoryContentPaths(directory, mask);

            if (aContentPaths != null)
            {
                string[] aContentNames = new string[aContentPaths.Length];

                for (int x = 0; x < aContentPaths.Length; x++)
                {
                    if (aContentPaths[x] == null || aContentPaths[x] == "")
                        aContentNames[x] = null;
                    else
                    {
                        aContentPaths[x] = aContentPaths[x].Replace("/", "\\");
                        string[] aTmpContent = aContentPaths[x].Split('\\');
                        aContentNames[x] = (aTmpContent[aTmpContent.Length - 1] == "") ? aTmpContent[aTmpContent.Length - 2] : aTmpContent[aTmpContent.Length - 1];
                    }
                }

                return aContentNames;
            }
            else
                return null;
        }

        public string[] GetDirectoryContentNames(string directory)
        {
            return GetDirectoryContentNames(directory, null);
        }
    }
}
