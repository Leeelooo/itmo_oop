using engine.core.machine;
using engine.utils;
using engine.vo;
using engine.vo.battle;
using System.Collections.Generic;
using System.Linq;

namespace engine.core.action
{
    public class Attack : ITargetBattleAction
    {
        private readonly Player _by;
        private readonly BattleUnitStack _from;
        private readonly IEnumerable<BattleUnitStack> _ally;
        private readonly IEnumerable<BattleUnitStack> _enemy;
        private IEnumerable<BattleUnitStack> _targets;

        public Attack(
            Player by,
            BattleUnitStack from,
            IEnumerable<BattleUnitStack> ally,
            IEnumerable<BattleUnitStack> enemy
        )
        {
            _by = by;
            _from = from;
            _ally = ally;
            _enemy = enemy;
        }

        public Player Player() => _by;
        public BattleUnitStack Stack() => _from;
        public IEnumerable<BattleUnitStack> AllyArmy() => _ally;
        public IEnumerable<BattleUnitStack> EnemyArmy() => _enemy;
        public IEnumerable<BattleUnitStack> Filter => _enemy.Where(x => x.IsStackAlive);
        public IEnumerable<BattleUnitStack> Targets() => _targets;
        public bool IsAppliable() => Filter.Any(x => x.IsStackAlive);
        public void OnTargetChoosed(IEnumerable<BattleUnitStack> targets)
        {
            _targets = targets;
            AttackMachine.PerformAttack(_from, targets);
        }
    }

    public class SpellCasted : ITargetBattleAction
    {
        public ISpell Spell { get; }

        private readonly Player _by;
        private readonly IEnumerable<BattleUnitStack> _ally;
        private readonly IEnumerable<BattleUnitStack> _enemy;

        public SpellCasted(
            Player by,
            ISpell spell,
            IEnumerable<BattleUnitStack> ally,
            IEnumerable<BattleUnitStack> enemy
        )
        {
            _by = by;
            Spell = spell;
            _ally = ally;
            _enemy = enemy;
        }

        public Player Player() => _by;
        public BattleUnitStack Stack() => Spell.CastedBy();
        public IEnumerable<BattleUnitStack> AllyArmy() => _ally;
        public IEnumerable<BattleUnitStack> EnemyArmy() => _enemy;
        public IEnumerable<BattleUnitStack> Filter
        {
            get
            {
                return Spell.Filter((Spell.ApplyFor()) switch
                {
                    TargetArmy.ENEMY => _enemy,
                    TargetArmy.ALLY => _ally,
                    _ => _ally.Concat(_enemy),
                });
            }
        }
        public IEnumerable<BattleUnitStack> Targets() => Spell.Targets();
        public bool IsAppliable() => Filter.Any(x => x.IsStackAlive);
        public void OnTargetChoosed(IEnumerable<BattleUnitStack> targets)
        {
            Config.Logger.LogMessage($"{Spell.GetType().Name} casted on {targets.First().Unit.GetType().Name}");
            Spell.Invoke(targets);
        }
    }

    public class Defence : INonTargetBattleAction
    {
        private readonly Player _by;
        private readonly BattleUnitStack _from;

        public Defence(Player by, BattleUnitStack stack)
        {
            _by = by;
            _from = stack;
            _from.IsDefending = true;
        }
        public Player Player() => _by;
        public BattleUnitStack Stack() => _from;
    }

    public class Await : INonTargetBattleAction
    {
        private readonly Player _by;
        private readonly BattleUnitStack _from;

        public Await(Player by, BattleUnitStack stack)
        {
            _by = by;
            _from = stack;
        }
        public Player Player() => _by;
        public BattleUnitStack Stack() => _from;
    }

    public class Escape : IGlobalAction
    {
        private readonly Player _escaper;
        public Escape(Player escaper)
        {
            _escaper = escaper;
        }
        public Player Player() => _escaper;
    }

    public class Pause : IGlobalAction
    {
        private readonly Player _by;
        public Pause(Player by)
        {
            _by = by;
        }
        public Player Player() => _by;
    }

}
