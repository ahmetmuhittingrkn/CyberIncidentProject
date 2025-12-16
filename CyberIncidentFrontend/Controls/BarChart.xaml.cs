using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CyberIncidentWPF.Controls
{
    public partial class BarChart : UserControl
    {
        public static readonly DependencyProperty DataProperty = DependencyProperty.Register(
            nameof(Data), typeof(List<BarChartItem>), typeof(BarChart),
            new PropertyMetadata(null, OnDataChanged));

        public List<BarChartItem> Data
        {
            get => (List<BarChartItem>)GetValue(DataProperty);
            set => SetValue(DataProperty, value);
        }

        public BarChart()
        {
            InitializeComponent();
            Loaded += (s, e) => 
            {
                if (ChartCanvas.ActualWidth > 0 && ChartCanvas.ActualHeight > 0)
                {
                    UpdateChart();
                }
            };
        }

        private static void OnDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is BarChart chart)
            {
                chart.UpdateChart();
            }
        }

        private void UpdateChart()
        {
            ChartCanvas.Children.Clear();

            if (Data == null || Data.Count == 0)
            {
                ChartCanvas.Visibility = Visibility.Collapsed;
                return;
            }

            ChartCanvas.Visibility = Visibility.Visible;

            var maxValue = Data.Max(item => item.Value);
            if (maxValue == 0)
            {
                ChartCanvas.Visibility = Visibility.Collapsed;
                return;
            }

            var canvasHeight = ChartCanvas.ActualHeight > 0 ? ChartCanvas.ActualHeight : 200;
            var canvasWidth = ChartCanvas.ActualWidth > 0 ? ChartCanvas.ActualWidth : 500;
            
            if (canvasWidth <= 0 || canvasHeight <= 0)
                return;
                
            var availableWidth = canvasWidth - 40;
            var spacing = 15;
            var barWidth = Math.Max(30, (availableWidth - (spacing * (Data.Count - 1))) / Data.Count);
            var maxBarHeight = canvasHeight - 60;

            var xStart = 20.0;
            var yBase = canvasHeight - 20;

            for (int i = 0; i < Data.Count; i++)
            {
                var item = Data[i];
                var barHeight = Math.Max(1, (item.Value / maxValue) * maxBarHeight);
                var x = xStart + i * (barWidth + spacing);

                // Bar
                var bar = new Rectangle
                {
                    Width = barWidth,
                    Height = barHeight,
                    Fill = new SolidColorBrush(item.Color),
                    Stroke = Brushes.White,
                    StrokeThickness = 1,
                    Cursor = System.Windows.Input.Cursors.Hand,
                    ToolTip = $"{item.Label}: {item.Value}"
                };

                Canvas.SetLeft(bar, x);
                Canvas.SetBottom(bar, yBase);
                ChartCanvas.Children.Add(bar);

                // Label below bar
                var label = new TextBlock
                {
                    Text = item.Label,
                    FontSize = 10,
                    Foreground = new SolidColorBrush(Color.FromRgb(107, 114, 128)),
                    TextAlignment = TextAlignment.Center,
                    Width = barWidth
                };
                Canvas.SetLeft(label, x);
                Canvas.SetBottom(label, yBase - 20);
                ChartCanvas.Children.Add(label);

                // Value on top of bar
                if (barHeight > 15)
                {
                    var valueLabel = new TextBlock
                    {
                        Text = item.Value.ToString(),
                        FontSize = 11,
                        FontWeight = FontWeights.Bold,
                        Foreground = new SolidColorBrush(Color.FromRgb(55, 65, 81)),
                        TextAlignment = TextAlignment.Center,
                        Width = barWidth
                    };
                    Canvas.SetLeft(valueLabel, x);
                    Canvas.SetBottom(valueLabel, yBase + barHeight + 5);
                    ChartCanvas.Children.Add(valueLabel);
                }

                // Hover effect
                bar.MouseEnter += (s, e) =>
                {
                    bar.Opacity = 0.8;
                    bar.RenderTransform = new ScaleTransform(1.05, 1.05, barWidth / 2, barHeight / 2);
                };
                bar.MouseLeave += (s, e) =>
                {
                    bar.Opacity = 1.0;
                    bar.RenderTransform = Transform.Identity;
                };
            }

            // Update legend
            LegendItems.ItemsSource = Data;
        }

        private void ChartCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.Width > 0 && e.NewSize.Height > 0)
            {
                UpdateChart();
            }
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            if (sizeInfo.NewSize.Width > 0 && sizeInfo.NewSize.Height > 0)
            {
                // Delay to ensure canvas is properly sized
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    if (ChartCanvas.ActualWidth > 0 && ChartCanvas.ActualHeight > 0)
                    {
                        UpdateChart();
                    }
                }), System.Windows.Threading.DispatcherPriority.Loaded);
            }
        }
    }

    public class BarChartItem
    {
        public string Label { get; set; } = string.Empty;
        public double Value { get; set; }
        public Color Color { get; set; }
    }
}
