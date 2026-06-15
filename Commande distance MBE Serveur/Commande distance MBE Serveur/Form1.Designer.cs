namespace Commande_distance_MBE_Serveur
{
    partial class Form1
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBox_Logs = new System.Windows.Forms.TextBox();
            this.button_EnvoyerFichier = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox_Logs
            // 
            this.textBox_Logs.Location = new System.Drawing.Point(37, 102);
            this.textBox_Logs.Multiline = true;
            this.textBox_Logs.Name = "textBox_Logs";
            this.textBox_Logs.ReadOnly = true;
            this.textBox_Logs.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_Logs.Size = new System.Drawing.Size(449, 268);
            this.textBox_Logs.TabIndex = 0;
            this.textBox_Logs.TextChanged += new System.EventHandler(this.textBox_Logs_TextChanged);
            // 
            // button_EnvoyerFichier
            // 
            this.button_EnvoyerFichier.Location = new System.Drawing.Point(571, 160);
            this.button_EnvoyerFichier.Name = "button_EnvoyerFichier";
            this.button_EnvoyerFichier.Size = new System.Drawing.Size(114, 45);
            this.button_EnvoyerFichier.TabIndex = 1;
            this.button_EnvoyerFichier.Text = "Send file";
            this.button_EnvoyerFichier.UseVisualStyleBackColor = true;
            this.button_EnvoyerFichier.Click += new System.EventHandler(this.button_EnvoyerFichier_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button_EnvoyerFichier);
            this.Controls.Add(this.textBox_Logs);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_Logs;
        private System.Windows.Forms.Button button_EnvoyerFichier;
    }
}

