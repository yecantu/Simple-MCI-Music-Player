using System;
using System.Windows.Forms;
using System.IO;

namespace osuP
{
    public partial class Form1 : Form
    {
        osuPlayer p = new osuPlayer();

        int currentIndex;
        bool playing;

        private const int MM_MCINOTIFY = 0x3B9;
        private const int MCI_NOTIFY_SUCCESS = 0x01;
        private const int MCI_NOTIFY_SUPERSEDED = 0x02;
        private const int MCI_NOTIFY_ABORTED = 0x04;
        private const int MCI_NOTIFY_FAILURE = 0x08;

        public Form1()
        {
            InitializeComponent();

            // Get song list
            getFiles();

            // Set Volume
            setVolumeBar();

            // Set to first song
            listView1.Items[0].Selected = true;


            // p.play();
            

           // playing = true;
            p.pause(this.Handle);

            pictureBox3.Image = Properties.Resources.pause;       

        }



        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (rightClick == false)
            //{

                // Stop previous song
                p.stop();


                if (listView1.SelectedItems.Count > 0)
                {

                    ListViewItem selected = listView1.SelectedItems[0];
                    string selectedFilePath = selected.Tag.ToString();

                    //label1.Text = p.getLength(selectedFilePath);
                    startTrackBar(selectedFilePath);

                    currentIndex = selected.Index;

                    label1.Text = Path.GetFileNameWithoutExtension(selectedFilePath);

                    p.play(selectedFilePath, this.Handle);

                }
                else
                {
                    p.stop();
                    label1.Text = "";
                    stopTrackBar();
                }
           // }
            //else
            //{
                // Show a message
            //}

            //}
            //else
            //{
                //listView1.SelectedItems.Clear();
            //}
          
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == MM_MCINOTIFY)
            {
                switch (m.WParam.ToInt32())
                {
                    case MCI_NOTIFY_SUCCESS:
                        // success handling
                        //If currentIndex + 1 is out of bounds go to index 0
                        // else switch to next song
                        if(listView1.Items.Count-1 > currentIndex)
                        {
                            listView1.Items[listView1.SelectedItems[0].Index + 1].Selected = true;
                            
                        }
                        else
                        {
                            listView1.Items[0].Selected = true;
                        }
                        
                        break;
                    case MCI_NOTIFY_SUPERSEDED:
                        // superseded handling
                        
                        break;
                    case MCI_NOTIFY_ABORTED:
                        // abort handling
                       
                        break;
                    case MCI_NOTIFY_FAILURE:
                        // failure! handling
                        break;
                    default:
                        // haha
                        break;
                }
            }
          
            base.WndProc(ref m);
        }
       
        // @TODO Get files from users osu folder recursively
        public void getFiles()
        {
            listView1.Items.Clear();

            listView1.View = View.List;

            string[] files = Directory.GetFiles(@"C:\Users\Public\Music\Sample Music", "*.mp3");

            foreach (string song in files)
            {
                string fileName = Path.GetFileNameWithoutExtension(song);
                ListViewItem item = new ListViewItem(fileName);
                item.Tag = song;

                listView1.Items.Add(item);
            }

            listView1.Show();
        }

        // Start track bar using timer1
        private void startTrackBar(string filePath)
        {
            timer1.Enabled = true;
            timer1.Start();
            timer1.Interval = 1000;

            trackBar2.Maximum = Int32.Parse(p.getLength((filePath)))/1000;
            trackBar2.Minimum = 0;
            trackBar2.Value = 0;

            trackBar2.Scroll += new System.EventHandler(trackBar2_Scroll);
            this.Controls.Add(this.trackBar2);

            timer1.Tick += new EventHandler(timer1_Tick);

            //@TODO  Timer 2 meant for label animation
            timer2.Enabled = true;
            timer2.Start();
            timer2.Interval = 1000;  

            timer2.Tick += new EventHandler(timer2_Tick);
        }

        // Stop track bar 
        private void stopTrackBar()
        {
            timer1.Stop();  
            trackBar2.Value = 0;   
        }

        // Timer for progress track bar
        private void timer1_Tick(object sender, EventArgs e)
        {
            if(listView1.SelectedItems.Count != 0)
            {
                if (trackBar2.Value != Int32.Parse(p.getLength((listView1.SelectedItems[0].Tag.ToString())))/1000)
                {
                    trackBar2.Value = p.getPosition()/1000;
                }
                else
                {
                    timer1.Stop();
                }
            }
        }
         

        // Event listener for progress track bar, seek function
        private void trackBar2_Scroll(object sender, EventArgs e)
        {
               p.seek(trackBar2.Value*1000, this.Handle);            
        }

        // Set volume to 500 (midway)
        private void setVolumeBar()
        {
            trackBar1.Maximum = 1000;
            trackBar1.Minimum = 0;

            trackBar1.Scroll += new System.EventHandler(trackBar1_Scroll);
            this.Controls.Add(this.trackBar2);

            trackBar1.Value = 500;
        }

        // Event listener for volume bar
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
                       
            p.setVolume(trackBar1.Value);

        }

        // Next song
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (listView1.Items.Count - 1 > currentIndex)
            {
                listView1.Items[listView1.SelectedItems[0].Index + 1].Selected = true;

            }
            else
            {
                listView1.Items[0].Selected = true;
            }

        }

        // Previous song
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count - 1 > 0)
            {
                listView1.Items[listView1.SelectedItems[0].Index - 1].Selected = true;

            }
            else
            {
                listView1.Items[listView1.Items.Count - 1].Selected = true;
            }
        }

        // Play/pause button
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            if (playing == true)
            {
                p.pause(this.Handle);
                playing = false;  
                
                pictureBox3.Image = null;
                pictureBox3.Image = Properties.Resources.play;
                pictureBox3.Refresh();

            }
            else
            {
                p.pause(this.Handle);
                playing = true;

                pictureBox3.Image = null;
                pictureBox3.Image = Properties.Resources.pause ;
                pictureBox3.Refresh();
                
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
      

        }
    }
    }

