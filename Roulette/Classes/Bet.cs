using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Roulette.Models {
    public class Bet {
        public TextBlock TextBlock { get; set; }
        public Border Border { get; set; }
        public FontWeight FontWeight { get; set; } = FontWeights.Bold;
        public VerticalAlignment VerticalAlignment { get; set; } = VerticalAlignment.Stretch;
        public HorizontalAlignment VorizontalAlignment { get; set; } = HorizontalAlignment.Stretch;
        public FontFamily FontFamily { get; set; } = new FontFamily("Yu Gothic");
        public Thickness BorderThickness { get; set; } = new Thickness(1);
        public CornerRadius CornerRadius { get; set; } = new CornerRadius(4);
        public Thickness Margin { get; set; } = new Thickness(2);
        public Brush Background { get; set; } = Brushes.Transparent;
        public Brush BorderBrush { get; set; } = Brushes.Gray;
        public Brush Foreground { get; set; } = Brushes.AntiqueWhite;

        public Bet(string name) {

            TextBlock = new TextBlock() {
                Text = name,
                Foreground = Foreground,
                FontWeight = FontWeight,
                FontFamily = FontFamily,
                TextWrapping = TextWrapping.Wrap, 
                VerticalAlignment = VerticalAlignment.Center, 
                HorizontalAlignment = HorizontalAlignment.Center, 
            };

            Border = new Border() {
                Name = "_" + name.Replace(" ", "_"),
                BorderBrush = BorderBrush,
                BorderThickness = BorderThickness,
                Background = Background,
                CornerRadius = CornerRadius,
                Margin = Margin,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Child = TextBlock
            };

            Border.SizeChanged += OnBorderSizeChanged;
        }

        private void OnBorderSizeChanged(object sender, SizeChangedEventArgs e) {
            double newWidth = e.NewSize.Width;
            double newHeight = e.NewSize.Height;

            // Calculate the scale factor based on the new size
            double scaleFactor = Math.Min(
                newWidth / TextBlock.ActualWidth,
                newHeight / TextBlock.ActualHeight
            );

            TextBlock.FontSize = (TextBlock.FontSize * scaleFactor) - 6;
            // Adjust the last number to your liking
        }
        public class RowBet : Bet {
            public RowBet(string name, int row): base(name) {
                TextBlock.RenderTransformOrigin = new Point(0.5, 0.5);
                RotateTransform rotateTransform = new RotateTransform(270);
                TextBlock.RenderTransform = rotateTransform;

                if(row + 1 == 1) Border.Name = "_1st_row";
                else if (row + 1 == 2) Border.Name = "_2nd_row";
                else Border.Name = "_3rd_row";

            }
        }

        public class ColorBet : Bet {
            public ColorBet(string name) : base(name) {
                Border.Background = name == "Red" ? Brushes.Red : Brushes.Transparent;
                TextBlock.Text = "";
            }
        }
        public static ObservableCollection<string> GetWinningBets(int num,
            int[] blackNums,
            MyViewModel ViewModel) {

            ObservableCollection<string> winningBets = new ObservableCollection<string>();

            winningBets.Add(num.ToString());

            if (num != 0) {
                if (num <= 18) winningBets.Add("1 to 18");
                else winningBets.Add("19 to 36");

                if (num % 2 == 0) winningBets.Add("Even");
                else winningBets.Add("Odd");

                if (num <= 12) winningBets.Add("1st 12");
                else if (num <= 24) winningBets.Add("2nd 12");
                else winningBets.Add("3rd 12");

                if (num % 3 == 0) winningBets.Add("1st row");
                else if ((num + 1) % 3 == 0) winningBets.Add("2nd row");
                else winningBets.Add("3rd row");

                if (blackNums.Contains(num)) {
                    winningBets.Add("Black");
                    SetNumberColor(Brushes.AntiqueWhite, ViewModel);
                }
                else {
                    winningBets.Add("Red");
                    SetNumberColor(Brushes.Red, ViewModel);
                }
            }
            else {
                SetNumberColor(Brushes.Green, ViewModel);

            }
            ViewModel.WinningBets = winningBets;

            return ViewModel.WinningBets;

        }

        public static void SetNumberColor(SolidColorBrush numColor, MyViewModel ViewModel) {
            ViewModel.WinningNumberColor = numColor;
        }
        public static void ShowBets(List<UIElement> bets, MyViewModel ViewModel) {
            foreach (var bet in bets) ShowBet(bet, ViewModel);
        }

        public static void ShowBet(object bet, MyViewModel ViewModel) {
            if (bet is Grid) {
                Grid grid = bet as Grid;

                // Find the Border child inside the Grid
                Border border = null;
                foreach (UIElement child in grid.Children) {
                    if (child is Border) {
                        border = child as Border;
                        break;
                    }
                }

                if (border != null) {
                    // Perform the animation on the Border
                    Animations.AnimateWinningColor(border, ViewModel.WinningBetColor.Color, 10);
                }
            }
            else if (bet is Border) {
                Border border = bet as Border;
                if (border != null) {
                    Animations.AnimateWinningColor(border, ViewModel.WinningBetColor.Color, 10);
                }
            }
        }

    }
}
