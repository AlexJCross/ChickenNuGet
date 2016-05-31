using System.Windows;
using Prism.Unity;
using Microsoft.Practices.Unity;
using System.Runtime.InteropServices;
using System;

namespace ChickenNuget
{
    public class DualBootstrapper : UnityBootstrapper
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;

        private readonly bool isConsoleMode;

        public DualBootstrapper(bool isConsoleMode = true)
        {
            this.isConsoleMode = isConsoleMode;
        }

        public override void Run(bool runWithDefaultConfiguration)
        {
            if (!this.isConsoleMode)
            {
                var handle = GetConsoleWindow();
                ShowWindow(handle, SW_HIDE);
            }

            base.Run(runWithDefaultConfiguration);
            if (this.isConsoleMode)
            {
                this.Container.Resolve<IConsoleRunner>().Run();
            }
        }

        protected override DependencyObject CreateShell()
        {
            if (this.isConsoleMode)
            {
                return null;
            }

            return this.Container.Resolve<Shell>();
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            this.Container.RegisterTypes(
                typeof (IConsoleRunner).Assembly.GetTypes(),
                WithMappings.FromMatchingInterface,
                WithName.Default,
                WithLifetime.ContainerControlled);
        }

        protected override void InitializeShell()
        {
            var app = new Application{MainWindow = (Window)this.Shell};
            Application.Current.MainWindow.Show();
            Application.Current.Run();
        }
    }
}