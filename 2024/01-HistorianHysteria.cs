using System;

var comparator = new ListsComparator(File.ReadAllLines("Inputs/01.txt").ToList());
Console.WriteLine($"Part 1 Differences: {comparator.CountDifferences()}");
Console.WriteLine($"Part 2 Similarities: {comparator.CountSimilarities()}");


class ListsComparator
{
    public List<string> InputRows { get; init; }
    public List<int> LeftList { get; set; } = new List<int>();
    public List<int> RightList { get; set; } = new List<int>();

    public ListsComparator(List<string> input)
    {
        InputRows = input;
        foreach (string row in InputRows) ProcessRow(row);
    }

    public void ProcessRow(string row)
    {
        string[] numbers = row.Split("   ");
        int leftNumber = Int32.Parse(numbers[0]);
        int rightNumber = Int32.Parse(numbers[1]);
        LeftList.Add(leftNumber);
        RightList.Add(rightNumber);
    }

    public int CountDifferences()
    {
        var differences = new List<int>();
        var SortedLeftList = new List<int>(LeftList);
        var SortedRightList = new List<int>(RightList);
        SortedLeftList.Sort();
        SortedRightList.Sort();

        for (int i = 0; i < InputRows.Count; i++)
        {
            int difference = Math.Abs(SortedLeftList[i] - SortedRightList[i]);
            differences.Add(difference);
        }
        return differences.Sum();      
    }

    public int CountSimilarities()
    {
        var totalCount = 0;
        foreach (int number in LeftList)
        {
            var count = RightList.Count(i => i == number);
            totalCount += number * count;
        }
        return totalCount;
    }
}