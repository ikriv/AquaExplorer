using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AquaExplorer.BusinessObjects;

namespace AquaExplorer.Model
{
    class AzureObjectAccount : IAzureObject
    {
        private readonly IAzureObject _parent;
        private readonly Account _account;

        public AzureObjectAccount(IAzureObject parent, Account account)
        {
            _parent = parent;
            _account = account;
        }

        public IAzureObject Parent
        {
            get { return _parent; }
        }

        public AzureObjectKind Kind
        {
            get { return AzureObjectKind.Account; }
        }

        public string Name
        {
            get { return _account.Credentials.Account; }
        }

        public bool CanHaveChildren
        {
            get { return true; }
        }

        public IEnumerable<IAzureObject> GetChildren()
        {
            throw new NotImplementedException();
        }
    }
}
