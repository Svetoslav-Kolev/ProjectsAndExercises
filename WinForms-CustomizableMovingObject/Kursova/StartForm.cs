using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kursova
{
    public partial class StartForm : Form
    {
        public StartForm()
        {
            InitializeComponent();
        }

        private void btnInfoForm_Click(object sender, EventArgs e)
        {
            Form infoForm = new InformationForm();
            this.Hide();
            infoForm.ShowDialog();
            this.Close();

        }

        private void btnStartApp_Click(object sender, EventArgs e)
        {
            Form MainForm = new MovingStuff();
            this.Hide();
            MainForm.ShowDialog();
            this.Close();

        }

        private void btnExitStartForm_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
