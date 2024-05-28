using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Arcanoid_Game
{
    public partial class Form1 : Form
    {
        //размер формы
        const int mapWidth = 20;
        const int mapHeight = 30;

        public int[,] map = new int[mapHeight, mapWidth];//карта

        //координаты шара
        public int dirX = 0;
        public int dirY = 0;
        //координаты платформы
        public int platformX;
        public int platformY;

        //координаты шара
        public int ballX;
        public int ballY;

        public Image arcanoidSet;//пресеты
        //счет
        public Label scoreLabel;
        public Label levelLabel;
        public Label livesLabel;
        public int score;
        public int level = 1;
        public int lives = 3;

        public Form1()
        {
            InitializeComponent();
            timer1.Tick += new EventHandler(update);

            scoreLabel = new Label();
            scoreLabel.Location = new Point((mapWidth) * 20 + 1, 50);
            scoreLabel.Text = "Score: " + score;

            levelLabel = new Label();
            levelLabel.Location = new Point((mapWidth) * 20 + 1, 70);
            levelLabel.Text = "Level: " + level;

            livesLabel = new Label();
            livesLabel.Location = new Point((mapWidth) * 20 + 1, 90);
            livesLabel.Text = "Lives: " + lives;

            this.Controls.Add(scoreLabel);
            this.Controls.Add(levelLabel);
            this.Controls.Add(livesLabel);

            this.KeyUp += new KeyEventHandler(inputCheck);
            Init();
        }

        public void AddLine()
        {
            for (int i = mapHeight - 2; i > 0; i--)
            {
                for (int j = 0; j < mapWidth; j += 2)
                {
                    map[i, j] = map[i - 1, j];
                }
            }
            Random r = new Random();
            for (int j = 0; j < mapWidth; j += 2)
            {
                int currPlatform = r.Next(1, 5);
                map[0, j] = currPlatform;
                map[0, j + 1] = currPlatform + currPlatform * 10;
            }
        }

        private void update(object sender, EventArgs e)
        {
            if (ballY + dirY > mapHeight - 1)
            {
                lives--;
                if (lives == 0)
                {
                    timer1.Stop();
                    MessageBox.Show("Game Over! Your final score is: " + score);
                    Init();
                }
                else
                {
                    ResetBall();
                }
            }

            if (ballY >= 0 && ballY < mapHeight && ballX >= 0 && ballX < mapWidth)
            {
                map[ballY, ballX] = 0;
            }

            if (!IsCollide())
            {
                if (ballX + dirX >= 0 && ballX + dirX < mapWidth)
                {
                    ballX += dirX;
                }
            }

            if (!IsCollide())
            {
                if (ballY + dirY >= 0 && ballY + dirY < mapHeight)
                {
                    ballY += dirY;
                }
            }

            if (ballY >= 0 && ballY < mapHeight && ballX >= 0 && ballX < mapWidth)
            {
                map[ballY, ballX] = 8;
            }

            if (platformY >= 0 && platformY < mapHeight && platformX >= 0 && platformX < mapWidth)
            {
                map[platformY, platformX] = 9;
            }

            if (platformY >= 0 && platformY < mapHeight && platformX + 1 >= 0 && platformX + 1 < mapWidth)
            {
                map[platformY, platformX + 1] = 99;
            }

            if (platformY >= 0 && platformY < mapHeight && platformX + 2 >= 0 && platformX + 2 < mapWidth)
            {
                map[platformY, platformX + 2] = 999;
            }

            Invalidate(); // Перерисовываем холст
        }

        private void inputCheck(object sender, KeyEventArgs e)
        {
            map[platformY, platformX] = 0;
            map[platformY, platformX + 1] = 0;
            map[platformY, platformX + 2] = 0;

            if (e.KeyCode == Keys.Right && platformX + 2 < mapWidth - 1)
            {
                platformX++;
            }
            else if (e.KeyCode == Keys.Left && platformX > 0)
            {
                platformX--;
            }

            map[platformY, platformX] = 9;
            if (platformX + 1 < mapWidth)
            {
                map[platformY, platformX + 1] = 99;
            }
            if (platformX + 2 < mapWidth)
            {
                map[platformY, platformX + 2] = 999;
            }
        }

        public void GeneratePlatforms()
        {
            Random r = new Random();
            for (int i = 0; i < mapHeight / 3; i++)
            {
                for (int j = 0; j < mapWidth; j += 2)
                {
                    int currPlatform = r.Next(1, 5);
                    map[i, j] = currPlatform;
                    map[i, j + 1] = currPlatform + currPlatform * 10;
                }
            }
        }

        public void DrawArea(Graphics g)
        {
            g.DrawRectangle(Pens.Black, new Rectangle(0, 0, mapWidth * 20, mapHeight * 20));
        }

        public void Init()
        {
            this.Width = (mapWidth + 5) * 20;
            this.Height = (mapHeight + 2) * 20;

            arcanoidSet = new Bitmap("C:\\Users\\rr033\\source\\repos\\Arcanoid_Game\\Arcanoid_Game\\Image\\arcanoid.png");
            timer1.Interval = 200;

            score = 0;
            lives = 3;

            scoreLabel.Text = "Score: " + score;
            levelLabel.Text = "Level: " + level;
            livesLabel.Text = "Lives: " + lives;

            for (int i = 0; i < mapHeight; i++)
            {
                for (int j = 0; j < mapWidth; j++)
                {
                    map[i, j] = 0;
                }
            }

            platformX = (mapWidth - 1) / 2;
            platformY = mapHeight - 1;

            map[platformY, platformX] = 9;
            map[platformY, platformX + 1] = 99;
            map[platformY, platformX + 2] = 999;

            ballY = platformY - 1;
            ballX = platformX + 1;

            map[ballY, ballX] = 8;

            dirX = 1;
            dirY = -1;

            GeneratePlatforms();

            timer1.Start();
        }

        private void ResetBall()
        {
            ballY = platformY - 1;
            ballX = platformX + 1;
            dirX = 1;
            dirY = -1;
            map[ballY, ballX] = 8;
            livesLabel.Text = "Lives: " + lives;
        }

        public bool IsCollide()
        {
            bool isColliding = false;

            // Check for collision with the left and right boundaries
            if (ballX + dirX >= mapWidth || ballX + dirX < 0)
            {
                dirX *= -1;
                isColliding = true;
            }

            // Check for collision with the top boundary
            if (ballY + dirY < 0)
            {
                dirY *= -1;
                isColliding = true;
            }

            // Check for collision with the platform
            if (ballY + dirY == platformY && (ballX + dirX >= platformX && ballX + dirX < platformX + 3))
            {
                dirY *= -1;
                isColliding = true;
            }

            // Check for collision with blocks in the vertical direction
            if (!isColliding && ballY + dirY < mapHeight && ballY + dirY >= 0 && map[ballY + dirY, ballX] != 0)
            {
                int nextY = ballY + dirY;
                bool addScore = false;
                isColliding = true;

                if (map[nextY, ballX] > 10 && map[nextY, ballX] < 99)
                {
                    map[nextY, ballX] = 0;
                    if (ballX > 0)
                    {
                        map[nextY, ballX - 1] = 0;
                    }
                    addScore = true;
                }
                else if (map[nextY, ballX] < 9)
                {
                    map[nextY, ballX] = 0;
                    if (ballX < mapWidth - 1)
                    {
                        map[nextY, ballX + 1] = 0;
                    }
                    addScore = true;
                }

                if (addScore)
                {
                    score += 50;
                    if (score % 200 == 0 && score > 0)
                    {
                        if (timer1.Interval != 10)
                        {
                            timer1.Interval -= 10;
                            level++;
                        }
                        AddLine();
                    }
                    if (score % 1000 == 0) // Gain a life every 1000 points
                    {
                        lives++;
                        livesLabel.Text = "Lives: " + lives;
                    }
                }
                dirY *= -1;
            }

            // Check for collision with blocks in the horizontal direction
            if (!isColliding && ballX + dirX < mapWidth && ballX + dirX >= 0 && map[ballY, ballX + dirX] != 0)
            {
                int nextX = ballX + dirX;
                bool addScore = false;
                isColliding = true;

                if (map[ballY, nextX] > 10 && map[ballY, nextX] < 99)
                {
                    map[ballY, nextX] = 0;
                    if (ballY > 0)
                    {
                        map[ballY - 1, nextX] = 0;
                    }
                    addScore = true;
                }
                else if (map[ballY, nextX] < 9)
                {
                    map[ballY, nextX] = 0;
                    if (ballY < mapHeight - 1)
                    {
                        map[ballY + 1, nextX] = 0;
                    }
                    addScore = true;
                }

                if (addScore)
                {
                    score += 50;
                    if (score % 200 == 0 && score > 0)
                    {
                        if (timer1.Interval != 10)
                        {
                            timer1.Interval -= 10;
                            level++;
                        }
                        AddLine();
                    }
                    if (score % 1000 == 0) // Gain a life every 1000 points
                    {
                        lives++;
                        livesLabel.Text = "Lives: " + lives;
                    }
                }
                dirX *= -1;
            }

            scoreLabel.Text = "Score: " + score;
            levelLabel.Text = "Level: " + level;
            return isColliding;
        }

        public void DrawMap(Graphics g)
        {
            for (int i = 0; i < mapHeight; i++)
            {
                for (int j = 0; j < mapWidth; j++)
                {
                    if (map[i, j] == 9)
                    {
                        g.DrawImage(arcanoidSet, new Rectangle(new Point(j * 20, i * 20), new Size(60, 20)), 398, 17, 150, 50, GraphicsUnit.Pixel);
                    }
                    if (map[i, j] == 8)
                    {
                        g.DrawImage(arcanoidSet, new Rectangle(new Point(j * 20, i * 20), new Size(20, 20)), 806, 548, 73, 73, GraphicsUnit.Pixel);
                    }
                    if (map[i, j] == 1)
                    {
                        g.DrawImage(arcanoidSet, new Rectangle(new Point(j * 20, i * 20), new Size(40, 20)), 20, 16, 170, 59, GraphicsUnit.Pixel);
                    }
                    if (map[i, j] == 2)
                    {
                        g.DrawImage(arcanoidSet, new Rectangle(new Point(j * 20, i * 20), new Size(40, 20)), 20, 16 + 77 * (map[i, j] - 1), 170, 59, GraphicsUnit.Pixel);
                    }
                    if (map[i, j] == 3)
                    {
                        g.DrawImage(arcanoidSet, new Rectangle(new Point(j * 20, i * 20), new Size(40, 20)), 20, 16 + 77 * (map[i, j] - 1), 170, 59, GraphicsUnit.Pixel);
                    }
                    if (map[i, j] == 4)
                    {
                        g.DrawImage(arcanoidSet, new Rectangle(new Point(j * 20, i * 20), new Size(40, 20)), 20, 16 + 77 * (map[i, j] - 1), 170, 59, GraphicsUnit.Pixel);
                    }
                    if (map[i, j] == 5)
                    {
                        g.DrawImage(arcanoidSet, new Rectangle(new Point(j * 20, i * 20), new Size(40, 20)), 20, 16 + 77 * (map[i, j] - 1), 170, 59, GraphicsUnit.Pixel);
                    }
                }
            }
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            DrawArea(e.Graphics);
            DrawMap(e.Graphics);
        }
    }
}
