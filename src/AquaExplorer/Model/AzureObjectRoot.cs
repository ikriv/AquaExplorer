using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AquaExplorer.Model
{
    class AzureObjectRoot : IAzureObject
    {
        public IAzureObject Parent
        {
            get { return null; }
        }

        public AzureObjectKind Kind
        {
            get { return AzureObjectKind.Root; }
        }

        public string Name
        {
            get { return "/"; }
        }

        bool CanHaveChildren 
        { 
            get { return true; } 
        }

        public IEnumerable<IAzureObject> GetChildren()
        {
            throw new NotImplementedException();
        }
    }
}
