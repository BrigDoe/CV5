using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CV5.Roles
{
    public partial class frmCargando : Form
    {
        
        public frmCargando()
        {
            InitializeComponent();
        }

        private void frmCargando_Load(object sender, EventArgs e)
        {
            progressBar1.Style = ProgressBarStyle.Marquee;
        }

        public void Carga(ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        public void CorrerComparacion()
        {
            progressBar1.MarqueeAnimationSpeed=30;
            progressBar1.Show();
        }

        public void PararComparacion()
        {
            progressBar1.MarqueeAnimationSpeed = 0;
        }

    }
}
