using System;
using System.Collections.Generic;
using System.Text;
using Meedio;
using System.Drawing;
using System.IO;

namespace meeNiceCovers_Converter
{
    public class Importer : Meedio.IMLImportPlugin
    {
        #region IMLImportPlugin Members

        //Les propriétés par defaut
        private string _TypeNiceCovers = "MOVIE";

        public bool GetProperty(int Index, Meedio.IMeedioPluginProperty Prop)
        {
            String[] choices = new String[2];
            //Index starts at 1
            if (Index == 1)
            {
                Prop.Name = "TypeNiceCovers";
                Prop.Caption = "Type de NiceCovers";
                Prop.HelpText = "Choisissez le type de NiceCovers à générer";
                Prop.DefaultValue = this._TypeNiceCovers;
                Prop.DataType = "string";
                choices[0] = "MOVIE";
                choices[1] = "MUSIC";
                Prop.Choices = choices;
                return true;
            }

            if (Index == 2)
            {
                Prop.Name = "GUI";
                Prop.DataType = "custom";
                Prop.HelpText = "Lancer NiceCovers Créator.";
                Prop.DefaultValue = "Cliquez pour lancer NiceCovers Créator";
                Prop.Caption = "Lancer NiceCovers Créator.";
                return true;
            }

            return false;
        }

        public bool EditCustomProperty(int Window, string PropName, ref string Value)
        {
            if (PropName == "GUI")
            {
                System.Diagnostics.ProcessStartInfo myInfo =
                new System.Diagnostics.ProcessStartInfo();
                myInfo.FileName = "NiceCovers_Creator.exe";
                System.Diagnostics.Process.Start(myInfo);
            }


            return true;

        }

        public bool Import(Meedio.IMLSection Section, Meedio.IMLImportProgress Progress)
        {
            int _progress = 1;
            int[] _ids = (int[])Section.GetAllItemIDs();
            foreach (int _id in _ids)
            {
                Meedio.IMLItem item = Section.FindItemByID(_id);
                string _FichierOriginal = item.ImageFile;
                string _FichierNiceCovers = _FichierOriginal.Replace(".jpg", "_NiceCovers.png");

                if (File.Exists(_FichierNiceCovers) == false)
                {
                    try
                    {
                        if (this._TypeNiceCovers == "MOVIE")
                        {
                            NiceCovers_Library.NiceCovers.Fusion(_FichierOriginal).Save(_FichierNiceCovers);
                        }
                        else
                        {
                            NiceCovers_Library.NiceCovers.FusionMusic(_FichierOriginal).Save(_FichierNiceCovers);
                        }
                        item.ImageFile = _FichierNiceCovers;
                        item.SaveTags();
                    }
                    catch (Exception)
                    {

                        //throw;
                    }
                }
                Progress.Progress(_progress * (100 / _ids.Length), _FichierOriginal + "   " + _FichierNiceCovers);
                _progress++;
                item.SaveTags();
            }
            Progress.Progress(100, "Convertion Terminée !");

            return true;
        }

        public bool SetProperties(Meedio.IMeedioItem Properties, out string ErrorText)
        {
            ErrorText = null;
            try
            {
                if (Properties["TypeNiceCovers"] != null) this._TypeNiceCovers = (string)Properties["TypeNiceCovers"];
            }
            catch (Exception ex)
            {
                ErrorText = ex.ToString();
                return false;
            }
            return true;
        }

        #endregion

    }
}
