using System;
using System.Windows;
using System.Windows.Controls;

namespace AquaExplorer.Controls
{

    public class RadialPanel : Panel
    {

        public static readonly DependencyProperty RadiusXProperty = DependencyProperty.Register("RadiusX", typeof(double), typeof(RadialPanel), new PropertyMetadata(20d));
        public double RadiusX
        {
            get { return (double)GetValue(RadiusXProperty); }
            set { SetValue(RadiusXProperty, value); }
        }

        public static readonly DependencyProperty RadiusYProperty = DependencyProperty.Register("RadiusY", typeof(double), typeof(RadialPanel), new PropertyMetadata(20d));
        public double RadiusY
        {
            get { return (double)GetValue(RadiusYProperty); }
            set { SetValue(RadiusYProperty, value); }
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            foreach (UIElement elem in Children)
            {
                elem.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            }
            return base.MeasureOverride(availableSize);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            if (Children.Count == 0)
                return finalSize;

            double angle = 0;
            double incrementalAngularSpace = -(360.0 / Children.Count) * (Math.PI / 180);

            double radiusX = RadiusX;
            double radiusY = RadiusY;

            foreach (UIElement elem in Children)
            {
                var childPoint = new Point(Math.Cos(angle) * radiusX, -Math.Sin(angle) * radiusY);
                var actualChildPoint = new Point(finalSize.Width / 2 + childPoint.X - elem.DesiredSize.Width / 2, finalSize.Height / 2 + childPoint.Y - elem.DesiredSize.Height / 2);

                elem.Arrange(new Rect(actualChildPoint.X, actualChildPoint.Y, elem.DesiredSize.Width, elem.DesiredSize.Height));
                angle += incrementalAngularSpace;

            }

            return finalSize;
        }
    }
}
