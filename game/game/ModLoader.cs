using engine.units;
using engine.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace game
{
    public static class ModLoader
    {
        private const string PATH = @"C:\Users\Pepega\source\repos\game\game\bin\Debug\netcoreapp3.0\units.dll";
        public static IConfig GetConfig() => (IConfig) Activator.CreateInstance(
            GetAssembly().GetTypes().First(x => x.GetInterfaces().Contains(typeof(IConfig))));
        public static IEnumerable<Unit> GetAllUnits() => GetAssembly().GetTypes()
            .Where(x => x.IsSubclassOf(typeof(Unit)))
            .Select(x => (Unit) x.GetMethod("get_Instance").Invoke(null, null));
        private static Assembly GetAssembly() => Assembly.LoadFile(PATH);
    }
}
