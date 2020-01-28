using engine.utils;
using engine.vo.battle;
using System;
using System.Collections.Generic;
using System.Linq;

namespace engine.vo.travel
{
    public class Army
    {
        private readonly List<UnitStack> _stacks;
        public List<UnitStack> Stacks => new List<UnitStack>(_stacks);
        public int Count => Stacks.Count;

        public Army(params UnitStack[] stacks)
        {
            if (stacks.Length > Config.MaxArmySize)
                throw new ArgumentException($"No more than {Config.MaxArmySize} stacks");
            _stacks = new List<UnitStack>(stacks);
        }

        public Army(IEnumerable<UnitStack> stacks) : this(stacks.ToArray()) { }

        public Army(BattleArmy army)
        {
            var list = new List<UnitStack>();
            army.Stacks.ForEach(
                stack =>
                {
                    if (stack.IsStackAlive)
                        list.Add(new UnitStack(stack.Unit, stack.Number));
                }
            );
            _stacks = list;
        }

        public Army() : this(Array.Empty<UnitStack>()) { }

        public void AddStack(UnitStack stack)
        {
            if (_stacks.Count + 1 > Config.MaxArmySize)
                throw new InvalidOperationException($"No more than {Config.MaxArmySize} stacks");
            _stacks.Add(stack);
        }

        public override int GetHashCode()
        {
            return 5 * _stacks.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Army))
                return false;
            var army = (Army)obj;
            if (this == army)
                return true;
            return GetHashCode() == army.GetHashCode() && _stacks.SequenceEqual(army._stacks);
        }

    }
}