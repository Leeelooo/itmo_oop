using engine.core.action;
using engine.vo;
using engine.vo.battle;
using System.Collections.Immutable;

namespace engine.core
{
    internal interface IBattleHelper
    {
        IBattleAction OnNextAction();
        void OnNextTurn();
        void OnAwait(BattleUnitStack stack);
        Player CheckLoser();
        ImmutableList<BattleUnitStack> GetCurrentAttackQueue();

        Player GetFirstPlayer();
        Player GetSecondPlayer();
        Player GetCurrentPlayer();
        BattleArmy GetFirstArmy();
        BattleArmy GetSecondArmy();
    }
}