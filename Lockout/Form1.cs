using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Timers;
namespace Lockout
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll")]
        public static extern void LockWorkStation();
        private static System.Timers.Timer tminus0;
        private static System.Timers.Timer tminus5;
        private static System.Timers.Timer tminus3;
        private static System.Timers.Timer tminus2;
        private static System.Timers.Timer tminus1;
        private static DateTime startTime;
        private static DateTime endTime;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            txtTime.Enabled = false;
            startTime = DateTime.Now;
            int min = 0;
            if (!int.TryParse(txtTime.Text, out min))
            {
                min = 15;
            }
            if (min < 5)
            {
                min = 15;
            }
            txtTime.Text = min.ToString();

            endTime = startTime.AddMinutes(min);
            lblMsg.Text = "End time is " + endTime;
            btnStart.Enabled = false;
            tminus0 = GetTimer(min);
            tminus0.Elapsed += Min_Elapsed;
            if (min > 5)
            {
                tminus5 = GetTimer(min - 5);
                tminus5.Elapsed += Min_Elapsed3;
                tminus5.SynchronizingObject = this;
                StartTimer(tminus5);
            }
            if (min > 3)
            {
                tminus3 = GetTimer(min - 3);
                tminus3.Elapsed += Min_Elapsed3;
                tminus3.SynchronizingObject = this;
                tminus2 = GetTimer(min - 2);
                tminus2.Elapsed += Min_Elapsed2;
                tminus2.SynchronizingObject = this;
                tminus1 = GetTimer(min - 1);
                tminus1.Elapsed += Min_Elapsed1;
                tminus1.SynchronizingObject = this;
                tminus0.SynchronizingObject = this;
                StartTimer(tminus3);
                StartTimer(tminus2);
                StartTimer(tminus1);
            }

            StartTimer(tminus0);
            notifyIcon1.Visible = true;
            notifyIcon1.ShowBalloonTip(500);
            this.Hide();
        }

        private void Min_Elapsed(object sender, ElapsedEventArgs e)
        {

            lblMsg.Text = e.SignalTime.ToString() + " : " + endTime.ToString();
            TimeOver(0, true);

        }
        private void Min_Elapsed3(object sender, ElapsedEventArgs e)
        {

            lblMsg.Text = e.SignalTime + " : " + endTime.ToString();
            TimeOver(3, false);
            var t= sender as System.Timers.Timer;
            t.Stop();

            
        }
        private void Min_Elapsed2(object sender, ElapsedEventArgs e)
        {

            lblMsg.Text = e.SignalTime + " : " + endTime;
            TimeOver(2, false);
            var t = sender as System.Timers.Timer;
            t.Stop();
        }
        private void Min_Elapsed1(object sender, ElapsedEventArgs e)
        {

            lblMsg.Text = e.SignalTime + " : " + endTime;
            TimeOver(1, false);
            var t = sender as System.Timers.Timer;
            t.Stop();
        }
        private void StartTimer(System.Timers.Timer t)
        {
            t.Enabled = true;
            t.Start();
        }
        private System.Timers.Timer GetTimer(int min)
        {
            var t = new System.Timers.Timer(min*60000);
            return t;
        }
        private void TimeOver(int minLeft, bool lockit)
        {
            if (lockit)
            {
                txtTime.Enabled = true;
                btnStart.Enabled = true;
                lblMsg.Text = string.Empty;
                LockWorkStation();
                return;
            }
            frmMsg m = new frmMsg(minLeft);
            m.ShowDialog();

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            
            var pw = new frmPass();
            DialogResult dr = pw.ShowDialog();
            dr = pw.passOk;
            if (dr == System.Windows.Forms.DialogResult.No)
                e.Cancel = true;
            notifyIcon1.Dispose();
        }

        private void txtTime_TextChanged(object sender, EventArgs e)
        {
            int min; 
            if (int.TryParse(txtTime.Text.Trim().ToString(), out min))
            {
                if (min > 15)
                {
                    var pw = new frmPass();
                    DialogResult dr = pw.ShowDialog();
                    dr = pw.passOk;
                    if (dr == System.Windows.Forms.DialogResult.No)
                        txtTime.Text = string.Empty;
                    else
                        txtTime.Text = min.ToString();

                }
            }
            else 
                txtTime.Text = string.Empty;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {


            if (FormWindowState.Minimized == this.WindowState)
            {
                notifyIcon1.Visible = true;
                notifyIcon1.ShowBalloonTip(500);
                this.Hide();
            }
            else if (FormWindowState.Normal == this.WindowState)
            {
                notifyIcon1.Visible = false;
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            notifyIcon1.BalloonTipTitle = "Lockout";
            notifyIcon1.BalloonTipText = "Double-clock to show.";
        }

    }
}
