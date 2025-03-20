using Assets.Scripts.Map.Counties;
using Assets.Scripts.Map.Players;

namespace Assets.Scripts.Map.AI.Contexts
{
    public record WarTargetInfo(Player AttackTarget, County County);
}
