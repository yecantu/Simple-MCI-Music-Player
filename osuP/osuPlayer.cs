using System;
using System.Runtime.InteropServices;
using System.Text;

namespace osuP
{
    class osuPlayer
    {    
        // Variables 
        bool paused;

        // Import dynamic link library, winmm, Windows Multimedia 
        [DllImport("winmm.dll")]

        // Media Control interface variable, to send commands
        private static extern long mciSendString(string lpstrCommand, StringBuilder lpstrReturnString, int uReturnLength, IntPtr hwndCallback);

        public osuPlayer()
        {
            paused = false;
        }

        // Open file
        public void open(string file)
        {
            mciSendString("open \"" + file + "\" type MPEGVideo alias MyMp3", null, 0, IntPtr.Zero);
        }

        // Play song 
        public void play()
        {  
            mciSendString("play MyMp3", null, 0, IntPtr.Zero);
            paused = false;
        }

        // Play song 
        public void play(string path, IntPtr handle)
        {

            open(path);
            mciSendString("play MyMp3 notify", null, 0, handle);
            paused = false;

        }

        // Pause song or resume
        public void pause(IntPtr handle)
        {
            if(paused == false)
            {
               
                mciSendString("pause MyMp3", null, 0, IntPtr.Zero);
                paused = true;
            }
            else
            {
                mciSendString("play MyMp3 notify", null, 0, handle);
                paused = false;
            }      
        }

        // Stop song
        public void stop()
        {     
            mciSendString("stop MyMp3", null, 0, IntPtr.Zero);
            mciSendString("close MyMp3", null, 0, IntPtr.Zero);
        }

        // Get length of song in milliseconds
        public string getLength(string path)
        {
            StringBuilder mssg = new StringBuilder(255);

            open(path);
            mciSendString("set MyMp3 time format ms", null, 0, IntPtr.Zero);
            mciSendString("status MyMp3 length", mssg, mssg.Capacity, IntPtr.Zero);

            return mssg.ToString();       
        }

        // Get current position of song
        public int getPosition()
        {
            StringBuilder mssg = new StringBuilder(255);
            mciSendString("status MyMp3 position", mssg, mssg.Capacity, IntPtr.Zero);

            return Int32.Parse(mssg.ToString());
        }

        // Skip to another part in song
        public void seek(int position, IntPtr handle)
        {
            mciSendString("play MyMp3 from " +  position.ToString() + " notify", null, 0, handle);
        }

        // Set volume for music player
        public void setVolume(int value)
        {
            mciSendString("setaudio MyMp3 volume to " + value.ToString(), null, 0, IntPtr.Zero);
        }

    }
}
