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
            this.button1 = new System.Windows.Forms.Button();
            this.lab1 = new System.Windows.Forms.Label();
            this.lab2 = new System.Windows.Forms.Label();
            this.ipInput = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.button1.Location = new System.Drawing.Point(250, 73);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(112, 47);
            this.button1.TabIndex = 0;
            this.button1.Text = "Neues Spiel";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // lab1
            // 
            this.lab1.AutoEllipsis = true;
            this.lab1.AutoSize = true;
            this.lab1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lab1.Location = new System.Drawing.Point(250, 30);
            this.lab1.MinimumSize = new System.Drawing.Size(200, 40);
            this.lab1.Name = "lab1";
            this.lab1.Size = new System.Drawing.Size(200, 40);
            this.lab1.TabIndex = 2;
            this.lab1.Text = "Spieler";
            // 
            // lab2
            // 
            this.lab2.AutoSize = true;
            this.lab2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lab2.Location = new System.Drawing.Point(20, 215);
            this.lab2.Name = "lab2";
            this.lab2.Size = new System.Drawing.Size(80, 20);
            this.lab2.TabIndex = 3;
            this.lab2.Text = "Ergebnis";
            // 
            // ipInput
            // 
            this.ipInput.Location = new System.Drawing.Point(250, 126);
            this.ipInput.Name = "ipInput";
            this.ipInput.Size = new System.Drawing.Size(112, 39);
            this.ipInput.TabIndex = 4;
            this.ipInput.Text = "IP Input";
            this.ipInput.UseVisualStyleBackColor = true;
            this.ipInput.Click += new System.EventHandler(this.button2_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.ipInput);
            this.Controls.Add(this.lab2);
            this.Controls.Add(this.lab1);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button button1;
        private Label lab1;
        private Label lab2;
        private Button ipInput;
    }
}