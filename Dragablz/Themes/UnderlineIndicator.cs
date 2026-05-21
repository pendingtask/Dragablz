using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Dragablz.Themes
{
    /// <summary>
    /// Material Design 动画下划线指示器。选中时水平展开，取消选中时收缩。
    /// </summary>
    public class UnderlineIndicator : Control
    {
        static UnderlineIndicator()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(UnderlineIndicator),
                new FrameworkPropertyMetadata(typeof(UnderlineIndicator)));
        }

        public static readonly DependencyProperty IsActivatedProperty = DependencyProperty.Register(
            nameof(IsActivated), typeof(bool), typeof(UnderlineIndicator),
            new PropertyMetadata(false, OnActivatedChanged));

        public bool IsActivated
        {
            get => (bool)GetValue(IsActivatedProperty);
            set => SetValue(IsActivatedProperty, value);
        }

        public static readonly DependencyProperty IndicatorBrushProperty = DependencyProperty.Register(
            nameof(IndicatorBrush), typeof(Brush), typeof(UnderlineIndicator),
            new PropertyMetadata(default(Brush)));

        public Brush IndicatorBrush
        {
            get => (Brush)GetValue(IndicatorBrushProperty);
            set => SetValue(IndicatorBrushProperty, value);
        }

        public static readonly DependencyProperty IndicatorThicknessProperty = DependencyProperty.Register(
            nameof(IndicatorThickness), typeof(double), typeof(UnderlineIndicator),
            new PropertyMetadata(2.0));

        public double IndicatorThickness
        {
            get => (double)GetValue(IndicatorThicknessProperty);
            set => SetValue(IndicatorThicknessProperty, value);
        }

        public static readonly DependencyProperty IndicatorCornerRadiusProperty = DependencyProperty.Register(
            nameof(IndicatorCornerRadius), typeof(double), typeof(UnderlineIndicator),
            new PropertyMetadata(0.0));

        public double IndicatorCornerRadius
        {
            get => (double)GetValue(IndicatorCornerRadiusProperty);
            set => SetValue(IndicatorCornerRadiusProperty, value);
        }

        private static void OnActivatedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var indicator = (UnderlineIndicator)d;
            VisualStateManager.GoToState(indicator, (bool)e.NewValue ? "Activated" : "Deactivated", true);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            VisualStateManager.GoToState(this, IsActivated ? "Activated" : "Deactivated", false);
        }
    }
}
