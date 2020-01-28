using engine.core.action;
using engine.utils;
using engine.vo;
using engine.vo.battle;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace engine.core.battle
{
    internal class BattleHelper : IBattleHelper
    {
        private readonly Player _firstPlayer;
        private readonly Player _secondPlayer;

        private Player _currentPlayer;
        private BattleArmy _currentArmy;
        private IEnumerable<BattleUnitStack> _enemyStacks;
        private readonly BattleArmy _firstArmy;
        private readonly BattleArmy _secondArmy;

        private IBattleQueue _queue;

        internal BattleHelper(Player firstPlayer, Player secondPlayer)
        {
            _firstPlayer = firstPlayer;
            _secondPlayer = secondPlayer;
            _firstArmy = new BattleArmy(_firstPlayer.Army);
            _secondArmy = new BattleArmy(_secondPlayer.Army);

            OnNextTurn();
        }

        public void OnNextTurn()
        {
            if (_currentPlayer == null || _currentPlayer.Equals(_secondPlayer))
            {
                _currentPlayer = _firstPlayer;
                _currentArmy = _firstArmy;
                _enemyStacks = _secondArmy.Stacks;
            }
            else
            {
                _currentPlayer = _secondPlayer;
                _currentArmy = _secondArmy;
                _enemyStacks = _firstArmy.Stacks;
            }
            Config.Logger.LogMessage($"{_currentPlayer.Name}'s turn");

            foreach (var stack in _currentArmy.Stacks)
            {
                stack.OnNextTurn();
            }

            _queue = new BattleQueue(_currentArmy.Stacks);
        }

        public IBattleAction OnNextAction()
        {
            var enemyStacks = _enemyStacks.Where(x => x.IsStackAlive);
            if (_queue.GetCount() != 0)
                return _currentPlayer.OnPlayerAction(_queue.Next(), _currentArmy.Stacks, enemyStacks);
            throw new Exception("Turn should be ended!");
        }
        public Player CheckLoser()
        {
            if (_firstArmy.Stacks.Count(x => x.IsStackAlive) == 0)
                return _firstPlayer;
            return _secondArmy.Stacks.Count(x => x.IsStackAlive) == 0 ? _secondPlayer : null;
        }

        public void OnAwait(BattleUnitStack stack) 
        {
            _queue.Await(stack);
            Config.Logger.LogMessage($"{stack.Unit.GetType().Name} is awaiting");
        }
        public ImmutableList<BattleUnitStack> GetCurrentAttackQueue() => _queue.ToImmutableList();
        public Player GetFirstPlayer() => _firstPlayer;
        public Player GetSecondPlayer() => _secondPlayer;
        public Player GetCurrentPlayer() => _currentPlayer;
        public BattleArmy GetFirstArmy() => _firstArmy;
        public BattleArmy GetSecondArmy() => _secondArmy;
    }
}