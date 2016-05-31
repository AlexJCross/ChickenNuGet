using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Data;

namespace ChickenNuget
{
    public interface IChickenNuget
    {
        void CopyFiles(ChickenNuGetSettings settings);
    }

    public class ChickenNuget : IChickenNuget
    {
        private readonly ILogger logger;
        int filesCopied, copyFailures;

        public ChickenNuget(ILogger logger)
        {
            this.logger = logger;
        }

        public void CopyFiles(ChickenNuGetSettings settings)
        {
            var distributionBinaries = this.GetFiles(settings.DistributionFolder);
            this.CopyFilesToFolder(distributionBinaries, settings.FusionPackagesFolder);
            this.ReportSuccess();
        }

        private void ReportSuccess()
        {
            string report;
            if (this.copyFailures > 0)
            {
                report = string.Format(@"Unsuccessful. {0} files failed to copy", this.copyFailures);
            }
            else
            {
                report = string.Format(@"All {0} files copied successfully.", this.filesCopied);
            }

            this.logger.Log("\n" + report);
        }

        private void CopyFilesToFolder(IList<FileInfo> distributionBinaries, string folder)
        {
            this.logger.Log(@".\" + Path.GetFileName(folder));
            var subFolders = Directory.GetDirectories(folder);
            // Subfolders
            foreach (var subFolder in subFolders)
            {
                this.logger.Indent();
                this.CopyFilesToFolder(distributionBinaries, subFolder);
                this.logger.Unindent();
            }

            // Files in folder
            this.logger.Indent();
            foreach (FileInfo file in GetFiles(folder))
            {
                if (distributionBinaries.Any(f => f.Name == file.Name))
                {
                    try
                    {
                        distributionBinaries.Single(f => f.Name == file.Name).CopyTo(Path.Combine(folder, file.Name), true);
                        this.logger.Log(" o " + file.Name);
                        this.filesCopied++;
                    }
                    catch (Exception)
                    {
                        this.logger.Log(" x " + file.Name + " (COPY FAILED)");
                        this.copyFailures++;
                    }
                }
            }

            this.logger.Unindent();
        }

        private IList<FileInfo> GetFiles(string folder)
        {
            return Directory.GetFiles(folder)
                .Where(file => Regex.IsMatch(file, @"."))
                .Select(file => new FileInfo(file))
                .ToList();
        }
    }
}