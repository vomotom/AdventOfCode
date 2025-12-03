using System;

var analyzer = new ReportAnalyzer(File.ReadAllLines("Inputs/02.txt").ToList());
Console.WriteLine($"Part 2 Safe Reports (using dampener): {analyzer.ReportsSafe(true)}");
Console.WriteLine($"Part 1 Safe Reports: {analyzer.ReportsSafe(false)}");


class ReportAnalyzer
{
    public List<Report> Reports { get; init; }

    public ReportAnalyzer(List<string> inputRows)
    {
        Reports = inputRows
            .Select(row => new Report(row.Split().Select(int.Parse).ToList()))
            .ToList();
    }

    public int ReportsSafe(bool dampener)
    {
        var safeReports = 0;
        foreach (Report report in Reports)
        {
            if (report.IsSafe(dampener)) safeReports++;
        }
        return safeReports;
    }   
}

public class Report(List<int> levels)
{
    public List<int> Levels { get; init; } = levels;

    public bool IsSafe(bool dampener)
    {
        var levelsSafe = true;
        for (int i = 1; i < Levels.Count; i++)
        {
            
            var difference = Math.Abs(Levels[i] - Levels[i-1]);
            var differenceCritical = difference > 3 || difference == 0;
            var directionDifference =  Levels[i-1] > Levels[i] != Levels[0] > Levels[1];
            if (differenceCritical || directionDifference) 
            {
                levelsSafe = false;
                break;
            }
        }
        if (levelsSafe)
        {
            return true;
        }
        else if (dampener)
        {
            for (int i = 0; i < Levels.Count; i++)
            {
                var levelRemoved = new Report(new List<int>(Levels));
                levelRemoved.Levels.RemoveAt(i);
                if (levelRemoved.IsSafe(false)) 
                {
                    return true;
                }
            }                    
            return false;
        }
        else
        {
            return false;
        }

    }
} 