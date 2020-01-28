using engine.vo;
using engine.vo.battle;

namespace engine.core.battle
{
    public interface IBattle
    {
        Player GetFirstPlayer();
        Player GetSecondPlayer();
        BattleArmy GetFirstArmy();
        BattleArmy GetSecondArmy();
        IBattleState GetBattleState();
    }
}