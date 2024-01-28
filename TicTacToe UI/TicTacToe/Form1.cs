using System.Globalization;
using System.Net;
using System.Text.RegularExpressions;

namespace TicTacToe
{
    public partial class Form1 : Form
    {
        private Button[,] but = new Button[3, 3];
        private string[,] map = new string[3,3];        
        private bool player1 = true, SpEnde;
        private string name;
        private int MaxLength = 15;

        public Form1()
        {
            InitializeComponent();

            // Eingabe des Namens
            name = Microsoft.VisualBasic.Interaction.InputBox("Bitte geben Sie Ihren Namen ein.", "Nameauswahl", "Name");         

            // Klickt man auf "Abbrechen" oder wird das Programm geschlossen
            if (String.IsNullOrEmpty(name))
            {
                System.Environment.Exit(0);
            }
                

            // Ist der Name länger als 15 Buchstaben wird er verkürzt
            if (name.Length > MaxLength)
                name = name.Substring(0, MaxLength);

            // Der Name des Spielers wird zu dem Namen des Forms
            this.Text = name;

            erzeuge();
            Clearall();
        }

        // Erzeugt alle buttons dynamisch
        private void erzeuge()     
        {
            int posX, posY;
            posX = 20;
            posY = 30;

            for (int i = 0; i < 3; i++)     // 9 Buttons für das Spiel Layout
            {
                for (int j = 0; j < 3; j++)
                {

                    // Erstellt ein neues Objekt der Klasse Button
                    but[i, j] = new Button();

                    but[i, j].Width = 50;
                    but[i, j].Height = 50;
                    but[i, j].Location = new Point(posX, posY);
                    but[i, j].Font = new Font("Microsoft sans serif", 24, FontStyle.Bold);
                    but[i, j].Name = $"but{i},{j}";

                    // Zuweisung der Methode "buttonsClick" zu der Aktion, wenn ein Button geklickt wird
                    but[i, j].Click += buttonsClick;

                    // Fügt den Button zu dem Form zu 
                    this.Controls.Add(but[i, j]);

                    // Verschiebung nach Rechts
                    posX += 65;
                }

                // Verschiebung zum Anfang der nächsten Reihe
                posX -= 195;
                posY += 60;
            }
            return;
        }

        // Die Methode setzt alles zurück wie es am Anfang war
        void Clearall()             
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
            player1 = true;
            return;
        }

        // Methode für die Buttons der Map
        private void buttonsClick(object sender, EventArgs e)
        {
            Button aktBut = (Button)sender;
            if (aktBut.Text == "" && SpEnde == false)
            {
                if (player1)
                {
                    aktBut.Text = "O";
                    player1 = false;

                    // Um die jetzige Position herauszufinden, muss man den Namen des Buttons benutzen
                    // Zum Beispiel "but0,0" ist der Name des erstens Buttons
                    map[Convert.ToInt32(aktBut.Name[3] - 48), Convert.ToInt32(aktBut.Name[5]) - 48] = aktBut.Text;

                    // Überpüft ob Spieler 1 gewonnen hat
                    if (CheckBoardWin(map, "O"))
                    {
                        lab2.Text = "Spieler 1 hat gewonnen.";
                        SpEnde = true;
                    }
                }
                else
                {
                    aktBut.Text = "X";
                    player1 = true;

                    map[Convert.ToInt32(aktBut.Name[3] - 48), Convert.ToInt32(aktBut.Name[5]) - 48] = aktBut.Text;

                    // Überpüft ob Spieler 2 gewonnen hat
                    if (CheckBoardWin(map, "X"))
                    {
                        lab2.Text = "Spieler 2 hat gewonnen.";
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

        // Überprüft ob jemand gewonnen hat
        private static bool CheckBoardWin( string[,] array, string place )
        {
            // 0: 0 1 2
            // 1: 0 1 2
            // 2: 0 1 2

            // Ist an dem Platz im Array ein Zeichen vorhanden, ist der boolean true 

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

        // Methode die überprüft ob es frei Platze gibt 
        // Kann man keine Spielzuge mehr machen und keiner hat gewonnen, ist das Spiel unentschieden
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

        // Wird aufgerufen klickt man auf "Neues Spiel"  
        private void button1_Click(object sender, EventArgs e)
        {
            Clearall();
        }
    } 
}