using engine.core.battle;
using engine.utils;
using engine.vo;
using engine.vo.travel;
using System.Linq;

namespace game
{
    public class Program
    {
        public static void Main()
        {
            Config.SetConfig(ModLoader.GetConfig());
            var units = ModLoader.GetAllUnits();//Angel Crossbowman Skeleton Hag BoneDragon Devil Cyclops Gryfon Shaman Lich Highborn
            var player1 = new Player(
                "pepega",
                new Army(
                    new UnitStack(units.Last(), 999999), //Highborn
                    new UnitStack(units.Take(3).Last(), 200), //Skeleton
                    new UnitStack(units.TakeLast(2).First(), 2) //Lich
                )
            );
            var player2 = new Player(
                "pepe",
                new Army(
                    new UnitStack(units.First(), 20), //Angel
                    new UnitStack(units.Take(5).Last(), 200), //BoneDragon
                    new UnitStack(units.TakeLast(4).First(), 20000) //Gryfon
                )
            );
            new Battle(player1, player2);
        }
    }
}
