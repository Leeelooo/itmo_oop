using engine.core.action;
using engine.vo.battle;
using engine.vo.travel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace engine.vo
{
    public class Player
    {
        public Army Army { get; private set; }
        public string Name { get; }

        public Player(string name)
        {
            Name = name;
            Army = new Army();
        }
        public Player(string name, Army army)
        {
            Name = name;
            Army = army;
        }
        public void ArmyFromBattle(BattleArmy army) => Army = new Army(army);
        public void BuyNewStack(UnitStack stack) => Army.AddStack(stack);
        public IBattleAction OnPlayerAction(
            BattleUnitStack stackToAttack,
            IEnumerable<BattleUnitStack> yourArmy,
            IEnumerable<BattleUnitStack> enemyStacks
        )
        {
            Console.WriteLine($"{stackToAttack.Unit.GetType().Name} {stackToAttack.Number}");
            var enemyArmySize = enemyStacks.Count();
            var inputAction = 0;

            while (inputAction < 1 || inputAction > 4)
            {
                Console.WriteLine("Your decision:");
                Console.WriteLine("1. Attack");
                Console.WriteLine("2. Spell");
                Console.WriteLine("3. Defence");
                Console.WriteLine("4. Await");
                var input = Console.ReadLine();
                int.TryParse(input, out inputAction);
            }

            switch (inputAction)
            {
                case 1:
                    var inputAttack = 0;
                    var iter = 1;
                    var attack = new Attack(this, stackToAttack, yourArmy, enemyStacks);
                    foreach (var stack in attack.Filter)
                    {
                        Console.WriteLine($"{iter++}. {stack.Number} of {stack.Unit.GetType().Name}");
                    }

                    var input = Console.ReadLine();
                    int.TryParse(input, out inputAttack);
                    while (true)
                    {
                        iter = 1;
                        foreach (var stack in attack.Filter)
                        {
                            if (iter == inputAttack)
                            {
                                var list = new List<BattleUnitStack>() { stack };
                                list.AddRange(attack.Filter.Where(x => !x.Equals(stack)));
                                attack.OnTargetChoosed(list);
                                return attack;
                            }

                            iter++;
                        }
                    }
                case 2:
                    var spell = 0;
                    var spellIter = 1;
                    foreach (var spell_ in stackToAttack.Unit.Spells)
                    {
                        Console.WriteLine($"{spellIter++}. {spell_.Name}");
                    }

                    var choosedSpell = Console.ReadLine();
                    int.TryParse(choosedSpell, out spell);
                    var nthSpell = stackToAttack.Unit.Spells.Skip(spell - 1).First();
                    var spellRefl = nthSpell.GetConstructor(new[] { typeof(BattleUnitStack) });
                    var createdSpell = (ISpell)spellRefl.Invoke(new object[] { stackToAttack });
                    var spellAction = new SpellCasted(this, createdSpell, yourArmy, enemyStacks);

                    spell = 0;
                    spellIter = 1;
                    foreach (var stack in spellAction.Filter)
                    {
                        Console.WriteLine($"{spellIter++}. {stack.Number} of {stack.Unit.GetType().Name}");
                    }

                    var inputSpell = Console.ReadLine();
                    int.TryParse(inputSpell, out spell);
                    while (true)
                    {
                        iter = 1;
                        foreach (var stack in spellAction.Filter)
                        {
                            if (iter == spell)
                            {
                                spellAction.OnTargetChoosed(new List<BattleUnitStack>() { stack });
                                return spellAction;
                            }

                            iter++;
                        }
                    }
                case 3:
                    return new Defence(this, stackToAttack);
                case 4:
                    return new Await(this, stackToAttack);
                default:
                    throw new Exception("?????????????????????????");
            }
        }

        public override int GetHashCode() => 5 * Army.GetHashCode();
        public override bool Equals(object obj)
        {
            if (!(obj is Player))
                return false;
            var player = (Player)obj;
            if (this == player)
                return true;
            return GetHashCode() == player.GetHashCode() && Army.Equals(player.Army);
        }
    }
}