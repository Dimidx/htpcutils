// ------------------------------------------------------------------------
//    XBMControl - A compact remote controller for XBMC (.NET 3.5)
//    Copyright (C) 2008  Bram van Oploo (bramvano@gmail.com)
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
//using System.Windows.Forms;

namespace XBMC
{
    public class XBMC_Database
    {
        XBMC_Communicator parent = null;

        public XBMC_Database(XBMC_Communicator p) 
        {
            parent = p;
        }



        public string[] GetArtists(string searchString)
        {
            string condition = (searchString == null) ? "" : " WHERE strArtist LIKE '%%" + searchString + "%%'";
            return parent.Request("QueryMusicDatabase", "SELECT strArtist FROM artist" + condition + " ORDER BY strArtist");
        }

        public string[] GetArtists()
        {
            return this.GetArtists(null);
        }



        public string[] GetArtistIds(string searchString)
        {
            string condition = (searchString == null) ? "" : " WHERE strArtist LIKE '%%" + searchString + "%%'";
            return parent.Request("QueryMusicDatabase", "SELECT idArtist FROM artist" + condition + " ORDER BY strArtist");
        }

        public string[] GetArtistIds()
        {
            return this.GetArtistIds(null);
        }



        public string[] GetAlbums(string searchString)
        {
            string condition = (searchString == null) ? "" : " WHERE strAlbum LIKE '%%" + searchString + "%%'";
            return parent.Request("QueryMusicDatabase", "SELECT strAlbum FROM album" + condition + " ORDER BY strAlbum");
        }

        public string[] GetAlbums()
        {
            return this.GetAlbums(null);
        }



        public string[] GetAlbumIds(string searchString)
        {
            string condition = (searchString == null) ? "" : " WHERE strAlbum LIKE '%%" + searchString + "%%'";
            return parent.Request("QueryMusicDatabase", "SELECT idAlbum FROM album" + condition + " ORDER BY strAlbum");
        }

        public string[] GetAlbumIds()
        {
            return this.GetAlbumIds(null);
        }



        public string GetArtistId(string artist)
        {
            string[] aArtistId = parent.Request("QueryMusicDatabase", "SELECT idArtist FROM artist WHERE strArtist='" + artist + "'");
            return (aArtistId != null) ? aArtistId[0] : null;
        }



        public string GetAlbumId(string artist, string album)
        {
            string conditions = " WHERE strAlbum='" + album + "'";
            if (artist != null) conditions += " AND idArtist='" + this.GetArtistId(artist) + "'";
            string[] aArtistId = parent.Request("QueryMusicDatabase", "SELECT idAlbum FROM album" + conditions);
            return (aArtistId != null) ? aArtistId[0] : null;
        }

        public string GetAlbumId(string album)
        {
            return this.GetAlbumId(null, album);
        }



        public string GetPathById(string pathId)
        {
            string[] aPath = parent.Request("QueryMusicDatabase", "SELECT strPath FROM path WHERE idPath='" + pathId + "'");
            return (aPath != null) ? aPath[0] : null;
        }



        public string GetAlbumPath(string albumId)
        {
            string[] aPathId = parent.Request("QueryMusicDatabase", "SELECT idPath FROM song WHERE idAlbum='" + albumId + "'");
            return (aPathId != null) ? GetPathById(aPathId[0]) : null;
        }



        public string GetSongPath(string songId)
        {
            string[] aPathId = parent.Request("QueryMusicDatabase", "SELECT idPath FROM song WHERE idSong='" + songId + "'");
            return (aPathId != null) ? GetPathById(aPathId[0]) : null;
        }



        public string[] GetAlbumsByArtist(string artist)
        {
            return GetAlbumsByArtistId(GetArtistId(artist));
        }

        public string[] GetAlbumsByArtistId(string artistId)
        {
            return (artistId == null) ? null : parent.Request("QueryMusicDatabase", "SELECT strAlbum FROM album WHERE idArtist='" + artistId + "' ORDER BY strAlbum");
        }



        public string[] GetAlbumIdsByArtist(string artist)
        {
            return GetAlbumIdsByArtistId(GetArtistId(artist));
        }

        public string[] GetAlbumIdsByArtistId(string artistId)
        {
            return (artistId == null) ? null : parent.Request("QueryMusicDatabase", "SELECT idAlbum FROM album WHERE idArtist='" + artistId + "' ORDER BY strAlbum");
        }



        public string[] GetTitlesByAlbum(string artist, string album)
        {
            return GetTitlesByAlbumId(GetAlbumId(artist, album));
        }

        public string[] GetTitlesByAlbum(string album)
        {
            return GetTitlesByAlbum(null, album);
        }

