using engine.units;
using engine.utils;
using System;

namespace engine.vo.travel
{
    public class UnitStack
    {
        public Unit Unit { get; }
        public int Number { get; }
        public bool IsStackAlive => Number > 0;

        public UnitStack(Unit unit, int number)
        {
            if (number > Config.MaxUnitCount)
                throw new ArgumentException("Too much units in stack");
            Unit = unit;
            Number = number;
        }

        public UnitStack(Unit unit) : this(unit, 1) { }

        public override int GetHashCode()
        {
            return 3 * Unit.GetHashCode() + 5 * Number.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is UnitStack))
                return false;
            var stack = (UnitStack)obj;
            if (GetHashCode() != stack.GetHashCode())
                return false;
            return Unit.Equals(stack.Unit) && Number == stack.Number;
        }

    }
}