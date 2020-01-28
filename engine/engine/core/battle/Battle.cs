using engine.core.action;
using engine.utils;
using engine.vo;
using engine.vo.battle;

namespace engine.core.battle
{
    //TODO: Action history, listeners lul
    public class Battle : IBattle
    {
        private IBattleHelper _battleHelper;
        private IBattleState _battleState;

        public Battle(Player firstPlayer, Player secondPlayer)
        {
            _battleHelper = new BattleHelper(firstPlayer, secondPlayer);
            _battleState = new Init();

            OnBattleStart();
        }

        public Player GetFirstPlayer() => _battleHelper.GetFirstPlayer();
        public Player GetSecondPlayer() => _battleHelper.GetSecondPlayer();
        public BattleArmy GetFirstArmy() => _battleHelper.GetFirstArmy();
        public BattleArmy GetSecondArmy() => _battleHelper.GetSecondArmy();
        public IBattleState GetBattleState() => _battleState;
        private void OnBattleStart()
        {
            _battleState = new InProgress(_battleState.GetRound() + 1);
            OnBattleContinuation();
        }

        private void OnBattleContinuation()
        {
            while (_battleState.GetRound() < Config.MaxRoundCount)
            {
                for (var i = 0; i < Config.MaxRoundCount; i++)
                {
                    while (_battleHelper.GetCurrentAttackQueue().Count != 0)
                    {
                        var action = _battleHelper.OnNextAction();
                        if (action is Defence)
                            Config.Logger.LogMessage($"{action.Stack().Unit.GetType().Name} is defending");
                        else if (action is Await)
                            _battleHelper.OnAwait(action.Stack());

                        var loser = _battleHelper.CheckLoser();
                        if (loser == null) continue;
                        var winner = loser.Equals(_battleHelper.GetFirstPlayer())
                            ? _battleHelper.GetFirstPlayer()
                            : _battleHelper.GetSecondPlayer();
                        _battleState = new PlayerWon(_battleState.GetRound(), winner);
                        OnBattleFinish();
                        return;
                    }
                    _battleHelper.OnNextTurn();
                }
                _battleState = new InProgress(_battleState.GetRound() + 1);
            }

            _battleState = new TimeLimit();
            OnBattleFinish();
        }

        private void OnPause(Player pausedBy) => _battleState = new Pause(_battleState.GetRound(), pausedBy);

        private void OnBattleFinish()
        {
            _battleHelper.GetFirstPlayer().ArmyFromBattle(_battleHelper.GetFirstArmy());
            _battleHelper.GetSecondPlayer().ArmyFromBattle(_battleHelper.GetSecondArmy());
        }
    }
}