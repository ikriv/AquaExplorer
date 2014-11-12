using System;
using System.Collections.Generic;

namespace IKriv.Azure
{
    /// <summary>
    /// Converts multiple batches of objects into one long enumerable
    /// </summary>
    internal class Batch
    {
        public class NextMarker
        {
            public string Value { get; set; }
        }

        public static IEnumerable<T> Fold<T>(Func<NextMarker, IEnumerable<T>> getBatch)
        {
            var marker = new NextMarker();
            while (true)
            {
                var batch = getBatch(marker);
                if (batch == null) yield break;

                foreach (var item in batch) yield return item;
                if (String.IsNullOrEmpty(marker.Value)) yield break;
            }
        }
    }
}
