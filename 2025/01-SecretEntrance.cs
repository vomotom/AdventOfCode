using System.Text.RegularExpressions;

var part1 = new SafeLock(1, File.ReadAllLines("Inputs/01.txt").ToList());
var part2 = new SafeLock(2, File.ReadAllLines("Inputs/01.txt").ToList());

class SafeLock
{
    private List<string> Rotations { get; init; }
    private int Part { get; init; }
    private int DialState { get; set; } = 50;
    private int ZeroStates { get; set; } = 0;

    public SafeLock(int part, List<string> rotations)
    {
        Part = part;
        Rotations = rotations;
        foreach (string rotation in Rotations) 
        {
            if (Part == 1) Part1Move(rotation);
            else Part2Move(rotation);
        }
        Console.WriteLine($"Part {Part} ZeroStates: {ZeroStates}");
    }


    private void Part2Move(string rotation)
    {  
        if (RotationValid(rotation))
        {
            bool clockwise = rotation.ToLower().Contains("r");
            int steps = Int32.Parse(rotation.Substring(1));
            int step = clockwise ? 1 : -1;

            for (int i = 0; i < steps; i++)
            {
                DialState = (DialState + step + 100) % 100; // moves by one step only
                if (DialState == 0) ZeroStates++;
            }
        }
        else
        {
            throw new ArgumentException($"{rotation} is not a valid rotation");
        }
    }

    private void Part1Move(string rotation)
    {  
        if (RotationValid(rotation))
        {
            bool clockwise = rotation.ToLower().Contains("r");
            int steps = Int32.Parse(rotation.Substring(1));
            int move = clockwise ? steps : -steps;
            int negativeCorrection = clockwise ? 0 : (steps / 100 + 1) * 100;

            DialState = (DialState + move + negativeCorrection) % 100;
            if (DialState == 0) ZeroStates++;
        }
        else
        {
            throw new ArgumentException($"{rotation} is not a valid rotation");
        }
    }

    private bool RotationValid(string rotation)
    {
        string pattern = @"^[LR]\d+$";
        return Regex.IsMatch(rotation.Trim(), pattern, RegexOptions.IgnoreCase);
    }    

    private void PrintState()
    {
       
    }
}

