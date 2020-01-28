using engine.utils;
using engine.vo.battle;
using System;
using System.Collections.Generic;
using System.Linq;

namespace engine.core.machine
{
    public enum AttackMachineState
    {
        STRAIGHT_ATTACK,
        STRAIGHT_DEFENCE,
        STRAIGHT_DEALING,
        RESPONSE_ATTACK,
        RESPONSE_DEFENCE,
        RESPONSE_DEALING,
        END
    }

    public static class AttackMachine
    {
        private static readonly List<Rule>[] _setOfRules = Enumerable
            .Range(0, 6)
            .Select((i) => new List<Rule>())
            .ToArray();

        static AttackMachine()
        {
            AddRule(
                (x, y) => true,
                (x, y) => (x.Attack, AttackMachineState.STRAIGHT_DEFENCE),
                AttackMachineState.STRAIGHT_ATTACK
            );
            AddRule(
                (x, y) => true,
                (x, y) => (x.Defence, AttackMachineState.STRAIGHT_DEALING),
                AttackMachineState.STRAIGHT_DEFENCE
            );
            AddRule(
                (x, y) => true,
                (x, y) => (1, y.First().IsResponded && y.First().IsStackAlive ? AttackMachineState.END : AttackMachineState.RESPONSE_ATTACK),
                AttackMachineState.STRAIGHT_DEALING
            );
            AddRule(
                (x, y) => true,
                (x, y) => (y.First().Attack, AttackMachineState.RESPONSE_DEFENCE),
                AttackMachineState.RESPONSE_ATTACK
            );
            AddRule(
                (x, y) => true,
                (x, y) => (y.First().Defence, AttackMachineState.RESPONSE_DEALING),
                AttackMachineState.RESPONSE_DEFENCE
            );
            AddRule(
                (x, y) => true,
                (x, y) => (1, AttackMachineState.END),
                AttackMachineState.RESPONSE_DEALING
            );
        }

        public static void AddRule(
            Rule rule,
            AttackMachineState forState
        )
        {
            _setOfRules[(int)forState].Add(rule);
        }

        public static void AddRule(
            Func<BattleUnitStack, IEnumerable<BattleUnitStack>, bool> isAppliable,
            Func<BattleUnitStack, IEnumerable<BattleUnitStack>, (int, AttackMachineState)> valueRule,
            AttackMachineState forState
        )
        {
            _setOfRules[(int)forState].Add(
                new Rule(isAppliable, valueRule)
            );
        }

        public static void PerformAttack(BattleUnitStack from, IEnumerable<BattleUnitStack> to)
        {
            Navigator.Start(from, to);
        }

        private static void CalculateStraightAttack(
            BattleUnitStack from,
            IEnumerable<BattleUnitStack> to
        )
        {
            var list = _setOfRules[(int)AttackMachineState.STRAIGHT_ATTACK]
                .Where(x => x.IsAppliable(from, to))
                .Select(x => x.ValueRule(from, to));
            var attack = list.Any(x => x.Item1 == 0) ? 0 : list.Sum(x => x.Item1);
            var nextState = list.Max(x => x.Item2);
            Navigator.NavigateTo(from, to, nextState, (attack, 0));
        }

        private static void CalculateStraightDefence(
            BattleUnitStack from,
            IEnumerable<BattleUnitStack> to,
            int attack
        )
        {
            var list = _setOfRules[(int)AttackMachineState.STRAIGHT_DEFENCE]
                .Where(x => x.IsAppliable(from, to))
                .Select(x => x.ValueRule(from, to));
            var defence = list.Any(x => x.Item1 == 0) ? 0 : list.Sum(x => x.Item1);
            var nextState = list.Max(x => x.Item2);
            Navigator.NavigateTo(from, to, nextState, (attack, defence));
        }

        private static void DealStraightDamage(
            BattleUnitStack from,
            IEnumerable<BattleUnitStack> to,
            int attack,
            int defence
        )
        {
            var list = _setOfRules[(int)AttackMachineState.STRAIGHT_DEALING]
                .Where(x => x.IsAppliable(from, to))
                .Select(x => x.ValueRule(from, to));
            if (list.Count() != 1)
                list = list.Skip(1);
            var isHitted = list.Min(x => x.Item1);
            var hitFor = HitPointCalculator.CalculateAttack(from, attack, defence);

            to.Take(isHitted).OnEach(x => x.HitStack(hitFor));
            var nextState = list.Max(x => x.Item2);
            Navigator.NavigateTo(from, to, nextState, (0, 0));
        }

