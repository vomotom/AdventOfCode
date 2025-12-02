using System.Text.RegularExpressions;

var part1 = new InvalidIdAdder(1, File.ReadAllText("02-Input.txt"));
var part2 = new InvalidIdAdder(2, File.ReadAllText("02-Input.txt"));

class InvalidIdAdder
{
    public string[] Ranges { get; init; }
    public List<long> InvalidIds { get; set; } = new List<long>();
    public int Part { get; set; }

    public InvalidIdAdder(int part, string input)
    {
        Part = part;
        Ranges = input.Split(',');

        foreach(string range in Ranges) FindInvalidIds(range.Trim());
        Console.WriteLine($"Part {Part} Result: {InvalidIds.Sum()}");
    }

    public bool RangeValid(string range)
    {
        string pattern = @"\A\d+-\d+\Z";
        return Regex.IsMatch(range.Trim(), pattern);
    }

    public bool Part1IdValid(long id)
    {
        var idString = id.ToString();
        var length = idString.Length;
        if (length % 2 != 0) return true;

        var firstHalf = idString.Substring(0, length / 2);
        var secondHalf = idString.Substring(length / 2);
        return firstHalf != secondHalf;
    }

    public bool Part2IdValid(long id)
    {
        var idString = id.ToString();
        var length = idString.Length;

        // checks all relevant variants of spliting the string evenly - by factors of the string's length
        // f.e. string length is 12 -> checks splitting to 2, 3, 4, 6 and 12 parts
        foreach (int factor in LengthFactors(length))
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

    public List<int> LengthFactors(int length)
    {
        var factors = new List<int>();
        for (int factor = 2; factor <= length; factor++)
        {
            if (length % factor == 0) factors.Add(factor);
        }
        return factors;
    }

    public void FindInvalidIds(string range)
    {
        if (RangeValid(range))
        {
            string[] ranges = range.Split('-');
            var floor = Int64.Parse(ranges[0]);
            var ceilling = Int64.Parse(ranges[1]);

            for (long id = floor; id <= ceilling; id++)
            {
                var idValid = (Part == 1) ? !Part1IdValid(id) : !Part2IdValid(id);
                if (idValid) InvalidIds.Add(id);
            }
        }
        else
        {
            throw new ArgumentException($"{range} is not a valid range");
        }
    }

}