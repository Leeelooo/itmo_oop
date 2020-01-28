using engine.vo;
using engine.vo.battle;
using System.Collections.Generic;

namespace engine.core.action
{
    public interface IAction
    {
        Player Player();
    }
    public interface IGlobalAction : IAction { }

    public interface IBattleAction : IAction
    {
        BattleUnitStack Stack();
    }

    public interface ITargetBattleAction : IBattleAction
    {
        IEnumerable<BattleUnitStack> AllyArmy();
        IEnumerable<BattleUnitStack> EnemyArmy();
        IEnumerable<BattleUnitStack> Filter { get; }

        IEnumerable<BattleUnitStack> Targets();
        bool IsAppliable();
        void OnTargetChoosed(IEnumerable<BattleUnitStack> targets);
    }

    public interface INonTargetBattleAction : IBattleAction { }

}