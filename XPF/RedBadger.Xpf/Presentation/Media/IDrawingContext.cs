namespace RedBadger.Xpf.Presentation.Media
{
    using RedBadger.Xpf.Graphics;

    public interface IDrawingContext
    {
        Rect ClippingRect { get; set; }

        void DrawImage(ImageSource imageSource, Rect rect);

        void DrawRectangle(Rect rect, Brush brush);

        void DrawText(ISpriteFont spriteFont, string text, Vector position, Brush brush);
    }
}