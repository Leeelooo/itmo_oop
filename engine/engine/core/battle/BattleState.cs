using engine.utils;
using engine.vo;

namespace engine.core.battle
{
    public interface IBattleState
    {
        uint GetRound();
    }

    public class Init : IBattleState
    {
        public Init()
        {
        }

        public uint GetRound() => 0;
    }

    public class InProgress : IBattleState
    {
        private readonly uint _round;

        public InProgress(uint round)
        {
            _round = round;
        }

        public uint GetRound() => _round;
    }

    public class PlayerWon : IBattleState
    {
        private readonly uint _round;
        public Player Winner { get; }

        public PlayerWon(uint round, Player winner)
        {
            _round = round;
            Winner = winner;
        }

        public uint GetRound() => _round;
    }

    public class TimeLimit : IBattleState
    {
        public TimeLimit() { }

        public uint GetRound() => Config.MaxRoundCount;
    }

    public class Pause : IBattleState
    {
        private readonly uint _round;
        public Player PausedBy { get; }

        public Pause(uint round, Player pausedBy)
        {
            _round = round;
            PausedBy = pausedBy;
        }

        public uint GetRound() => _round;
    }
}