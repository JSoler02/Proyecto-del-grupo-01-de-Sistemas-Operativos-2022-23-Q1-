
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
            this.components = new System.ComponentModel.Container();
            this.desconnectButton = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.NewAccountButton = new System.Windows.Forms.Button();
            this.LogInButton = new System.Windows.Forms.Button();
            this.passwordBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.usernameBox = new System.Windows.Forms.TextBox();
            this.PuntMax_But = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.PartidasMapa_But = new System.Windows.Forms.Button();
            this.PartidasDia_But = new System.Windows.Forms.Button();
            this.GridConectados = new System.Windows.Forms.DataGridView();
            this.lbl_lista_con = new System.Windows.Forms.Label();
            this.CrearPartidaBut = new System.Windows.Forms.Button();
            this.mapaTbx = new System.Windows.Forms.TextBox();
            this.fechaTbox = new System.Windows.Forms.TextBox();
            this.timer_personaj = new System.Windows.Forms.Timer(this.components);
            this.timer_saludo = new System.Windows.Forms.Timer(this.components);
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GridConectados)).BeginInit();
            this.SuspendLayout();
            // 
            // desconnectButton
            // 
            this.desconnectButton.Location = new System.Drawing.Point(93, 373);
            this.desconnectButton.Name = "desconnectButton";
            this.desconnectButton.Size = new System.Drawing.Size(133, 37);
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
            this.panel1.Size = new System.Drawing.Size(293, 322);
            this.panel1.TabIndex = 6;
            // 
            // NewAccountButton
            // 
            this.NewAccountButton.Location = new System.Drawing.Point(68, 270);
            this.NewAccountButton.Name = "NewAccountButton";
            this.NewAccountButton.Size = new System.Drawing.Size(133, 34);
            this.NewAccountButton.TabIndex = 12;
            this.NewAccountButton.Text = "New Account";
            this.NewAccountButton.UseVisualStyleBackColor = true;
            this.NewAccountButton.Click += new System.EventHandler(this.NewAccountButton_Click);
            // 
            // LogInButton
            // 
            this.LogInButton.Location = new System.Drawing.Point(68, 219);
            this.LogInButton.Name = "LogInButton";
            this.LogInButton.Size = new System.Drawing.Size(133, 34);
            this.LogInButton.TabIndex = 11;
            this.LogInButton.Text = "Log In";
            this.LogInButton.UseVisualStyleBackColor = true;
            this.LogInButton.Click += new System.EventHandler(this.LogInButton_Click);
            // 
            // passwordBox
            // 
            this.passwordBox.Location = new System.Drawing.Point(118, 167);
            this.passwordBox.Name = "passwordBox";
            this.passwordBox.Size = new System.Drawing.Size(142, 22);
            this.passwordBox.TabIndex = 10;
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
            // PuntMax_But
            // 
            this.PuntMax_But.Location = new System.Drawing.Point(22, 483);
            this.PuntMax_But.Name = "PuntMax_But";
            this.PuntMax_But.Size = new System.Drawing.Size(137, 69);
            this.PuntMax_But.TabIndex = 13;
            this.PuntMax_But.Text = "Puntuación Máxima";
            this.PuntMax_But.UseVisualStyleBackColor = true;
            this.PuntMax_But.Click += new System.EventHandler(this.PuntMax_But_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(17, 443);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(106, 25);
            this.label3.TabIndex = 14;
            this.label3.Text = "Consultas:";
            // 
            // PartidasMapa_But
            // 
            this.PartidasMapa_But.Location = new System.Drawing.Point(165, 483);
            this.PartidasMapa_But.Name = "PartidasMapa_But";
            this.PartidasMapa_But.Size = new System.Drawing.Size(172, 69);
            this.PartidasMapa_But.TabIndex = 15;
            this.PartidasMapa_But.Text = "Partidas en el mapa (escribir abajo)";
            this.PartidasMapa_But.UseVisualStyleBackColor = true;
            this.PartidasMapa_But.Click += new System.EventHandler(this.PartidasMapa_But_Click);
            // 
            // PartidasDia_But
            // 
            this.PartidasDia_But.Location = new System.Drawing.Point(343, 483);
            this.PartidasDia_But.Name = "PartidasDia_But";
            this.PartidasDia_But.Size = new System.Drawing.Size(165, 69);
            this.PartidasDia_But.TabIndex = 16;
            this.PartidasDia_But.Text = "Partidas del dia (dd/mm/aa)";
            this.PartidasDia_But.UseVisualStyleBackColor = true;
            this.PartidasDia_But.Click += new System.EventHandler(this.PartidasDia_But_Click);
            // 
            // GridConectados
            // 
            this.GridConectados.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridConectados.Location = new System.Drawing.Point(375, 65);
            this.GridConectados.Margin = new System.Windows.Forms.Padding(2);
            this.GridConectados.Name = "GridConectados";
            this.GridConectados.RowHeadersWidth = 82;
            this.GridConectados.RowTemplate.Height = 33;
            this.GridConectados.Size = new System.Drawing.Size(121, 187);
            this.GridConectados.TabIndex = 19;
            this.GridConectados.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.GridConectados_CellContentDoubleClick);
            // 
            // lbl_lista_con
            // 
            this.lbl_lista_con.AutoSize = true;
            this.lbl_lista_con.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_lista_con.Location = new System.Drawing.Point(334, 24);
            this.lbl_lista_con.Name = "lbl_lista_con";
            this.lbl_lista_con.Size = new System.Drawing.Size(197, 25);
            this.lbl_lista_con.TabIndex = 14;
            this.lbl_lista_con.Text = "Lista de Conectados:";
            // 
            // CrearPartidaBut
            // 
            this.CrearPartidaBut.Location = new System.Drawing.Point(535, 65);
            this.CrearPartidaBut.Name = "CrearPartidaBut";
            this.CrearPartidaBut.Size = new System.Drawing.Size(72, 83);
            this.CrearPartidaBut.TabIndex = 23;
            this.CrearPartidaBut.Text = "Crear Partida";
            this.CrearPartidaBut.UseVisualStyleBackColor = true;
            this.CrearPartidaBut.Click += new System.EventHandler(this.CrearPartidaBut_Click);
            // 
            // mapaTbx
            // 
            this.mapaTbx.Location = new System.Drawing.Point(180, 558);
            this.mapaTbx.Name = "mapaTbx";
            this.mapaTbx.Size = new System.Drawing.Size(142, 22);
            this.mapaTbx.TabIndex = 13;
            // 
            // fechaTbox
            // 
            this.fechaTbox.Location = new System.Drawing.Point(354, 559);
            this.fechaTbox.Name = "fechaTbox";
            this.fechaTbox.Size = new System.Drawing.Size(142, 22);
            this.fechaTbox.TabIndex = 24;
            // 
            // timer_personaj
            // 
            this.timer_personaj.Interval = 1000;
            this.timer_personaj.Tick += new System.EventHandler(this.timer_personaj_Tick);
            // 
            // timer_saludo
            // 
            this.timer_saludo.Tick += new System.EventHandler(this.timer_saludo_Tick);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(694, 593);
            this.Controls.Add(this.fechaTbox);
            this.Controls.Add(this.mapaTbx);
            this.Controls.Add(this.CrearPartidaBut);
            this.Controls.Add(this.GridConectados);
            this.Controls.Add(this.PartidasDia_But);
            this.Controls.Add(this.PartidasMapa_But);
            this.Controls.Add(this.lbl_lista_con);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.PuntMax_But);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.desconnectButton);
            this.Name = "Main";
            this.Text = "Menú Principal";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Main_FormClosed);
            this.Load += new System.EventHandler(this.Main_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GridConectados)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
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
        private System.Windows.Forms.Button PartidasMapa_But;
        private System.Windows.Forms.Button PartidasDia_But;
        private System.Windows.Forms.DataGridView GridConectados;
        private System.Windows.Forms.Label lbl_lista_con;
        private System.Windows.Forms.Button CrearPartidaBut;
        private System.Windows.Forms.TextBox mapaTbx;
        private System.Windows.Forms.TextBox fechaTbox;
        private System.Windows.Forms.Timer timer_personaj;
        private System.Windows.Forms.PictureBox J1;
        private System.Windows.Forms.PictureBox J2;
        private System.Windows.Forms.PictureBox J3;
        private System.Windows.Forms.PictureBox J4;
        private System.Windows.Forms.Timer timer_saludo;
    }
}

