using System;

namespace ChickenNuget
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            bool isConsoleApp = true;
            var bs = new DualBootstrapper(isConsoleApp);
            bs.Run();
        }
    }
}