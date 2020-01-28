using engine.vo.battle;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace engine.core
{
    public class BattleQueue : IBattleQueue
    {
        private readonly Queue<BattleUnitStack> _firstQueue;
        private readonly HashSet<BattleUnitStack> _awaitQueue;

        public BattleQueue(IEnumerable<BattleUnitStack> stacks)
        {
            _firstQueue = new Queue<BattleUnitStack>(
                stacks.Where(x => x.IsStackAlive)
                    .OrderByDescending(x => x.Unit.Initiative)
            );
            _awaitQueue = new HashSet<BattleUnitStack>();
        }

        public BattleUnitStack Next()
        {
            if (_firstQueue.Count != 0)
                return _firstQueue.Dequeue();
            if (_awaitQueue.Count == 0) throw new Exception("Nothing in stacks!");
            var leastInitiative =
                _awaitQueue.Min(x => x.Initiative);
            var leastStack = _awaitQueue.First(x =>
                Math.Abs(leastInitiative - x.Unit.Initiative) < 0.1
            );
            _awaitQueue.Remove(leastStack);
            return leastStack;
        }

        public void Await(BattleUnitStack stack) => _awaitQueue.Add(stack);
        public int GetCount() => _firstQueue.Count + _awaitQueue.Count;

        public ImmutableList<BattleUnitStack> ToImmutableList()
        {
            var temp = _firstQueue.ToList();
            temp.AddRange(_awaitQueue);
            return temp.ToImmutableList();
        }
    }
}