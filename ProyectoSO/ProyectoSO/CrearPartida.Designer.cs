
namespace ProyectoSO
{
    partial class CrearPartida
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
            this.DosJugadores = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.TresJugadores = new System.Windows.Forms.Button();
            this.CuatroJugadores = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // DosJugadores
            // 
            this.DosJugadores.Location = new System.Drawing.Point(30, 162);
            this.DosJugadores.Name = "DosJugadores";
            this.DosJugadores.Size = new System.Drawing.Size(212, 50);
            this.DosJugadores.TabIndex = 0;
            this.DosJugadores.Text = "2 Jugadores";
            this.DosJugadores.UseVisualStyleBackColor = true;
            this.DosJugadores.Click += new System.EventHandler(this.DosJugadores_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(176, 77);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(452, 42);
            this.label1.TabIndex = 1;
            this.label1.Text = "¿Cuántos jugadores sois?";
            // 
            // TresJugadores
            // 
            this.TresJugadores.Location = new System.Drawing.Point(269, 162);
            this.TresJugadores.Name = "TresJugadores";
            this.TresJugadores.Size = new System.Drawing.Size(212, 50);
            this.TresJugadores.TabIndex = 2;
            this.TresJugadores.Text = "3 Jugadores";
            this.TresJugadores.UseVisualStyleBackColor = true;
            this.TresJugadores.Click += new System.EventHandler(this.TresJugadores_Click);
            // 
            // CuatroJugadores
            // 
            this.CuatroJugadores.Location = new System.Drawing.Point(518, 162);
            this.CuatroJugadores.Name = "CuatroJugadores";
            this.CuatroJugadores.Size = new System.Drawing.Size(212, 50);
            this.CuatroJugadores.TabIndex = 3;
            this.CuatroJugadores.Text = "4 Jugadores";
            this.CuatroJugadores.UseVisualStyleBackColor = true;
            this.CuatroJugadores.Click += new System.EventHandler(this.CuatroJugadores_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(269, 252);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(212, 71);
            this.button1.TabIndex = 4;
            this.button1.Text = "Aceptar";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // CrearPartida
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 360);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.CuatroJugadores);
            this.Controls.Add(this.TresJugadores);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.DosJugadores);
            this.Name = "CrearPartida";
            this.Text = "CrearPartida";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button DosJugadores;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button TresJugadores;
        private System.Windows.Forms.Button CuatroJugadores;
        private System.Windows.Forms.Button button1;
    }
}