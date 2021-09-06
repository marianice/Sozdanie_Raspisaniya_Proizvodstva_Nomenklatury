
namespace Sozdanie_Raspisaniya_Proizvodstva_Nomenklatury
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnReadFiles = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.pbReport = new System.Windows.Forms.PictureBox();
            this.btnExcel = new System.Windows.Forms.Button();
            this.btnCalc = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbReport)).BeginInit();
            this.SuspendLayout();
            // 
            // btnReadFiles
            // 
            this.btnReadFiles.Location = new System.Drawing.Point(4, 35);
            this.btnReadFiles.Name = "btnReadFiles";
            this.btnReadFiles.Size = new System.Drawing.Size(140, 35);
            this.btnReadFiles.TabIndex = 6;
            this.btnReadFiles.Text = "Прочитать входные параметры...";
            this.btnReadFiles.UseVisualStyleBackColor = true;
            this.btnReadFiles.Click += new System.EventHandler(this.btnReadFiles_Click);
            // 
            // folderBrowserDialog1
            // 
            this.folderBrowserDialog1.Description = "Выберите расположение входных файлов";
            this.folderBrowserDialog1.ShowNewFolderButton = false;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(4, 307);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(1033, 236);
            this.dataGridView1.TabIndex = 10;
            // 
            // pbReport
            // 
            this.pbReport.Location = new System.Drawing.Point(4, 76);
            this.pbReport.Name = "pbReport";
            this.pbReport.Size = new System.Drawing.Size(1033, 225);
            this.pbReport.TabIndex = 9;
            this.pbReport.TabStop = false;
            // 
            // btnExcel
            // 
            this.btnExcel.Enabled = false;
            this.btnExcel.Location = new System.Drawing.Point(277, 35);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(121, 35);
            this.btnExcel.TabIndex = 7;
            this.btnExcel.Text = "Показать файл Excel";
            this.btnExcel.UseVisualStyleBackColor = true;
            this.btnExcel.Click += new System.EventHandler(this.btnExcel_Click);
            // 
            // btnCalc
            // 
            this.btnCalc.Enabled = false;
            this.btnCalc.Location = new System.Drawing.Point(150, 35);
            this.btnCalc.Name = "btnCalc";
            this.btnCalc.Size = new System.Drawing.Size(121, 35);
            this.btnCalc.TabIndex = 8;
            this.btnCalc.Text = "Расчет";
            this.btnCalc.UseVisualStyleBackColor = true;
            this.btnCalc.Click += new System.EventHandler(this.btnCalc_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(269, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Файлы расположены в папке Sozdanie_Raspisaniya";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1041, 450);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnReadFiles);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.pbReport);
            this.Controls.Add(this.btnExcel);
            this.Controls.Add(this.btnCalc);
            this.Name = "Form1";
            this.Text = "Расчет расписания работы оборудования";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbReport)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnReadFiles;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.PictureBox pbReport;
        private System.Windows.Forms.Button btnExcel;
        private System.Windows.Forms.Button btnCalc;
        private System.Windows.Forms.Label label1;
    }
}

