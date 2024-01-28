namespace TicTacToe
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
            button1 = new Button();
            lab1 = new Label();
            lab2 = new Label();
            SuspendLayout();
            // 
            // button1
            // 
            button1.AutoSize = true;
            button1.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point);
            button1.Location = new Point(250, 73);
            button1.Name = "button1";
            button1.Size = new Size(119, 49);
            button1.TabIndex = 0;
            button1.Text = "Neues Spiel";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // lab1
            // 
            lab1.AutoEllipsis = true;
            lab1.AutoSize = true;
            lab1.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point);
            lab1.Location = new Point(250, 30);
            lab1.MinimumSize = new Size(200, 40);
            lab1.Name = "lab1";
            lab1.Size = new Size(200, 40);
            lab1.TabIndex = 2;
            lab1.Text = "Spieler";
            // 
            // lab2
            // 
            lab2.AutoSize = true;
            lab2.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point);
            lab2.Location = new Point(20, 215);
            lab2.Name = "lab2";
            lab2.Size = new Size(80, 20);
            lab2.TabIndex = 3;
            lab2.Text = "Ergebnis";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(lab2);
            Controls.Add(lab1);
            Controls.Add(button1);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private Label lab1;
        private Label lab2;
    }
}