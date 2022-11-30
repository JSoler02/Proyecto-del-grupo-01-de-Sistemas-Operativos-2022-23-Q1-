
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
            this.Juan120_But = new System.Windows.Forms.Button();
            this.Templo_But = new System.Windows.Forms.Button();
            this.conectar_bt = new System.Windows.Forms.Button();
            this.GridConectados = new System.Windows.Forms.DataGridView();
            this.tableroJuego = new System.Windows.Forms.Panel();
            this.bicho_pb = new System.Windows.Forms.PictureBox();
            this.lbl_lista_con = new System.Windows.Forms.Label();
            this.chatbox = new System.Windows.Forms.TextBox();
            this.chatGrid = new System.Windows.Forms.DataGridView();
            this.EnviarChatBut = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GridConectados)).BeginInit();
            this.tableroJuego.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bicho_pb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chatGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // desconnectButton
            // 
            this.desconnectButton.Location = new System.Drawing.Point(140, 583);
            this.desconnectButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.desconnectButton.Name = "desconnectButton";
            this.desconnectButton.Size = new System.Drawing.Size(200, 58);
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
            this.panel1.Location = new System.Drawing.Point(38, 41);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(440, 503);
            this.panel1.TabIndex = 6;
            // 
            // NewAccountButton
            // 
            this.NewAccountButton.Location = new System.Drawing.Point(102, 422);
            this.NewAccountButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.NewAccountButton.Name = "NewAccountButton";
            this.NewAccountButton.Size = new System.Drawing.Size(200, 53);
            this.NewAccountButton.TabIndex = 12;
            this.NewAccountButton.Text = "New Account";
            this.NewAccountButton.UseVisualStyleBackColor = true;
            this.NewAccountButton.Click += new System.EventHandler(this.NewAccountButton_Click);
            // 
            // LogInButton
            // 
            this.LogInButton.Location = new System.Drawing.Point(102, 342);
            this.LogInButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.LogInButton.Name = "LogInButton";
            this.LogInButton.Size = new System.Drawing.Size(200, 53);
            this.LogInButton.TabIndex = 11;
            this.LogInButton.Text = "Log In";
            this.LogInButton.UseVisualStyleBackColor = true;
            this.LogInButton.Click += new System.EventHandler(this.LogInButton_Click);
            // 
            // passwordBox
            // 
            this.passwordBox.Location = new System.Drawing.Point(177, 261);
            this.passwordBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.passwordBox.Name = "passwordBox";
            this.passwordBox.Size = new System.Drawing.Size(211, 31);
            this.passwordBox.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 214);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 25);
            this.label1.TabIndex = 7;
            this.label1.Text = "Usuario:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 269);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(129, 25);
            this.label2.TabIndex = 9;
            this.label2.Text = "Contraseña:";
            // 
            // usernameBox
            // 
            this.usernameBox.Location = new System.Drawing.Point(177, 209);
            this.usernameBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.usernameBox.Name = "usernameBox";
            this.usernameBox.Size = new System.Drawing.Size(211, 31);
            this.usernameBox.TabIndex = 8;
            // 
            // PuntMax_But
            // 
            this.PuntMax_But.Location = new System.Drawing.Point(33, 755);
            this.PuntMax_But.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.PuntMax_But.Name = "PuntMax_But";
            this.PuntMax_But.Size = new System.Drawing.Size(206, 108);
            this.PuntMax_But.TabIndex = 13;
            this.PuntMax_But.Text = "Puntuación Máxima de Maria";
            this.PuntMax_But.UseVisualStyleBackColor = true;
            this.PuntMax_But.Click += new System.EventHandler(this.PuntMax_But_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(26, 692);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(169, 37);
            this.label3.TabIndex = 14;
            this.label3.Text = "Consultas:";
            // 
            // Juan120_But
            // 
            this.Juan120_But.Location = new System.Drawing.Point(248, 755);
            this.Juan120_But.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Juan120_But.Name = "Juan120_But";
            this.Juan120_But.Size = new System.Drawing.Size(258, 108);
            this.Juan120_But.TabIndex = 15;
            this.Juan120_But.Text = "Partidas en las que Juan ha pasado más de 120 segundos";
            this.Juan120_But.UseVisualStyleBackColor = true;
            this.Juan120_But.Click += new System.EventHandler(this.Juan120_But_Click);
            // 
            // Templo_But
            // 
            this.Templo_But.Location = new System.Drawing.Point(514, 755);
            this.Templo_But.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Templo_But.Name = "Templo_But";
            this.Templo_But.Size = new System.Drawing.Size(248, 108);
            this.Templo_But.TabIndex = 16;
            this.Templo_But.Text = "Jugadores que han jugado como J1 en \"templo\"";
            this.Templo_But.UseVisualStyleBackColor = true;
            this.Templo_But.Click += new System.EventHandler(this.Templo_But_Click);
            // 
            // conectar_bt
            // 
            this.conectar_bt.Location = new System.Drawing.Point(6, 3);
            this.conectar_bt.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.conectar_bt.Name = "conectar_bt";
            this.conectar_bt.Size = new System.Drawing.Size(150, 36);
            this.conectar_bt.TabIndex = 18;
            this.conectar_bt.Text = "Conectar";
            this.conectar_bt.UseVisualStyleBackColor = true;
            this.conectar_bt.Click += new System.EventHandler(this.conectar_bt_Click);
            // 
            // GridConectados
            // 
            this.GridConectados.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridConectados.Location = new System.Drawing.Point(562, 102);
            this.GridConectados.Name = "GridConectados";
            this.GridConectados.RowHeadersWidth = 82;
            this.GridConectados.RowTemplate.Height = 33;
            this.GridConectados.Size = new System.Drawing.Size(182, 292);
            this.GridConectados.TabIndex = 19;
            this.GridConectados.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.GridConectados_CellContentDoubleClick);
            // 
            // tableroJuego
            // 
            this.tableroJuego.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tableroJuego.Controls.Add(this.bicho_pb);
            this.tableroJuego.Location = new System.Drawing.Point(908, 38);
            this.tableroJuego.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tableroJuego.Name = "tableroJuego";
            this.tableroJuego.Size = new System.Drawing.Size(844, 603);
            this.tableroJuego.TabIndex = 20;
            this.tableroJuego.MouseClick += new System.Windows.Forms.MouseEventHandler(this.tableroJuego_MouseClick);
            // 
            // bicho_pb
            // 
            this.bicho_pb.BackColor = System.Drawing.Color.Red;
            this.bicho_pb.Location = new System.Drawing.Point(218, 266);
            this.bicho_pb.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.bicho_pb.Name = "bicho_pb";
            this.bicho_pb.Size = new System.Drawing.Size(38, 48);
            this.bicho_pb.TabIndex = 0;
            this.bicho_pb.TabStop = false;
            // 
            // lbl_lista_con
            // 
            this.lbl_lista_con.AutoSize = true;
            this.lbl_lista_con.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_lista_con.Location = new System.Drawing.Point(501, 38);
            this.lbl_lista_con.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_lista_con.Name = "lbl_lista_con";
            this.lbl_lista_con.Size = new System.Drawing.Size(318, 37);
            this.lbl_lista_con.TabIndex = 14;
            this.lbl_lista_con.Text = "Lista de Conectados:";
            // 
            // chatbox
            // 
            this.chatbox.Location = new System.Drawing.Point(908, 798);
            this.chatbox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chatbox.Name = "chatbox";
            this.chatbox.Size = new System.Drawing.Size(726, 31);
            this.chatbox.TabIndex = 21;
            // 
            // chatGrid
            // 
            this.chatGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.chatGrid.Location = new System.Drawing.Point(908, 650);
            this.chatGrid.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chatGrid.Name = "chatGrid";
            this.chatGrid.RowHeadersWidth = 51;
            this.chatGrid.RowTemplate.Height = 24;
            this.chatGrid.Size = new System.Drawing.Size(844, 133);
            this.chatGrid.TabIndex = 22;
            // 
            // EnviarChatBut
            // 
            this.EnviarChatBut.Location = new System.Drawing.Point(1644, 792);
            this.EnviarChatBut.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.EnviarChatBut.Name = "EnviarChatBut";
            this.EnviarChatBut.Size = new System.Drawing.Size(108, 47);
            this.EnviarChatBut.TabIndex = 23;
            this.EnviarChatBut.Text = "Enviar";
            this.EnviarChatBut.UseVisualStyleBackColor = true;
            this.EnviarChatBut.Click += new System.EventHandler(this.EnviarChatBut_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1803, 883);
            this.Controls.Add(this.EnviarChatBut);
            this.Controls.Add(this.chatGrid);
            this.Controls.Add(this.chatbox);
            this.Controls.Add(this.tableroJuego);
            this.Controls.Add(this.GridConectados);
            this.Controls.Add(this.conectar_bt);
            this.Controls.Add(this.Templo_But);
            this.Controls.Add(this.Juan120_But);
            this.Controls.Add(this.lbl_lista_con);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.PuntMax_But);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.desconnectButton);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Main";
            this.Text = "Menú Principal";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Main_FormClosed);
            this.Load += new System.EventHandler(this.Main_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GridConectados)).EndInit();
            this.tableroJuego.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bicho_pb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chatGrid)).EndInit();
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
        private System.Windows.Forms.Button Juan120_But;
        private System.Windows.Forms.Button Templo_But;
        private System.Windows.Forms.Button conectar_bt;
        private System.Windows.Forms.DataGridView GridConectados;
        private System.Windows.Forms.Panel tableroJuego;
        private System.Windows.Forms.Label lbl_lista_con;
        private System.Windows.Forms.PictureBox bicho_pb;
        private System.Windows.Forms.DataGridView chatGrid;
        private System.Windows.Forms.Button EnviarChatBut;
        protected System.Windows.Forms.TextBox chatbox;
    }
}

