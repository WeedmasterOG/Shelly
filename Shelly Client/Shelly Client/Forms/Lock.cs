using System;
using System.Windows.Forms;

namespace Shelly_Client.Forms
{
    public partial class Lock : Form
    {
        public Lock()
        {
            InitializeComponent();
        }

        private void Lock_Load(object sender, EventArgs e)
        {
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            TopMost = true;
        }
    }
}
