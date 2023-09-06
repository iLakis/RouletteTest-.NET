
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Roulette.Models {
    public class RouletteNumber {

        public int number { get; set; } // 0 to 36
        public int FontSize { get; set; } = 14;
        public FontWeight FontWeight { get; set; } = FontWeights.Bold;
        public VerticalAlignment VerticalAlignment { get; set; } = VerticalAlignment.Center;
        public HorizontalAlignment HorizontalAlignment { get; set; } = HorizontalAlignment.Center;
        public FontFamily FontFamily { get; set; } = new FontFamily("Yu Gothic");
        public TextBlock textBlock { get; set; }
        public Border border { get; set; }
        public Thickness BorderThickness { get; set; } = new Thickness(1);
        public CornerRadius CornerRadius { get; set; } = new CornerRadius(4);
        public Thickness Margin { get; set; } = new Thickness(2);
        public Grid grid { get; set; }



        public RouletteNumber(int num) {
            if (num < 0 || num > 36) return;
            number = num;
            textBlock = new TextBlock {
                Text = number.ToString(),
                VerticalAlignment = VerticalAlignment,
                HorizontalAlignment = HorizontalAlignment,
                FontFamily = FontFamily
            };
            border = new Border {
                Name = $"_{number}",
                BorderThickness = BorderThickness,
                CornerRadius = CornerRadius,
                Margin = Margin,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Child = textBlock
            };

            grid = new Grid() {
                Children = { border }
            };

            border.SizeChanged += OnBorderSizeChanged;

        }

        public class RedNum : RouletteNumber {
            Brush Foreground = Brushes.Black;
            Brush BorderBrush = Brushes.Gray;
            Brush Background = Brushes.Red;


            public RedNum(int num) : base(num){
                textBlock.Foreground = this.Foreground;
                border.BorderBrush = this.BorderBrush;
                border.Background = this.Background;
            }
        
        }
        public class BlackNum : RouletteNumber {

            Brush Foreground = Brushes.AntiqueWhite;
            Brush BorderBrush = Brushes.Green;
            Brush Background = Brushes.Transparent;
            public BlackNum(int num) : base(num) {
                textBlock.Foreground = this.Foreground;
                border.BorderBrush = this.BorderBrush;
                border.Background = this.Background;
            }

        }
        private void OnBorderSizeChanged(object sender, SizeChangedEventArgs e) {
            double newWidth = e.NewSize.Width;
            double newHeight = e.NewSize.Height;

            // Calculate the scale factor based on the new size
            double scaleFactor = Math.Min(newWidth / textBlock.ActualWidth, newHeight / textBlock.ActualHeight);

            textBlock.FontSize = (textBlock.FontSize * scaleFactor) - 10;
        }

    }
}
