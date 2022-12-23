
namespace ProyectoSO
{
    partial class prueba_Teclas
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
            this.label_Envio = new System.Windows.Forms.Label();
            this.label_Recibo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label_Envio
            // 
            this.label_Envio.Location = new System.Drawing.Point(103, 51);
            this.label_Envio.Name = "label_Envio";
            this.label_Envio.Size = new System.Drawing.Size(226, 104);
            this.label_Envio.TabIndex = 0;
            this.label_Envio.Text = "label1";
            // 
            // label_Recibo
            // 
            this.label_Recibo.Location = new System.Drawing.Point(442, 51);
            this.label_Recibo.Name = "label_Recibo";
            this.label_Recibo.Size = new System.Drawing.Size(226, 104);
            this.label_Recibo.TabIndex = 0;
            this.label_Recibo.Text = "label1";
            // 
            // prueba_Teclas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label_Recibo);
            this.Controls.Add(this.label_Envio);
            this.Name = "prueba_Teclas";
            this.Text = "prueba_Teclas";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.prueba_Teclas_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.prueba_Teclas_KeyUp);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label_Envio;
        private System.Windows.Forms.Label label_Recibo;
    }
}