        public string[] GetTitlesByAlbumId(string albumId)
        {
            return (albumId == null) ? null : parent.Request("QueryMusicDatabase", "SELECT strTitle FROM song WHERE idAlbum='" + albumId + "' ORDER BY iTrack");
        }



        public string[] GetPathsByAlbum(string artist, string album)
        {
            return GetPathsByAlbumId(GetAlbumId(artist, album));
        }

        public string[] GetPathsByAlbum(string album)
        {
            return GetPathsByAlbum(null, album);
        }

        public string[] GetPathsByAlbumId(string albumId)
        {
            string[] aPath = null;

            if (albumId != null)
            {
                string[] aFileName = parent.Request("QueryMusicDatabase", "SELECT strFileName FROM song WHERE idAlbum='" + albumId + "' ORDER BY iTrack");

                if (aFileName != null)
                {
                    aPath = new string[aFileName.Length];
                    aPath[0] = "";

                    for (int x = 0; x < aFileName.Length; x++)
                        aPath[x] = GetAlbumPath(albumId) + aFileName[x];
                }
            }

            return aPath;
        }

        public string[] GetSearchSongTitles(string searchString)
        {
            return parent.Request("QueryMusicDatabase", "SELECT strTitle FROM song WHERE strTitle LIKE '%%" + searchString + "%%' ORDER BY strTitle");
        }

        public string[] GetSearchSongPaths(string searchString)
        {
            string[] aSongPaths = null;
            string[] aSongPathIds = parent.Request("QueryMusicDatabase", "SELECT idPath FROM song WHERE strTitle LIKE '%%" + searchString + "%%' ORDER BY strTitle");
            string[] aFileNames = parent.Request("QueryMusicDatabase", "SELECT strFileName FROM song WHERE strTitle LIKE '%%" + searchString + "%%' ORDER BY strTitle");

            if (aSongPathIds != null)
            {
                aSongPaths = new string[aSongPathIds.Length];

                for (int x = 0; x < aSongPathIds.Length; x++)
                { 
                    string[] aSongPath = parent.Request("QueryMusicDatabase", "SELECT strPath FROM path WHERE idPath='" + aSongPathIds[x] + "'");
                    aSongPaths[x] = aSongPath[0] + aFileNames[x];
                }
            }

            return aSongPaths;
        }



        public string[] GetTitlesByArtist(string artist)
        {
            return GetTitlesByArtistId(GetArtistId(artist));
        }

        public string[] GetTitlesByArtistId(string artistId)
        {
            return (artistId == null) ? null : parent.Request("QueryMusicDatabase", "SELECT strTitle FROM song WHERE idArtist='" + artistId + "' ORDER BY iTrack");
        }




        public string[] GetPathsByArtist(string artist)
        {
            return GetPathsByArtistId(GetArtistId(artist));
        }

        public string[] GetPathsByArtistId(string artistId)
        {
            string[] aPaths = null;
            string[] aSongPathIds = null;
            string[] aFileNames = null;

            if (artistId != null)
            {
                aSongPathIds = parent.Request("QueryMusicDatabase", "SELECT idPath FROM song WHERE idArtist='" + artistId + "'");
                aFileNames = parent.Request("QueryMusicDatabase", "SELECT strFileName FROM song WHERE idArtist='" + artistId + "'");

                if (aFileNames != null)
                {
                    aPaths = new string[aFileNames.Length];
                    for (int x = 0; x < aFileNames.Length; x++)
                        aPaths[x] =  GetPathById(aSongPathIds[x]) + aFileNames[x];
                }
            }

            return aPaths;
        }




        public string GetPathBySongTitle(string artistAlbumId, string songTitle, bool artist)
        {
            string songPath = null;
            string idField = (artist) ? "idArtist" : "idAlbum";
            string[] aSongPathIds = parent.Request("QueryMusicDatabase", "SELECT idPath FROM song WHERE " + idField+ "='" + artistAlbumId + "' AND strTitle='" + songTitle + "'");
            string[] aSongFileNames = parent.Request("QueryMusicDatabase", "SELECT strFileName FROM song WHERE " + idField + "='" + artistAlbumId + "' AND strTitle='" + songTitle + "'");

            if (aSongPathIds != null)
            {
                string[] aSongPaths = parent.Request("QueryMusicDatabase", "SELECT strPath FROM path WHERE idPath='" + aSongPathIds[0]+ "'");

                if (aSongPaths != null)
                    songPath = aSongPaths[0] + aSongFileNames[0];
            }

            return songPath;
        }

        public string GetPathBySongTitle(string artistAlbumId, string songTitle)
        {
            return GetPathBySongTitle(artistAlbumId, songTitle, false);
        }
    }
}
