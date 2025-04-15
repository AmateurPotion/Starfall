namespace Starfall.IO.CUI
{
  public static class MenuUtil
  {
    public static int OpenMenu(bool clean, params string[] selection)
    {
      if (selection.Length <= 0) return 0;
      int select = 0;
      var Close = InputManager.StartCUIPart(false);

    Render:
      var (sx, sy) = Console.GetCursorPosition();

      for (var i = 0; i < selection.Length; i++)
      {
        var (vx, vy) = Console.GetCursorPosition();
        Console.SetCursorPosition(sx, vy);
        Console.ResetColor();
        if (i == select)
        {
          Console.BackgroundColor = ConsoleColor.White;
          Console.ForegroundColor = ConsoleColor.Black;
        }

        Console.WriteLine(selection[i]);
      }
      Console.WriteLine();

      // 입력 처리
      var (ix, iy) = Console.GetCursorPosition();
    Input:
      var input = Console.ReadKey();
      switch (input.Key)
      {
        case ConsoleKey.UpArrow:
          select = select == 0 ? selection.Length - 1 : select - 1;
          goto Render;

        case ConsoleKey.DownArrow:
          select = select == selection.Length - 1 ? 0 : select + 1;
          goto Render;

        case ConsoleKey.Enter: break;

        case ConsoleKey.Escape: select = -1; break;

        default:
          Console.SetCursorPosition(ix, iy);
          Console.Write(" ");
          Console.SetCursorPosition(ix, iy);
          goto Input;
      }

      // clean 이 true일시 띄웠던 메뉴 화면 지우고 원래 커서 위치로 이동
      if (clean)
      {
        Console.ResetColor();
        for (int i = 0; i < selection.Length; i++)
        {
          Console.SetCursorPosition(sx, sy + i);
          for (int j = 0; j < selection[i].Length; j++)
          {
            Console.Write('ㅤ');
          }
        }
        Console.SetCursorPosition(sx, sy);
      }
      Close();
      return select;
    }

    public static int OpenMenu(params string[] selection) => OpenMenu(false, selection);
  }
}