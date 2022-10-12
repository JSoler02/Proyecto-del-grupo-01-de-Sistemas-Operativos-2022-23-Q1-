
namespace ProyectoSO
{
    partial class Main
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.connectButton = new System.Windows.Forms.Button();
            this.desconnectButton = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.passwordBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.usernameBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.LogInButton = new System.Windows.Forms.Button();
            this.NewAccountButton = new System.Windows.Forms.Button();
            this.PuntMax_But = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.Juan120_But = new System.Windows.Forms.Button();
            this.Templo_But = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // connectButton
            // 
            this.connectButton.Location = new System.Drawing.Point(46, 374);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(111, 37);
            this.connectButton.TabIndex = 4;
            this.connectButton.Text = "Conectar";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // desconnectButton
            // 
            this.desconnectButton.Location = new System.Drawing.Point(185, 374);
            this.desconnectButton.Name = "desconnectButton";
            this.desconnectButton.Size = new System.Drawing.Size(116, 37);
            this.desconnectButton.TabIndex = 5;
            this.desconnectButton.Text = "Desconectar";
            this.desconnectButton.UseVisualStyleBackColor = true;
            this.desconnectButton.Click += new System.EventHandler(this.desconnectButton_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.NewAccountButton);
            this.panel1.Controls.Add(this.LogInButton);
            this.panel1.Controls.Add(this.passwordBox);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.usernameBox);
            this.panel1.Location = new System.Drawing.Point(25, 26);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(293, 306);
            this.panel1.TabIndex = 6;
            // 
            // passwordBox
            // 
            this.passwordBox.Location = new System.Drawing.Point(118, 167);
            this.passwordBox.Name = "passwordBox";
            this.passwordBox.Size = new System.Drawing.Size(142, 22);
            this.passwordBox.TabIndex = 10;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 172);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 17);
            this.label2.TabIndex = 9;
            this.label2.Text = "Contraseña:";
            // 
            // usernameBox
            // 
            this.usernameBox.Location = new System.Drawing.Point(118, 134);
            this.usernameBox.Name = "usernameBox";
            this.usernameBox.Size = new System.Drawing.Size(142, 22);
            this.usernameBox.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 137);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 17);
            this.label1.TabIndex = 7;
            this.label1.Text = "Usuario:";
            // 
            // LogInButton
            // 
            this.LogInButton.Location = new System.Drawing.Point(102, 219);
            this.LogInButton.Name = "LogInButton";
            this.LogInButton.Size = new System.Drawing.Size(99, 34);
            this.LogInButton.TabIndex = 11;
            this.LogInButton.Text = "Log In";
            this.LogInButton.UseVisualStyleBackColor = true;
            this.LogInButton.Click += new System.EventHandler(this.LogInButton_Click);
            // 
            // NewAccountButton
            // 
            this.NewAccountButton.Location = new System.Drawing.Point(102, 259);
            this.NewAccountButton.Name = "NewAccountButton";
            this.NewAccountButton.Size = new System.Drawing.Size(99, 34);
            this.NewAccountButton.TabIndex = 12;
            this.NewAccountButton.Text = "New Account";
            this.NewAccountButton.UseVisualStyleBackColor = true;
            this.NewAccountButton.Click += new System.EventHandler(this.NewAccountButton_Click);
            // 
            // PuntMax_But
            // 
            this.PuntMax_But.Location = new System.Drawing.Point(495, 77);
            this.PuntMax_But.Name = "PuntMax_But";
            this.PuntMax_But.Size = new System.Drawing.Size(150, 54);
            this.PuntMax_But.TabIndex = 13;
            this.PuntMax_But.Text = "Puntuación Máxima";
            this.PuntMax_But.UseVisualStyleBackColor = true;
            this.PuntMax_But.Click += new System.EventHandler(this.PuntMax_But_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(524, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(106, 25);
            this.label3.TabIndex = 14;
            this.label3.Text = "Consultas:";
            // 
            // Juan120_But
            // 
            this.Juan120_But.Location = new System.Drawing.Point(495, 172);
            this.Juan120_But.Name = "Juan120_But";
            this.Juan120_But.Size = new System.Drawing.Size(150, 69);
            this.Juan120_But.TabIndex = 15;
            this.Juan120_But.Text = "Partidas en las que Juan ha pasado más de 120 segundos";
            this.Juan120_But.UseVisualStyleBackColor = true;
            this.Juan120_But.Click += new System.EventHandler(this.Juan120_But_Click);
            // 
            // Templo_But
            // 
            this.Templo_But.Location = new System.Drawing.Point(495, 278);
            this.Templo_But.Name = "Templo_But";
            this.Templo_But.Size = new System.Drawing.Size(150, 70);
            this.Templo_But.TabIndex = 16;
            this.Templo_But.Text = "Jugadores que han jugado como J1 en \"templo\"";
            this.Templo_But.UseVisualStyleBackColor = true;
            this.Templo_But.Click += new System.EventHandler(this.Templo_But_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.Templo_But);
            this.Controls.Add(this.Juan120_But);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.PuntMax_But);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.desconnectButton);
            this.Controls.Add(this.connectButton);
            this.Name = "Main";
            this.Text = "Menú Principal";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.Button desconnectButton;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button NewAccountButton;
        private System.Windows.Forms.Button LogInButton;
        private System.Windows.Forms.TextBox passwordBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox usernameBox;
        private System.Windows.Forms.Button PuntMax_But;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button Juan120_But;
        private System.Windows.Forms.Button Templo_But;
    }
}

