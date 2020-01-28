using engine.vo.battle;
using System;

namespace engine.vo
{
    public class Effect
    {
        public bool IsPositive { get; }
        public int Duration { get; set; }
        public Action<BattleUnitStack> Apply { get; }

        public Effect(
            bool isPositive,
            int duration,
            Action<BattleUnitStack> apply
        )
        {
            IsPositive = isPositive;
            Duration = duration;
            Apply = apply;
        }
    }
}
