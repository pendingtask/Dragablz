using System.Windows;
using System.Windows.Media;

namespace Dragablz.Themes
{
    /// <summary>
    /// Material Design 主题的附加属性扩展。
    /// 提供头部着色、指示器动画等额外主题配置能力。
    /// </summary>
    public static class MaterialDesignHeaderedAssist
    {
        /// <summary>
        /// Tab 头部条的背景色。支持继承，可在父级控件上设置后自动应用到所有子 Tab 项。
        /// </summary>
        public static readonly DependencyProperty HeaderBackgroundProperty = DependencyProperty.RegisterAttached(
            "HeaderBackground", typeof(Brush), typeof(MaterialDesignHeaderedAssist),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

        public static void SetHeaderBackground(DependencyObject element, Brush value) =>
            element.SetValue(HeaderBackgroundProperty, value);

        public static Brush GetHeaderBackground(DependencyObject element) =>
            (Brush)element.GetValue(HeaderBackgroundProperty);

        /// <summary>
        /// Tab 头部条的前景色。支持继承。
        /// </summary>
        public static readonly DependencyProperty HeaderForegroundProperty = DependencyProperty.RegisterAttached(
            "HeaderForeground", typeof(Brush), typeof(MaterialDesignHeaderedAssist),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

        public static void SetHeaderForeground(DependencyObject element, Brush value) =>
            element.SetValue(HeaderForegroundProperty, value);

        public static Brush GetHeaderForeground(DependencyObject element) =>
            (Brush)element.GetValue(HeaderForegroundProperty);

        /// <summary>
        /// 是否启用下划线指示器的 ScaleTransform 动画。默认为 true。支持继承。
        /// </summary>
        public static readonly DependencyProperty IsIndicatorAnimatedProperty = DependencyProperty.RegisterAttached(
            "IsIndicatorAnimated", typeof(bool), typeof(MaterialDesignHeaderedAssist),
            new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.Inherits));

        public static void SetIsIndicatorAnimated(DependencyObject element, bool value) =>
            element.SetValue(IsIndicatorAnimatedProperty, value);

        public static bool GetIsIndicatorAnimated(DependencyObject element) =>
            (bool)element.GetValue(IsIndicatorAnimatedProperty);

        /// <summary>
        /// 下划线指示器的厚度。默认为 2.0。支持继承。
        /// </summary>
        public static readonly DependencyProperty IndicatorThicknessProperty = DependencyProperty.RegisterAttached(
            "IndicatorThickness", typeof(double), typeof(MaterialDesignHeaderedAssist),
            new FrameworkPropertyMetadata(2.0, FrameworkPropertyMetadataOptions.Inherits));

        public static void SetIndicatorThickness(DependencyObject element, double value) =>
            element.SetValue(IndicatorThicknessProperty, value);

        public static double GetIndicatorThickness(DependencyObject element) =>
            (double)element.GetValue(IndicatorThicknessProperty);
    }
}
