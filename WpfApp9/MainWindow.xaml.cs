using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
namespace WpfApp9
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool goLeft, goRight = false;

        int enemyImage = 0;

        int totalEnemys;

        int enemySpeed = 6;

        int fireTimer;

        int fireTimerLim = 90;

        DispatcherTimer dispatcherTimer = new DispatcherTimer();

        ImageBrush playerImage = new ImageBrush();
        List<Rectangle> removeItems = new List<Rectangle>();
        public MainWindow()
        {
            InitializeComponent();
            dispatcherTimer.Tick += GameEngine;
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(20);
            dispatcherTimer.Start();
            playerImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/player.png"));
            mainHero.Fill = playerImage;
            MakeEnemy(20);
        }
        private void Canvas_KeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.D)
            {
                goRight = true;
            }
            if (e.Key == Key.A)
            {
                goLeft = true;
            }
        }

        private void Canvas_KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.D)
            {
                goRight = false;
            }
            if (e.Key == Key.A)
            {
                goLeft = false;
            }
            if (e.Key == Key.Space)
            {
                removeItems.Clear();


                Rectangle newBullet = new Rectangle
                {
                    Tag = "bullet",
                    Height = 20,
                    Width = 5,
                    Fill = Brushes.White,
                    Stroke = Brushes.Blue

                };

                Canvas.SetTop(newBullet, Canvas.GetTop(mainHero) - newBullet.Height);
                Canvas.SetLeft(newBullet, Canvas.GetLeft(mainHero) + mainHero.Width / 2);
                My_canvas.Children.Add(newBullet);
            }
        }
        private void EnemyFire(double x, double y)
        {
            Rectangle newEnemyFire = new Rectangle
            {
                Tag = "enemyFire",
                Height = 20,
                Width = 15,
                Fill = Brushes.Yellow,
                Stroke = Brushes.Red,
                StrokeThickness = 5
            };
            Canvas.SetTop(newEnemyFire, y);
            Canvas.SetLeft(newEnemyFire, x);
            My_canvas.Children.Add(newEnemyFire);
        }
        private void MakeEnemy(int MaxEnemy)
        {
            int left = 0;
            totalEnemys = MaxEnemy;

            for (int i = 0; i < MaxEnemy; i++)
            {
                ImageBrush enemySkin = new ImageBrush();
                Rectangle newEnemy = new Rectangle
                {
                    Tag = "enemy",
                    Height = 45,
                    Width = 45,
                    Fill = enemySkin,
                };
                Canvas.SetTop(newEnemy, 10);
                Canvas.SetLeft(newEnemy, left);

                My_canvas.Children.Add(newEnemy);

                left -= 60;

                enemyImage++;

                if (enemyImage > 8)
                {
                    enemyImage = 1;
                }
                switch (enemyImage)
                {
                    case 1:
                        enemySkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/invader1.gif"));
                        break;
                    case 2:
                        enemySkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/invader2.gif"));
                        break;
                    case 3:
                        enemySkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/invader3.gif"));
                        break;
                    case 4:
                        enemySkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/invader4.gif"));
                        break;
                    case 5:
                        enemySkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/invader5.gif"));
                        break;
                    case 6:
                        enemySkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/invader6.gif"));
                        break;
                    case 7:
                        enemySkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/invader7.gif"));
                        break;
                    case 8:
                        enemySkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/invader8.gif"));
                        break;
                }

            }

        }
        private void GameEngine(object sender, EventArgs e)
        {
            Rect player = new Rect(Canvas.GetLeft(mainHero), Canvas.GetTop(mainHero), mainHero.Width, mainHero.Height);
            enemyleft.Content = "Врагов осталось:" + totalEnemys;
            if (goLeft && Canvas.GetLeft(mainHero) > 0)
            {
                Canvas.SetLeft(mainHero, Canvas.GetLeft(mainHero) - 10);
            }
            else if (goRight && Canvas.GetLeft(mainHero) + 80 < Application.Current.MainWindow.Width)
            {
                Canvas.SetLeft(mainHero, Canvas.GetLeft(mainHero) + 10);
            }
            fireTimer -= 3;

            if (fireTimer < 0)
            {
                EnemyFire((Canvas.GetLeft(mainHero) + 20), 10);
                fireTimer = fireTimerLim;
            }
            if (totalEnemys < 10)
            {
                enemySpeed = 15;
            }
            foreach (var x in My_canvas.Children.OfType<Rectangle>())
            {

                if (x is Rectangle && (string)x.Tag == "bullet")
                {


                    Canvas.SetTop(x, Canvas.GetTop(x) - 20);


                    Rect bullet = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);


                    if (Canvas.GetTop(x) < 10)
                    {
                        removeItems.Add(x);

                    }


                    foreach (var y in My_canvas.Children.OfType<Rectangle>())
                    {

                        if (y is Rectangle && (string)y.Tag == "enemy")
                        {

                            Rect enemy = new Rect(Canvas.GetLeft(y), Canvas.GetTop(y), y.Width, y.Height);


                            if (bullet.IntersectsWith(enemy))
                            {
                                removeItems.Add(x);
                                removeItems.Add(y);
                                totalEnemys -= 1;
                            }
                        }



                    }
                }
                if (x is Rectangle && (string)x.Tag == "enemy")
                {
                    Canvas.SetLeft(x, Canvas.GetLeft(x) + enemySpeed);


                    if (Canvas.GetLeft(x) > 820)
                    {

                        Canvas.SetLeft(x, -80);

                        Canvas.SetTop(x, Canvas.GetTop(x) + (x.Height + 10));

                    }
                    Rect enemy = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);

                    if (player.IntersectsWith(enemy))
                    {

                        dispatcherTimer.Stop();
                        MessageBox.Show("Ты проиграл!");
                    }
                }
                if (x is Rectangle && (string)x.Tag == "enemyFire")
                {
                    Canvas.SetTop(x, Canvas.GetTop(x) + 10);

                    if (Canvas.GetTop(x) > 480)
                    {
                        removeItems.Add(x);
                    }
                    Rect enemyBullets = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);
                    

                    if (enemyBullets.IntersectsWith(player))
                    {

                        dispatcherTimer.Stop();
                        MessageBox.Show("Ты проиграл!");
                    }
                }
            }                                                                                                               
            foreach (Rectangle y in removeItems)
            {
                My_canvas.Children.Remove(y);
            }

            if(totalEnemys < 1)
            {
                dispatcherTimer.Stop();
                MessageBox.Show("Ты победил! Поздравляю!");
            }
        }
    }
}
