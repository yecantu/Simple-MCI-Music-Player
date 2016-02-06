using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace osuP
{
    class osuPlayer
    {    
        Form end = new Form();

        bool paused;

        [DllImport("winmm.dll")]
        private static extern long mciSendString(string lpstrCommand, StringBuilder lpstrReturnString, int uReturnLength, IntPtr hwndCallback);

        public osuPlayer()
        {
            paused = false;
        }

        public void open(string file)
        {
            //string command = "open \"" + file + "\" type MPEGVideo alias MyMp3";
            //mciSendString(command, null, 0, 0);
        }

        public void play()
        {
            //string command = "play MyMp3";
            //mciSendString(command, null, 0, 0);
        }

        public void play(string path, IntPtr handle)
        {
           // this.end = cb;

            string command = "open \"" + path + "\" type MPEGVideo alias MyMp3";
            mciSendString(command, null, 0, IntPtr.Zero);
            command = "play MyMp3 notify";
            mciSendString(command, null, 0, handle);

                 
            //mciSendString(command, null, 0, );
            
        }

        public void pause(IntPtr handle)
        {
            string command;

            if(paused == false)
            {
                command = "pause MyMp3";
                mciSendString(command, null, 0, IntPtr.Zero);
                paused = true;
            }
            else
            {
                command = "play MyMp3 notify";
                mciSendString(command, null, 0, handle);
                paused = false;
            }
          
        }

        public void stop()
        {
            string command = "stop MyMp3";
            mciSendString(command, null, 0, IntPtr.Zero);

            command = "close MyMp3";
            mciSendString(command, null, 0, IntPtr.Zero);
        }

        public string getLength(string path)
        {
            StringBuilder mssg = new StringBuilder(255);

            string command = "open \"" + path + "\" type MPEGVideo alias MyMp3";
            mciSendString(command, null, 0, IntPtr.Zero);
            mciSendString("set MyMp3 time format ms", null, 0, IntPtr.Zero);
            mciSendString("status MyMp3 length", mssg, mssg.Capacity, IntPtr.Zero);

            return mssg.ToString();       
        }

        public int getPosition()
        {
            StringBuilder mssg = new StringBuilder(255);
            mciSendString("status MyMp3 position", mssg, mssg.Capacity, IntPtr.Zero);

            return Int32.Parse(mssg.ToString());
        }

        public void seek(int position, IntPtr handle)
        {
            mciSendString("play MyMp3 from " +  position.ToString() + " notify", null, 0, handle);
        }

        public void setVolume(int value)
        {
            mciSendString("setaudio MyMp3 volume to " + value.ToString(), null, 0, IntPtr.Zero);
        }

    }
}
