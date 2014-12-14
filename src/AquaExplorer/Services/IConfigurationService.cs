using System;
using AquaExplorer.BusinessObjects;

namespace AquaExplorer.Services
{
    interface IConfigurationService
    {
        Configuration ReadConfiguration();
        void WriteConfiguration(Configuration config);
        event Action<Configuration> ConfigurationChanged;
    }
}
