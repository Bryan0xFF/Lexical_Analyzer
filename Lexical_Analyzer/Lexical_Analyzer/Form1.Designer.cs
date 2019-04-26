namespace Lexical_Analyzer
{
    partial class Form1
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
            this.button1 = new System.Windows.Forms.Button();
            this.DGVFollow = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.tbxCompiler = new System.Windows.Forms.TextBox();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnCompilar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.DGVFollow)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 194);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 62);
            this.button1.TabIndex = 0;
            this.button1.Text = "leer tokens";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // DGVFollow
            // 
            this.DGVFollow.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGVFollow.Location = new System.Drawing.Point(116, 80);
            this.DGVFollow.Name = "DGVFollow";
            this.DGVFollow.RowTemplate.Height = 24;
            this.DGVFollow.Size = new System.Drawing.Size(290, 320);
            this.DGVFollow.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(244, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Automata";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // tbxCompiler
            // 
            this.tbxCompiler.Location = new System.Drawing.Point(671, 32);
            this.tbxCompiler.Multiline = true;
            this.tbxCompiler.Name = "tbxCompiler";
            this.tbxCompiler.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbxCompiler.Size = new System.Drawing.Size(530, 547);
            this.tbxCompiler.TabIndex = 3;
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(1257, 283);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(75, 31);
            this.btnExport.TabIndex = 4;
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnCompilar
            // 
            this.btnCompilar.Location = new System.Drawing.Point(1257, 391);
            this.btnCompilar.Name = "btnCompilar";
            this.btnCompilar.Size = new System.Drawing.Size(93, 31);
            this.btnCompilar.TabIndex = 5;
            this.btnCompilar.Text = "Compilar";
            this.btnCompilar.UseVisualStyleBackColor = true;
            this.btnCompilar.Click += new System.EventHandler(this.btnCompilar_Click_1);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1377, 675);
            this.Controls.Add(this.btnCompilar);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.tbxCompiler);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.DGVFollow);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DGVFollow)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridView DGVFollow;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbxCompiler;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnCompilar;
    }
}

