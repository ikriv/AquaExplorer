using System;

namespace IKriv.Azure
{
    public class Blob
    {
        internal Blob(Container container)
        {
            if (container == null) throw new ArgumentNullException("container");
            Container = container;
        }

        public Container Container { get; internal set; }
        public string Name { get; internal set; }
        public string Url { get; internal set; }
        public DateTime LastModified {  get; internal set; }
        public long ContentLength { get; internal set; }
        public string ContentType { get; internal set; }
    }
}
