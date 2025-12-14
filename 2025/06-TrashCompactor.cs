var calculator = new Calculator("Inputs/06.txt");
Console.WriteLine(calculator.SumSolutions());

class Calculator(string filePath)
{
    public List<Problem> Problems { get; set; } = LoadData(filePath);

    
    public class Problem(char Operation)
    {
        public List<int> Numbers = [];
        public char Operation {get; set;}= Operation;

        public long Solve() => Operation == '+' ? Numbers.Sum() : Numbers.Select(n => (long)n).Aggregate((a,b) => a * b);
    }
    
    public static List<Problem> LoadData(string filePath)
    {
        var rows = File.ReadAllLines(filePath).Select(line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries)).ToList();
        List<Problem> problems = [.. rows[^1].Select(p => new Problem(char.Parse(p)))];
        var problemsCount = problems.Count;
        for (int i = 0; i < problemsCount; i++)
        {
            problems[i].Numbers = [.. rows[..^1].Select(row => int.Parse(row[i]))];
        }
        return problems;
    }

    public long SumSolutions() => Problems.Sum(p => p.Solve());

}