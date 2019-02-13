namespace CV5.Roles
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.lstUsers = new System.Windows.Forms.ListBox();
            this.btnBorrarUsuario = new System.Windows.Forms.Button();
            this.txtNuevoNombre = new System.Windows.Forms.TextBox();
            this.btnAgregarUsuario = new System.Windows.Forms.Button();
            this.txtNuevaPassword = new System.Windows.Forms.TextBox();
            this.txtNuevoEmail = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnForwardARoles = new System.Windows.Forms.Button();
            this.btnBackToUsers = new System.Windows.Forms.Button();
            this.lstRoles = new System.Windows.Forms.ListBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.txtNuevoRol = new System.Windows.Forms.TextBox();
            this.btnBorrarRol = new System.Windows.Forms.Button();
            this.btnRenombarRol = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(120, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Usuarios";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // lstUsers
            // 
            this.lstUsers.FormattingEnabled = true;
            this.lstUsers.ItemHeight = 20;
            this.lstUsers.Location = new System.Drawing.Point(124, 73);
            this.lstUsers.Name = "lstUsers";
            this.lstUsers.Size = new System.Drawing.Size(201, 124);
            this.lstUsers.TabIndex = 1;
            // 
            // btnBorrarUsuario
            // 
            this.btnBorrarUsuario.Location = new System.Drawing.Point(360, 73);
            this.btnBorrarUsuario.Name = "btnBorrarUsuario";
            this.btnBorrarUsuario.Size = new System.Drawing.Size(118, 33);
            this.btnBorrarUsuario.TabIndex = 2;
            this.btnBorrarUsuario.Text = "Borrar";
            this.btnBorrarUsuario.UseVisualStyleBackColor = true;
            // 
            // txtNuevoNombre
            // 
            this.txtNuevoNombre.Location = new System.Drawing.Point(124, 231);
            this.txtNuevoNombre.Name = "txtNuevoNombre";
            this.txtNuevoNombre.Size = new System.Drawing.Size(201, 26);
            this.txtNuevoNombre.TabIndex = 3;
            // 
            // btnAgregarUsuario
            // 
            this.btnAgregarUsuario.Location = new System.Drawing.Point(360, 228);
            this.btnAgregarUsuario.Name = "btnAgregarUsuario";
            this.btnAgregarUsuario.Size = new System.Drawing.Size(140, 33);
            this.btnAgregarUsuario.TabIndex = 4;
            this.btnAgregarUsuario.Text = " Agregar nuevo";
            this.btnAgregarUsuario.UseVisualStyleBackColor = true;
            this.btnAgregarUsuario.Click += new System.EventHandler(this.btnAgregarUsuario_Click);
            // 
            // txtNuevaPassword
            // 
            this.txtNuevaPassword.Location = new System.Drawing.Point(124, 263);
            this.txtNuevaPassword.Name = "txtNuevaPassword";
            this.txtNuevaPassword.Size = new System.Drawing.Size(201, 26);
            this.txtNuevaPassword.TabIndex = 5;
            // 
            // txtNuevoEmail
            // 
            this.txtNuevoEmail.Location = new System.Drawing.Point(124, 295);
            this.txtNuevoEmail.Name = "txtNuevoEmail";
            this.txtNuevoEmail.Size = new System.Drawing.Size(201, 26);
            this.txtNuevoEmail.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(33, 234);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 20);
            this.label2.TabIndex = 7;
            this.label2.Text = "Usuarios";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(33, 266);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 20);
            this.label3.TabIndex = 8;
            this.label3.Text = "Password";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(33, 298);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 20);
            this.label4.TabIndex = 9;
            this.label4.Text = "Email";
            // 
            // btnForwardARoles
            // 
            this.btnForwardARoles.Location = new System.Drawing.Point(482, 125);
            this.btnForwardARoles.Name = "btnForwardARoles";
            this.btnForwardARoles.Size = new System.Drawing.Size(91, 28);
            this.btnForwardARoles.TabIndex = 10;
            this.btnForwardARoles.Text = ">>";
            this.btnForwardARoles.UseVisualStyleBackColor = true;
            // 
            // btnBackToUsers
            // 
            this.btnBackToUsers.Location = new System.Drawing.Point(482, 159);
            this.btnBackToUsers.Name = "btnBackToUsers";
            this.btnBackToUsers.Size = new System.Drawing.Size(91, 28);
            this.btnBackToUsers.TabIndex = 11;
            this.btnBackToUsers.Text = "<<";
            this.btnBackToUsers.UseVisualStyleBackColor = true;
            // 
            // lstRoles
            // 
            this.lstRoles.FormattingEnabled = true;
            this.lstRoles.ItemHeight = 20;
            this.lstRoles.Location = new System.Drawing.Point(635, 73);
            this.lstRoles.Name = "lstRoles";
            this.lstRoles.Size = new System.Drawing.Size(201, 124);
            this.lstRoles.TabIndex = 13;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(631, 38);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(50, 20);
            this.label5.TabIndex = 12;
            this.label5.Text = "Roles";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(587, 234);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(33, 20);
            this.label6.TabIndex = 16;
            this.label6.Text = "Rol";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(876, 228);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(145, 33);
            this.button2.TabIndex = 15;
            this.button2.Text = "Agregar nuevo";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // txtNuevoRol
            // 
            this.txtNuevoRol.Location = new System.Drawing.Point(640, 231);
            this.txtNuevoRol.Name = "txtNuevoRol";
            this.txtNuevoRol.Size = new System.Drawing.Size(201, 26);
            this.txtNuevoRol.TabIndex = 14;
            // 
            // btnBorrarRol
            // 
            this.btnBorrarRol.Location = new System.Drawing.Point(876, 73);
            this.btnBorrarRol.Name = "btnBorrarRol";
            this.btnBorrarRol.Size = new System.Drawing.Size(118, 33);
            this.btnBorrarRol.TabIndex = 17;
            this.btnBorrarRol.Text = "Borrar";
            this.btnBorrarRol.UseVisualStyleBackColor = true;
            // 
            // btnRenombarRol
            // 
            this.btnRenombarRol.Location = new System.Drawing.Point(876, 125);
            this.btnRenombarRol.Name = "btnRenombarRol";
            this.btnRenombarRol.Size = new System.Drawing.Size(118, 33);
            this.btnRenombarRol.TabIndex = 18;
            this.btnRenombarRol.Text = "Renombrar";
            this.btnRenombarRol.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(1167, 38);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(132, 20);
            this.label7.TabIndex = 19;
            this.label7.Text = "Usuarios en roles";
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(1142, 73);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(197, 188);
            this.treeView1.TabIndex = 20;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1393, 338);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.btnRenombarRol);
            this.Controls.Add(this.btnBorrarRol);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.txtNuevoRol);
            this.Controls.Add(this.lstRoles);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnBackToUsers);
            this.Controls.Add(this.btnForwardARoles);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtNuevoEmail);
            this.Controls.Add(this.txtNuevaPassword);
            this.Controls.Add(this.btnAgregarUsuario);
            this.Controls.Add(this.txtNuevoNombre);
            this.Controls.Add(this.btnBorrarUsuario);
            this.Controls.Add(this.lstUsers);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Manejo de roles";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lstUsers;
        private System.Windows.Forms.Button btnBorrarUsuario;
        private System.Windows.Forms.TextBox txtNuevoNombre;
        private System.Windows.Forms.Button btnAgregarUsuario;
        private System.Windows.Forms.TextBox txtNuevaPassword;
        private System.Windows.Forms.TextBox txtNuevoEmail;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnForwardARoles;
        private System.Windows.Forms.Button btnBackToUsers;
        private System.Windows.Forms.ListBox lstRoles;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox txtNuevoRol;
        private System.Windows.Forms.Button btnBorrarRol;
        private System.Windows.Forms.Button btnRenombarRol;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TreeView treeView1;
    }
}