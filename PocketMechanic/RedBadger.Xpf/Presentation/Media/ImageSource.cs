namespace RedBadger.Xpf.Presentation.Media
{
    using System.Windows;

    public abstract class ImageSource : DependencyObject
    {
        public abstract double Height { get; }

        public abstract double Width { get; }

        internal virtual Size Size
        {
            get
            {
                return new Size(this.Width, this.Height);
            }
        }
    }
}