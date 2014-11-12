using IKriv.Azure;
namespace AquaExplorer.BusinessObjects
{
    class AzureLocation
    {
        public AzureLocation(Account account, Container container)
        {
            Account = account;
            Container = container;
        }

        public Account Account { get; private set; }
        public Container Container {  get; private set; }

        public static AzureLocation Root = new AzureLocation(null, null);

        public string Name
        {
            get { return ContainerName ?? AccountName; } 
        }

        public string AccountName
        {
            get {  return (Account != null) ? Account.Credentials.Account : null;  }
        }

        public string ContainerName
        {
            get {  return (Container != null) ? Container.Name : null; }
        }

        public AzureLocation GetParent()
        {
            if (Container != null) return new AzureLocation(Account, null);
            if (Account != null) return Root;
            return null; // root has no parent
        }
    }
}
