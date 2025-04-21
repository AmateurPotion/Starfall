using Spectre.Console;
using Starfall.Core;
using Starfall.IO;
using Starfall.IO.CUI;
using Starfall.IO.Dataset;

namespace Starfall;
public class Program
{
    public static void Main(params string[] args)
    {
        GameManager.Init();

        GameManager.EnterMain();
    }
}