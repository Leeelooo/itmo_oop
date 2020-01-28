using engine.vo.battle;
using System.Collections.Generic;

namespace engine.vo
{
    public enum TargetArmy
    {
        ENEMY,
        ALLY,
        BOTH
    }

    public interface ISpell
    {
        TargetArmy ApplyFor();
        BattleUnitStack CastedBy();
        IEnumerable<BattleUnitStack> Targets();
        IEnumerable<BattleUnitStack> Filter(IEnumerable<BattleUnitStack> stacks);
        void Invoke(IEnumerable<BattleUnitStack> targets);
    }
}