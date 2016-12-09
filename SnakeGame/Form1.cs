using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SnakeGame
{
    public partial class Form1 : Form
    {
        enum Direction
        {
            Right,
            Left,
            Up,
            Down
        };

        Bitmap bitmap;
        SolidBrush myBrush;
        SolidBrush foodBrush;
        Color backgroundColor = Color.FromArgb(180, 190, 0);
        int xPosition, yPosition, snakeBits, xLast, yLast;
        int points = 0;
        Rectangle snake;
        List<Rectangle> snakeLenght;
        Rectangle food;
        Graphics graphics;
        Graphics graphicsOverride;
        Direction direction = Direction.Right;
        
        public Form1()
        {
            InitializeComponent();

            myBrush = new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(97, 73, 14));
            foodBrush = new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(62 , 36, 0));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            NewGame();
        }

        void NewGame()
        {
            points = 0;
            this.Text = "Points: " + points;
            xPosition = 200;
            yPosition = 200;
            snakeBits = 0;
            bitmap = new Bitmap(this.Width, this.Height);
            graphics = Graphics.FromImage(bitmap);
            graphicsOverride = this.CreateGraphics();
            snake = new Rectangle(xPosition, yPosition, 9, 9);
            snakeLenght = new List<Rectangle>();

            FoodSpawn();
            timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            graphics.Clear(backgroundColor);
            graphics.FillRectangle(foodBrush, food);
            graphics.FillRectangle(myBrush, snake);
            xLast = snake.X;
            yLast = snake.Y;

            if(snakeBits > 0)
            {
                AddSnakeBits();

                int currentValue = 0;
                snakeLenght.Reverse();
                foreach (Rectangle rect in snakeLenght)
                {
                    currentValue++;
                    graphics.FillRectangle(myBrush, rect);
                    if(currentValue > snakeBits)
                    {
                        break;
                    }
                }
                snakeLenght.Reverse();
            }

            graphicsOverride.DrawImage(bitmap, 0, 0, this.Width, this.Height);
            DirectionTaken();
            FoodCollision();
            SnakeSelfCollision();
        }

        void SnakeSelfCollision()
        {
            int currentValue = 0;
            snakeLenght.Reverse();
            foreach (Rectangle rect in snakeLenght)
            {
                currentValue++;
                
                if (rect.X == snake.X && rect.Y == snake.Y)
                {
                    timer.Stop();
                    MessageBox.Show("You died. You got " + points + " points. Game will restart.");
                    NewGame();
                }
                if (currentValue > snakeBits)
                {
                    break;
                }
            }
            snakeLenght.Reverse();
        }

        void AddSnakeBits()
        {
            snakeLenght.Add(new Rectangle(xLast, yLast, 9, 9));
        }

        void FoodCollision()
        {
            if(snake.X == food.X && snake.Y == food.Y)
            {
                points += 10;
                snakeBits++;
                this.Text = "Points: " + points;
                FoodSpawn();
            }
        }

        void FoodSpawn()
        {
            Random random = new Random();
            int randomValue = random.Next(2, 35);
            randomValue = randomValue * 10;
            int randomValue2 = random.Next(2, 35);
            randomValue2 = randomValue2 * 10;

            food = new Rectangle(randomValue, randomValue2, 9, 9);
        }

        void DirectionTaken()
        {
            if (direction == Direction.Right)
            {
                snake.X += 10;
                if(snake.X > 380)
                {
                    snake.X = 0;
                }
            }
            else if (direction == Direction.Left)
            {
                snake.X -= 10;
                if (snake.X < 0)
                {
                    snake.X = 380;
                }
            }
            else if (direction == Direction.Up)
            {
                snake.Y -= 10;
                if (snake.Y < 0)
                {
                    snake.Y = 380;
                }
            }
            else if (direction == Direction.Down)
            {
                snake.Y += 10;
                if (snake.Y > 380)
                {
                    snake.Y = 10;
                }
            }
        }

        

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if(keyData == Keys.Right)
            {
                if(direction != Direction.Left)
                {
                    direction = Direction.Right;
                }
            }
            if (keyData == Keys.Left)
            {
                if (direction != Direction.Right)
                {
                    direction = Direction.Left;
                }
            }
            if (keyData == Keys.Up)
            {
                if (direction != Direction.Down)
                {
                    direction = Direction.Up;
                }
            }
            if (keyData == Keys.Down)
            {
                if (direction != Direction.Up)
                {
                    direction = Direction.Down;
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
