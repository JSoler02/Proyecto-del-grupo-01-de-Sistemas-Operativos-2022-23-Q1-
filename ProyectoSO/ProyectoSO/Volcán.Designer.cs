namespace ProyectoSO
{
    partial class Volcán
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
            this.AcabarPartida_But = new System.Windows.Forms.Button();
            this.EnviarChatBut = new System.Windows.Forms.Button();
            this.chatGrid = new System.Windows.Forms.DataGridView();
            this.chatbox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.chatGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // AcabarPartida_But
            // 
            this.AcabarPartida_But.Location = new System.Drawing.Point(419, 491);
            this.AcabarPartida_But.Name = "AcabarPartida_But";
            this.AcabarPartida_But.Size = new System.Drawing.Size(137, 37);
            this.AcabarPartida_But.TabIndex = 81;
            this.AcabarPartida_But.Text = "Acabar Partida";
            this.AcabarPartida_But.UseVisualStyleBackColor = true;
            // 
            // EnviarChatBut
            // 
            this.EnviarChatBut.Location = new System.Drawing.Point(707, 454);
            this.EnviarChatBut.Name = "EnviarChatBut";
            this.EnviarChatBut.Size = new System.Drawing.Size(72, 30);
            this.EnviarChatBut.TabIndex = 80;
            this.EnviarChatBut.Text = "Enviar";
            this.EnviarChatBut.UseVisualStyleBackColor = true;
            // 
            // chatGrid
            // 
            this.chatGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.chatGrid.Location = new System.Drawing.Point(216, 11);
            this.chatGrid.Name = "chatGrid";
            this.chatGrid.RowHeadersWidth = 51;
            this.chatGrid.RowTemplate.Height = 24;
            this.chatGrid.Size = new System.Drawing.Size(563, 437);
            this.chatGrid.TabIndex = 79;
            // 
            // chatbox
            // 
            this.chatbox.Location = new System.Drawing.Point(216, 458);
            this.chatbox.Name = "chatbox";
            this.chatbox.Size = new System.Drawing.Size(485, 22);
            this.chatbox.TabIndex = 78;
            // 
            // Volcán
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(920, 545);
            this.Controls.Add(this.AcabarPartida_But);
            this.Controls.Add(this.EnviarChatBut);
            this.Controls.Add(this.chatGrid);
            this.Controls.Add(this.chatbox);
            this.Name = "Volcán";
            this.Text = "Volcán";
            ((System.ComponentModel.ISupportInitialize)(this.chatGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button AcabarPartida_But;
        private System.Windows.Forms.Button EnviarChatBut;
        private System.Windows.Forms.DataGridView chatGrid;
        protected System.Windows.Forms.TextBox chatbox;
    }
}