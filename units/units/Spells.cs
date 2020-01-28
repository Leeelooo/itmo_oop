using engine.vo;
using engine.vo.battle;
using System;
using System.Collections.Generic;
using System.Linq;

namespace units
{
    public class Resurrection : ISpell
    {
        private readonly BattleUnitStack _from;
        private IEnumerable<BattleUnitStack> targets;

        public readonly int HealFor = 100;

        public Resurrection(BattleUnitStack from) => _from = from;
        public BattleUnitStack CastedBy() => _from;
        public TargetArmy ApplyFor() => TargetArmy.ALLY;
        public IEnumerable<BattleUnitStack> Targets() => targets;
        public IEnumerable<BattleUnitStack> Filter(IEnumerable<BattleUnitStack> stacks) => stacks.Where(x => x.Unit is IUndead);
        public void Invoke(IEnumerable<BattleUnitStack> targets)
        {
            this.targets = targets;
            this.targets.First().ResurrectStack(HealFor * _from.Number);
        }
    }

    public class PunishmentStrike : ISpell
    {
        private readonly BattleUnitStack _from;
        private IEnumerable<BattleUnitStack> targets;

        public readonly int _upFor = 12;
        public readonly int _rounds = 3;

        public PunishmentStrike(BattleUnitStack from) => _from = from;
        public BattleUnitStack CastedBy() => _from;
        public TargetArmy ApplyFor() => TargetArmy.ENEMY;
        public IEnumerable<BattleUnitStack> Targets() => targets;
        public IEnumerable<BattleUnitStack> Filter(IEnumerable<BattleUnitStack> stacks) => stacks.Where(x => x.IsStackAlive);
        public void Invoke(IEnumerable<BattleUnitStack> targets)
        {
            this.targets = targets;
            this.targets.First().AddEffect(
                new Effect(
                    isPositive: false,
                    duration: _rounds,
                    apply: x => { x.Attack += _upFor; if (x.Attack < 0) x.Attack = 0; }
                )
            );
        }
    }

    public class Curse : ISpell
    {
        private readonly BattleUnitStack _from;
        private IEnumerable<BattleUnitStack> targets;

        public readonly int _downFor = -12;
        public readonly int _rounds = 3;

        public Curse(BattleUnitStack from) => _from = from;
        public BattleUnitStack CastedBy() => _from;
        public TargetArmy ApplyFor() => TargetArmy.ENEMY;
        public IEnumerable<BattleUnitStack> Targets() => targets;
        public IEnumerable<BattleUnitStack> Filter(IEnumerable<BattleUnitStack> stacks) => stacks.Where(x => x.IsStackAlive);
        public void Invoke(IEnumerable<BattleUnitStack> targets)
        {
            this.targets = targets;
            this.targets.First().AddEffect(
                new Effect(
                    isPositive: false,
                    duration: _rounds,
                    apply: x => { x.Attack += _downFor; if (x.Attack < 0) x.Attack = 0; }
                )
            );
        }
    }

    public class Weakening : ISpell
    {
        private readonly BattleUnitStack _from;
        private IEnumerable<BattleUnitStack> targets;

        private readonly int _downFor = -12;
        private readonly int _rounds = 3;

        public Weakening(BattleUnitStack from) => _from = from;
        public BattleUnitStack CastedBy() => _from;
        public TargetArmy ApplyFor() => TargetArmy.ENEMY;
        public IEnumerable<BattleUnitStack> Targets() => targets;
        public IEnumerable<BattleUnitStack> Filter(IEnumerable<BattleUnitStack> stacks) => stacks.Where(x => x.IsStackAlive);
        public void Invoke(IEnumerable<BattleUnitStack> targets)
        {
            this.targets = targets;
            this.targets.First().AddEffect(
                new Effect(
                    isPositive: false,
                    duration: _rounds,
                    apply: x => { x.Defence += _downFor; if (x.Defence < 0) x.Defence = 0; }
                )
            );
        }
    }

    public class Haste : ISpell
    {
        private readonly BattleUnitStack _from;
        private IEnumerable<BattleUnitStack> targets;

        private readonly double _multiplier = 1.4;
        public readonly int _rounds = 3;

        public Haste(BattleUnitStack from) => _from = from;
        public BattleUnitStack CastedBy() => _from;
        public TargetArmy ApplyFor() => TargetArmy.ALLY;
        public IEnumerable<BattleUnitStack> Targets() => targets;
        public IEnumerable<BattleUnitStack> Filter(IEnumerable<BattleUnitStack> stacks) => stacks.Where(x => x.IsStackAlive);
        public void Invoke(IEnumerable<BattleUnitStack> targets)
        {
            this.targets = targets;
            this.targets.First().AddEffect(
                new Effect(
                    isPositive: true,
                    duration: _rounds,
                    apply: x => x.Initiative += (int)Math.Round(targets.First().Unit.Initiative * _multiplier)
                )
            );
        }
    }

    public class DeathTouch : ISpell
    {
        private readonly BattleUnitStack _from;
        private IEnumerable<BattleUnitStack> targets;

        public DeathTouch(BattleUnitStack from) => _from = from;
        public BattleUnitStack CastedBy() => _from;
        public TargetArmy ApplyFor() => TargetArmy.ENEMY;
        public IEnumerable<BattleUnitStack> Targets() => targets;
        public IEnumerable<BattleUnitStack> Filter(IEnumerable<BattleUnitStack> stacks) => stacks.Where(x => x.IsStackAlive);
        public void Invoke(IEnumerable<BattleUnitStack> targets)
        {
            this.targets = targets;
            this.targets.First().HitStack(this.targets.First().Number * this.targets.First().Unit.HitPoints);
        }
    }
}
