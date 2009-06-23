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
    public class XBMC_Playlist
    {
        XBMC_Communicator parent = null;
        private string[] aCurrentPlaylist = null;

        public XBMC_Playlist(XBMC_Communicator p)
        {
            parent = p;
        }

        public string[] Get(bool parse, bool refresh)
        {
            if (refresh)
            {
                string[] aPlaylistTemp = parent.Request("GetPlaylistContents(GetCurrentPlaylist)");

                if (parse == true)
                {
                if (aPlaylistTemp != null)
                {
                    aCurrentPlaylist = new string[aPlaylistTemp.Length];
                    for (int x = 0; x < aPlaylistTemp.Length; x++)
                    {
                        int i = aPlaylistTemp[x].LastIndexOf(".");
                        if (i > 1)
                        {
                            string extension = aPlaylistTemp[x].Substring(i, aPlaylistTemp[x].Length - i);
                                aPlaylistTemp[x] = aPlaylistTemp[x].Replace("/", "\\");
                            string[] aPlaylistEntry = aPlaylistTemp[x].Split('\\');
                            string playlistEntry = aPlaylistEntry[aPlaylistEntry.Length - 1].Replace(extension, "");
                            aCurrentPlaylist[x] = playlistEntry;
                        }
                        else
                            aCurrentPlaylist[x] = "";
                    }
                    }
                }
                else
                {
                    aCurrentPlaylist = aPlaylistTemp;
                }
            }

            return aCurrentPlaylist;
        }

        public void PlaySong(int position)
        {
            parent.Request("SetPlaylistSong(" + position.ToString() + ")");
        }

        public void Remove(int position)
        {
            parent.Request("RemoveFromPlaylist(" + position.ToString() + ")");
        }

        public string GetCurrentIdentifier()
        {
            string[] curPlaylist = parent.Request("GetCurrentPlaylist()");
            return (curPlaylist == null) ? null : curPlaylist[0];
        }

        public void Clear()
        {
            parent.Request("ClearPlayList()");
        }

        public void AddDirectoryContent(string folderPath, string mask, bool recursive)
        {
            string p = "";
            string m = "";
            string r = "";

            if (mask != null)
            {
                m = ";[" + mask + "]";
                p = ";0";
                r = (recursive) ? ";1" : ";0";
            }

            parent.Request("AddToPlayList(" + folderPath + p + m + r + ")");
        }

        public void AddDirectoryContent(string folderPath, string mask)
        {
            this.AddDirectoryContent(folderPath, mask, false);
        }

        public void AddFilesToPlaylist(string filePath)
        {
            this.AddDirectoryContent(filePath, null);
        }

        public void SetSong(int position)
        {
            parent.Request("SetPlaylistSong(" + position.ToString() + ")");
        }

        public void SetType(string type)
        {
            string playlistType = (type == "video") ? "1" : "0";
            parent.Request("SetCurrentPlaylist(" + playlistType + ")");
        }
    }
}
