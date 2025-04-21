namespace Starfall.Core
{
  public struct Dungeon
    (string label = "", float requireDef = 0, int failureProbability = 40, int reward = 1000)
  {
    public string Label { get; set; } = label;
    public float RequireDef { get; set; } = requireDef;
    public int FailureProbability { get; set; } = failureProbability;
    public int Reward { get; set; } = reward;

    public readonly void Deconstruct
      (out string label, out float requireDef, out int failureProbability, out int reward)
    {
      label = this.Label;
      requireDef = this.RequireDef;
      failureProbability = this.FailureProbability;
      reward = this.Reward;
    }
  }
}