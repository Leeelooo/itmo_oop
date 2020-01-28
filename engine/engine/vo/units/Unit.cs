using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace engine.units
{
    public class Unit
    {
        public int HitPoints { get; }
        public int Attack { get; }
        public int Defence { get; }
        public (int, int) Damage { get; }
        public double Initiative { get; }

        public readonly ImmutableList<Type> Spells;

        public Unit(
            int hitPoints,
            int attack,
            int defence,
            (int, int) damage,
            double initiative,
            params Type[] spells
        )
        {
            if (hitPoints == 0)
                throw new ArgumentException("Cannot create unit with zero health");
            HitPoints = hitPoints;
            Attack = attack;
            Defence = defence;
            Damage = damage.Item1 > damage.Item2 ? (damage.Item2, damage.Item1) : damage;
            Initiative = initiative;
            Spells = new List<Type>(spells).ToImmutableList();
        }

        public override int GetHashCode()
        {
            return 5 * HitPoints.GetHashCode() +
                   7 * Attack.GetHashCode() +
                   11 * Defence.GetHashCode() +
                   13 * Damage.GetHashCode() +
                   17 * Initiative.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Unit))
                return false;
            var unit = (Unit)obj;
            if (GetHashCode() != unit.GetHashCode())
                return false;
            return HitPoints == unit.HitPoints &&
                   Attack == unit.Attack &&
                   Defence == unit.Defence &&
                   Damage.Equals(unit.Damage) &&
                   Math.Abs(Initiative - unit.Initiative) < .0;
        }
    }
}