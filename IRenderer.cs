public interface IRenderer
{
  void Clear();
  void DrawPoint(Color color, int x, int y);
  void ShowMessage(string message);
}