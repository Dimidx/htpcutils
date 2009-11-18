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
using System.Drawing;
using System.IO;
//using System.Windows.Forms;

namespace XBMC
{
    public class XBMC_Controls
    {
        XBMC_Communicator parent = null;

        public XBMC_Controls(XBMC_Communicator p)
        {
            parent = p;
        }

        public void Play()
        {
            parent.Request("ExecBuiltIn", "PlayerControl(Play)");
        }

        public void PlayFile(string filename)
        {
            parent.Request("PlayFile(" + filename + ")");
        }

        public void PlayMedia(string media)
        {
            parent.Request("ExecBuiltIn", "PlayMedia(" + media + ")");
        }

        public void Stop()
        {
            parent.Request("ExecBuiltIn", "PlayerControl(Stop)");
        }

        public void Next()
        {
            parent.Request("ExecBuiltIn", "PlayerControl(Next)");
        }

        public void PlayListNext()
        {
            parent.Request("PlayListNext");
        }

        public void Previous()
        {
            parent.Request("ExecBuiltIn", "PlayerControl(Previous)");
        }

        public void ToggleShuffle()
        {
            parent.Request("ExecBuiltIn", "PlayerControl(Random)");
        }

        public void TogglePartymode()
        {
            parent.Request("ExecBuiltIn", "PlayerControl(Partymode(music))");
        }

        public void Repeat(bool enable)
        {
            string mode = (enable) ? "Repeat" : "RepeatOff";
            parent.Request("ExecBuiltIn", "PlayerControl(" + mode + ")");
        }

        public void LastFmLove()
        {
            parent.Request("ExecBuiltIn", "LastFM.Love(false)");
        }

        public void LastFmHate()
        {
            parent.Request("ExecBuiltIn", "LastFM.Ban(false)");
        }

        public void ToggleMute()
        {
            parent.Request("ExecBuiltIn", "Mute");
            parent.Status.GetVolume();
        }

        public void SetVolume(int percentage)
        {

 
            parent.Request("ExecBuiltIn", "SetVolume(" + Convert.ToString(percentage) + ")");
            parent.Status.GetVolume();
        }

        public void SeekPercentage(int percentage)
        {
            parent.Request("SeekPercentage", Convert.ToString(percentage));
            parent.Status.GetProgress();
        }

        public void Reboot()
        {
            parent.Request("ExecBuiltIn", "Reboot");
        }

        public void Shutdown()
        {
            parent.Request("ExecBuiltIn", "Shutdown");
        }

        public void Restart()
        {
            parent.Request("ExecBuiltIn", "RestartApp");
        }

        public string GetGuiDescription(string field)
        {
            string returnValue = null;
            string[] aGuiDescription = parent.Request("GetGUIDescription");

            for (int x = 0; x < aGuiDescription.Length; x++)
            {
                int splitIndex = aGuiDescription[x].IndexOf(':') + 1;
                if (splitIndex > 1)
                {
                    string resultField = aGuiDescription[x].Substring(0, splitIndex - 1).Replace(" ", "").ToLower();
                    if (resultField == field) returnValue = aGuiDescription[x].Substring(splitIndex, aGuiDescription[x].Length - splitIndex);
                }
            }

            return returnValue;
        }

        public string GetScreenshotBase64()
        {
            //string[] base64screenshot = parent.Request("takescreenshot", ";false;0;" + this.GetGuiDescription("width") + ";" + this.GetGuiDescription("height") + ";75;true;");
            string[] base64screenshot = parent.Request("takescreenshot", ";false;0;640;480;100;true;");

            return (base64screenshot == null) ? null : base64screenshot[0];
        }

        public Image Base64StringToImage(string base64String)
        {
            Bitmap file = null;
            byte[] bytes = Convert.FromBase64String(base64String);
            MemoryStream stream = new MemoryStream(bytes);

            if (base64String != null && base64String != "")
                file = new Bitmap(Image.FromStream(stream));

            return file;
        }

        public Image GetScreenshot()
        {
            Image screenshot = null;
            string base64ImageString = this.GetScreenshotBase64();

            if (base64ImageString != null && !base64ImageString.Contains("Error"))
                screenshot = this.Base64StringToImage(base64ImageString);

            return screenshot;
        }

        public void UpdateLibrary(string library)
        {
            if(library == "music" || library == "video")
                parent.Request("ExecBuiltIn", "updatelibrary(" + library + ")");
        }

        public bool SetResponseFormat()
        {
            string[] aResult = null;
            string ip = parent.GetIp();

            if (ip != null && ip != "")
                aResult = parent.Request("SetResponseFormat", null, ip);

            if (aResult == null)
                return false;
            else
            {
                return (aResult[0] == "OK") ? true : false;
            }
        }
    }
}
