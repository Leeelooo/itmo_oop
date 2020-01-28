using engine.units;
using engine.utils;
using engine.vo.travel;
using System;
using System.Collections.Generic;

namespace engine.vo.battle
{
    public class BattleUnitStack
    {
        public Unit Unit { get; }
        public int Number { get; private set; }
        public int LastStackHp { get; set; }
        public bool IsStackAlive => Number > 0;
        public readonly List<Effect> Effects;

        public int Defence;
        public int Attack;
        public double Initiative;

        public bool IsResponded = false;
        public bool IsDefending = false;
        public bool IsStunned = false;

        private readonly int _startNumber;

        public BattleUnitStack(UnitStack stack)
        {
            Unit = stack.Unit;
            LastStackHp = stack.Unit.HitPoints;
            Number = stack.Number;
            Effects = new List<Effect>();
            _startNumber = stack.Number;
        }

        public BattleUnitStack(Unit unit, int number = 1)
        {
            if (number > Config.MaxUnitCount)
                throw new ArgumentException("Too much units in stack");
            Unit = unit;
            LastStackHp = Unit.HitPoints;
            Number = number;
            Effects = new List<Effect>();
            _startNumber = number;
        }

        public void OnNextTurn()
        {
            IsDefending = false;
            IsResponded = false;
            IsStunned = false;

            Attack = Unit.Attack;
            Defence = Unit.Defence;
            Initiative = Unit.Initiative;

            Effects.RemoveAll(x => x.Duration < 2);
            Effects.OnEach(x => { x.Apply(this); x.Duration--; });
        }

        public void AddEffect(Effect effect)
        {
            Config.Logger.LogMessage($"To {Unit.GetType().Name} applied effect {effect.GetType().Name}");
            Effects.Add(effect);
        }

        public void HitStack(int hpToSubstract)
        {
            Config.Logger.LogMessage($"{Unit.GetType().Name} with number {Number} hitted for {hpToSubstract}");
            var whole = hpToSubstract / Unit.HitPoints;
            var diff = hpToSubstract % Unit.HitPoints;
            if (diff > LastStackHp)
            {
                LastStackHp += Unit.HitPoints - diff;
                whole++;
            }
            else
            {
                LastStackHp -= diff;
            }

            if (whole >= Number)
            {
                Number = 0;
                LastStackHp = 0;
                Config.Logger.LogMessage($"{Unit.GetType().Name} died");
            }
            else
            {
                Number -= whole;
                Config.Logger.LogMessage($"{Unit.GetType().Name} still alive with number {Number}");
            }
        }

        public void ResurrectStack(int healFor)
        {
            Config.Logger.LogMessage($"{Unit.GetType().Name} with {Number} healed for {healFor}");
            var whole = healFor / Unit.HitPoints;
            var diff = healFor % Unit.HitPoints;

            if (diff + LastStackHp > Unit.HitPoints)
                whole++;
            LastStackHp += diff;
            LastStackHp %= Unit.HitPoints;
            Number += whole;
            if (Number <= _startNumber) return;
            LastStackHp = Unit.HitPoints;
            Number = _startNumber;
        }

        public override int GetHashCode()
        {
            return 5 * Unit.GetHashCode() + 7 * Number.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is BattleUnitStack))
                return false;
            var stack = (BattleUnitStack)obj;
            if (GetHashCode() != stack.GetHashCode())
                return false;
            return Unit.Equals(stack.Unit) && Number == stack.Number;
        }
    }
}