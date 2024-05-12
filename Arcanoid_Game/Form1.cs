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
        const int mapWidth = 30;
        const int mapHeight = 40;

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
        public Label LevelLabel;
        public int score;
        public int level = 1;
        public Form1()
        {
            InitializeComponent();
            timer1.Tick += new EventHandler(update);

            scoreLabel = new Label();
            scoreLabel.Location = new Point((mapWidth) * 20 + 1, 50);
            scoreLabel.Text = "Score: " + score;
            LevelLabel = new Label();
            LevelLabel.Location = new Point((mapWidth) * 20 + 1, 70);
            LevelLabel.Text = "Level: " + level;
            this.Controls.Add(scoreLabel);
            this.Controls.Add(LevelLabel);
            this.KeyDown += new KeyEventHandler(inputCheck);
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
                Init();
            }

            if (ballY >= 0 && ballY < mapHeight && ballX >= 0 && ballX < mapWidth) // Проверяем границы перед обращением к ballY и ballX
            {
                map[ballY, ballX] = 0;
            }

            if (!IsCollide())
            {
                if (ballX + dirX >= 0 && ballX + dirX < mapWidth) // Проверяем границы перед изменением ballX
                {
                    ballX += dirX;
                }
            }

            if (!IsCollide())
            {
                if (ballY + dirY >= 0 && ballY + dirY < mapHeight) // Проверяем границы перед изменением ballY
                {
                    ballY += dirY;
                }
            }

            if (ballY >= 0 && ballY < mapHeight && ballX >= 0 && ballX < mapWidth) // Проверяем границы перед обращением к ballY и ballX
            {
                map[ballY, ballX] = 8;
            }

            if (platformY >= 0 && platformY < mapHeight && platformX >= 0 && platformX < mapWidth) // Проверяем границы перед обращением к platformY и platformX
            {
                map[platformY, platformX] = 9;
            }

            if (platformY >= 0 && platformY < mapHeight && platformX + 1 >= 0 && platformX + 1 < mapWidth) // Проверяем границы перед обращением к platformY и platformX + 1
            {
                map[platformY, platformX + 1] = 99;
            }

            if (platformY >= 0 && platformY < mapHeight && platformX + 2 >= 0 && platformX + 2 < mapWidth) // Проверяем границы перед обращением к platformY и platformX + 2
            {
                map[platformY, platformX + 2] = 999;
            }

            Invalidate(); // Перерисовываем холст
        }

        private void inputCheck(object sender, KeyEventArgs e)
        {
            map[platformY, platformX] = 0;
            if (e.KeyCode == Keys.Right && platformX + 2 < mapWidth) // Проверяем границы перед перемещением вправо
            {
                platformX++;
            }
            else if (e.KeyCode == Keys.Left && platformX > 0) // Проверяем границы перед перемещением влево
            {
                platformX--;
            }
            map[platformY, platformX] = 9;
            if (platformX + 1 < mapWidth) // Проверяем границы перед обновлением platformX + 1
            {
                map[platformY, platformX + 1] = 99;
            }
            if (platformX + 2 < mapWidth) // Проверяем границы перед обновлением platformX + 2
            {
                map[platformY, platformX + 2] = 999;
            }
        }
        //отрисовка веерхних платформ
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

        //границы карты
        public void DrawArea(Graphics g)
        {
            g.DrawRectangle(Pens.Black, new Rectangle(0, 0, mapWidth * 20, mapHeight * 20));
        }

        //инициализация элементов формы
        public void Init()
        {
            this.Width = (mapWidth + 5) * 20;
            this.Height = (mapHeight + 2) * 20;

            arcanoidSet = new Bitmap("C:\\Users\\rr033\\source\\repos\\Arcanoid_Game\\Arcanoid_Game\\Image\\arcanoid.png");
            timer1.Interval = 200;

            score = 0;

            scoreLabel.Text = "Score: " + score;
            LevelLabel.Text = "Level: " + level;
            //для начала заполняю  нулями
            for (int i = 0; i < mapHeight; i++)
            {
                for (int j = 0; j < mapWidth; j++)
                {
                    map[i, j] = 0;
                }
            }

            //начальное положение платформы
            platformX = (mapWidth - 1) / 2;
            platformY = mapHeight - 1;

            //добавляю платформу на карту(платформа занимает 3 ячейки). Инициализирую как значение 9, 99, 999
            map[platformY, platformX] = 9;
            map[platformY, platformX + 1] = 99;//чтобы отрисовка не рисовала платформу дважды (часть платформы)
            map[platformY, platformX + 2] = 999;//часть платформы

            //инициализация шара, находящегося на платформе
            ballY = platformY - 1;
            ballX = platformX + 1;

            //добавляю  шар на карту. Инициализирую как значение 8
            map[ballY, ballX] = 8;

            dirX = 1; //добавить
            dirY = -1;//добавить

            GeneratePlatforms();

            timer1.Start();
        }

        public bool IsCollide()
        {
            bool isColliding = false;
            if (ballX + dirX > mapWidth - 1 || ballX + dirX < 0)
            {
                dirX *= -1;
                isColliding = true;
            }
            if (ballY + dirY < 0)
            {
                dirY *= -1;
                isColliding = true;
            }

            if (map[ballY + dirY, ballX] != 0)
            {
                bool addScore = false;
                isColliding = true;

                if (map[ballY + dirY, ballX] > 10 && map[ballY + dirY, ballX] < 99)
                {
                    map[ballY + dirY, ballX] = 0;
                    map[ballY + dirY, ballX - 1] = 0;
                    addScore = true;
                }
                else if (map[ballY + dirY, ballX] < 9)
                {
                    map[ballY + dirY, ballX] = 0;
                    map[ballY + dirY, ballX + 1] = 0;
                    addScore = true;
                }
                if (addScore)
                {
                    score += 50;
                    if (score % 200 == 0 && score > 0)
                    {
                        if(timer1.Interval != 10) 
                        { 
                        timer1.Interval = timer1.Interval - 10;
                            level++;
                        }
                        AddLine();
                    }
                }
                dirY *= -1;
            }
            if (map[ballY, ballX + dirX] != 0)
            {
                bool addScore = false;
                isColliding = true;

                if (map[ballY, ballX + dirX] > 10 && map[ballY + dirY, ballX] < 99)
                {
                    map[ballY, ballX + dirX] = 0;
                    map[ballY, ballX + dirX - 1] = 0;
                    addScore = true;
                }
                else if (map[ballY, ballX + dirX] < 9)
                {
                    map[ballY, ballX + dirX] = 0;
                    map[ballY, ballX + dirX + 1] = 0;
                    addScore = true;
                }
                if (addScore)
                {
                    score += 50;
                    if (score % 200 == 0 && score > 0)
                    {
                        if (timer1.Interval != 10)
                        {
                            timer1.Interval = timer1.Interval - 10;
                            level++;
                        }
                        AddLine();
                    }
                }
                dirX *= -1;
            }
            scoreLabel.Text = "Score: " + score;
            LevelLabel.Text = "Level: " + level;
            return isColliding;
        }

        //отрисовка объектов
        public void DrawMap(Graphics g)
        {
            for (int i = 0; i < mapHeight; i++)
            {
                for (int j = 0; j < mapWidth; j++)
                {
                    if (map[i, j] == 9)//платформа
                    {
                        g.DrawImage(arcanoidSet, new Rectangle(new Point(j * 20, i * 20), new Size(60, 20)), 398, 17, 150, 50, GraphicsUnit.Pixel);
                    }
                    if (map[i, j] == 8)//шар
                    {
                        g.DrawImage(arcanoidSet, new Rectangle(new Point(j * 20, i * 20), new Size(20, 20)), 806, 548, 73, 73, GraphicsUnit.Pixel);
                    }
                    if (map[i, j] == 1)//платформа1
                    {
                        g.DrawImage(arcanoidSet, new Rectangle(new Point(j * 20, i * 20), new Size(40, 20)), 20, 16, 170, 59, GraphicsUnit.Pixel);
                    }
                    if (map[i, j] == 2)//платформа2
                    {
                        g.DrawImage(arcanoidSet, new Rectangle(new Point(j * 20, i * 20), new Size(40, 20)), 20, 16 + 77 * (map[i, j] - 1), 170, 59, GraphicsUnit.Pixel);
                    }
                    if (map[i, j] == 3)//платформа3
                    {
                        g.DrawImage(arcanoidSet, new Rectangle(new Point(j * 20, i * 20), new Size(40, 20)), 20, 16 + 77 * (map[i, j] - 1), 170, 59, GraphicsUnit.Pixel);
                    }
                    if (map[i, j] == 4)//платформа4
                    {
                        g.DrawImage(arcanoidSet, new Rectangle(new Point(j * 20, i * 20), new Size(40, 20)), 20, 16 + 77 * (map[i, j] - 1), 170, 59, GraphicsUnit.Pixel);
                    }
                    if (map[i, j] == 5)//платформа5
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
