using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace osuP
{
    public partial class Form1 : Form
    {
        osuPlayer p = new osuPlayer();

        int currentIndex;

        bool playing;

        bool rightClick;

        Form notify = new Form();

        private const int MM_MCINOTIFY = 0x3B9;
        private const int MCI_NOTIFY_SUCCESS = 0x01;
        private const int MCI_NOTIFY_SUPERSEDED = 0x02;
        private const int MCI_NOTIFY_ABORTED = 0x04;
        private const int MCI_NOTIFY_FAILURE = 0x08;

        public Form1()
        {
            InitializeComponent();

            //Get song list
            getFiles();

            setVolumeBar();

            //Set to first song
            listView1.Items[0].Selected = true;


            // p.play();
            //  p.pause(this.Handle);

            playing = true;
            pictureBox3.Image = Properties.Resources.pause;

         

        }

       

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            label1.Text = openFileDialog1.FileName;
            p.open(openFileDialog1.FileName);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            p.play();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            p.stop();
        }

        // Previous Song
        private void button4_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count-1 > 0)
            {
                listView1.Items[listView1.SelectedItems[0].Index -1].Selected = true;

            }
            else
            {
                listView1.Items[listView1.Items.Count-1].Selected = true;
            }
        }

        // Next Song
        private void button5_Click(object sender, EventArgs e)
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

        private void listView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                //Should do the sorting thing here
                rightClick = true;
            }
            else
            {
                rightClick = false;
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if(rightClick == false)
            //{
                p.stop();

                
                if (listView1.SelectedItems.Count > 0)
                {
                   
                    ListViewItem selected = listView1.SelectedItems[0];
                    string selectedFilePath = selected.Tag.ToString();

                    //label1.Text = p.getLength(selectedFilePath);
                    startProgressBar1(selectedFilePath);

                    currentIndex = selected.Index;

                    label1.Text = Path.GetFileNameWithoutExtension(selectedFilePath);
                  
                    p.play(selectedFilePath, this.Handle);

                 }
                 else
                 {
                    // Show a message
                 }
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

        public void openFile()
        {

        }

        //Pause Music
        private void button6_Click(object sender, EventArgs e)
        {
            if (playing == true)
            {
                p.pause(this.Handle);
                playing = false;
            }
            else
            {
                p.pause(this.Handle);
                playing = true;
            }
           
        }

        private void startProgressBar1(string filePath)
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

            timer2.Enabled = true;
            timer2.Start();
            timer2.Interval = 1000;  

            timer2.Tick += new EventHandler(timer2_Tick);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(trackBar2.Value != Int32.Parse(p.getLength((listView1.SelectedItems[0].Tag.ToString())))/1000)
            {
                trackBar2.Value = p.getPosition()/1000;
            }
            else
            {
                timer1.Stop();
            }
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
               p.seek(trackBar2.Value*1000, this.Handle);            
        }

        private void setVolumeBar()
        {
            trackBar1.Maximum = 1000;
            trackBar1.Minimum = 0;

            trackBar1.Scroll += new System.EventHandler(trackBar1_Scroll);
            this.Controls.Add(this.trackBar2);

            trackBar1.Value = 500;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            
            p.setVolume(trackBar1.Value);

         

        }

        //Next song
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

        //Previous song
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

        private void trackBar1_Scroll_1(object sender, EventArgs e)
        {

        }

        private void timer2_Tick(object sender, EventArgs e)
        {
      

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
    }

