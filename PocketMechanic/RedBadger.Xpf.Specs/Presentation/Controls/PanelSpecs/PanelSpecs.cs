//-------------------------------------------------------------------------------------------------
// <auto-generated> 
// Marked as auto-generated so StyleCop will ignore BDD style tests
// </auto-generated>
//-------------------------------------------------------------------------------------------------

#pragma warning disable 169
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local

namespace RedBadger.Xpf.Specs.Presentation.Controls.PanelSpecs
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Media;

    using Machine.Specifications;

    using Moq;

    using RedBadger.Xpf.Presentation;
    using RedBadger.Xpf.Presentation.Controls;
    using RedBadger.Xpf.Presentation.Media;

    using Brush = RedBadger.Xpf.Presentation.Media.Brush;
    using It = Machine.Specifications.It;
    using SolidColorBrush = RedBadger.Xpf.Presentation.Media.SolidColorBrush;
    using UIElement = RedBadger.Xpf.Presentation.UIElement;

    public abstract class a_Panel
    {
        protected static Mock<IDrawingContext> DrawingContext;

        protected static Mock<Panel> Panel;

        protected static RootElement RootElement;

        private Establish context = () =>
            {
                var renderer = new Mock<IRenderer>();
                DrawingContext = new Mock<IDrawingContext>();
                renderer.Setup(r => r.GetDrawingContext(Moq.It.IsAny<IElement>())).Returns(DrawingContext.Object);

                RootElement = new RootElement(new Rect(new Size(100, 100)), renderer.Object);
                Panel = new Mock<Panel> { CallBase = true };
                RootElement.Content = Panel.Object;
            };
    }

    [Subject(typeof(Panel), "Children")]
    public class when_a_panel_has_children : a_Panel
    {
        private static readonly Mock<UIElement> child1 = new Mock<UIElement>();

        private static readonly Mock<UIElement> child2 = new Mock<UIElement>();

        private static IEnumerable<IElement> children;

        private Establish context = () =>
            {
                Panel.Object.Children.Add(child1.Object);
                Panel.Object.Children.Add(child2.Object);
            };

        private Because of = () => children = Panel.Object.GetChildren();

        private It should_contain_the_correct_number_of_children = () => children.Count().ShouldEqual(2);

        private It should_return_the_first_child_first = () => children.First().ShouldBeTheSameAs(child1.Object);

        private It should_return_the_last_child_last = () => children.Last().ShouldBeTheSameAs(child2.Object);
    }

    [Subject(typeof(Panel), "Background")]
    public class when_panel_background_is_not_specified : a_Panel
    {
        private Because of = () =>
            {
                RootElement.Update();
                RootElement.Draw();
            };

        private It should_not_render_a_background =
            () =>
            DrawingContext.Verify(
                drawingContext => drawingContext.DrawRectangle(Moq.It.IsAny<Rect>(), Moq.It.IsAny<Brush>()), 
                Times.Never());
    }

    [Subject(typeof(Panel), "Background")]
    public class when_panel_background_is_specified : a_Panel
    {
        private static SolidColorBrush expectedBackground;

        private static Thickness margin;

        private Establish context = () =>
        {
            expectedBackground = new SolidColorBrush(Colors.Blue);

            margin = new Thickness(1, 2, 3, 4);
            Panel.Object.Margin = margin;
        };

        private Because of = () =>
            {
                Panel.Object.Background = expectedBackground;
                RootElement.Update();
            };

        private It should_render_the_background_in_the_right_place = () =>
            {
                var area = new Rect(
                    margin.Left,
                    margin.Top,
                    Panel.Object.ActualWidth - (margin.Left + margin.Right),
                    Panel.Object.ActualHeight - (margin.Top + margin.Bottom));

                DrawingContext.Verify(
                    drawingContext =>
                    drawingContext.DrawRectangle(Moq.It.Is<Rect>(rect => rect.Equals(area)), Moq.It.IsAny<Brush>()));
            };

        private It should_render_with_the_specified_background_color =
            () =>
            DrawingContext.Verify(
                drawingContext => drawingContext.DrawRectangle(Moq.It.IsAny<Rect>(), expectedBackground));
    }
}