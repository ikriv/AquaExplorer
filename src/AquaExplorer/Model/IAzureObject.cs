using System.Collections.Generic;

namespace AquaExplorer.Model
{
    interface IAzureObject
    {
        IAzureObject Parent { get; }
        AzureObjectKind Kind { get; }
        string Name { get; }
        bool CanHaveChildren { get; }
        IEnumerable<IAzureObject> GetChildren();
    }
}
