using System;
using System.Windows.Forms;

namespace Lockout
{
    public partial class frmPass : Form
    {
        private DialogResult _passOk;
        public DialogResult passOk
        {
         get
        {
            return _passOk;;
        }
        }
        public frmPass()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {

            if (txtPass.Text.Trim() != "anupa" + System.DateTime.Today.Day)
                _passOk = DialogResult.No;
            else
            _passOk = DialogResult.OK;
            Close();
        }

        private void frmPass_Load(object sender, EventArgs e)
        {
            _passOk = DialogResult.No;
        }
    }
}
