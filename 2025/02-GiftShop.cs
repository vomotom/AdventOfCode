var giftshop = new GiftShop(LoadRanges("Inputs/02.txt"));
Console.WriteLine($"InvalidIdSum (check for repeats of two): {giftshop.SumInvalidIds(RepeatCheckMode.HalvesOnly)}");
Console.WriteLine($"InvalidIdSum (check all possible repeats): {giftshop.SumInvalidIds(RepeatCheckMode.AllFactors)}");

static IEnumerable<IdRange> LoadRanges(string filePath) =>
    File.ReadAllText(filePath).Split(",")
        .Select(row => new IdRange(
            long.Parse(row.Split("-")[0]), 
            long.Parse(row.Split("-")[1])
            ));


class IdRange(long start, long end)
{
    public long Start { get; } = start;
    public long End { get; } = end;

    public long SumInvalidIds(RepeatCheckMode mode=RepeatCheckMode.AllFactors)
    {
        var sum = 0L;
        for (long id = Start; id <= End; id++)
        {
            if (!IdValid(id, mode)) sum += id;
        }
        return sum;      
    }

    public bool IdValid(long id, RepeatCheckMode mode=RepeatCheckMode.AllFactors)
    {
        var idString = id.ToString();
        var length = idString.Length;
        if (mode == RepeatCheckMode.HalvesOnly && length % 2 != 0) return true;
        var factors = (mode == RepeatCheckMode.HalvesOnly) ? new List<int> { 2 } : LengthFactors(length);

        // checks all relevant variants of spliting the string evenly - by factors of the string's length
        // f.e. string length is 12 -> checks splitting to 2, 3, 4, 6 and 12 parts
        foreach (int factor in factors)
        {
            var parts = new List<string>();
            for (int i = 0; i < factor; i++)
            {
                var substringStart = i * length / factor;
                var substringLength = length / factor;
                var part = idString.Substring(substringStart, substringLength);
                parts.Add(part);
            }
            // checks if there are 'not' any parts that do not match the first part
            // -> so if all parts are the same return false
            if (!parts.Any(o => o != parts[0])) return false; 
            
        }
        return true;
    }

    public IEnumerable<int> LengthFactors(int length)
    {
        var factors = new List<int>();
        for (int factor = 2; factor <= length; factor++)
        {
            if (length % factor == 0) factors.Add(factor);
        }
        return factors;
    }
}

class GiftShop(IEnumerable<IdRange> ranges)
{
    public List<IdRange> Ranges { get; } = ranges.ToList();

    public long SumInvalidIds(RepeatCheckMode mode=RepeatCheckMode.AllFactors) =>
        Ranges.Sum(range => range.SumInvalidIds(mode));
}

enum RepeatCheckMode
{
    HalvesOnly,
    AllFactors
}