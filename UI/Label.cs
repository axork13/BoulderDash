using System.Numerics;
using Raylib_cs;

public class Label
{
    public string text;

    public Rectangle bounds;
    public int fontSize;
    
    public Label(string text, Rectangle bounds, int fontSize=20)
    {
        this.text = text;
        this.bounds = bounds;
        this.fontSize = fontSize;
    }

    public void Draw()
    {
        Vector2 textSize = Raylib.MeasureTextEx(Raylib.GetFontDefault(), text, fontSize, 1);
        Vector2 position = new Vector2();
        position.X = bounds.X + (bounds.Width - textSize.X) / 2;
        position.Y = bounds.Y + (bounds.Height - textSize.Y) / 2;
        Raylib.DrawText(text, (int)position.X, (int)position.Y, fontSize, Color.White);
    }
}