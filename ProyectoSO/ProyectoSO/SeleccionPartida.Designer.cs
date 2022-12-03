namespace ProyectoSO
{
    partial class SeleccionPartida
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
            this.EnviarChatBut = new System.Windows.Forms.Button();
            this.chatGrid = new System.Windows.Forms.DataGridView();
            this.chatbox = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.map_escogido_lbl = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBox_Mapa = new System.Windows.Forms.ComboBox();
            this.empezarPartida_but = new System.Windows.Forms.Button();
            this.SelJ4_but = new System.Windows.Forms.Button();
            this.SelJ3_but = new System.Windows.Forms.Button();
            this.SelJ2_but = new System.Windows.Forms.Button();
            this.SelJ1_but = new System.Windows.Forms.Button();
            this.usuario4 = new System.Windows.Forms.Label();
            this.usuario3 = new System.Windows.Forms.Label();
            this.usuario2 = new System.Windows.Forms.Label();
            this.usuario1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.chatGrid)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            this.SuspendLayout();
            // 
            // EnviarChatBut
            // 
            this.EnviarChatBut.Location = new System.Drawing.Point(755, 558);
            this.EnviarChatBut.Name = "EnviarChatBut";
            this.EnviarChatBut.Size = new System.Drawing.Size(72, 30);
            this.EnviarChatBut.TabIndex = 33;
            this.EnviarChatBut.Text = "Enviar";
            this.EnviarChatBut.UseVisualStyleBackColor = true;
            this.EnviarChatBut.Click += new System.EventHandler(this.EnviarChatBut_Click);
            // 
            // chatGrid
            // 
            this.chatGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.chatGrid.Location = new System.Drawing.Point(264, 467);
            this.chatGrid.Name = "chatGrid";
            this.chatGrid.RowHeadersWidth = 51;
            this.chatGrid.RowTemplate.Height = 24;
            this.chatGrid.Size = new System.Drawing.Size(563, 85);
            this.chatGrid.TabIndex = 32;
            // 
            // chatbox
            // 
            this.chatbox.Location = new System.Drawing.Point(264, 562);
            this.chatbox.Name = "chatbox";
            this.chatbox.Size = new System.Drawing.Size(485, 22);
            this.chatbox.TabIndex = 31;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Silver;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.map_escogido_lbl);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.comboBox_Mapa);
            this.panel1.Controls.Add(this.empezarPartida_but);
            this.panel1.Controls.Add(this.SelJ4_but);
            this.panel1.Controls.Add(this.SelJ3_but);
            this.panel1.Controls.Add(this.SelJ2_but);
            this.panel1.Controls.Add(this.SelJ1_but);
            this.panel1.Controls.Add(this.usuario4);
            this.panel1.Controls.Add(this.usuario3);
            this.panel1.Controls.Add(this.usuario2);
            this.panel1.Controls.Add(this.usuario1);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.pictureBox2);
            this.panel1.Controls.Add(this.pictureBox3);
            this.panel1.Controls.Add(this.pictureBox4);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(100, 46);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(876, 415);
            this.panel1.TabIndex = 30;
            // 
            // map_escogido_lbl
            // 
            this.map_escogido_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.map_escogido_lbl.Location = new System.Drawing.Point(311, 379);
            this.map_escogido_lbl.Name = "map_escogido_lbl";
            this.map_escogido_lbl.Size = new System.Drawing.Size(147, 23);
            this.map_escogido_lbl.TabIndex = 7;
            this.map_escogido_lbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(171, 382);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(134, 17);
            this.label5.TabIndex = 6;
            this.label5.Text = "Mapa seleccionado:";
            // 
            // comboBox_Mapa
            // 
            this.comboBox_Mapa.FormattingEnabled = true;
            this.comboBox_Mapa.Items.AddRange(new object[] {
            "Templo",
            "Templo Helado",
            "Volcán",
            "Cueva Marítima"});
            this.comboBox_Mapa.Location = new System.Drawing.Point(8, 382);
            this.comboBox_Mapa.Name = "comboBox_Mapa";
            this.comboBox_Mapa.Size = new System.Drawing.Size(155, 24);
            this.comboBox_Mapa.TabIndex = 5;
            this.comboBox_Mapa.SelectedIndexChanged += new System.EventHandler(this.comboBox_Mapa_SelectedIndexChanged);
            // 
            // empezarPartida_but
            // 
            this.empezarPartida_but.Location = new System.Drawing.Point(746, 367);
            this.empezarPartida_but.Name = "empezarPartida_but";
            this.empezarPartida_but.Size = new System.Drawing.Size(125, 39);
            this.empezarPartida_but.TabIndex = 4;
            this.empezarPartida_but.Text = "¡Empezar!";
            this.empezarPartida_but.UseVisualStyleBackColor = true;
            this.empezarPartida_but.Click += new System.EventHandler(this.empezarPartida_but_Click);
            // 
            // SelJ4_but
            // 
            this.SelJ4_but.Location = new System.Drawing.Point(708, 309);
            this.SelJ4_but.Name = "SelJ4_but";
            this.SelJ4_but.Size = new System.Drawing.Size(122, 24);
            this.SelJ4_but.TabIndex = 3;
            this.SelJ4_but.Text = "Seleccionar";
            this.SelJ4_but.UseVisualStyleBackColor = true;
            this.SelJ4_but.Click += new System.EventHandler(this.SelJ4_but_Click);
            // 
            // SelJ3_but
            // 
            this.SelJ3_but.Location = new System.Drawing.Point(499, 309);
            this.SelJ3_but.Name = "SelJ3_but";
            this.SelJ3_but.Size = new System.Drawing.Size(122, 24);
            this.SelJ3_but.TabIndex = 2;
            this.SelJ3_but.Text = "Seleccionar";
            this.SelJ3_but.UseVisualStyleBackColor = true;
            this.SelJ3_but.Click += new System.EventHandler(this.SelJ3_but_Click);
            // 
            // SelJ2_but
            // 
            this.SelJ2_but.Location = new System.Drawing.Point(277, 309);
            this.SelJ2_but.Name = "SelJ2_but";
            this.SelJ2_but.Size = new System.Drawing.Size(122, 24);
            this.SelJ2_but.TabIndex = 2;
            this.SelJ2_but.Text = "Seleccionar";
            this.SelJ2_but.UseVisualStyleBackColor = true;
            this.SelJ2_but.Click += new System.EventHandler(this.SelJ2_but_Click);
            // 
            // SelJ1_but
            // 
            this.SelJ1_but.Location = new System.Drawing.Point(41, 309);
            this.SelJ1_but.Name = "SelJ1_but";
            this.SelJ1_but.Size = new System.Drawing.Size(122, 24);
            this.SelJ1_but.TabIndex = 2;
            this.SelJ1_but.Text = "Seleccionar";
            this.SelJ1_but.UseVisualStyleBackColor = true;
            this.SelJ1_but.Click += new System.EventHandler(this.SelJ1_but_Click);
            // 
            // usuario4
            // 
            this.usuario4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.usuario4.Location = new System.Drawing.Point(722, 248);
            this.usuario4.Name = "usuario4";
            this.usuario4.Size = new System.Drawing.Size(100, 23);
            this.usuario4.TabIndex = 1;
            this.usuario4.Text = "Usuario 4";
            this.usuario4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // usuario3
            // 
            this.usuario3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.usuario3.Location = new System.Drawing.Point(499, 248);
            this.usuario3.Name = "usuario3";
            this.usuario3.Size = new System.Drawing.Size(100, 23);
            this.usuario3.TabIndex = 1;
            this.usuario3.Text = "Usuario 3";
            this.usuario3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // usuario2
            // 
            this.usuario2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.usuario2.Location = new System.Drawing.Point(277, 248);
            this.usuario2.Name = "usuario2";
            this.usuario2.Size = new System.Drawing.Size(100, 23);
            this.usuario2.TabIndex = 1;
            this.usuario2.Text = "Usuario 2";
            this.usuario2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // usuario1
            // 
            this.usuario1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.usuario1.Location = new System.Drawing.Point(53, 248);
            this.usuario1.Name = "usuario1";
            this.usuario1.Size = new System.Drawing.Size(100, 23);
            this.usuario1.TabIndex = 1;
            this.usuario1.Text = "Usuario 1";
            this.usuario1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(56, 52);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 150);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Tag = "Personaje";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(277, 52);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(100, 150);
            this.pictureBox2.TabIndex = 0;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Tag = "Personaje";
            // 
            // pictureBox3
            // 
            this.pictureBox3.Location = new System.Drawing.Point(499, 52);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(100, 150);
            this.pictureBox3.TabIndex = 0;
            this.pictureBox3.TabStop = false;
            this.pictureBox3.Tag = "Personaje";
            // 
            // pictureBox4
            // 
            this.pictureBox4.Location = new System.Drawing.Point(708, 52);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(125, 150);
            this.pictureBox4.TabIndex = 0;
            this.pictureBox4.TabStop = false;
            this.pictureBox4.Tag = "Personaje";
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(708, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(99, 27);
            this.label4.TabIndex = 0;
            this.label4.Text = "Cloudgirl";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(499, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 27);
            this.label3.TabIndex = 0;
            this.label3.Text = "Rockboy";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(281, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 27);
            this.label2.TabIndex = 0;
            this.label2.Text = "Watergirl";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(57, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 27);
            this.label1.TabIndex = 0;
            this.label1.Text = "Fireboy";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SeleccionPartida
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1074, 601);
            this.Controls.Add(this.EnviarChatBut);
            this.Controls.Add(this.chatGrid);
            this.Controls.Add(this.chatbox);
            this.Controls.Add(this.panel1);
            this.Name = "SeleccionPartida";
            this.Text = "SeleccionPartida";
            this.Load += new System.EventHandler(this.PantallaEleccionPersonaje_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chatGrid)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button EnviarChatBut;
        private System.Windows.Forms.DataGridView chatGrid;
        protected System.Windows.Forms.TextBox chatbox;
        internal System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label map_escogido_lbl;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBox_Mapa;
        private System.Windows.Forms.Button empezarPartida_but;
        private System.Windows.Forms.Button SelJ4_but;
        private System.Windows.Forms.Button SelJ3_but;
        private System.Windows.Forms.Button SelJ2_but;
        private System.Windows.Forms.Button SelJ1_but;
        private System.Windows.Forms.Label usuario4;
        private System.Windows.Forms.Label usuario3;
        private System.Windows.Forms.Label usuario2;
        private System.Windows.Forms.Label usuario1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}