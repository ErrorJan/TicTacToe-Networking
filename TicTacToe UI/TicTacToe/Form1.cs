namespace TicTacToe
{
    public partial class Form1 : Form
    {
        bool Spieler1 = true, SpEnde = false;
        int SiegeSp1 = 0, SiegeSp2 = 0;
        private Button[] but = new Button[9];
        public Form1()
        {
            InitializeComponent();
            erzeuge();
        }

        private void erzeuge()          // erzeugt alle buttons dynamisch
        {
            int posX, posY;
            posX = 20;
            posY = 30;


            for (int i = 0; i < 9; i++)
            {
                but[i] = new Button();
                but[i].Width = 50;
                but[i].Height = 50;
                but[i].Location = new Point(posX, posY);
                but[i].Font = new Font("Microsoft sans serif", 24, FontStyle.Bold);
                but[i].Name = "but" + (i + 1);
                but[i].Click += buttonsClick;
                this.Controls.Add(but[i]);

                posX += 65;

                if (i == 2 || i == 5)
                {
                    posX -= 195;
                    posY += 60;
                }
            }
            return;
        }

        private void buttonsClick(object sender, EventArgs e)
        {
            Button aktBut = (Button)sender;
            if (aktBut.Text == "")
            {
                if (Spieler1)
                {
                    aktBut.Text = "X";
                    Spieler1 = false;
                }
                else
                {
                    aktBut.Text = "O";
                    Spieler1 = true;
                }
                
            }
            return;
        }
    }
}