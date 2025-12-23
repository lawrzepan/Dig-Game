using System;
using System.Collections.Generic;

namespace DigGame.Draw;

/// <summary>
/// describes whether a range is full or empty
/// </summary>
public enum RangeType
{
    Empty,
    Any, // used when the type is unimportant or unknown
    Full
}

/// <summary>
/// Describes a range in indices in a vertex array
/// </summary>
public struct Range(int start, int end, RangeType rangeType)
{
    /// <summary>
    /// the start of the range, inclusive
    /// </summary>
    public int Start { get; set; } = start;

    /// <summary>
    /// the end of the range, inclusive
    /// </summary>
    public int End { get; set; } = end;
    
    public int Length
    {
        get => End - Start + 1; // add one because both sides are inclusive, e.g. the range 0 to 0 has a length of 1
    }

    /// <summary>
    /// whether this range is full of vertices or empty
    /// </summary>
    public RangeType RangeType { get; set; } = rangeType;

    public override string ToString()
    {
        return $"start: {Start}, end: {End}, type: {RangeType}";
    }

    public static bool operator ==(Range range1, Range range2)
    {
        return range1.Start == range2.Start &&
               range1.End == range2.End &&
               range1.RangeType == range2.RangeType;
    }
    
    public static bool operator !=(Range range1, Range range2)
    {
        return !(range1 == range2);
    }
}

public class VertexPager
{
    public List<Range> Ranges;
    public int ArrayLength;

    public VertexPager(int arrayLength)
    {
        Ranges = new List<Range>();
        
        Ranges.Add(new Range(0, arrayLength, RangeType.Empty));
        
        ArrayLength = arrayLength;
    }

    /// <summary>
    /// Returns a possible empty range that can be used to store vertices, use alongside AllocateRange to allocate a
    /// suitable range.
    /// </summary>
    /// <param name="size">The amount of indices that needs to be stored.</param>
    /// <returns>The first suitable range with a length of the given Size, or null if none could be found</returns>
    public Range? FindNextAvailableBlock(int size)
    {
        if (size > ArrayLength) return null;

        for (int i = 0; i < Ranges.Count; i++)
        {
            if (Ranges[i].RangeType == RangeType.Empty && Ranges[i].Length >= size)
            {
                var range = Ranges[i];
                range.End = range.Start + size - 1;
                
                return range;
            }
        }

        return null;
    }

    /// <summary>
    /// Allocates a specific range to store vertices.
    /// </summary>
    /// <param name="range">The range to allocate.</param>
    /// <param name="knownEmptiness">Whether the range is known to be empty, do not set to true unless certain or
    /// unless the range comes from FindNextAvailableBlock.</param>
    /// <returns>A boolean that represents whether this operation was successful.</returns>
    public bool AllocateRange(Range range, bool knownEmptiness)
    {
        if (!knownEmptiness && !IsEmpty(range)) return false; // dont allocate a full range

        int emptyRangeIndex = 0;

        while (Ranges[emptyRangeIndex].Start <= range.Start)
        {
            emptyRangeIndex++;
            if (emptyRangeIndex >= Ranges.Count) break;
        }

        emptyRangeIndex--;

        var emptyRange = Ranges[emptyRangeIndex];
        range.RangeType = RangeType.Full;
        Ranges[emptyRangeIndex] = range;

        // if there is a gap after the end of range, fill it with an empty range
        if (emptyRange.End > range.End)
        {
            Ranges.Insert(emptyRangeIndex + 1, new Range(range.End + 1, emptyRange.End, RangeType.Empty));
        }

        // if there is a gap before start of range, fill with an empty range
        if (emptyRange.Start < range.Start)
        {
            Ranges.Insert(emptyRangeIndex, new Range(emptyRange.Start, range.Start - 1, RangeType.Empty));
        }
        
        /*Console.WriteLine($"after adding range (start: {range.Start}, end: {range.End}), the pager is at:");
        for (int i = 0; i < Ranges.Count; i++)
        {
            Console.WriteLine($"\tstart: {Ranges[i].Start}, end: {Ranges[i].End}, type: {Ranges[i].RangeType.ToString()}");
        }*/

        return true;
    }

    public bool IsEmpty(Range range)
    {
        int i = 0;
        while (Ranges[i].End < range.Start)
        {
            i++;
        }

        while (Ranges[i].Start <= range.End)
        {
            if (Ranges[i].RangeType == RangeType.Full || Ranges[i].RangeType == RangeType.Any) // for this reason, try to not allocate ranges with a RangeType of any
            {
                return false;
            }
        }

        return true;
    }

    public int? GetFromRange(Range range)
    {
        for (int i = 0; i < Ranges.Count; i++)
        {
            if (Ranges[i] == range) return i;
        }

        return null;
    }

    public void MergeEmptyBlocks()
    {
        for (int i = Ranges.Count - 2; i >= 0; i--)
        {
            if (Ranges[i].RangeType == RangeType.Empty && Ranges[i + 1].RangeType == RangeType.Empty)
            {
                var range = Ranges[i];
                range.End = Ranges[i + 1].End;
                Ranges[i] = range;
                Ranges.RemoveAt(i + 1);
            }
        }
    }
}