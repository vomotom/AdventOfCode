using System.Text.RegularExpressions;
var db = new IdDatabase("Inputs/05.txt");
Console.WriteLine($"Fresh ingredients count: {db.CountFreshIngredients()}");
Console.WriteLine($"All possible fresh ids count: {db.CountPossibleFreshIds()}");

class IdDatabase(string inputFilename)
{
    public IReadOnlyList<Range> FreshIdRanges { get; } = LoadRanges(inputFilename);
    public IReadOnlyList<long> IngredientIds { get; } = LoadIds(inputFilename);

    private static readonly Regex rangeRegex = new(@"^\d+-\d+$", RegexOptions.Compiled);
    private static readonly Regex idRegex    = new(@"^\d+$", RegexOptions.Compiled);

    public readonly record struct Range(long Start, long End)
    {
        public long Length => End - Start + 1;

        public bool Contains(long value) =>
            Start <= value && value <= End;

        public bool Overlaps(Range other) =>
            Start <= other.End && End >= other.Start;

        public Range Merge(Range other) =>
            new(Math.Min(Start, other.Start), Math.Max(End, other.End));
    }

    private static List<Range> LoadRanges(string filename) =>
        File.ReadLines(filename)
            .Where(line => rangeRegex.IsMatch(line))
            .Select(line => 
            {
                var parts = line.Split('-');
                return new Range(long.Parse(parts[0]), long.Parse(parts[1]));
            })
            .ToList();

    private static List<long> LoadIds(string filename) =>
        File.ReadLines(filename)
            .Where(line => idRegex.IsMatch(line))
            .Select(long.Parse)
            .ToList();

    public long CountFreshIngredients() =>
        IngredientIds.Count(id => FreshIdRanges.Any(r => r.Contains(id)));

    public long CountPossibleFreshIds()
    {
        var consolidatedRanges = ConsolidateAllRanges();
        return consolidatedRanges.Sum(r => r.Length);
    }

    public List<Range> ConsolidateAllRanges()
    {
        var ranges = new List<Range>(FreshIdRanges);
        if (ranges.Count == 0) return ranges;

        ranges.Sort((x, y) => x.Start.CompareTo(y.Start));
        var result = new List<Range>();
        var current = ranges[0];
        for (var i = 1; i < ranges.Count; i++)
        {
            var next = ranges[i];
            if (current.Overlaps(next))
            {
                current = current.Merge(next);
            }
            else
            {
                result.Add(current);
                current = next;
            }
        }
        result.Add(current);
        return result;
    }
}