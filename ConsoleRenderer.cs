using System;

public class ConsoleRenderer : IRenderer
{
  public ConsoleRenderer()
  {
    Console.CursorVisible = false;
  }

  public void Clear() => Console.Clear();

  public void DrawPoint(Color color, int x, int y)
  {
    Console.SetCursorPosition(x * 2, y);
    Console.ForegroundColor = (ConsoleColor)color;
    Console.Write("██");
  }

  public void ShowMessage(string message)
  {
    Console.ResetColor();
    Console.WriteLine(message);
    Console.ReadLine();
  }
}