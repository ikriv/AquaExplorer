using System;

namespace AquaExplorer.Services
{
    interface ITimeService
    {
        DateTime GetUtcNow();
    }
}
