//-------------------------------------------------------------------------------------------------
// <auto-generated> 
// Marked as auto-generated so StyleCop will ignore BDD style tests
// </auto-generated>
//-------------------------------------------------------------------------------------------------

#pragma warning disable 169
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local

namespace RedBadger.Xpf.Specs.Presentation.Controls.ButtonSpecs
{
    using Machine.Specifications;

    using Microsoft.Xna.Framework;

    using Moq;

    using RedBadger.Xpf.Presentation;
    using RedBadger.Xpf.Presentation.Controls;
    using RedBadger.Xpf.Presentation.Media;

    using It = Machine.Specifications.It;

    public abstract class a_Button
    {
        protected static Button Button;

        protected static Mock<RootElement> RootElement;

        private Establish context = () =>
            {
                var renderer = new Mock<IRenderer>();
                DrawingContext = new Mock<IDrawingContext>();
                renderer.Setup(r => r.GetDrawingContext(Moq.It.IsAny<IElement>())).Returns(DrawingContext.Object);
                RootElement = new Mock<RootElement>(renderer.Object, new Rect(new Size(200, 200))) { CallBase = true };

                Button = new Button();
                RootElement.Object.Content = Button;
            };

        protected static Mock<IDrawingContext> DrawingContext;
    }

    [Subject(typeof(Button), "Padding")]
    public class when_padding_is_specified : a_Button
    {
        private const float ChildHeight = 20;

        private const float ChildWidth = 10;

        private static Mock<UIElement> child;

        private Establish context = () =>
            {
                child = new Mock<UIElement> { CallBase = true };
                child.Object.Width = ChildWidth;
                child.Object.Height = ChildHeight;
                child.Object.HorizontalAlignment = HorizontalAlignment.Left;
                child.Object.VerticalAlignment = VerticalAlignment.Top;

                Button.HorizontalAlignment = HorizontalAlignment.Left;
                Button.VerticalAlignment= VerticalAlignment.Top;
                Button.Content = child.Object;
            };

        private Because of = () =>
            {
                padding = new Thickness(30, 40, 50, 60);
                Button.Padding = padding;
                RootElement.Object.Update();
            };

        private It should_increase_the_desired_size =
            () =>
            Button.DesiredSize.ShouldEqual(
                new Size(ChildWidth + padding.Left + padding.Right, ChildHeight + padding.Top + padding.Bottom));

        private It should_have_a_child_with_the_correct_visual_offset =
            () => child.Object.VisualOffset.ShouldEqual(new Vector2(padding.Left, padding.Top));

        private static Thickness padding;
    }
}