var path = "Inputs/06.txt";
var humanCalculator = new Calculator(LoadData(path));
var cephalodCalculator = new Calculator(LoadCephalodData(path));
Console.WriteLine(humanCalculator.SumSolutions());
Console.WriteLine(cephalodCalculator.SumSolutions());

static List<Problem> LoadData(string filePath)
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

static List<Problem> LoadCephalodData(string filePath)
{
    List<Problem> problems = [];
    List<string> rows = [.. File.ReadAllLines(filePath)];
    for (int i = 0; i < rows[^1].Length; i++)
    {
        var operation = rows[^1][i];
        if (operation != ' ') problems.Add(new Problem(operation));
        
        var strNumber = "";
        for (int row = 0; row < rows.Count-1; row++)
        {
            var character = rows[row][i];
            if (character != ' ') strNumber += character;
        }
        if (strNumber != "") problems[^1].Numbers.Add(int.Parse(strNumber));
    }
    return problems;
}

class Problem(char Operation)
{
    public List<int> Numbers = [];
    public char Operation {get; set;}= Operation;

    public long Solve() => Operation == '+' ? Numbers.Sum() : Numbers.Select(n => (long)n).Aggregate((a,b) => a * b);
}

class Calculator(List<Problem> data)
{
    public List<Problem> Problems { get; set; } = data;

    
    public long SumSolutions() => Problems.Sum(p => p.Solve());

}