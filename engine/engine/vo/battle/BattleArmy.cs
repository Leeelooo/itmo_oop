using engine.utils;
using engine.vo.travel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace engine.vo.battle
{
    public class BattleArmy
    {
        public List<BattleUnitStack> Stacks;
        public int Count => Stacks.Count;

        public BattleArmy(params BattleUnitStack[] stacks)
        {
            if (stacks.Length > Config.MaxBattleArmySize)
                throw new ArgumentException($"No more than {Config.MaxBattleArmySize} stacks");
            Stacks = new List<BattleUnitStack>(stacks);
        }

        public BattleArmy(IEnumerable<BattleUnitStack> stacks)
        {
            var list = new List<BattleUnitStack>(stacks);
            if (list.Count > Config.MaxBattleArmySize)
                throw new ArgumentException($"No more than {Config.MaxBattleArmySize} stacks");
            Stacks = list;
        }

        public BattleArmy(Army army)
        {
            Stacks = new List<BattleUnitStack>(
                army.Stacks.Select(it => new BattleUnitStack(it.Unit, it.Number))
            );
        }

        public BattleArmy() : this(Array.Empty<BattleUnitStack>()) { }

        public void AddStack(BattleUnitStack stack)
        {
            if (Stacks.Count + 1 > Config.MaxBattleArmySize)
                throw new InvalidOperationException($"No more than {Config.MaxBattleArmySize} stacks");
            Stacks.Add(stack);
        }

        public override int GetHashCode()
        {
            return 5 * Stacks.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is BattleArmy))
                return false;
            var army = (BattleArmy)obj;
            return GetHashCode() == army.GetHashCode() && Stacks.SequenceEqual(army.Stacks);
        }

    }
}