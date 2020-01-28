using engine.core.machine;
using engine.utils;
using System;
using System.Linq;

namespace units
{
    public class LibraryConfig : DefaultConfig
    {
        static LibraryConfig()
        {
            AttackMachine.AddRule(
                (x, y) => x.Unit is IArcher || y.First().Unit is IArcher,
                (x, y) => (x.Attack, AttackMachineState.END),
                AttackMachineState.STRAIGHT_DEALING
            );

            AttackMachine.AddRule(
                (x, y) => x.Unit is IPiercingShot,
                (x, y) => (0, AttackMachineState.STRAIGHT_DEALING),
                AttackMachineState.STRAIGHT_DEFENCE
            );
            AttackMachine.AddRule(
                (x, y) => y.First().Unit is IPiercingShot,
                (x, y) => (0, AttackMachineState.RESPONSE_DEALING),
                AttackMachineState.RESPONSE_DEFENCE
            );

            AttackMachine.AddRule(
                (x, y) => x.Unit is IHeavyWounds,
                (x, y) => (x.Attack, AttackMachineState.END),
                AttackMachineState.STRAIGHT_DEALING
            );

            AttackMachine.AddRule(
                (x, y) => x.Unit is ICleave,
                (x, y) => (y.Count(), AttackMachineState.END),
                AttackMachineState.STRAIGHT_DEALING
            );

            AttackMachine.AddRule(
                (x, y) => y.First().Unit is IBattleFever,
                (x, y) => (x.Attack, AttackMachineState.RESPONSE_ATTACK),
                AttackMachineState.STRAIGHT_DEALING
            );

            AttackMachine.AddRule(
                (x, y) => x.Unit is IGhost,
                (x, y) => (new Random().Next(0, 4) == 0 ? 0 : 1, AttackMachineState.RESPONSE_ATTACK),
                AttackMachineState.STRAIGHT_DEALING
            );

        }
    }
}
