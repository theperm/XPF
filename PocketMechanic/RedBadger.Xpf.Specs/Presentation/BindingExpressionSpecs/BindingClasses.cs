//-------------------------------------------------------------------------------------------------
// <auto-generated> 
// Marked as auto-generated so StyleCop will ignore BDD style tests
// </auto-generated>
//-------------------------------------------------------------------------------------------------

#pragma warning disable 169
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local

namespace RedBadger.Xpf.Specs.Presentation.BindingExpressionSpecs
{
    using System.ComponentModel;

    using RedBadger.Xpf.Presentation.Media;

    public class BorderBrushBindingObject : INotifyPropertyChanged
    {
        private SolidColorBrush brush;

        public event PropertyChangedEventHandler PropertyChanged;

        public SolidColorBrush Brush
        {
            get
            {
                return this.brush;
            }

            set
            {
                this.brush = value;
                this.InvokePropertyChanged(new PropertyChangedEventArgs("Brush"));
            }
        }

        public void InvokePropertyChanged(PropertyChangedEventArgs e)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }

    public class MyBindingObject : INotifyPropertyChanged
    {
        private double myWidth;

        public event PropertyChangedEventHandler PropertyChanged;

        public double MyWidth
        {
            get
            {
                return this.myWidth;
            }

            set
            {
                this.myWidth = value;
                this.InvokePropertyChanged(new PropertyChangedEventArgs("MyWidth"));
            }
        }

        public void InvokePropertyChanged(PropertyChangedEventArgs e)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}