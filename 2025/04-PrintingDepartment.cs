var stock = new PaperStock(LoadStock("Inputs/04.txt"));
Console.WriteLine($"Part1 - Paper rolls accessible: {stock.AllAccessibleCoords().Count}");
Console.WriteLine($"Part2 - Paper rolls removed: {stock.IterateRemoval()}");

static char[][] LoadStock(string fileName) =>
    File.ReadLines(fileName)
        .Select(line => line.ToCharArray())
        .ToArray();

class PaperStock(char[][] grid)
{
    public char[][] PaperGrid { get; } = grid;

    public List<(int, int)> AllAccessibleCoords()
    {
        var accessibleCoords = new List<(int, int)>();
        for (int y = 0; y < PaperGrid.Length; y++)
        {
            for (int x = 0; x < PaperGrid[y].Length; x++)
            {
                if (PaperRollAccessible(x, y))
                    accessibleCoords.Add((x, y));
            }
        }
        return accessibleCoords;
    }

    public int IterateRemoval()
    {
        var removedTotal = 0;
        while (true)
        {
            var accessibles = AllAccessibleCoords();
            if (accessibles.Count == 0)
                return removedTotal;

            RemoveAccessibles(accessibles);
            removedTotal += accessibles.Count;
        }
    }

    public void RemoveAccessibles(List<(int, int)> coords)
    {
        foreach((int x, int y) in coords) PaperGrid[y][x] = '.';
    }

    public bool PaperRollAccessible(int x, int y) =>
        PaperGrid[y][x] == '@' && CountAdjacentRolls(x, y) < 4;

    public int CountAdjacentRolls(int x, int y)
    {
        var gridWidth = PaperGrid[y].Length;
        var gridHeight = PaperGrid.Length;
        var rollCount = 0;
        var adjacentCoords = new[] 
        { 
            ( x - 1, y - 1 ),   // top left
            ( x, y - 1),        // top
            ( x + 1, y - 1 ),   // top right
            ( x - 1, y),        // left
            ( x + 1, y),        // right
            ( x - 1, y + 1 ),   // bottom left
            ( x, y + 1 ),       // bottom
            ( x + 1, y + 1 ),   // bottom right
        };
        foreach(var (xAdj, yAdj) in adjacentCoords)
        {
            var outOfBounds = 
                xAdj < 0 || xAdj >= gridWidth || 
                yAdj < 0 || yAdj >= gridHeight;

            if (!outOfBounds && PaperGrid[yAdj][xAdj] == '@')
                rollCount++;
        }
        return rollCount;
    }
}