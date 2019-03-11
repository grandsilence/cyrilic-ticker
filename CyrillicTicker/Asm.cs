using System.IO;
using System.Reflection;

namespace CyrillicTicker
{
    public static class Asm
    {
        public static string GetCurrentPath()
        {
            var asm = Assembly.GetExecutingAssembly();
            string location = asm.Location;
            return !string.IsNullOrEmpty(location) ? Path.GetDirectoryName(location) : null;
        }
    }
}