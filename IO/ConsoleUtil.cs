using System.Reflection;

namespace Starfall.IO
{
  public static class ConsoleUtil
  {
    public static Action StartCUIPart(bool cursorVisible = true)
    {
      ConsoleColor color = Console.ForegroundColor, background = Console.BackgroundColor;
      var visible = Console.CursorVisible;
      Console.CursorVisible = cursorVisible;

      return () =>
      {
        Console.BackgroundColor = background;
        Console.ForegroundColor = color;
        Console.CursorVisible = visible;
      };
    }

    public static void PrintTextFile(string path, ConsoleColor color, ConsoleColor background)
    {
      if (StorageController.TryGetResource(path, out var stream))
      {
        var Close = StartCUIPart();
        Console.ForegroundColor = color;
        Console.BackgroundColor = background;

        using var reader = new StreamReader(stream);
        Console.WriteLine(reader.ReadToEnd());
        reader.Close();
        stream.Close();
        Close();
      }
    }

    public static void PrintTextFile(string path) => PrintTextFile(path, Console.ForegroundColor, Console.BackgroundColor);
  }
}