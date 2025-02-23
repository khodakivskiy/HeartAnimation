using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace ParticleHeartAnimation
{
    public partial class MainWindow : Window
    {
        private const int ParticleCount = 3700;
        private List<Ellipse> particles = new List<Ellipse>();
        private List<Point> targetPositions = new List<Point>();
        private Random rand = new Random();
        private DispatcherTimer heartbeatTimer = new DispatcherTimer();
        private bool isDimming = true;
        private ScaleTransform heartScale = new ScaleTransform(1.0, 1.0, 300, 300); // Центр масштабування

        public MainWindow()
        {
            InitializeComponent();
            CreateHeartParticles();
            CompositionTarget.Rendering += UpdateParticles;
            this.MouseMove += OnMouseMove;

            // Додаємо трансформацію масштабу до Canvas
            HeartCanvas.RenderTransform = heartScale;

            // Таймер для пульсації (~75-80 уд/хв)
            heartbeatTimer.Interval = TimeSpan.FromMilliseconds(800);
            heartbeatTimer.Tick += HeartbeatEffect;
            heartbeatTimer.Start();
        }

        private void CreateHeartParticles()
        {
            double scale = 8;
            double centerX = 300;
            double centerY = 300;
            int gridSize = (int)Math.Sqrt(ParticleCount);

            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    double u = (double)i / gridSize;
                    double v = (double)j / gridSize;

                    double angle = u * 2 * Math.PI;
                    double radius = Math.Sqrt(v);

                    double x = radius * 16 * Math.Pow(Math.Sin(angle), 3);
                    double y = radius * (13 * Math.Cos(angle) - 5 * Math.Cos(2 * angle) -
                                         2 * Math.Cos(3 * angle) - Math.Cos(4 * angle));

                    double posX = centerX + x * scale;
                    double posY = centerY - y * scale;

                    Ellipse particle = new Ellipse
                    {
                        Width = 1,
                        Height = 1,
                        Fill = Brushes.Red,
                        Opacity = 0.0 // Початково невидимі
                    };
                    HeartCanvas.Children.Add(particle);
                    particles.Add(particle);
                    targetPositions.Add(new Point(posX, posY));

                    double startX = rand.Next(600);
                    double startY = rand.Next(600);
                    Canvas.SetLeft(particle, startX);
                    Canvas.SetTop(particle, startY);

                    // Анімація появи
                    DoubleAnimation fadeIn = new DoubleAnimation
                    {
                        To = 0.9,
                        Duration = TimeSpan.FromSeconds(rand.NextDouble() * 1.2),
                        AutoReverse = false
                    };
                    particle.BeginAnimation(Ellipse.OpacityProperty, fadeIn);
                }
            }
        }

        private void HeartbeatEffect(object sender, EventArgs e)
        {
            double targetScale = isDimming ? 1.0 : 1.05; // Серце розширюється і повертається
            double duration = 0.3;

            DoubleAnimation scaleAnim = new DoubleAnimation
            {
                To = targetScale,
                Duration = TimeSpan.FromSeconds(duration),
                AutoReverse = true, // Автоматично повертається у вихідне положення
                EasingFunction = new SineEase() // Плавний ефект
            };

            heartScale.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnim);
            heartScale.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnim);

            isDimming = !isDimming;
        }

        private void UpdateParticles(object sender, EventArgs e)
        {
            for (int i = 0; i < particles.Count; i++)
            {
                Ellipse particle = particles[i];
                Point target = targetPositions[i];
                double currentX = Canvas.GetLeft(particle);
                double currentY = Canvas.GetTop(particle);

                double dx = target.X - currentX;
                double dy = target.Y - currentY;

                if (Math.Abs(dx) > 0.5 || Math.Abs(dy) > 0.5)
                {
                    Canvas.SetLeft(particle, currentX + dx * 0.07);
                    Canvas.SetTop(particle, currentY + dy * 0.07);
                }
            }
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            Point mousePos = e.GetPosition(this);
            double expandedRepelDistance = 30;

            foreach (Ellipse particle in particles)
            {
                double currentX = Canvas.GetLeft(particle);
                double currentY = Canvas.GetTop(particle);

                double dx = currentX - mousePos.X;
                double dy = currentY - mousePos.Y;
                double distanceSquared = dx * dx + dy * dy;

                if (distanceSquared < expandedRepelDistance * expandedRepelDistance)
                {
                    double distance = Math.Sqrt(distanceSquared);
                    double repelFactor = (expandedRepelDistance - distance) / expandedRepelDistance;

                    Canvas.SetLeft(particle, currentX + dx * repelFactor * 0.3);
                    Canvas.SetTop(particle, currentY + dy * repelFactor * 0.3);
                }
            }
        }
    }
}
