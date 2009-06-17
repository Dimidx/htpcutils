using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using System.Data.Common;
using System.IO;
using System.Security.Cryptography;
using System.Xml;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;


namespace MediaManager
{
    /// <summary>
    /// Manage connection to the Sqlite Database
    /// </summary>
    class Database
    {


        private static readonly string SqliteConnString = @"Data Source=MediaManager.sqlite;Version=3";

        private static SQLiteConnection sql_conn = new SQLiteConnection(SqliteConnString);

        private static SQLiteCommand sql_cmd = sql_conn.CreateCommand();

        private static SQLiteDataReader sql_datareader;

        /// <summary>
        /// Different kinds of Filter Movielist
        /// </summary>
        public enum SearchParameter
        {
            Actor,
            Genre,
            Year,
            Titel,
            All
        }
                
        /// <summary>
        /// Create the Database for iMedia in the Homefolder of iMedia
        /// </summary>
        public static void CreateDB()
         {            
                SQLiteConnection sqlCn = new SQLiteConnection(SqliteConnString);

                sqlCn.Open();

                SQLiteCommand sqlcom = sqlCn.CreateCommand();

                StringBuilder sbCreateTables = new StringBuilder();

                sbCreateTables.Append("CREATE TABLE Actor (ActorID INTEGER PRIMARY KEY NOT NULL , Name TEXT NOT NULL,Thumb TEXT);");

                sbCreateTables.Append("CREATE TABLE Backdrop (BackdropID INTEGER PRIMARY KEY    NOT NULL , Backdrop TEXT NOT NULL, Size TEXT NOT NULL );");

                sbCreateTables.Append("CREATE TABLE Categorie (CategorieID INTEGER PRIMARY KEY    NOT NULL , Categorie TEXT NOT NULL );");

                sbCreateTables.Append("CREATE TABLE Genre (GenreID INTEGER PRIMARY KEY    NOT NULL , Genre TEXT NOT NULL );");

                sbCreateTables.Append("CREATE TABLE Job (JobID INTEGER PRIMARY KEY    NOT NULL , Job TEXT NOT NULL );");

                sbCreateTables.Append("CREATE TABLE LinkActorMovie (MovieID INTEGER NOT NULL , ActorID INTEGER NOT NULL , JobID INTEGER NOT NULL, RollID INTEGER );");

                sbCreateTables.Append("CREATE TABLE LinkBackdropMovie (MovieID INTEGER NOT NULL , BackdropID INTEGER NOT NULL);");

                sbCreateTables.Append("CREATE TABLE LinkCategorieMovie (MovieID INTEGER NOT NULL , CategorieID INTEGER NOT NULL );");

                sbCreateTables.Append("CREATE TABLE LinkGenreMovie (MovieID INTEGER NOT NULL , GenreID INTEGER NOT NULL );");

                sbCreateTables.Append("CREATE TABLE LinkPosterMovie (MovieID INTEGER NOT NULL , PosterID INTEGER NOT NULL);");

                sbCreateTables.Append("CREATE TABLE LinkProductionlandMovie (MovieID INTEGER NOT NULL , ProductionlandID INTEGER NOT NULL);");

                sbCreateTables.Append("CREATE TABLE Movie (MovieID INTEGER PRIMARY KEY    NOT NULL , Titel TEXT NOT NULL , Alt_Titel TEXT, Year INTEGER, IMDBID TEXT, Note DOUBLE, ShortOverview TEXT, Overview TEXT, Runtime TEXT,Tagline TEXT,MPAA TEXT,Votes INTEGER);");

                sbCreateTables.Append("CREATE TABLE Poster (PosterID INTEGER PRIMARY KEY    NOT NULL , PosterURL TEXT ,Size TEXT NOT NULL);");

                sbCreateTables.Append("CREATE TABLE Productionland (ProductionlandID INTEGER PRIMARY KEY    NOT NULL , Productionland TEXT NOT NULL);");

                sbCreateTables.Append("CREATE TABLE Rolles (RollID INTEGER PRIMARY KEY    NOT NULL , RollName TEXT NOT NULL );");

                sbCreateTables.Append("CREATE TABLE Studio (StudioID INTEGER PRIMARY KEY    NOT NULL , Studio TEXT NOT NULL );");

                sbCreateTables.Append("CREATE TABLE LinkStudioMovie (MovieID INTEGER NOT NULL , StudioID INTEGER NOT NULL);");

                sbCreateTables.Append("CREATE TABLE Files (FilesID INTEGER PRIMARY KEY NOT NULL ,Source TEXT, FilePath TEXT NOT NULL,FileSize DOUBLE NOT NULL,FileName TEXT NOT NULL,MD5 TEXT NOT NULL);");

                sbCreateTables.Append("CREATE TABLE LinkFilesMovie (MovieID INTEGER NOT NULL , FilesID INTEGER NOT NULL);");

                sbCreateTables.Append("CREATE TABLE VideoFileInfo (VideoFileInfoID INTEGER PRIMARY KEY NOT NULL , Width TEXT NOT NULL,Height TEXT NOT NULL,Aspectratio DOUBLE,Codec TEXT NOT NULL,Formatinfo TEXT NOT NULL,Duration TEXT NOT NULL,Bitrate TEXT NOT NULL,Bitratemode TEXT NOT NULL,Bitratemax TEXT NOT NULL,Container TEXT NOT NULL,Codecid TEXT NOT NULL,Codecidinfo TEXT NOT NULL,Scantype TEXT NOT NULL);");

                sbCreateTables.Append("CREATE TABLE LinkVideoFile(FilesID INTEGER NOT NULL , VideoFileInfoID INTEGER NOT NULL);");

                sbCreateTables.Append("CREATE TABLE FileLanguage (FileLanguageID INTEGER PRIMARY KEY NOT NULL , LanguageID INTEGER NOT NULL,Codec TEXT NOT NULL,Channels TEXT NOT NULL,Bitrate TEXT NOT NULL);");

                sbCreateTables.Append("CREATE TABLE LinkFileLanguageMovie (FilesID INTEGER NOT NULL , FileLanguageID INTEGER NOT NULL);");
         
                sbCreateTables.Append("CREATE TABLE LinkFileSubtitleLanguage (FilesID INTEGER NOT NULL , LanguageID INTEGER NOT NULL);");

                sbCreateTables.Append("CREATE TABLE Language (LanguageID INTEGER PRIMARY KEY NOT NULL , LanguageCode Text NOT NULL,LanguageName TEXT NOT NULL);");

                sbCreateTables.Append("CREATE TABLE Version (Version INTEGER);");

                sbCreateTables.Append("INSERT INTO Version (Version) VALUES (1);");

                sqlcom.CommandText = sbCreateTables.ToString();
                try
                {
                    sqlcom.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    MessageBox.Show("Can't create Database");
                }

                sqlCn.Close();
        }


