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
                cmd.CommandText = "Select count(*) from user where username " +
                    "= '" + txtNombre.Text + "' and password ='" + txtClave.Text + "'";
                int valor = int.Parse(cmd.ExecuteScalar().ToString());
                if (valor == 1)
                {
                    MessageBox.Show("Se ha ingresado correctamente");
                    this.Hide();
                    Home fr_report = new Home();
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



        private void pictureBox1_Click_1(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click_1(object sender, EventArgs e)
        {

        }

        private void txtNombre_Enter(object sender, EventArgs e)
        {
            if (txtNombre.Text=="USUARIO") {
                txtNombre.Text = "";
                txtNombre.ForeColor = Color.LightGray;
                txtNombre.Text.ToUpper();
            }


        }

        private void txtNombre_Leave(object sender, EventArgs e)
        {
            if (txtNombre.Text == "")
            {
                txtNombre.Text = "USUARIO";
                txtNombre.ForeColor = Color.DimGray;
                txtNombre.Text.ToUpper();
            }
        }

        private void txtClave_Enter(object sender, EventArgs e)
        {
            if (txtClave.Text == "CLAVE")
            {
                txtClave.Text = "";
                txtClave.ForeColor = Color.LightGray;
                txtClave.UseSystemPasswordChar = true;
            }


        }

        private void txtClave_Leave(object sender, EventArgs e)
        {
            if (txtClave.Text == "")
            {
                txtClave.Text = "CLAVE";
                txtClave.ForeColor = Color.DimGray;
                txtClave.UseSystemPasswordChar = false;
            }
        }
            
        private void txtNombre_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
