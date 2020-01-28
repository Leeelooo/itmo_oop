using engine.vo.battle;
using System.Collections.Immutable;

namespace engine.core
{
    public interface IBattleQueue
    {
        BattleUnitStack Next();
        void Await(BattleUnitStack stack);

        int GetCount();
        ImmutableList<BattleUnitStack> ToImmutableList();
    }
}