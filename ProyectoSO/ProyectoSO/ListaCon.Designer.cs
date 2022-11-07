namespace ProyectoSO
{
    partial class ListaCon
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
            this.GridConectados = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.GridConectados)).BeginInit();
            this.SuspendLayout();
            // 
            // GridConectados
            // 
            this.GridConectados.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridConectados.Location = new System.Drawing.Point(175, 33);
            this.GridConectados.Name = "GridConectados";
            this.GridConectados.RowHeadersWidth = 51;
            this.GridConectados.RowTemplate.Height = 24;
            this.GridConectados.Size = new System.Drawing.Size(441, 352);
            this.GridConectados.TabIndex = 0;
            // 
            // ListaCon
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.GridConectados);
            this.Name = "ListaCon";
            this.Text = "Lista Conectados";
            this.Load += new System.EventHandler(this.ListaCon_Load);
            ((System.ComponentModel.ISupportInitialize)(this.GridConectados)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView GridConectados;
    }
}