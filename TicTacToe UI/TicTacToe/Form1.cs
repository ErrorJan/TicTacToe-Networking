namespace TicTacToe
{
    public partial class Form1 : Form
    {
        bool Spieler1 = true, SpEnde = false;
        int SiegeSp1 = 0, SiegeSp2 = 0;
        private Button[] but = new Button[9];
        private Label[] lab = new Label[5];
        public Form1()
        {
            InitializeComponent();
            erzeuge();
            Clearall();
        }

        private void erzeuge()      // erzeugt alle buttons dynamisch
        {
            int posX, posY;
            posX = 20;
            posY = 30;


            for (int i = 0; i < 9; i++)     // 9 Buttons für das Spiel Layout
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

                if (i == 2 || i == 5)      // passiert nur wenn eine neue Zeile gebraucht wird
                {
                    posX -= 195;
                    posY += 60;
                }
            }



            for (int i = 0; i < 5; i++)    // 5 verschiedend Labels die gebraucht sind (noch nicht alle fertig)
            {                              // bin mir unsicher ob wir alle brauchen uberhaupt, ist jetzt da einfach
                lab[i] = new Label();
                switch (i)
                {
                    case 0:                // Label 1: Status der Spieler Zuge 
                        lab[i].Width = 200;
                        lab[i].Height = 40;
                        lab[i].Location = new Point(250, 30);
                        lab[i].Font = new Font("Microsoft sans serif", 12, FontStyle.Bold);
                        lab[i].Name = "lab" + (i + 1);
                        lab[i].Text = "Spieler 1 am Zug";
                        this.Controls.Add(lab[i]);
                        break;
                    case 1: break;        // Label 2: 
                    case 2: break;
                    case 3: break;
                    case 4: break;
                }
            }
            return;
        }

        void Clearall()            // Alles auf Anfang setzen 
        {
            for (int i = 0; i < 9; i++)
            {
                but[i].Text = "";
            }

            lab[0].Text = "Spieler 1 am Zug";

            Spieler1 = true;
            return;
        }

        // Funktion für die Buttons der Map
        private void buttonsClick(object sender, EventArgs e)       
        {
            Button aktBut = (Button)sender;
            if (aktBut.Text == "")
            {
                if (Spieler1)
                {
                    aktBut.Text = "X";
                    lab[0].Text = "Spieler 2 am Zug";
                    Spieler1 = false;
                }
                else
                {
                    lab[0].Text = "Spieler 1 am Zug";
                    aktBut.Text = "O";
                    Spieler1 = true;
                }

            }

            return;
        }

        // Funktion für den Button des neues Spieles
        // wird am Ende vielleicht auch nicht gebraucht, jetzt für Probe da
        private void button1_Click(object sender, EventArgs e)   
        {                
            Clearall();
        }
    }
}