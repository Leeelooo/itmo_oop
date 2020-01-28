using engine.units;

namespace units
{
    public sealed class Angel : Unit
    {
        private static Angel _instance;

        public static Angel Instance => _instance ??= new Angel();

        private Angel() : base(180, 27, 27, (45, 45), 11, typeof(PunishmentStrike))
        {
            _instance = this;
        }
    }

    public sealed class Crossbowman : Unit, IArcher, IPiercingShot
    {
        private static Crossbowman _instance;

        public static Crossbowman Instance => _instance ??= new Crossbowman();

        private Crossbowman() : base(10, 4, 4, (2, 8), 8)
        {
            _instance = this;
        }
    }

    public sealed class Skeleton : Unit, IUndead
    {
        private static Skeleton _instance;

        public static Skeleton Instance => _instance ??= new Skeleton();

        private Skeleton() : base(5, 1, 2, (1, 1), 10)
        {
            _instance = this;
        }
    }

    public sealed class Hag : Unit, IHeavyWounds
    {
        private static Hag _instance;

        public static Hag Instance => _instance ??= new Hag();

        private Hag() : base(16, 5, 3, (5, 7), 16)
        {
            _instance = this;
        }
    }

    public sealed class BoneDragon : Unit, ICleave, IHeavyWounds
    {
        private static BoneDragon _instance;

        public static BoneDragon Instance => _instance ??= new BoneDragon();

        private BoneDragon() : base(150, 27, 28, (15, 30), 11, typeof(Curse))
        {
            _instance = this;
        }
    }

    public sealed class Devil : Unit
    {
        private static Devil _instance;

        public static Devil Instance => _instance ??= new Devil();

        private Devil() : base(166, 27, 25, (36, 66), 11, typeof(Weakening))
        {
            _instance = this;
        }
    }

    public sealed class Cyclops : Unit, IArcher
    {
        private static Cyclops _instance;

        public static Cyclops Instance => _instance ??= new Cyclops();

        private Cyclops() : base(85, 20, 15, (18, 26), 10)
        {
            _instance = this;
        }
    }

    public sealed class Gryfon : Unit, IBattleFever, ICleave
    {
        private static Gryfon _instance;

        public static Gryfon Instance => _instance ??= new Gryfon();

        private Gryfon() : base(30, 7, 5, (5, 10), 15)
        {
            _instance = this;
        }
    }

    public sealed class Shaman : Unit
    {
        private static Shaman _instance;

        public static Shaman Instance => _instance ??= new Shaman();

        private Shaman() : base(40, 12, 10, (7, 12), 10.5)
        {
            _instance = this;
        }
    }

    public sealed class Lich : Unit, IUndead, IArcher
    {
        private static Lich _instance;

        public static Lich Instance => _instance ??= new Lich();

        private Lich() : base(50, 15, 15, (12, 17), 10, typeof(Resurrection))
        {
            _instance = this;
        }
    }

    public sealed class Highborn : Unit, IGhost
    {
        private static Highborn _instance;

        public static Highborn Instance => _instance ??= new Highborn();

        private Highborn() : base(1, 1, 1, (1, 1), 5, typeof(DeathTouch))
        {
            _instance = this;
        }
    }
}
    