using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using MySql.Data;

namespace CV5
{
    public partial class Login : Form
    {
        MySqlConnection cn = new MySqlConnection("Server = localhost; Uid= root; Password=root; Database= cv5_db; ");
        MySqlCommand cmd = new MySqlCommand();

        public Login()
        {
            InitializeComponent();
            txtCodigo.Select();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Ingreso();
        }

        private void Ingreso()
        {
            cn.Open();
            cmd.Connection = cn;
            try
            {
                cmd.CommandText = "Select count(*) from Usuarios where Codigo " +
                    "= '" + txtCodigo.Text + "' and Username ='" + txtNombre.Text +
                    "' and Clave ='" + txtClave.Text + "'";
                int valor = int.Parse(cmd.ExecuteScalar().ToString());
                if (valor == 1)
                {
                    MessageBox.Show("Se ha ingresado correctamente");
                    this.Hide();
                    Form1 fr_report = new Form1();
                    fr_report.MdiParent = this.ParentForm;
                    fr_report.Show();
                }
                else
                {
                    MessageBox.Show("Usuario invalido, favor vuelva a intentar");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message + " . Favor contacte con el administrador.");
            }
            cn.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                Ingreso();
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }


        private void txtCod_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
    }
}
