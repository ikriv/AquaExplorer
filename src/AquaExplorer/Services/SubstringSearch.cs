using System;

namespace AquaExplorer.Services
{
    static class SubstringSearch
    {
        public static int IndexOf(string text, string toFind)
        {
            if (String.IsNullOrEmpty(toFind)) return -1;
            if (text == null) return -1;
            return text.IndexOf(toFind, StringComparison.CurrentCultureIgnoreCase);
        }

        public static bool HasMatch(string text, string toFind)
        {
            return String.IsNullOrEmpty(toFind) || IndexOf(text, toFind) >= 0;
        }

    }
}
