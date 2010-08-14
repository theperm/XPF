﻿namespace RedBadger.Xpf.Graphics
{
    using System.Windows;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class SpriteFontAdapter : ISpriteFont
    {
        private readonly SpriteFont spriteFont;

        public SpriteFontAdapter(SpriteFont spriteFont)
        {
            this.spriteFont = spriteFont;
        }

        public SpriteFont Value
        {
            get
            {
                return this.spriteFont;
            }
        }

        public Size MeasureString(string text)
        {
            Vector2 size = this.spriteFont.MeasureString(text ?? string.Empty);
            return new Size(size.X, size.Y);
        }
    }
}
