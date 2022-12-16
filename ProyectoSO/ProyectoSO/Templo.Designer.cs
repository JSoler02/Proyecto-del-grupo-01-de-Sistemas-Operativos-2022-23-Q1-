namespace ProyectoSO
{
    partial class Templo
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
            this.components = new System.ComponentModel.Container();
            this.tiempoJuego = new System.Windows.Forms.Timer(this.components);
            this.MainTimerJuego = new System.Windows.Forms.Timer(this.components);
            this.lblPuntos = new System.Windows.Forms.Label();
            this.label_tiempo = new System.Windows.Forms.Label();
            this.label_mensaje = new System.Windows.Forms.Label();
            this.pictureBox11 = new System.Windows.Forms.PictureBox();
            this.pictureBox10 = new System.Windows.Forms.PictureBox();
            this.pictureBox9 = new System.Windows.Forms.PictureBox();
            this.pictureBox8 = new System.Windows.Forms.PictureBox();
            this.pictureBox7 = new System.Windows.Forms.PictureBox();
            this.pictureBox5 = new System.Windows.Forms.PictureBox();
            this.pictureBox6 = new System.Windows.Forms.PictureBox();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.plataformaVertical1_Bot_limit = new System.Windows.Forms.PictureBox();
            this.plataformaVertical1_Top_limit = new System.Windows.Forms.PictureBox();
            this.placa1 = new System.Windows.Forms.PictureBox();
            this.vertical1 = new System.Windows.Forms.PictureBox();
            this.horizontal1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.J44 = new System.Windows.Forms.PictureBox();
            this.J33 = new System.Windows.Forms.PictureBox();
            this.J22 = new System.Windows.Forms.PictureBox();
            this.J11 = new System.Windows.Forms.PictureBox();
            this.EnviarChatBut = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox11)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.plataformaVertical1_Bot_limit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.plataformaVertical1_Top_limit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.placa1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.vertical1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.horizontal1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.J44)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.J33)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.J22)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.J11)).BeginInit();
            this.SuspendLayout();
            // 
            // tiempoJuego
            // 
            this.tiempoJuego.Enabled = true;
            this.tiempoJuego.Interval = 1000;
            this.tiempoJuego.Tick += new System.EventHandler(this.tiempoJuego_Tick);
            // 
            // MainTimerJuego
            // 
            this.MainTimerJuego.Enabled = true;
            this.MainTimerJuego.Interval = 30;
            this.MainTimerJuego.Tick += new System.EventHandler(this.MainGameTimerEvent);
            // 
            // lblPuntos
            // 
            this.lblPuntos.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPuntos.Location = new System.Drawing.Point(335, 43);
            this.lblPuntos.Name = "lblPuntos";
            this.lblPuntos.Size = new System.Drawing.Size(227, 30);
            this.lblPuntos.TabIndex = 70;
            this.lblPuntos.Text = "Puntos: 0";
            this.lblPuntos.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label_tiempo
            // 
            this.label_tiempo.Location = new System.Drawing.Point(694, 50);
            this.label_tiempo.Name = "label_tiempo";
            this.label_tiempo.Size = new System.Drawing.Size(100, 23);
            this.label_tiempo.TabIndex = 56;
            this.label_tiempo.Text = "label1";
            this.label_tiempo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_mensaje
            // 
            this.label_mensaje.Location = new System.Drawing.Point(18, 43);
            this.label_mensaje.Name = "label_mensaje";
            this.label_mensaje.Size = new System.Drawing.Size(321, 46);
            this.label_mensaje.TabIndex = 55;
            this.label_mensaje.Text = "label1";
            // 
            // pictureBox11
            // 
            this.pictureBox11.BackColor = System.Drawing.Color.Goldenrod;
            this.pictureBox11.Image = global::ProyectoSO.Properties.Resources.diamante_rock;
            this.pictureBox11.Location = new System.Drawing.Point(300, 203);
            this.pictureBox11.Name = "pictureBox11";
            this.pictureBox11.Size = new System.Drawing.Size(25, 25);
            this.pictureBox11.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox11.TabIndex = 73;
            this.pictureBox11.TabStop = false;
            this.pictureBox11.Tag = "diamante3";
            // 
            // pictureBox10
            // 
            this.pictureBox10.BackColor = System.Drawing.Color.Goldenrod;
            this.pictureBox10.Image = global::ProyectoSO.Properties.Resources.diamante_water;
            this.pictureBox10.Location = new System.Drawing.Point(1006, 142);
            this.pictureBox10.Name = "pictureBox10";
            this.pictureBox10.Size = new System.Drawing.Size(25, 25);
            this.pictureBox10.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox10.TabIndex = 72;
            this.pictureBox10.TabStop = false;
            this.pictureBox10.Tag = "diamante2";
            // 
            // pictureBox9
            // 
            this.pictureBox9.BackColor = System.Drawing.Color.Goldenrod;
            this.pictureBox9.Image = global::ProyectoSO.Properties.Resources.diamante_fire;
            this.pictureBox9.Location = new System.Drawing.Point(580, 213);
            this.pictureBox9.Name = "pictureBox9";
            this.pictureBox9.Size = new System.Drawing.Size(25, 25);
            this.pictureBox9.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox9.TabIndex = 71;
            this.pictureBox9.TabStop = false;
            this.pictureBox9.Tag = "diamante1";
            // 
            // pictureBox8
            // 
            this.pictureBox8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.pictureBox8.Location = new System.Drawing.Point(81, 263);
            this.pictureBox8.Name = "pictureBox8";
            this.pictureBox8.Size = new System.Drawing.Size(11, 259);
            this.pictureBox8.TabIndex = 69;
            this.pictureBox8.TabStop = false;
            this.pictureBox8.Tag = "pared_derecha";
            // 
            // pictureBox7
            // 
            this.pictureBox7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.pictureBox7.Location = new System.Drawing.Point(846, 263);
            this.pictureBox7.Name = "pictureBox7";
            this.pictureBox7.Size = new System.Drawing.Size(11, 259);
            this.pictureBox7.TabIndex = 68;
            this.pictureBox7.TabStop = false;
            this.pictureBox7.Tag = "pared_izquierda";
            // 
            // pictureBox5
            // 
            this.pictureBox5.BackColor = System.Drawing.Color.Goldenrod;
            this.pictureBox5.Location = new System.Drawing.Point(846, 512);
            this.pictureBox5.Name = "pictureBox5";
            this.pictureBox5.Size = new System.Drawing.Size(220, 10);
            this.pictureBox5.TabIndex = 67;
            this.pictureBox5.TabStop = false;
            this.pictureBox5.Tag = "techo";
            // 
            // pictureBox6
            // 
            this.pictureBox6.BackColor = System.Drawing.Color.Olive;
            this.pictureBox6.Location = new System.Drawing.Point(857, 263);
            this.pictureBox6.Name = "pictureBox6";
            this.pictureBox6.Size = new System.Drawing.Size(203, 259);
            this.pictureBox6.TabIndex = 66;
            this.pictureBox6.TabStop = false;
            this.pictureBox6.Tag = "plataforma";
            // 
            // pictureBox4
            // 
            this.pictureBox4.BackColor = System.Drawing.Color.Goldenrod;
            this.pictureBox4.Location = new System.Drawing.Point(91, 509);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(764, 13);
            this.pictureBox4.TabIndex = 65;
            this.pictureBox4.TabStop = false;
            this.pictureBox4.Tag = "techo";
            // 
            // pictureBox3
            // 
            this.pictureBox3.BackColor = System.Drawing.Color.Goldenrod;
            this.pictureBox3.Location = new System.Drawing.Point(549, 371);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(101, 10);
            this.pictureBox3.TabIndex = 64;
            this.pictureBox3.TabStop = false;
            this.pictureBox3.Tag = "techo";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Olive;
            this.pictureBox1.Location = new System.Drawing.Point(549, 351);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(101, 30);
            this.pictureBox1.TabIndex = 63;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Tag = "plataforma";
            // 
            // plataformaVertical1_Bot_limit
            // 
            this.plataformaVertical1_Bot_limit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.plataformaVertical1_Bot_limit.Location = new System.Drawing.Point(715, 427);
            this.plataformaVertical1_Bot_limit.Name = "plataformaVertical1_Bot_limit";
            this.plataformaVertical1_Bot_limit.Size = new System.Drawing.Size(66, 20);
            this.plataformaVertical1_Bot_limit.TabIndex = 62;
            this.plataformaVertical1_Bot_limit.TabStop = false;
            // 
            // plataformaVertical1_Top_limit
            // 
            this.plataformaVertical1_Top_limit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.plataformaVertical1_Top_limit.Location = new System.Drawing.Point(715, 121);
            this.plataformaVertical1_Top_limit.Name = "plataformaVertical1_Top_limit";
            this.plataformaVertical1_Top_limit.Size = new System.Drawing.Size(66, 20);
            this.plataformaVertical1_Top_limit.TabIndex = 61;
            this.plataformaVertical1_Top_limit.TabStop = false;
            // 
            // placa1
            // 
            this.placa1.BackColor = System.Drawing.Color.DeepPink;
            this.placa1.Location = new System.Drawing.Point(549, 474);
            this.placa1.Name = "placa1";
            this.placa1.Size = new System.Drawing.Size(106, 20);
            this.placa1.TabIndex = 60;
            this.placa1.TabStop = false;
            this.placa1.Tag = "placa1";
            // 
            // vertical1
            // 
            this.vertical1.BackColor = System.Drawing.Color.HotPink;
            this.vertical1.Location = new System.Drawing.Point(715, 157);
            this.vertical1.Name = "vertical1";
            this.vertical1.Size = new System.Drawing.Size(66, 30);
            this.vertical1.TabIndex = 59;
            this.vertical1.TabStop = false;
            this.vertical1.Tag = "plataforma";
            // 
            // horizontal1
            // 
            this.horizontal1.BackColor = System.Drawing.Color.Olive;
            this.horizontal1.Location = new System.Drawing.Point(119, 121);
            this.horizontal1.Name = "horizontal1";
            this.horizontal1.Size = new System.Drawing.Size(117, 30);
            this.horizontal1.TabIndex = 58;
            this.horizontal1.TabStop = false;
            this.horizontal1.Tag = "plataforma";
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Olive;
            this.pictureBox2.Location = new System.Drawing.Point(91, 483);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(764, 39);
            this.pictureBox2.TabIndex = 57;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Tag = "plataforma";
            // 
            // J44
            // 
            this.J44.BackColor = System.Drawing.SystemColors.WindowText;
            this.J44.Location = new System.Drawing.Point(471, 304);
            this.J44.Name = "J44";
            this.J44.Size = new System.Drawing.Size(25, 29);
            this.J44.TabIndex = 54;
            this.J44.TabStop = false;
            // 
            // J33
            // 
            this.J33.BackColor = System.Drawing.SystemColors.WindowText;
            this.J33.Location = new System.Drawing.Point(415, 304);
            this.J33.Name = "J33";
            this.J33.Size = new System.Drawing.Size(25, 29);
            this.J33.TabIndex = 52;
            this.J33.TabStop = false;
            // 
            // J22
            // 
            this.J22.BackColor = System.Drawing.SystemColors.WindowText;
            this.J22.Location = new System.Drawing.Point(367, 304);
            this.J22.Name = "J22";
            this.J22.Size = new System.Drawing.Size(25, 29);
            this.J22.TabIndex = 53;
            this.J22.TabStop = false;
            // 
            // J11
            // 
            this.J11.BackColor = System.Drawing.SystemColors.WindowText;
            this.J11.Location = new System.Drawing.Point(253, 304);
            this.J11.Name = "J11";
            this.J11.Size = new System.Drawing.Size(25, 29);
            this.J11.TabIndex = 51;
            this.J11.TabStop = false;
            // 
            // EnviarChatBut
            // 
            this.EnviarChatBut.Location = new System.Drawing.Point(1588, 474);
            this.EnviarChatBut.Name = "EnviarChatBut";
            this.EnviarChatBut.Size = new System.Drawing.Size(72, 30);
            this.EnviarChatBut.TabIndex = 76;
            this.EnviarChatBut.Text = "Enviar";
            this.EnviarChatBut.UseVisualStyleBackColor = true;
            // 
            // Templo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1683, 563);
            this.Controls.Add(this.EnviarChatBut);
            this.Controls.Add(this.pictureBox11);
            this.Controls.Add(this.pictureBox10);
            this.Controls.Add(this.pictureBox9);
            this.Controls.Add(this.lblPuntos);
            this.Controls.Add(this.pictureBox8);
            this.Controls.Add(this.pictureBox7);
            this.Controls.Add(this.pictureBox5);
            this.Controls.Add(this.pictureBox6);
            this.Controls.Add(this.pictureBox4);
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.plataformaVertical1_Bot_limit);
            this.Controls.Add(this.plataformaVertical1_Top_limit);
            this.Controls.Add(this.placa1);
            this.Controls.Add(this.vertical1);
            this.Controls.Add(this.horizontal1);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.label_tiempo);
            this.Controls.Add(this.label_mensaje);
            this.Controls.Add(this.J44);
            this.Controls.Add(this.J33);
            this.Controls.Add(this.J22);
            this.Controls.Add(this.J11);
            this.Name = "Templo";
            this.Text = "Templo";
            this.Load += new System.EventHandler(this.Templo_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.KeyIsDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.KeyIsUp);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox11)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.plataformaVertical1_Bot_limit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.plataformaVertical1_Top_limit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.placa1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.vertical1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.horizontal1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.J44)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.J33)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.J22)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.J11)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer tiempoJuego;
        private System.Windows.Forms.Timer MainTimerJuego;
        private System.Windows.Forms.PictureBox pictureBox11;
        private System.Windows.Forms.PictureBox pictureBox10;
        private System.Windows.Forms.PictureBox pictureBox9;
        private System.Windows.Forms.Label lblPuntos;
        private System.Windows.Forms.PictureBox pictureBox8;
        private System.Windows.Forms.PictureBox pictureBox7;
        private System.Windows.Forms.PictureBox pictureBox5;
        private System.Windows.Forms.PictureBox pictureBox6;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox plataformaVertical1_Bot_limit;
        private System.Windows.Forms.PictureBox plataformaVertical1_Top_limit;
        private System.Windows.Forms.PictureBox placa1;
        private System.Windows.Forms.PictureBox vertical1;
        private System.Windows.Forms.PictureBox horizontal1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label_tiempo;
        private System.Windows.Forms.Label label_mensaje;
        private System.Windows.Forms.PictureBox J44;
        private System.Windows.Forms.PictureBox J33;
        private System.Windows.Forms.PictureBox J22;
        private System.Windows.Forms.PictureBox J11;
        private System.Windows.Forms.Button EnviarChatBut;
    }
}