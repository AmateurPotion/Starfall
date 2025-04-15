using Starfall.IO;
using Starfall.Worlds;

namespace Starfall
{
  public static class GameManager
  {
    public static bool loaded { get; private set; } = false;
    public static void Init()
    {
      if (loaded) return;
      StorageController.Init();

      loaded = true;
    }

    public static Game StartGame()
    {
      var game = new Game();

      return game;
    }
  }
}