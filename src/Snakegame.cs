using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Drawing;
using System.Windows.Forms;

namespace SNAKE
{
    public partial class Snakegame : Form
    {
        OleDbConnection con = new OleDbConnection();
        private List<Circle> Snake = new List<Circle>();
        private Circle food = new Circle();
        public string num;
        int sc = 0, smax = 0;

        public Snakegame(string nume)
        {
            InitializeComponent();
            con.ConnectionString =
           @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\AN 3\SEM 1\IS\SNAKE\JUCATORI.accdb";
            num = nume;
            new Settings();

            //viteza 
            gameTimer.Interval = 1000 / Settings.Speed;
            gameTimer.Tick += UpdateScreen;
            gameTimer.Start();

            StartGame();

        }
        string preluarenume = string.Empty;
        string preluareprenume = string.Empty;
        string preluareemail = string.Empty;

        private void Form1_Load(object sender, EventArgs e)
        {
            preluarenume = Crearesnake.numedat;
            preluareprenume = Crearesnake.prenumedat;
            preluareemail = Crearesnake.emaildat;
        }

        private void StartGame()
        {
            lblGameOver.Visible = false;
            new Settings();

            //Creare sarpe
            Snake.Clear();
            Circle head = new Circle { X = 10, Y = 5 };
            Snake.Add(head);

            lblScore.Text = Settings.Score.ToString();
            GenerateFood();
        }

        //Plasare mancare
        private void GenerateFood()
        {
            int maxXPos = pbCanvas.Size.Width / Settings.Width;
            int maxYPos = pbCanvas.Size.Height / Settings.Height;

            Random random = new Random();
            food = new Circle { X = random.Next(0, maxXPos), Y = random.Next(0, maxYPos) };
        }

        private void UpdateScreen(object sender, EventArgs e)
        {
            if (Settings.GameOver)
            {
                //verifica daca este apasat enter
                if (Input.KeyPressed(Keys.Enter))
                {
                    StartGame();
                }
            }
            else
            {
                if (Input.KeyPressed(Keys.Right) && Settings.direction != Direction.Left)
                    Settings.direction = Direction.Right;
                else if (Input.KeyPressed(Keys.Left) && Settings.direction != Direction.Right)
                    Settings.direction = Direction.Left;
                else if (Input.KeyPressed(Keys.Up) && Settings.direction != Direction.Down)
                    Settings.direction = Direction.Up;
                else if (Input.KeyPressed(Keys.Down) && Settings.direction != Direction.Up)
                    Settings.direction = Direction.Down;
                MovePlayer();
            }

            pbCanvas.Invalidate();

        }

        private void pbCanvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;
            if (!Settings.GameOver)
            {
                //colorare sarpe
                for (int i = 0; i < Snake.Count; i++)
                {
                    Brush snakeColour;
                    if (i == 0)
                        snakeColour = Brushes.Black; //cap
                    else
                        snakeColour = Brushes.Yellow;//corp

                    //coloraresarpe
                    canvas.FillEllipse(snakeColour,
                        new Rectangle(Snake[i].X * Settings.Width,
                                      Snake[i].Y * Settings.Height,
                                      Settings.Width, Settings.Height));
                    //coloraremancare
                    canvas.FillEllipse(Brushes.Red,
                        new Rectangle(food.X * Settings.Width,
                             food.Y * Settings.Height, Settings.Width, Settings.Height));
                }
            }

            else
            {
                string gameOver = "Jocul s-a sfarsit! \nScor final: " + Settings.Score + "\nApasa Enter pentru a incerca din nou";
                lblGameOver.Text = gameOver;
                lblGameOver.Visible = true;
            }
        }


        private void MovePlayer()
        {
            for (int i = Snake.Count - 1; i >= 0; i--)
            {
                //miscare
                if (i == 0)
                {
                    switch (Settings.direction)
                    {
                        case Direction.Right:
                            Snake[i].X++;
                            break;
                        case Direction.Left:
                            Snake[i].X--;
                            break;
                        case Direction.Up:
                            Snake[i].Y--;
                            break;
                        case Direction.Down:
                            Snake[i].Y++;
                            break;
                    }
                    int maxXPos = pbCanvas.Size.Width / Settings.Width;
                    int maxYPos = pbCanvas.Size.Height / Settings.Height;

                    //daca se loveste de zid
                    if (Snake[i].X < 0 || Snake[i].Y < 0 || Snake[i].X >= maxXPos || Snake[i].Y >= maxYPos)
                    {
                        Die();
                    }
                    //daca isi mananca coada
                    for (int j = 1; j < Snake.Count; j++)
                    {
                        if (Snake[i].X == Snake[j].X &&
                           Snake[i].Y == Snake[j].Y)
                        {
                            Die();
                        }
                    }
                    //daca se intalneste cu bucata de mancare
                    if (Snake[0].X == food.X && Snake[0].Y == food.Y)
                    {
                        Eat();
                    }
                }
                else
                {
                    //miscare corp
                    Snake[i].X = Snake[i - 1].X;
                    Snake[i].Y = Snake[i - 1].Y;
                }
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            Input.ChangeState(e.KeyCode, true);
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            Input.ChangeState(e.KeyCode, false);
        }

        private void Eat()
        {
            //crestere corp
            Circle circle = new Circle
            {
                X = Snake[Snake.Count - 1].X,
                Y = Snake[Snake.Count - 1].Y
            };
            Snake.Add(circle);

            //update scor
            Settings.Score += Settings.Points;
            lblScore.Text = Settings.Score.ToString();
            GenerateFood();
        }
        private void Die()
        {
            Settings.GameOver = true;
            if (int.Parse(Settings.Score.ToString()) > sc)
            {
                sc = int.Parse(Settings.Score.ToString());
                label3.Text = sc.ToString();
            }
            int l = int.Parse(Settings.Score.ToString());
            if (l > smax)
            {
                con.Open();
                smax = l;
                string sqlup = @"UPDATE Participanti_snake set Scor=" + smax + " WHERE Nume='" + num + "'";
                OleDbCommand cmdup = new OleDbCommand(sqlup, con);
                cmdup.ExecuteNonQuery(); con.Close();
            }
            else
                Die1(smax);
        }


        private void Die1(int s)
        {
            int smax = s;
            Settings.GameOver = true;
            con.Open();
            string sqlup = @"UPDATE Participanti_snake set Scor=" + smax + " WHERE Nume='" + num + "'";
            OleDbCommand cmdup = new OleDbCommand(sqlup, con);
            cmdup.ExecuteNonQuery();
            con.Close();
        }
    }
}
