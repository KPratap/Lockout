using System;
using System.Windows.Forms;

namespace Lockout
{
    public partial class frmMsg : Form
    {
        public int _time;
        private static System.Timers.Timer tminus0;
        public frmMsg(int min)
        {
            InitializeComponent();
            _time = min;
        }

        private void frmMsg_Load(object sender, EventArgs e)
        {
            if (_time == 0) this.Close();
            this.Activate();
            lblMsg.Text = _time > 1 ? _time + " minutes!" : _time + " minute!";
            tminus0 = GetTimer(5);
            tminus0.SynchronizingObject = this;
            tminus0.Elapsed += Min_Elapsed;
            tminus0.Start();
        }

        private void Min_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.Close();
        }
        private System.Timers.Timer GetTimer(int sec)
        {
            System.Timers.Timer t = new System.Timers.Timer(sec * 1000);
            return t;
        }
    }
}
