//-------------------------------------------------------------------------------------------------
// <auto-generated> 
// Marked as auto-generated so StyleCop will ignore BDD style tests
// </auto-generated>
//-------------------------------------------------------------------------------------------------

#pragma warning disable 169
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local

namespace RedBadger.Xpf.Specs.Input.MouseInputSpecs.LeftButtonDownSpecs
{
    using System.Collections.Generic;
    using System.Windows;

    using Machine.Specifications;

    using Moq;
    using Moq.Protected;

    using RedBadger.Xpf.Graphics;
    using RedBadger.Xpf.Presentation.Controls;
    using RedBadger.Xpf.Presentation.Input;
    using RedBadger.Xpf.Presentation.Media;

    using It = Machine.Specifications.It;

    public abstract class a_RootElement_with_input_manager
    {
        protected const string OnNextMouseData = "OnNextMouseData";

        protected static Mock<IInputManager> InputManager;

        protected static Subject<MouseData> MouseData;

        protected static Mock<Renderer> Renderer;

        protected static RootElement RootElement;

        protected static Rect ViewPort = new Rect(10, 20, 100, 100);

        private Establish context = () =>
            {
                Renderer = new Mock<Renderer>(new Mock<ISpriteBatch>().Object, new Mock<IPrimitivesService>().Object)
                    {
                       CallBase = true 
                    };

                MouseData = new Subject<MouseData>();
                InputManager = new Mock<IInputManager>();
                InputManager.SetupGet(inputManager => inputManager.MouseData).Returns(MouseData);

                RootElement = new RootElement(ViewPort, Renderer.Object, InputManager.Object);
            };
    }

    [Subject("Mouse Input - Left Button Down")]
    public class when_a_single_control_is_placed_inside_a_root_element : a_RootElement_with_input_manager
    {
        private static Mock<ButtonBase> button;

        private Establish context = () =>
            {
                button = new Mock<ButtonBase> { CallBase = true };
                RootElement.Content = button.Object;
            };

        private Because of = () =>
            {
                RootElement.Update();
                MouseData.OnNext(new MouseData(MouseAction.LeftButtonDown, new Point(40, 50)));
            };

        private It should_notify_the_button_that_the_left_mouse_was_pressed =
            () =>
            button.Protected().Verify(
                OnNextMouseData, Times.Once(), ItExpr.Is<MouseData>(data => data.Action == MouseAction.LeftButtonDown));
    }

    [Subject("Mouse Input - Left Button Down")]
    public class when_a_stack_of_elements_on_top_of_each_other_are_placed_inside_a_root_element :
        a_RootElement_with_input_manager
    {
        private static Mock<ButtonBase> button1;

        private static Mock<ButtonBase> button2;

        private static Mock<Grid> grid;

        private Establish context = () =>
            {
                grid = new Mock<Grid> { CallBase = true };

                button1 = new Mock<ButtonBase> { CallBase = true };
                button1.Object.Width = button1.Object.Height = 100;
                grid.Object.Children.Add(button1.Object);

                button2 = new Mock<ButtonBase> { CallBase = true };
                button2.Object.Width = button2.Object.Height = 100;
                grid.Object.Children.Add(button2.Object);

                RootElement.Content = grid.Object;
            };

        private Because of = () =>
            {
                RootElement.Update();
                MouseData.OnNext(new MouseData(MouseAction.LeftButtonDown, new Point(40, 50)));
            };

        private It should_not_raise_left_mouse_button_down_event_on_the_bottom_most_element =
            () => button1.Protected().Verify(OnNextMouseData, Times.Never(), ItExpr.IsAny<MouseData>());

        private It should_raise_left_mouse_button_down_event_on_the_top_most_element =
            () =>
            button2.Protected().Verify(
                OnNextMouseData, Times.Once(), ItExpr.Is<MouseData>(data => data.Action == MouseAction.LeftButtonDown));
    }

    [Subject("Mouse Input - Left Button Down")]
    public class when_a_stack_of_elements_inside_each_other_are_placed_inside_a_root_element :
        a_RootElement_with_input_manager
    {
        private static Mock<ButtonBase> button1;

        private static Mock<ButtonBase> button2;

        private Establish context = () =>
            {
                button1 = new Mock<ButtonBase> { CallBase = true };
                button1.Object.Width = button1.Object.Height = 100;

                button2 = new Mock<ButtonBase> { CallBase = true };
                button2.Object.Width = button2.Object.Height = 100;

                button1.Object.Content = button2.Object;
                RootElement.Content = button1.Object;
            };

        private Because of = () =>
            {
                RootElement.Update();
                MouseData.OnNext(new MouseData(MouseAction.LeftButtonDown, new Point(40, 50)));
            };

        private It should_not_raise_left_mouse_button_down_event_on_the_bottom_most_element =
            () =>
            button1.Protected().Verify(
                OnNextMouseData, Times.Never(), ItExpr.Is<MouseData>(data => data.Action == MouseAction.LeftButtonDown));

        private It should_raise_left_mouse_button_down_event_on_the_top_most_element =
            () =>
            button2.Protected().Verify(
                OnNextMouseData, Times.Once(), ItExpr.Is<MouseData>(data => data.Action == MouseAction.LeftButtonDown));
    }

    [Subject("Mouse Input - Left Button Down")]
    public class when_a_stack_of_elements_after_each_other_are_placed_inside_a_root_element :
        a_RootElement_with_input_manager
    {
        private static Mock<ButtonBase> button1;

        private static Mock<ButtonBase> button2;

        private static Mock<StackPanel> stackPanel;

        private Establish context = () =>
            {
                stackPanel = new Mock<StackPanel> { CallBase = true };

                button1 = new Mock<ButtonBase> { CallBase = true };
                button1.Object.Width = 100;
                button1.Object.Height = 50;
                stackPanel.Object.Children.Add(button1.Object);

                button2 = new Mock<ButtonBase> { CallBase = true };
                button2.Object.Width = 100;
                button2.Object.Height = 50;
                stackPanel.Object.Children.Add(button2.Object);

                RootElement.Content = stackPanel.Object;
            };

        private Because of = () =>
            {
                RootElement.Update();
                MouseData.OnNext(new MouseData(MouseAction.LeftButtonDown, new Point(40, 50)));
            };

        private It should_not_raise_left_mouse_button_down_event_on_the_bottom_most_element =
            () =>
            button2.Protected().Verify(
                OnNextMouseData, Times.Never(), ItExpr.Is<MouseData>(data => data.Action == MouseAction.LeftButtonDown));

        private It should_raise_left_mouse_button_down_event_on_the_top_most_element =
            () =>
            button1.Protected().Verify(
                OnNextMouseData, Times.Once(), ItExpr.Is<MouseData>(data => data.Action == MouseAction.LeftButtonDown));
    }
}