        private static void CalculateResponsetAttack(
            BattleUnitStack from,
            IEnumerable<BattleUnitStack> to
        )
        {
            var list = _setOfRules[(int)AttackMachineState.RESPONSE_ATTACK]
                .Where(x => x.IsAppliable(from, to))
                .Select(x => x.ValueRule(from, to));
            var attack = list.Any(x => x.Item1 == 0) ? 0 : list.Sum(x => x.Item1);
            var nextState = list.Max(x => x.Item2);
            Navigator.NavigateTo(from, to, nextState, (attack, 0));
        }

        private static void CalculateResponseDefence(
            BattleUnitStack from,
            IEnumerable<BattleUnitStack> to,
            int attack
        )
        {
            var list = _setOfRules[(int)AttackMachineState.RESPONSE_DEFENCE]
                .Where(x => x.IsAppliable(from, to))
                .Select(x => x.ValueRule(from, to));
            var defence = list.Any(x => x.Item1 == 0) ? 0 : list.Sum(x => x.Item1);
            var nextState = list.Max(x => x.Item2);
            Navigator.NavigateTo(from, to, nextState, (attack, defence));
        }

        private static void DealResponseDamage(
            BattleUnitStack from,
            IEnumerable<BattleUnitStack> to,
            int attack,
            int defence
        )
        {
            var list = _setOfRules[(int)AttackMachineState.RESPONSE_DEALING]
                .Where(x => x.IsAppliable(from, to))
                .Select(x => x.ValueRule(from, to));
            var isHitted = list.Min(x => x.Item1);
            if (isHitted != 0)
            {
                to.First().IsResponded = true;
                var hitFor = HitPointCalculator.CalculateAttack(to.First(), attack, defence);
                from.HitStack(hitFor);
            }
            var nextState = list.Max(x => x.Item2);
            Navigator.NavigateTo(from, to, nextState, (0, 0));
        }

        private static class Navigator
        {
            public static void Start(
                BattleUnitStack from,
                IEnumerable<BattleUnitStack> to
            )
            {
                CalculateStraightAttack(from, to);
            }

            public static void NavigateTo(
                BattleUnitStack from,
                IEnumerable<BattleUnitStack> to,
                AttackMachineState nextState,
                (int, int) values
            )
            {
                switch (nextState)
                {
                    case AttackMachineState.STRAIGHT_ATTACK:
                        throw new Exception("Invalid state");
                    case AttackMachineState.STRAIGHT_DEFENCE:
                        CalculateStraightDefence(from, to, values.Item1);
                        break;
                    case AttackMachineState.STRAIGHT_DEALING:
                        DealStraightDamage(from, to, values.Item1, values.Item2);
                        break;
                    case AttackMachineState.RESPONSE_ATTACK:
                        CalculateResponsetAttack(from, to);
                        break;
                    case AttackMachineState.RESPONSE_DEFENCE:
                        CalculateResponseDefence(from, to, values.Item1);
                        break;
                    case AttackMachineState.RESPONSE_DEALING:
                        DealResponseDamage(from, to, values.Item1, values.Item2);
                        break;
                    case AttackMachineState.END:
                        break;
                }
            }
        }

    }

    public class Rule
    {
        public Func<BattleUnitStack, IEnumerable<BattleUnitStack>, bool> IsAppliable { get; }
        public Func<BattleUnitStack, IEnumerable<BattleUnitStack>, (int, AttackMachineState)> ValueRule { get; }

        public Rule(
            Func<BattleUnitStack, IEnumerable<BattleUnitStack>, bool> isAppliable,
            Func<BattleUnitStack, IEnumerable<BattleUnitStack>, (int, AttackMachineState)> valueRule
        )
        {
            IsAppliable = isAppliable;
            ValueRule = valueRule;
        }
    }
}
