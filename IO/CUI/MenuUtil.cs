using Spectre.Console;

namespace Starfall.IO.CUI
{
	public static class MenuUtil
	{
		public static int OpenMenu(int startIndex, bool clean, params string[] selection)
		{
			if (selection.Length <= 0) return 0;
			int select = startIndex;
			var Close = ConsoleUtil.StartCUIPart(false);

		Render:
			var (sx, sy) = Console.GetCursorPosition();

			for (var i = 0; i < selection.Length; i++)
			{
				var (_, vy) = Console.GetCursorPosition();
				Console.SetCursorPosition(sx, vy);
				// Console.ResetColor();
				if (i == select) AnsiConsole.MarkupLine("[black on white]" + selection[i] + "[/]");
				else AnsiConsole.MarkupLine(selection[i]);
			}
			Console.WriteLine();

			// 입력 처리
			var (ix, iy) = Console.GetCursorPosition();
		Input:
			var input = Console.ReadKey();
			// 가로 세로 방향키(↑↓←→)로 항목 선택 가능하게 구현
			switch (input.Key)
			{
				case ConsoleKey.UpArrow:
					select = select == 0 ? selection.Length - 1 : select - 1;
					Console.SetCursorPosition(sx, sy);
					goto Render;

				case ConsoleKey.DownArrow:
					select = select == selection.Length - 1 ? 0 : select + 1;
					Console.SetCursorPosition(sx, sy);
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

		// 메뉴 항목을 string[][] 형식의 2차원 배열로 받음
		public static int OpenMenu(int startIndex, bool clean, string[][] selection)
		{
			int rows = selection.Length;
			int[] colsPerRow = selection.Select(row => row.Length).ToArray();

			int total = 0;
			int row = 0, col = 0;

			for (int i = 0; i < rows; i++)
			{
				if (startIndex < total + selection[i].Length)
				{
					row = i;
					col = startIndex - total;
					break;
				}
				total += selection[i].Length;
			}

			System.Action Close = ConsoleUtil.StartCUIPart(false);

		Render:
			Console.Clear();
			(int sx, int sy) = Console.GetCursorPosition();
			// 메뉴 항목 전부 selection [i][j] 로 접근중 
			for (int i = 0; i < rows; i++)
			{
				for (int j = 0; j < selection[i].Length; j++)
				{
					if (i == row && j == col)
						AnsiConsole.Markup("[black on white]" + selection[i][j].PadRight(10) + "[/]");
					else
						AnsiConsole.Markup(selection[i][j].PadRight(10));
				}
				Console.WriteLine();
			}

			(int ix, int iy) = Console.GetCursorPosition();

		Input:
			ConsoleKeyInfo input = Console.ReadKey();
			switch (input.Key)
			{
				case ConsoleKey.UpArrow:
					row = (row - 1 + rows) % rows;
					col = Math.Min(col, colsPerRow[row] - 1);
					goto Render;

				case ConsoleKey.DownArrow:
					row = (row + 1) % rows;
					col = Math.Min(col, colsPerRow[row] - 1);
					goto Render;

				case ConsoleKey.LeftArrow:
					col = (col - 1 + colsPerRow[row]) % colsPerRow[row];
					goto Render;

				case ConsoleKey.RightArrow:
					col = (col + 1) % colsPerRow[row];
					goto Render;

				case ConsoleKey.Enter:
					break;

				case ConsoleKey.Escape:
					return -1;

				default:
					Console.SetCursorPosition(ix, iy);
					Console.Write(" ");
					Console.SetCursorPosition(ix, iy);
					goto Input;
			}

			if (clean)
			{
				Console.ResetColor();
				for (int i = 0; i < rows; i++)
				{
					Console.SetCursorPosition(sx, sy + i);
					for (int j = 0; j < selection[i].Length; j++)
					{
						Console.Write("          ");
					}
				}
				Console.SetCursorPosition(sx, sy);
			}

			Close();

			int index = 0;
			for (int i = 0; i < row; i++)
				index += selection[i].Length;

			return index + col;
		}
		public static string[][] To3x2Grid(string[] source) // 2차원 배열을 2행3열로 만들어주는코드 
		{
			int rowCount = 3;
			int colCount = 2;
			string[][] result = new string[rowCount][];

			for (int i = 0; i < rowCount; i++)
			{
				result[i] = new string[colCount];
				for (int j = 0; j < colCount; j++)
				{
					int index = i * colCount + j;
					result[i][j] = (index < source.Length) ? source[index] : "";
				}
			}

			return result;
		}


		public static int OpenMenu(params string[] selection) => OpenMenu(0, false, selection);
	}
}
