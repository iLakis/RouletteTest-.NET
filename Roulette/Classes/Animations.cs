using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Roulette.Models {
    public class Animations {
        public static void FadeAnim(object obj, int duration) {
            Border border = (Border)obj;
            DoubleAnimation fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(duration / 2));
            fadeIn.AutoReverse = true;
            border.BeginAnimation(Border.OpacityProperty, fadeIn);
        }
        public static void AnimateWinningColor(object obj, Color targetColor, int duration) {
            Border _object = (Border)obj;
            Color initialColor = (_object.Background as SolidColorBrush).Color;
            ColorAnimation colorAnimation = new ColorAnimation(
                initialColor,
                targetColor,
                TimeSpan.FromSeconds(duration / 2));

            colorAnimation.AutoReverse = true;
            SolidColorBrush borderBrush = _object.Background as SolidColorBrush;

            if (borderBrush == null || borderBrush.IsFrozen) {
                borderBrush = new SolidColorBrush();
                _object.Background = borderBrush;
            }


            borderBrush.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimation);
        }
        public static void AnimateZero(Path obj, Color targetColor, int duration) {
            Color initialColor = (obj.Fill as SolidColorBrush).Color;
            ColorAnimation colorAnimation = new ColorAnimation(
                initialColor,
                targetColor,
                TimeSpan.FromSeconds(duration / 2));

            colorAnimation.AutoReverse = true;
            SolidColorBrush pathBrush = obj.Fill as SolidColorBrush;

            if (pathBrush == null || pathBrush.IsFrozen) {
                pathBrush = new SolidColorBrush();
                obj.Fill = pathBrush;
            }

            pathBrush.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimation);
        }

    }
}
