using System.Globalization;
using System.Net;
using System.Text.RegularExpressions;

namespace TicTacToe
{
    public partial class Form1 : Form
    {
        private Button[,] but = new Button[3, 3];
        private string[,] map = new string[3,3];
        private string name;
        private bool player1 = true,SpEnde;
        private int MaxLength = 15;

        public Form1()
        {
            InitializeComponent();
            name = Microsoft.VisualBasic.Interaction.InputBox("Bitte geben Sie Ihren Namen ein.", "Nameauswahl", "Name");
            if (name.Length > MaxLength)
                name = name.Substring(0, MaxLength);
            this.Text = name;
            erzeuge();
            Clearall();
        }
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
            return;
        }

        void Clearall()            // Alles auf Anfang setzen 
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    but[i, j].Text = "";
                    map[i, j] = "";
                }
            }

            lab1.Text = name;
            lab2.Text = "";
            SpEnde = false;
            return;
        }

        // Funktion für die Buttons der Map
        private void buttonsClick(object sender, EventArgs e)
        {
            Button aktBut = (Button)sender;
            if (aktBut.Text == "" && SpEnde == false)
            {
                if (player1)
                {
                    aktBut.Text = "O";
                    player1 = false;
                    map[Convert.ToInt32(aktBut.Name[3] - 48), Convert.ToInt32(aktBut.Name[5]) - 48] = aktBut.Text;
                    if (CheckBoardWin(map, "O"))
                    {
                        lab2.Text = "Player 1 won.";
                        SpEnde = true;
                    }
                }
                else
                {
                    aktBut.Text = "X";
                    player1 = true;
                    map[Convert.ToInt32(aktBut.Name[3] - 48), Convert.ToInt32(aktBut.Name[5]) - 48] = aktBut.Text;
                    if (CheckBoardWin(map, "X"))
                    {
                        lab2.Text = "Player 2 won.";
                        SpEnde = true;
                    }
                }

                if(!CheckBoardFree(map) && SpEnde == false)
                {
                    lab2.Text = "Das Spiel hat keinen Sieger";
                }
                                           
            }
            return;
        }
        private static bool CheckBoardWin( string[,] array, string place )
        {
            // 0: 0 1 2
            // 1: 0 1 2
            // 2: 0 1 2

            bool topLeft     = array[0,0] == place;
            bool topRight    = array[0,2] == place;
            bool bottomLeft  = array[2,0] == place;
            bool bottomRight = array[2,2] == place;

            bool middleLeft = array[1,0] == place;
            bool middleRight = array[1,2] == place;
        
            bool top = array[0,1] == place;
            bool middle = array[1,1] == place;
            bool bottom = array[2,1] == place;
        
            return 
            ( middle && ( topLeft && bottomRight || topRight && bottomLeft || ( top && bottom ) || ( middleRight && middleLeft ) ) ) ||
            ( topLeft && ( ( topRight && top ) || ( bottomLeft && middleLeft ) ) ) ||
            ( bottomRight && ( ( topRight && middleRight ) || ( bottomLeft && bottom ) ) ); 
        }

        private static bool CheckBoardFree( string[,] array )
        {
            bool free = false;

            foreach ( string pArr in array )
            {          
                if ( String.IsNullOrEmpty(pArr))
                {
                    free = true;
                }              
            }
            return free;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Clearall();
        }
    } 
}