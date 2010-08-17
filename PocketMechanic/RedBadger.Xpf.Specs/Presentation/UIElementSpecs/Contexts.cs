//-------------------------------------------------------------------------------------------------
// <auto-generated> 
// Marked as auto-generated so StyleCop will ignore BDD style tests
// </auto-generated>
//-------------------------------------------------------------------------------------------------

#pragma warning disable 169
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local

namespace RedBadger.Xpf.Specs.Presentation.UIElementSpecs
{
    using System.Windows;

    using Machine.Specifications;

    using Moq;
    using Moq.Protected;

    using RedBadger.Xpf.Graphics;
    using RedBadger.Xpf.Presentation.Controls;
    using RedBadger.Xpf.Presentation.Media;

    using UIElement = RedBadger.Xpf.Presentation.UIElement;

    public abstract class a_UIElement
    {
        protected const string ArrangeOverride = "ArrangeOverride";

        protected const string MeasureOverride = "MeasureOverride";

        protected static Mock<UIElement> UIElement;

        private Establish context = () => UIElement = new Mock<UIElement> { CallBase = true };
    }

    public abstract class a_Measured_UIElement : a_UIElement
    {
        protected const string VisualOffset = "VisualOffset";

        protected static readonly Size availableSize = new Size(100, 100);

        protected static readonly Size desiredSize = new Size(100, 100);

        private Establish context = () =>
            {
                UIElement.Protected().Setup<Size>(MeasureOverride, ItExpr.Is<Size>(size => size.Equals(availableSize))).
                    Returns(desiredSize);
                UIElement.Object.Measure(availableSize);
            };
    }

    public abstract class a_Measured_and_Arranged_UIElement : a_Measured_UIElement
    {
        protected static readonly Size finalSize = new Size(100, 100);

        private Establish context = () =>
            {
                UIElement.Protected().Setup<Size>(ArrangeOverride, ItExpr.IsAny<Size>()).Returns(finalSize);
                UIElement.Object.Arrange(new Rect(finalSize));
            };
    }

    public abstract class a_UIElement_in_a_RootElement : a_UIElement
    {
        private Establish context = () =>
            {
                var viewPort = new Rect(30, 40, 200, 200);

                var renderer = new Mock<Renderer>(
                    new Mock<ISpriteBatch>().Object, new Mock<IPrimitivesService>().Object) {
                                                                                               CallBase = true 
                                                                                            };
                var rootElement = new Mock<RootElement>(viewPort, renderer.Object)
                    {
                       CallBase = true 
                    };

                rootElement.Object.Content = UIElement.Object;
                rootElement.Object.Update();
            };
    }
}