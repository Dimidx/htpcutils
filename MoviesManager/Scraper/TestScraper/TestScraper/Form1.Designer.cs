namespace TestScraper
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
            this.sai_recherche = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.liste_resultats = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // sai_recherche
            // 
            this.sai_recherche.Location = new System.Drawing.Point(12, 16);
            this.sai_recherche.Name = "sai_recherche";
            this.sai_recherche.Size = new System.Drawing.Size(218, 20);
            this.sai_recherche.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(236, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(71, 27);
            this.button1.TabIndex = 1;
            this.button1.Text = "Recherche";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // liste_resultats
            // 
            this.liste_resultats.FormattingEnabled = true;
            this.liste_resultats.Location = new System.Drawing.Point(12, 45);
            this.liste_resultats.Name = "liste_resultats";
            this.liste_resultats.Size = new System.Drawing.Size(295, 368);
            this.liste_resultats.TabIndex = 2;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(776, 431);
            this.Controls.Add(this.liste_resultats);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.sai_recherche);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox sai_recherche;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListBox liste_resultats;
    }
}

