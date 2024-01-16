using System.Globalization;
using System.Net;
using System.Text.RegularExpressions;
using TicTacToe_Shared;

namespace TicTacToe
{
    public partial class Form1 : Form
    {
        private Button[,] but = new Button[3, 3];
        private Label[] lab = new Label[5];
        private string name;
        private string Ep;
        public Form1()
        {
            InitializeComponent();
            name = Microsoft.VisualBasic.Interaction.InputBox("Bitte geben Sie Ihren Namen ein.", "Nameauswahl", "Name");
            erzeuge();
            Clearall();
        }
        /* public Form2()
         {
             InitializeComponent();
             name = Microsoft.VisualBasic.Interaction.InputBox("Bitte server", "Nameauswahl", "Name");
             erzeuge();
             Clearall();
         }*/
        private void erzeuge()      // erzeugt alle buttons dynamisch
        {
            int posX, posY;
            posX = 20;
            posY = 30;


            for (int i = 0; i < 3; i++)     // 9 Buttons für das Spiel Layout
            {
                for (int j = 0; j < 3; j++)
                {
                    but[i, j] = new Button();
                    but[i, j].Width = 50;
                    but[i, j].Height = 50;
                    but[i, j].Location = new Point(posX, posY);
                    but[i, j].Font = new Font("Microsoft sans serif", 24, FontStyle.Bold);
                    but[i, j].Name = $"but{i},{j}";
                    but[i, j].Click += buttonsClick;
                    this.Controls.Add(but[i, j]);

                    posX += 65;
                }
                posX -= 195;
                posY += 60;
            }



            for (int i = 0; i < 5; i++)    // 5 verschieden Labels die gebraucht sind (noch nicht alle fertig)
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
                        lab[i].Text = name;
                        this.Controls.Add(lab[i]);
                        break;
                    case 1:
                        lab[i].Width = 200;
                        lab[i].Height = 40;
                        lab[i].Location = new Point(250, 180);
                        lab[i].Font = new Font("Microsoft sans serif", 12, FontStyle.Bold);
                        lab[i].Name = "lab" + (i + 1);
                        lab[i].Text = "";
                        this.Controls.Add(lab[i]);
                        break;        // Label 2: 
                    case 2:break;
                    case 3: break;
                    case 4: break;
                }
            }
            return;
        }

        void Clearall()            // Alles auf Anfang setzen 
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    but[i, j].Text = "";
                }
            }

            lab[0].Text = name;
            lab[1].Text = "";
            return;
        }

        // Funktion für die Buttons der Map
        private void buttonsClick(object sender, EventArgs e)
        {
            Button aktBut = (Button)sender;
            if (aktBut.Text == "" && Connection.currentTurn)
            {
                Connection.SendAction(Convert.ToInt32(aktBut.Name[3]) - 48, Convert.ToInt32(aktBut.Name[5]) - 48);
            }
            return;
        }
        private void PlayerEvent(BoardMove move)
        {
            but[move.x, move.y].Text = move.place.ToString();
        }

        private void PlayerTurnEvent(PlayerData player)
        {
            lab[0].Text = player.playerName + " ist am Zug.";
        }

        private void PlayerWonEvent(PlayerData player)
        {
            lab[1].Text = player.playerName + " hat gewonnen";
        }

        ClientConnection Connection;
        private void button1_Click(object sender, EventArgs e)
        {
            Clearall();
            Connection = new(name, IPEndPoint.Parse(Ep)); // $"127.0.1.2:{StaticGameInfo.GAME_PORT}") <--- stay here
            Connection.playerEvent += PlayerEvent;
            Connection.playerTurnEvent += PlayerTurnEvent;
            Connection.playerWonEvent += PlayerWonEvent;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            InitializeComponent();
            Ep = Microsoft.VisualBasic.Interaction.InputBox("Wählen sie die Ip", "IP-Selector", "127.0.1.2");
            Ep = Ep + ":" + StaticGameInfo.GAME_PORT;
            erzeuge();
            Clearall();
        }

    }
}