﻿//-------------------------------------------------------------------------------------------------
// <auto-generated> 
// Marked as auto-generated so StyleCop will ignore BDD style tests
// </auto-generated>
//-------------------------------------------------------------------------------------------------
#pragma warning disable 169
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local

namespace RedBadger.Xpf.Specs.Presentation.RectSpecs
{
    using System.Windows;

    using Machine.Specifications;

    using Rect = RedBadger.Xpf.Presentation.Rect;
    using Vector = RedBadger.Xpf.Presentation.Vector;

    public abstract class a_Rect
    {
        protected const double X = 5;

        protected const double Y = 15;

        protected const double Height = 10;

        protected const double Width = 20;

        protected static Rect rect;

        private Establish context = () => rect = new Rect(X, Y, Width, Height);
    }

    public abstract class an_Empty_Rect
    {
        protected static Rect rect;

        private Establish context = () => rect = new Rect(0, 0, 0, 0);
    }

    [Subject(typeof(Rect))]
    public class when_initialised : a_Rect
    {
        private It should_have_the_expected_Position = () => rect.Position.ShouldEqual(new Vector(X, Y));

        private It should_have_the_expected_Size = () => rect.Size.ShouldEqual(new Size(Width, Height));
    }

    [Subject(typeof(Rect))]
    public class when_an_empty_rect_is_initialised : an_Empty_Rect
    {
        private It should_have_a_position_at_zero_zero = () => rect.Position.ShouldEqual(new Vector(0, 0));

        private It should_have_a_size_of_zero_zero = () => rect.Size.ShouldEqual(new Size(0, 0));

        private It should_be_equal_to_an_empty_rect = () => rect.ShouldEqual(Rect.Empty);

        private It should_say_it_is_empty = () => rect.IsEmpty.ShouldBeTrue();
    }
}