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
        public int score;
        public Form1()
        {
            InitializeComponent();
            timer1.Tick += new EventHandler(update);

            scoreLabel = new Label();
            scoreLabel.Location = new Point((mapWidth) * 20 + 1, 50);
            scoreLabel.Text = "Score: " + score;
            this.Controls.Add(scoreLabel);

            Init();
        }
        private void update(object sender, EventArgs e)
        {
            Invalidate();//перерисовка холста
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

            arcanoidSet = new Bitmap("C:\\Users\\unico\\source\\repos\\ArcanoidGame\\ArcanoidGame\\Image\\arcanoid.png");
            timer1.Interval = 40;

            //счет
            score = 0;
            scoreLabel.Text = "Score: " + score;

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

            //добавляю платформы
            GeneratePlatforms();

            timer1.Start();
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
