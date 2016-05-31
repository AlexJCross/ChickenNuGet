namespace ChickenNuget
{
    public interface IConsoleRunner
    {
        void Run();
    }

    public class ConsoleRunner : IConsoleRunner
    {
        private readonly IChickenNuget chickenNuget;

        public ConsoleRunner(IChickenNuget chickenNuget)
        {
            this.chickenNuget = chickenNuget;
        }

        public void Run()
        {
            var settings = this.GetSettings();
            this.chickenNuget.CopyFiles(settings);
        }

        private ChickenNuGetSettings GetSettings()
        {
            return new ChickenNuGetSettings
            {
                DistributionFolder = @"C:\Users\Alex\Documents\Repos\LearningsInC\C Sharp Overview\CalculatorApp\bin\Debug",
                FusionPackagesFolder = @"C:\Users\Alex\Documents\Repos\LearningsInC\C Sharp Overview\packages"
            };
        }
    }
}