        public static bool ClearDatabase()
        {

            StringBuilder ClearTablesStringBuilder = new StringBuilder();

            sql_conn.Open();

            sql_cmd = sql_conn.CreateCommand();

            ClearTablesStringBuilder.Append("DELETE FROM Actor;");
            ClearTablesStringBuilder.Append("DELETE FROM Backdrop;");
            ClearTablesStringBuilder.Append("DELETE FROM Categorie;");
            ClearTablesStringBuilder.Append("DELETE FROM Genre;");
            ClearTablesStringBuilder.Append("DELETE FROM Job;");
            ClearTablesStringBuilder.Append("DELETE FROM LinkActorMovie;");
            ClearTablesStringBuilder.Append("DELETE FROM LinkBackdropMovie;");
            ClearTablesStringBuilder.Append("DELETE FROM LinkCategorieMovie;");
            ClearTablesStringBuilder.Append("DELETE FROM LinkGenreMovie;");
            ClearTablesStringBuilder.Append("DELETE FROM LinkPosterMovie;");
            ClearTablesStringBuilder.Append("DELETE FROM LinkProductionlandMovie;");
            ClearTablesStringBuilder.Append("DELETE FROM Movie;");
            ClearTablesStringBuilder.Append("DELETE FROM Poster;");
            ClearTablesStringBuilder.Append("DELETE FROM Productionland;");
            ClearTablesStringBuilder.Append("DELETE FROM Rolles;");
            ClearTablesStringBuilder.Append("DELETE FROM Studio;");
            ClearTablesStringBuilder.Append("DELETE FROM LinkStudioMovie;");
            ClearTablesStringBuilder.Append("DELETE FROM Files;");
            ClearTablesStringBuilder.Append("DELETE FROM LinkFilesMovie;");
            ClearTablesStringBuilder.Append("DELETE FROM VideoFileInfo;");
            ClearTablesStringBuilder.Append("DELETE FROM LinkVideoFile;");
            ClearTablesStringBuilder.Append("DELETE FROM FileLanguage;");
            ClearTablesStringBuilder.Append("DELETE FROM LinkFileLanguageMovie;");
            ClearTablesStringBuilder.Append("DELETE FROM LinkFileSubtitleLanguage;");

            sql_cmd.CommandText = ClearTablesStringBuilder.ToString();

            try
            {
                sql_cmd.ExecuteNonQuery();
                sql_conn.Close();
                return true;
            }
            catch (Exception)
            {
                sql_conn.Close();
                return false;
            }

            
        
        }
        
   

