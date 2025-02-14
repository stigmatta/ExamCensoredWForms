namespace ExamCensoredWForms
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            openFileDialog1 = new OpenFileDialog();
            button1 = new Button();
            textBox1 = new TextBox();
            label1 = new Label();
            folderBrowserDialog1 = new FolderBrowserDialog();
            listView1 = new ListView();
            Отчет = new Label();
            buttonStart = new Button();
            progressBar1 = new ProgressBar();
            buttonEnd = new Button();
            folderBrowserAnalyze = new FolderBrowserDialog();
            button3 = new Button();
            SuspendLayout();
            // 
            // openFileDialog1
            // 
            openFileDialog1.Filter = "(*.txt)|*.txt";
            // 
            // button1
            // 
            button1.Location = new Point(417, 59);
            button1.Name = "button1";
            button1.Size = new Size(169, 23);
            button1.TabIndex = 0;
            button1.Text = "Выбрать исходный файл";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(417, 30);
            textBox1.Name = "textBox1";
            textBox1.ScrollBars = ScrollBars.Horizontal;
            textBox1.Size = new Size(252, 23);
            textBox1.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label1.Location = new Point(417, 10);
            label1.Name = "label1";
            label1.Size = new Size(264, 17);
            label1.TabIndex = 2;
            label1.Text = "Список запрещенных слов (через пробел)";
            // 
            // listView1
            // 
            listView1.Location = new Point(12, 29);
            listView1.Name = "listView1";
            listView1.Size = new Size(344, 185);
            listView1.TabIndex = 4;
            listView1.UseCompatibleStateImageBehavior = false;
            // 
            // Отчет
            // 
            Отчет.AutoSize = true;
            Отчет.Location = new Point(94, 12);
            Отчет.Name = "Отчет";
            Отчет.Size = new Size(157, 15);
            Отчет.TabIndex = 5;
            Отчет.Text = "Отчет о найденных файлах";
            // 
            // buttonStart
            // 
            buttonStart.Location = new Point(406, 192);
            buttonStart.Name = "buttonStart";
            buttonStart.Size = new Size(76, 23);
            buttonStart.TabIndex = 6;
            buttonStart.Text = "Начать";
            buttonStart.UseVisualStyleBackColor = true;
            buttonStart.Click += buttonStart_Click;
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(406, 163);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(263, 23);
            progressBar1.TabIndex = 7;
            // 
            // buttonEnd
            // 
            buttonEnd.Enabled = false;
            buttonEnd.Location = new Point(593, 192);
            buttonEnd.Name = "buttonEnd";
            buttonEnd.Size = new Size(76, 23);
            buttonEnd.TabIndex = 8;
            buttonEnd.Text = "Завершить";
            buttonEnd.UseVisualStyleBackColor = true;
            buttonEnd.Click += buttonEnd_Click;
            // 
            // button3
            // 
            button3.Location = new Point(417, 90);
            button3.Name = "button3";
            button3.Size = new Size(169, 23);
            button3.TabIndex = 9;
            button3.Text = "Выбрать папку для анализа";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(702, 230);
            Controls.Add(button3);
            Controls.Add(buttonEnd);
            Controls.Add(progressBar1);
            Controls.Add(buttonStart);
            Controls.Add(Отчет);
            Controls.Add(listView1);
            Controls.Add(label1);
            Controls.Add(textBox1);
            Controls.Add(button1);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private OpenFileDialog openFileDialog1;
        private Button button1;
        private TextBox textBox1;
        private Label label1;
        private FolderBrowserDialog folderBrowserDialog1;
        private ListView listView1;
        private Label Отчет;
        private Button buttonStart;
        private ProgressBar progressBar1;
        private Button buttonEnd;
        private FolderBrowserDialog folderBrowserAnalyze;
        private Button button3;
    }
}
