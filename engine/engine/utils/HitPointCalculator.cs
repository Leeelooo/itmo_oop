using engine.vo.battle;
using System;

namespace engine.utils
{
    public static class HitPointCalculator
    {
        public static int CalculateAttack(BattleUnitStack from, int attack, int enemyDefence)
        {
            var k = attack - enemyDefence;
            var random = new Random();
            var result = (int)
                Math.Round(from.Number
                           * random.Next(from.Unit.Damage.Item1, from.Unit.Damage.Item2)
                           * (1 + k * 0.05)
                );
            return result < 1 ? 0 : result;
        }
    }
}