        /// <summary>
        /// Count the Number of Movies in DB
        /// </summary>
        /// <returns>Number of Movies in Database</returns>
        public static int GetNumberOfMovie()
         {
             int intMovieCount = 0;

             try
             {
                 sql_conn.Open();

                 sql_cmd.CommandText = String.Format(@"Select Count(Id_Film) FROM Films");

                 sql_datareader = sql_cmd.ExecuteReader();

                 if (sql_datareader.Read())
                 {
                     intMovieCount = Convert.ToInt32(sql_datareader[0]);
                 }

                 sql_conn.Close();

                 return intMovieCount;
             }
               
             catch (Exception e)
             {
                 Console.WriteLine(e.Message);
                 return 0;
             }

             
         }
        
  


        /// <summary>
        /// Execute SQL Commands
        /// </summary>
        /// <param name="sqlCommand">SQL Command</param>
        private static void ExecuteSQL(string sqlCommand)
        {

            SQLiteConnection SqliteConnEx = new SQLiteConnection(SqliteConnString);

            SQLiteCommand SqliteComEx = SqliteConnEx.CreateCommand();

            SqliteConnEx.Open();

            SqliteComEx.CommandText = sqlCommand;

            SqliteComEx.ExecuteNonQuery();

            SqliteConnEx.Close();

        }

                
        /// <summary>
        /// Gibt einen MD5 Hash als String zurück
        /// </summary>
        /// <param name="TextToHash">string der Gehasht werden soll.</param>
        /// <returns>Hash als string.</returns>
        public static string GetMD5Hash(string TextToHash)
        {
          //Prüfen ob Daten übergeben wurden.
          if((TextToHash == null) || (TextToHash.Length == 0))
          {
            return string.Empty;
          }

          //MD5 Hash aus dem String berechnen. Dazu muss der string in ein Byte[]
          //zerlegt werden. Danach muss das Resultat wieder zurück in ein string.
          MD5 md5 = new MD5CryptoServiceProvider();
          byte[] textToHash = Encoding.Default.GetBytes (TextToHash);
          byte[] result = md5.ComputeHash(textToHash); 

          return System.BitConverter.ToString(result); 
        }

  
    
    }
}
