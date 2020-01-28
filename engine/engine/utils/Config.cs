using engine.utils.logger;

namespace engine.utils
{
    public static class Config
    {
        private static IConfig _value;

        static Config() => _value ??= new DefaultConfig();
        public static void SetConfig(IConfig config) => _value = config;

        public static int MaxUnitCount => _value.GetMaxUnitCount();
        public static int MaxArmySize => _value.GetMaxArmySize();
        public static int MaxBattleArmySize => _value.GetMaxBattleArmySize();
        public static uint MaxRoundCount => _value.GetMaxRoundCount();
        public static double DefenceMultiplier => _value.GetDefenceMultiplier();
        public static ILogger Logger => _value.GetLogger();
        public static int MaxPlayerCount => _value.GetMaxPlayerCount();
    }

    public interface IConfig
    {
        int GetMaxUnitCount();
        int GetMaxArmySize();
        int GetMaxBattleArmySize();
        uint GetMaxRoundCount();
        double GetDefenceMultiplier();
        ILogger GetLogger();
        int GetMaxPlayerCount() => 2;
    }

    public class DefaultConfig : IConfig
    {
        public ILogger GetLogger() => ConsoleLogger.Instance;
        public double GetDefenceMultiplier() => 1.3;
        public int GetMaxArmySize() => 6;
        public int GetMaxBattleArmySize() => 9;
        public uint GetMaxRoundCount() => 50;
        public int GetMaxUnitCount() => 999_999;
    }
}