using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Data.Common;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Chess
{
    public partial class Form2 : Form
    {
        private Bitmap PawnW = Properties.Resources.PawnW;
        private Bitmap PawnB = Properties.Resources.PawnB;
        private Bitmap RookW = Properties.Resources.RookW;
        private Bitmap RookB = Properties.Resources.RookB;
        private Bitmap KnightW = Properties.Resources.KnightW;
        private Bitmap KnightB = Properties.Resources.KnightB;
        private Bitmap BishopW = Properties.Resources.BishopW;
        private Bitmap BishopB = Properties.Resources.BishopB;
        private Bitmap QueenW = Properties.Resources.QueenW;
        private Bitmap QueenB = Properties.Resources.QueenB;
        private Bitmap KingW = Properties.Resources.KingW;
        private Bitmap KingB = Properties.Resources.KingB;

        private ChessPiece[,] chessboard = new ChessPiece[8, 8];

        public Form2()
        {
            InitializeComponent();
            InitializeChessboard();

        }
        private string currentTurn = "White"; // initialize with the starting color

        private void InitializeChessboard()
        {
            InitializePlayers();
            // initialize the chessboard with empty spaces
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    chessboard[row, col] = null;
                }
            }

            // set the label text to current date and time
            label1.Text = DateTime.Now.ToString();

            // set the starting positions of the pieces
            for (int i = 0; i < 8; i++)
            {
                chessboard[6, i] = new ChessPiece(PawnW, "Pawn", "White");
                chessboard[1, i] = new ChessPiece(PawnB, "Pawn", "Black");
                if (i == 0)
                {
                    chessboard[7, i] = new ChessPiece(RookW, "Rook", "White");
                    chessboard[7, 7 - i] = new ChessPiece(RookW, "Rook", "White");

                    chessboard[0, i] = new ChessPiece(RookB, "Rook", "Black");
                    chessboard[0, 7 - i] = new ChessPiece(RookB, "Rook", "Black");
                }
                if (i == 1)
                {
                    chessboard[7, i] = new ChessPiece(KnightW, "Knight", "White");
                    chessboard[7, 7 - i] = new ChessPiece(KnightW, "Knight", "White");

                    chessboard[0, i] = new ChessPiece(KnightB, "Knight", "Black");
                    chessboard[0, 7 - i] = new ChessPiece(KnightB, "Knight", "Black");
                }
                if (i == 2)
                {
                    chessboard[7, i] = new ChessPiece(BishopW, "Bishop", "White");
                    chessboard[7, 7 - i] = new ChessPiece(BishopW, "Bishop", "White");

                    chessboard[0, i] = new ChessPiece(BishopB, "Bishop", "Black");
                    chessboard[0, 7 - i] = new ChessPiece(BishopB, "Bishop", "Black");
                }
                if (i == 3)
                {
                    chessboard[7, i] = new ChessPiece(QueenW, "Queen", "White");
                    chessboard[7, 7 - i] = new ChessPiece(KingW, "King", "White");

                    chessboard[0, i] = new ChessPiece(QueenB, "Queen", "Black");
                    chessboard[0, 7 - i] = new ChessPiece(KingB, "King", "Black");
                }
            }

        }
        
        String connectionString = "Data source=ChessDB.db;Version=3";
        SQLiteConnection connection;

        //pass values from form1
        string name1 = Form1.name1;
        string name2 = Form1.name2;
        int timer = Form1.timer;

        private void Form2_Load(object sender, EventArgs e)
        {

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // create  table 
                String createTableSQL = "CREATE TABLE IF NOT EXISTS Games(" +
                    "Player1 TEXT," +
                    "Player2 TEXT," +
                    "Date TEXT)";
                SQLiteCommand createTableCommand = new SQLiteCommand(createTableSQL, connection);
                createTableCommand.ExecuteNonQuery();

                // insert the data into the table
                String insertSQL = "INSERT INTO Games(Player1, Player2, Date) VALUES(@player1, @player2, @date)";
                SQLiteCommand insertCommand = new SQLiteCommand(insertSQL, connection);
                insertCommand.Parameters.AddWithValue("@player1", name1);
                insertCommand.Parameters.AddWithValue("@player2", name2);
                insertCommand.Parameters.AddWithValue("@date", label1.Text);
                insertCommand.ExecuteNonQuery();
            }

            //set timer
            timer1.Interval = (int)TimeSpan.FromMinutes(timer).TotalMilliseconds;
            TimeSpan RemainingTime = TimeSpan.FromMinutes(timer);
            string TimeText = RemainingTime.ToString("mm':'ss");
            myTimer1.Text = TimeText;
            myTimer2.Text = TimeText;
            timer1.Interval = 1000; // 1000 milliseconds = 1 second
            timer1.Enabled = true;  // Start the timer


            //place Chessboard            
            this.pictureBox1.Image = Properties.Resources.Board;
            this.pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            //set the label player name
            Player2.Text = name2;
            Player1.Text = name1;
            //place chess pieces
            for (int i = 0; i < 8; i++)
            {
                PlacePawn(6, i, PawnW);
                if (i == 0)
                {
                    PlacePawn(7, i, RookW);
                    PlacePawn(7, 7 - i, RookW);
                }
                if (i == 1)
                {
                    PlacePawn(7, i, KnightW);
                    PlacePawn(7, 7 - i, KnightW);
                }
                if (i == 2)
                {
                    PlacePawn(7, i, BishopW);
                    PlacePawn(7, 7 - i, BishopW);
                }
                if (i == 3)
                {
                    PlacePawn(7, i, QueenW);
                    PlacePawn(7, 7 - i, KingW);
                }
            }
            for (int i = 0; i < 8; i++)
            {
                PlacePawn(1, i, PawnB);
                if (i == 0)
                {
                    PlacePawn(0, i, RookB);
                    PlacePawn(0, 7 - i, RookB);
                }
                if (i == 1)
                {
                    PlacePawn(0, i, KnightB);
                    PlacePawn(0, 7 - i, KnightB);
                }
                if (i == 2)
                {
                    PlacePawn(0, i, BishopB);
                    PlacePawn(0, 7 - i, BishopB);
                }
                if (i == 3)
                {
                    PlacePawn(0, i, QueenB);
                    PlacePawn(0, 7 - i, KingB);
                }
            }

        }
        private void PlacePawn(int row, int column, Bitmap Pawn)
        {
            // get the position to place the pawn on the chessboard
            int x = column * (this.pictureBox1.Image.Width / 8);
            int y = row * (this.pictureBox1.Image.Height / 8);
            // Draw the pawn onto the chessboard image  if   not null
            if (Pawn != null)
            {
                using (Graphics g = Graphics.FromImage(this.pictureBox1.Image))
                {
                    g.DrawImage(Pawn, x, y, this.pictureBox1.Image.Width / 8, this.pictureBox1.Image.Height / 8);
                }
            }

            // Update the PictureBox 
            this.pictureBox1.Image = this.pictureBox1.Image;
        }

        private Player whitePlayer;
        private Player blackPlayer;

        private void InitializePlayers()
        {
            // Initialize players with the specified color and initial time
            whitePlayer = new Player("White", Form1.timer, timer1, myTimer1);
            blackPlayer = new Player("Black", Form1.timer, timer1, myTimer2);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            // Update the time for the current turn
            if (currentTurn == "White")
                whitePlayer.UpdateTime();
            else
                blackPlayer.UpdateTime();
        }
 

        private ChessPiece piece;
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {            
            int clickedRow = e.Y / 40;
            int clickedColumn = e.X / 40;
            if (IsValidPosition(clickedRow, clickedColumn))
            {
                ChessPiece clickedPiece = chessboard[clickedRow, clickedColumn];

                if (clickedPiece != null && clickedPiece.Color == currentTurn)
                {
                    piece = clickedPiece;
                    chessboard[clickedRow, clickedColumn] = null;
                    RemovePawn(clickedRow, clickedColumn);

                    // Redraw the chessboard 
                    RedrawChessboard();
                 
                }
            }
        }
        private void RemovePawn(int row, int column)
        {
            // Remove the pawn from the chessboard
            chessboard[row, column] = null;
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {

            int droppedRow = e.Y / 40;
            int droppedColumn = e.X / 40;

            if (IsValidPosition(droppedRow, droppedColumn) && piece != null && currentTurn == piece.Color)
            {
                chessboard[droppedRow, droppedColumn] = piece;
                // Redraw the chessboard 
                RedrawChessboard();

                // Switch the turn to the opposite color
                currentTurn = (currentTurn == "White") ? "Black" : "White";
            }

        }
        private bool IsValidPosition(int row, int column)
        {
            return row >= 0 && row < 8 && column >= 0 && column < 8;
        }
        private void RedrawChessboard()
        {
            // Clear the PictureBox
            pictureBox1.Image = Properties.Resources.Board;

            // Re-place all the pieces on the chessboard
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    if (chessboard[row, col] != null)
                    {
                        Bitmap pieceImage = chessboard[row, col].Image;
                        PlacePawn(row, col, pieceImage);
                    }
                }
            }
        }
             
        // Define a ChessPiece class to store information about each piece
        public class ChessPiece
        {
            public Bitmap Image { get; }
            public string Type { get; }
            public string Color { get; }
            public Point Location { get; internal set; }

            public ChessPiece(Bitmap image, string type, string color)
            {
                Image = image;
                Type = type;
                Color = color;

            }
        }
        public class Player
        {
            public string Color { get; }
            public int TimeInSeconds { get; private set; }
            public Label TimerLabel { get; set; }

            private Timer playerTimer;

            public Player(string color, int initialTimeInMinutes, Timer timer, Label timerLabel)
            {
                Color = color;
                TimeInSeconds = initialTimeInMinutes * 60; // Convert minutes to seconds
                playerTimer = timer;
                TimerLabel = timerLabel;
            }

            public void UpdateTime()
            {
                TimeInSeconds--;

                // Update the label showing the remaining time
                TimeSpan remainingTime = TimeSpan.FromSeconds(TimeInSeconds);
                string timeText = remainingTime.ToString("mm':'ss");

                // Use the TimerLabel property to access the timer label
                TimerLabel.Text = timeText;

                // Check if the player's time has run out
                if (TimeInSeconds <= 0)
                {
                    playerTimer.Stop();
                    MessageBox.Show($"{Color} player's time is up.");
                }
            }
        }
    }
}

