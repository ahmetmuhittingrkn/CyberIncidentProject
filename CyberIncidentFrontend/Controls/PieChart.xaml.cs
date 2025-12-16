using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CyberIncidentWPF.Controls
{
    public partial class PieChart : UserControl
    {
        public static readonly DependencyProperty DataProperty = DependencyProperty.Register(
            nameof(Data), typeof(List<PieChartItem>), typeof(PieChart),
            new PropertyMetadata(null, OnDataChanged));

        public List<PieChartItem> Data
        {
            get => (List<PieChartItem>)GetValue(DataProperty);
            set => SetValue(DataProperty, value);
        }

        public PieChart()
        {
            InitializeComponent();
        }

        private static void OnDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PieChart chart)
            {
                chart.UpdateChart();
            }
        }

        private void UpdateChart()
        {
            ChartCanvas.Children.Clear();

            if (Data == null || Data.Count == 0)
                return;

            var total = Data.Sum(item => item.Value);
            if (total == 0)
                return;

            var centerX = ChartCanvas.Width / 2;
            var centerY = ChartCanvas.Height / 2;
            var radius = Math.Min(centerX, centerY) - 20;

            double startAngle = -90; // Start from top

            foreach (var item in Data)
            {
                var percentage = item.Value / total;
                var sweepAngle = percentage * 360;

                var path = new Path
                {
                    Fill = new SolidColorBrush(item.Color),
                    Stroke = Brushes.White,
                    StrokeThickness = 2,
                    Data = CreatePieSlice(centerX, centerY, radius, startAngle, sweepAngle),
                    Cursor = System.Windows.Input.Cursors.Hand,
                    ToolTip = $"{item.Label}: {item.Value} ({percentage:P1})"
                };

                path.MouseEnter += (s, e) =>
                {
                    path.StrokeThickness = 4;
                    path.RenderTransform = new ScaleTransform(1.05, 1.05, centerX, centerY);
                };
                path.MouseLeave += (s, e) =>
                {
                    path.StrokeThickness = 2;
                    path.RenderTransform = Transform.Identity;
                };

                ChartCanvas.Children.Add(path);
                startAngle += sweepAngle;
            }

            // Update legend
            LegendItems.ItemsSource = Data.OrderByDescending(d => d.Value);
        }

        private Geometry CreatePieSlice(double centerX, double centerY, double radius, double startAngle, double sweepAngle)
        {
            var pathFigure = new PathFigure
            {
                StartPoint = new Point(centerX, centerY),
                IsClosed = true
            };

            var startAngleRad = startAngle * Math.PI / 180;
            var endAngleRad = (startAngle + sweepAngle) * Math.PI / 180;

            var startX = centerX + radius * Math.Cos(startAngleRad);
            var startY = centerY + radius * Math.Sin(startAngleRad);

            var endX = centerX + radius * Math.Cos(endAngleRad);
            var endY = centerY + radius * Math.Sin(endAngleRad);

            pathFigure.Segments.Add(new LineSegment(new Point(startX, startY), true));
            pathFigure.Segments.Add(new ArcSegment(
                new Point(endX, endY),
                new Size(radius, radius),
                0,
                sweepAngle > 180,
                SweepDirection.Clockwise,
                true));

            var pathGeometry = new PathGeometry();
            pathGeometry.Figures.Add(pathFigure);
            return pathGeometry;
        }
    }

    public class PieChartItem
    {
        public string Label { get; set; } = string.Empty;
        public double Value { get; set; }
        public Color Color { get; set; }
    }
}
