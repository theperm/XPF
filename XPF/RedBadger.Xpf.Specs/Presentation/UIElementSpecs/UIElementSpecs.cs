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
    using System.Collections.Generic;
    using System.Linq;

    using Machine.Specifications;

    using RedBadger.Xpf.Presentation;

    [Subject(typeof(UIElement), "Children")]
    public class when_children_are_requested : a_UIElement
    {
        private static IEnumerable<IElement> enumerable;

        private Because of = () => enumerable = Subject.Object.GetVisualChildren();

        private It should_not_return_any_children = () => enumerable.Count().ShouldEqual(0);
    }

    [Subject(typeof(UIElement), "Hit Testing")]
    public class when_a_point_is_inside_an_element : a_UIElement_in_a_RootElement
    {
        private static bool hitTestResult;

        private Establish context = () => RootElement.Object.Update();

        private Because of = () => hitTestResult = Subject.Object.HitTest(new Point(40, 50));

        private It should_return_a_positive_hit_test = () => hitTestResult.ShouldBeTrue();
    }

    [Subject(typeof(UIElement), "Hit Testing")]
    public class when_a_point_is_outside_an_element : a_UIElement_in_a_RootElement
    {
        private static bool hitTestResult;

        private Establish context = () => RootElement.Object.Update();

        private Because of = () => hitTestResult = Subject.Object.HitTest(new Point(20, 30));

        private It should_return_a_negative_hit_test = () => hitTestResult.ShouldBeFalse();
    }
}