protected sealed override Size MeasureOverride(Size constraint)
{
    this.VerifyReentrancy();
    this._textBlockCache = null;
    this.EnsureTextBlockCache();
    LineProperties lineProperties = this._textBlockCache._lineProperties;

    if (this.CheckFlags(Flags.PendingTextContainerEventInit))
    {
        Invariant.Assert(this._complexContent != null);
        this.InitializeTextContainerListeners();
        this.SetFlags(false, Flags.PendingTextContainerEventInit);
    }

    int lineCount = this.LineCount;

    if (((lineCount > 0) && base.IsMeasureValid) && (this.InlineObjects == null))
    {
        bool flag;
        if (lineProperties.TextTrimming == TextTrimming.None)
        {
            flag = DoubleUtil.AreClose(constraint.Width, this._referenceSize.Width) || (lineProperties.TextWrapping == TextWrapping.NoWrap);
        }
        else
        {
            flag = (DoubleUtil.AreClose(constraint.Width, this._referenceSize.Width) && (lineProperties.TextWrapping == TextWrapping.NoWrap)) && (DoubleUtil.AreClose(constraint.Height, this._referenceSize.Height) || (lineCount == 1));
        }
        if (flag)
        {
            this._referenceSize = constraint;
            return this._previousDesiredSize;
        }
    }

    this._referenceSize = constraint;
    this.CheckFlags(Flags.FormattedOnce);
    double num2 = this._baselineOffset;
    this.InlineObjects = null;
    int capacity = (this._subsequentLines == null) ? 1 : this._subsequentLines.Count;
    this.ClearLineMetrics();

    if (this._complexContent != null)
    {
        this._complexContent.TextView.Invalidate();
    }

    lineProperties.IgnoreTextAlignment = true;
    this.SetFlags(true, Flags.RequiresAlignment);
    this.SetFlags(true, Flags.FormattedOnce);
    this.SetFlags(false, Flags.HasParagraphEllipses);
    this.SetFlags(true, Flags.TreeInReadOnlyMode | Flags.MeasureInProgress);
    Size size = new Size();
    bool flag2 = true;

    try
    {
        Line line = this.CreateLine(lineProperties);
        bool endOfParagraph = false;
        int dcp = 0;
        TextLineBreak textLineBreak = null;
        Thickness padding = this.Padding;
        Size size2 = new Size(Math.Max((double) 0.0, (double) (constraint.Width - (padding.Left + padding.Right))), Math.Max((double) 0.0, (double) (constraint.Height - (padding.Top + padding.Bottom))));
        TextDpi.EnsureValidLineWidth(ref size2);
        while (!endOfParagraph)
        {
            using (line)
            {
                line.Format(dcp, size2.Width, this.GetLineProperties(dcp == 0, lineProperties), textLineBreak, this._textBlockCache._textRunCache, false);
                double height = this.CalcLineAdvance(line.Height, lineProperties);
                LineMetrics item = new LineMetrics(line.Length, line.Width, height, line.BaselineOffset, line.HasInlineObjects(), textLineBreak);
                if (!this.CheckFlags(Flags.HasFirstLine))
                {
                    this.SetFlags(true, Flags.HasFirstLine);
                    this._firstLine = item;
                }
                else
                {
                    if (this._subsequentLines == null)
                    {
                        this._subsequentLines = new List<LineMetrics>(capacity);
                    }
                    this._subsequentLines.Add(item);
                }
                size.Width = Math.Max(size.Width, line.GetCollapsedWidth());
                if (((lineProperties.TextTrimming == TextTrimming.None) || (size2.Height >= (size.Height + height))) || (dcp == 0))
                {
                    this._baselineOffset = size.Height + line.BaselineOffset;
                    size.Height += height;
                }
                else
                {
                    this.SetFlags(true, Flags.HasParagraphEllipses);
                }
                textLineBreak = line.GetTextLineBreak();
                endOfParagraph = line.EndOfParagraph;
                dcp += line.Length;
                continue;
            }
        }
        size.Width += padding.Left + padding.Right;
        size.Height += padding.Top + padding.Bottom;
        Invariant.Assert(textLineBreak == null);
        flag2 = false;
    }
    finally
    {
        lineProperties.IgnoreTextAlignment = false;
        this.SetFlags(false, Flags.TreeInReadOnlyMode | Flags.MeasureInProgress);
        if (flag2)
        {
            this._textBlockCache._textRunCache = null;
            this.ClearLineMetrics();
        }
    }
    if (!DoubleUtil.AreClose(num2, this._baselineOffset))
    {
        base.CoerceValue(BaselineOffsetProperty);
    }
    this._previousDesiredSize = size;
    return size;
}

