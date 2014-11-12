using System;

namespace AquaExplorer.Services
{
    class TimeService : ITimeService
    {
        public DateTime GetUtcNow()
        {
            return DateTime.UtcNow;
        }
    }
}
