
using System;

namespace AquaExplorer.Services
{
    class SizeAbbreviationService
    {
        public string Abbreviate(long size)
        {
            return AbbreviateImpl(size).ToString();
        }

        public string Abbreviate(long part, long whole)
        {
            var partSize = AbbreviateImpl(part);
            var wholeSize = AbbreviateImpl(whole);

            if (partSize.Unit == wholeSize.Unit)
            {
                return String.Format("{0} of {1}", partSize.Value, wholeSize); // 1.47 of 3.29 MB
            }
            else
            {
                return String.Format("{0} of {1}", partSize, wholeSize);
            }
        }

        private struct AbbreviatedSize
        {
            private readonly string _value;
            private readonly string _unit;

            private AbbreviatedSize(string value, string unit)
            {
                _value = value;
                _unit = unit;
            }

            public static AbbreviatedSize Make(string value, string unit)
            {
                return new AbbreviatedSize(value, unit);
            }

            public static AbbreviatedSize Make(double value, string unit)
            {
                return new AbbreviatedSize(FormatNumber(value), unit);
            }

            public string Value { get { return _value; } }
            public string Unit { get { return _unit; } }

            public override string ToString()
            {
                return Value + " " + Unit;
            }
        }

        private AbbreviatedSize AbbreviateImpl(long size)
        {
            if (size == 1) return AbbreviatedSize.Make("1", "byte");
            if (size < 1000) return AbbreviatedSize.Make(size.ToString(), "bytes");

            double kb = size/1024.0;
            if (kb < 999.5) return AbbreviatedSize.Make(kb, "KB");

            double mb = size/1048576.0;
            if (mb < 999.5) return AbbreviatedSize.Make(mb, "MB");

            double gb = size/1073741824.0;
            if (gb < 999.5) return AbbreviatedSize.Make(gb, "GB");

            double tb = size/1099511627776.0;
            return AbbreviatedSize.Make(tb, "TB");

        }

        private static string FormatNumber(double n)
        {
            string format;
            if (n < 10.0) format = "0.00";
            else if (n < 100.0) format = "0.0";
            else format = "0";

            return n.ToString(format);
        }
    }
}
