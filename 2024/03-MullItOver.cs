using System.Text.RegularExpressions;

var calc = new Calculator(File.ReadAllText("Inputs/03.txt"));
Console.WriteLine(calc.RunAll());

class Calculator(string input)
{
    public IReadOnlyList<string> Instructions { get; } = ParseInstructions(input).ToList();

    private static readonly Regex InstructionRegex = 
        new(@"mul\(\d{1,3},\d{1,3}\)|do\(\)|don't\(\)", RegexOptions.Compiled);
    
    private static readonly Regex MulRegex = 
        new(@"mul\((\d{1,3}),(\d{1,3})\)", RegexOptions.Compiled); // uses groups !!

    static IEnumerable<string> ParseInstructions(string input) => 
        InstructionRegex.Matches(input)
                        .Cast<Match>()
                        .Select(m => m.Value);  
        
    public int RunAll()
    {
        var doEnabled = true;
        var sum = 0;

        foreach (var instruction in Instructions)
        {
            switch (instruction)
            {
                case "do()":
                    doEnabled = true;
                    continue;

                case "don't()":
                    doEnabled = false;
                    continue;
            }
            if (!doEnabled) continue;

            var match = MulRegex.Match(instruction);
            var a = int.Parse(match.Groups[1].Value);
            var b = int.Parse(match.Groups[2].Value);
            sum += a * b;
        }

        return sum;
    }    
}