protected sealed override Size ArrangeOverride(Size arrangeSize)
{
    this.VerifyReentrancy();
    if (this._complexContent != null)
    {
        this._complexContent.VisualChildren.Clear();
    }
    ArrayList inlineObjects = this.InlineObjects;
    int lineCount = this.LineCount;
    if ((inlineObjects != null) && (lineCount > 0))
    {
        bool flag = true;
        this.SetFlags(true, Flags.TreeInReadOnlyMode);
        this.SetFlags(true, Flags.ArrangeInProgress);
        try
        {
            this.EnsureTextBlockCache();
            LineProperties lineProperties = this._textBlockCache._lineProperties;
            double wrappingWidth = this.CalcWrappingWidth(arrangeSize.Width);
            Vector vector = this.CalcContentOffset(arrangeSize, wrappingWidth);
            Line line = this.CreateLine(lineProperties);
            int dcp = 0;
            Vector lineOffset = vector;
            for (int i = 0; i < lineCount; i++)
            {
                LineMetrics metrics = this.GetLine(i);
                if (metrics.HasInlineObjects)
                {
                    using (line)
                    {
                        bool showParagraphEllipsis = this.ParagraphEllipsisShownOnLine(i, lineOffset.Y - vector.Y);
                        line.Format(dcp, wrappingWidth, this.GetLineProperties(dcp == 0, lineProperties), metrics.TextLineBreak, this._textBlockCache._textRunCache, showParagraphEllipsis);
                        line.Arrange(this._complexContent.VisualChildren, lineOffset);
                    }
                }
                lineOffset.Y += metrics.Height;
                dcp += metrics.Length;
            }
            flag = false;
        }
        finally
        {
            this.SetFlags(false, Flags.TreeInReadOnlyMode);
            this.SetFlags(false, Flags.ArrangeInProgress);
            if (flag)
            {
                this._textBlockCache._textRunCache = null;
                this.ClearLineMetrics();
            }
        }
    }
    if (this._complexContent != null)
    {
        base.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new DispatcherOperationCallback(this.OnValidateTextView), EventArgs.Empty);
    }
    base.InvalidateVisual();
    return arrangeSize;
}

protected sealed override void OnRender(DrawingContext ctx)
{
    this.VerifyReentrancy();
    if (ctx == null)
    {
        throw new ArgumentNullException("ctx");
    }
    if (this.IsLayoutDataValid)
    {
        Brush background = this.Background;
        if (background != null)
        {
            ctx.DrawRectangle(background, null, new Rect(0.0, 0.0, base.RenderSize.Width, base.RenderSize.Height));
        }
        this.SetFlags(false, Flags.RequiresAlignment);
        this.SetFlags(true, Flags.TreeInReadOnlyMode);
        try
        {
            this.EnsureTextBlockCache();
            LineProperties lineProperties = this._textBlockCache._lineProperties;
            double wrappingWidth = this.CalcWrappingWidth(base.RenderSize.Width);
            Vector vector = this.CalcContentOffset(base.RenderSize, wrappingWidth);
            Point lineOffset = new Point(vector.X, vector.Y);
            Line line = this.CreateLine(lineProperties);
            int dcp = 0;
            bool showParagraphEllipsis = false;
            this.SetFlags(this.CheckFlags(Flags.HasParagraphEllipses), Flags.RequiresAlignment);
            int lineCount = this.LineCount;
            for (int i = 0; i < lineCount; i++)
            {
                LineMetrics metrics = this.GetLine(i);
                double num5 = Math.Max((double) 0.0, (double) (base.RenderSize.Height - this.Padding.Bottom));
                if (this.CheckFlags(Flags.HasParagraphEllipses) && ((i + 1) < lineCount))
                {
                    double num6 = (this.GetLine(i + 1).Height + metrics.Height) + lineOffset.Y;
                    showParagraphEllipsis = DoubleUtil.GreaterThan(num6, num5) && !DoubleUtil.AreClose(num6, num5);
                }
                if ((!this.CheckFlags(Flags.HasParagraphEllipses) || DoubleUtil.LessThanOrClose(metrics.Height + lineOffset.Y, num5)) || (i == 0))
                {
                    using (line)
                    {
                        line.Format(dcp, wrappingWidth, this.GetLineProperties(dcp == 0, showParagraphEllipsis, lineProperties), metrics.TextLineBreak, this._textBlockCache._textRunCache, showParagraphEllipsis);
                        if (!this.CheckFlags(Flags.HasParagraphEllipses))
                        {
                            metrics = this.UpdateLine(i, metrics, line.Start, line.Width);
                        }
                        line.Render(ctx, lineOffset);
                        lineOffset.Y += metrics.Height;
                        dcp += metrics.Length;
                    }
                }
            }
        }
        finally
        {
            this.SetFlags(false, Flags.TreeInReadOnlyMode);
            this._textBlockCache = null;
        }
    }
}

private int get_LineCount()
{
    if (!this.CheckFlags(Flags.HasFirstLine))
    {
        return 0;
    }
    if (this._subsequentLines != null)
    {
        return (this._subsequentLines.Count + 1);
    }
    return 1;
}

private double CalcWrappingWidth(double width)
{
    if (width < this._previousDesiredSize.Width)
    {
        width = this._previousDesiredSize.Width;
    }
    if (width > this._referenceSize.Width)
    {
        width = this._referenceSize.Width;
    }
    width = Math.Max((double) 0.0, (double) (width - (this.Padding.Left + this.Padding.Right)));
    TextDpi.EnsureValidLineWidth(ref width);
    return width;
}

private Vector CalcContentOffset(Size computedSize, double wrappingWidth)
{
    Vector vector = new Vector();
    Thickness padding = this.Padding;
    Size size = new Size(Math.Max((double) 0.0, (double) (computedSize.Width - (padding.Left + padding.Right))), Math.Max((double) 0.0, (double) (computedSize.Height - (padding.Top + padding.Bottom))));
    switch (this.TextAlignment)
    {
        case TextAlignment.Right:
            vector.X = size.Width - wrappingWidth;
            break;

        case TextAlignment.Center:
            vector.X = (size.Width - wrappingWidth) / 2.0;
            break;
    }
    vector.X += padding.Left;
    vector.Y += padding.Top;
    return vector;
}

private Line CreateLine(LineProperties lineProperties)
{
    if (this._complexContent == null)
    {
        return new SimpleLine(this, this.Text, lineProperties.DefaultTextRunProperties);
    }
    return new ComplexLine(this);
}

private LineMetrics GetLine(int index)
{
    if (index != 0)
    {
        return this._subsequentLines[index - 1];
    }
    return this._firstLine;
}