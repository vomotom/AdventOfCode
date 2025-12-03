var field = new BatteryField(LoadBanks("Inputs/03.txt"));
Console.WriteLine($"Total output using 2 batteries: {field.TotalOutput(2)}");
Console.WriteLine($"Total output using 12 batteries: {field.TotalOutput(12)}");

static IEnumerable<BatteryBank> LoadBanks(string filePath) =>
    File.ReadLines(filePath)
        .Select(row => new BatteryBank(row.Select(c => c - '0')));

class BatteryField(IEnumerable<BatteryBank> banks)
{
    public List<BatteryBank> Banks { get; } = banks.ToList();

    public long TotalOutput(int usedBatteryCount) =>
        Banks.Sum(x => x.BestOutput(usedBatteryCount));
}

class BatteryBank(IEnumerable<int> batteries)
{
    public List<int> Batteries { get; } = batteries.ToList();

    public long BestOutput(int usedBatteryCount)
    {
        long output = 0;
        var lastBatteryIndex = -1;
        for (int i = 0; i < usedBatteryCount; i++)
        {
            var rangeStart = lastBatteryIndex + 1;
            var rangeEnd = Batteries.Count - usedBatteryCount + 1 + i;
            var bestBattery = Batteries[rangeStart..rangeEnd]
                // "index = index + rangeStart" matches index with original list
                .Select((value, index) => new { value, index = index + rangeStart}) 
                .MaxBy(b => b.value);
            lastBatteryIndex = bestBattery.index; 
            output = output * 10 + bestBattery.value;
        }
        // Console.WriteLine($"Bank: {string.Join("", Batteries)}, Output: {output}");
        return output;
    }
}