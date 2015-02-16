using System;
using System.Collections.Generic;

namespace AquaExplorer.Util
{
    internal class StringHighlighter
    {
        public static IEnumerable<Segment> GetSegments(string text, string toHighlight)
        {
            if (String.IsNullOrEmpty(text)) yield break;

            if (String.IsNullOrEmpty(toHighlight))
            {
                yield return new Segment {Start = 0, End = text.Length, IsHighlighted = false};
                yield break;
            }

            var currentSegment = new Segment {Start = 0, End = 0, IsHighlighted = false};

            while (true)
            { 
                if (currentSegment.End >= text.Length)
                {
                    yield return currentSegment;
                    yield break;
                }

                int nextHighlight = text.IndexOf(toHighlight, currentSegment.End, StringComparison.CurrentCultureIgnoreCase);

                if (currentSegment.IsHighlighted)
                {
                    if (nextHighlight == currentSegment.End) // two adjacent highlighted segments: merge them
                    {
                        currentSegment.End += toHighlight.Length;
                    }
                    else
                    {
                        yield return currentSegment;

                        int endOfPlainSegment = (nextHighlight != -1) ? nextHighlight : text.Length;

                        currentSegment = new Segment
                        {
                            Start = currentSegment.End,
                            End = endOfPlainSegment,
                            IsHighlighted = false
                        };

                        yield return currentSegment;

                        if (nextHighlight == -1)
                        {
                            yield break;
                        }

                        currentSegment = new Segment
                        {
                            Start = nextHighlight,
                            End = nextHighlight + toHighlight.Length,
                            IsHighlighted = true
                        };
                    }
                }
                else
                {
                    // current segment is not highlighted
                    int end = (nextHighlight != -1) ? nextHighlight : text.Length;
                    currentSegment.End = end;
                    if (currentSegment.End != currentSegment.Start)
                    {
                        yield return currentSegment;
                    }

                    if (nextHighlight != -1)
                    {
                        currentSegment = new Segment
                        {
                            Start = nextHighlight,
                            End = nextHighlight + toHighlight.Length,
                            IsHighlighted = true
                        };
                    }
                    else
                    {
                        yield break; // reached end fo text
                    }
                }
            }
        }

        public class Segment
        {
            public int Start; 
            public int End;
            public bool IsHighlighted;

            public override bool Equals(object obj)
            {
                var other = obj as Segment;
                if (other == null) return false;
                return other.Start == this.Start && other.End == this.End && other.IsHighlighted == this.IsHighlighted;
            }

            public override int GetHashCode()
            {
                return 42; // this satisfies the rules, but dictionaries of with keys of type Segment will not be very efficient; 
                           // this is alright since we don't really create any dictionaries with keys of type Segment
            }

            public override string ToString()
            {
                return String.Format("({0}, {1}, {2})", Start, End, IsHighlighted);
            }
        }

    }
}
