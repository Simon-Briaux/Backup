namespace Backup
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
            button1=new Button();
            Logs=new ListBox();
            button2=new Button();
            progressBar1=new ProgressBar();
            button3=new Button();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location=new Point(558, 12);
            button1.Name="button1";
            button1.Size=new Size(230, 122);
            button1.TabIndex=1;
            button1.Text="Lancer le programme";
            button1.UseVisualStyleBackColor=true;
            button1.Click+=button1_Click;
            // 
            // Logs
            // 
            Logs.FormattingEnabled=true;
            Logs.ItemHeight=20;
            Logs.Location=new Point(12, 14);
            Logs.Name="Logs";
            Logs.Size=new Size(513, 424);
            Logs.TabIndex=2;
            // 
            // button2
            // 
            button2.Location=new Point(558, 337);
            button2.Name="button2";
            button2.Size=new Size(230, 101);
            button2.TabIndex=3;
            button2.Text="initier la sauvegarde";
            button2.UseVisualStyleBackColor=true;
            button2.Click+=button2_Click;
            // 
            // progressBar1
            // 
            progressBar1.Location=new Point(558, 189);
            progressBar1.Name="progressBar1";
            progressBar1.Size=new Size(230, 35);
            progressBar1.TabIndex=4;
            progressBar1.Click+=progressBar1_Click;
            // 
            // button3
            // 
            button3.Location=new Point(558, 230);
            button3.Name="button3";
            button3.Size=new Size(230, 101);
            button3.TabIndex=5;
            button3.Text="Télécharger USB Autorun";
            button3.UseVisualStyleBackColor=true;
            button3.Click+=button3_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions=new SizeF(8F, 20F);
            AutoScaleMode=AutoScaleMode.Font;
            ClientSize=new Size(800, 450);
            Controls.Add(button3);
            Controls.Add(progressBar1);
            Controls.Add(button2);
            Controls.Add(Logs);
            Controls.Add(button1);
            Name="Form1";
            Text="Form1";
            Load+=Form1_Load;
            ResumeLayout(false);
        }

        #endregion
        private Button button1;
        private ListBox Logs;
        private Button button2;
        private ProgressBar progressBar1;
        private Button button3;
    }
}