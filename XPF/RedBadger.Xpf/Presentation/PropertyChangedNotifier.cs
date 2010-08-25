namespace RedBadger.Xpf.Presentation
{
    using System;
    using System.Windows;
    using System.Windows.Data;

    internal sealed class PropertyChangedNotifier : DependencyObject, IDisposable
    {
        public static readonly XpfDependencyProperty ValueProperty = XpfDependencyProperty.Register(
            "Value", typeof(object), typeof(PropertyChangedNotifier), new PropertyMetadata(null, OnPropertyChanged));

        private readonly BindingExpression bindingExpression;

        private readonly WeakReference propertySource;

        private bool isDisposed;

        public PropertyChangedNotifier(
            DependencyObject propertySource, XpfDependencyProperty property)
        {
            if (null == propertySource)
            {
                throw new ArgumentNullException("propertySource");
            }

            if (null == property)
            {
                throw new ArgumentNullException("property");
            }

            this.propertySource = new WeakReference(propertySource);

            this.bindingExpression = new BindingExpression(this, ValueProperty);
            var binding = new Binding(property.Name) { Mode = BindingMode.OneWay, Source = propertySource };
            this.bindingExpression.SetBinding(binding);

            // BindingOperations.SetBinding(this, ValueProperty, binding);
        }

        ~PropertyChangedNotifier()
        {
            this.Dispose(false);
        }

        public event EventHandler<PropertyChangedEventArgs> ValueChanged;

        public DependencyObject PropertySource
        {
            get
            {
                try
                {
                    return this.propertySource.IsAlive ? this.propertySource.Target as DependencyObject : null;
                }
                catch
                {
                    return null;
                }
            }
        }

        public object Value
        {
            get
            {
                return this.GetValue(ValueProperty.Value);
            }

            set
            {
                this.SetValue(ValueProperty.Value, value);
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var notifier = (PropertyChangedNotifier)d;
            if (null != notifier.ValueChanged)
            {
                notifier.ValueChanged(notifier, new PropertyChangedEventArgs(e.OldValue, e.NewValue));
            }
        }

        private void Dispose(bool disposing)
        {
            if (!this.isDisposed)
            {
                if (disposing)
                {
                    this.ClearValue(ValueProperty.Value);
                }

                this.isDisposed = true;
            }
        }
    }
}