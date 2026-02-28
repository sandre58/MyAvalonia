using System;
using System.Collections.ObjectModel;

namespace MyNet.Avalonia.PerfTest.ViewModels;

public class DataGridViewModel : ViewModelBase
{
    public ObservableCollection<DataItem> Items { get; } = new();

    public DataGridViewModel()
    {
        // Generate 500 items for performance testing
        var random = new Random(42);
        for (int i = 0; i < 500; i++)
        {
            Items.Add(new DataItem
            {
                Id = i + 1,
                Name = $"Item {i + 1}",
                Description = $"Description for item {i + 1} with some additional text to make it longer",
                Value = random.Next(0, 10000),
                Percentage = random.NextDouble() * 100,
                Date = DateTime.Now.AddDays(-random.Next(0, 365)),
                IsActive = random.Next(0, 2) == 1,
                Category = GetRandomCategory(random),
                Status = GetRandomStatus(random),
                Tags = $"Tag{random.Next(1, 5)}, Tag{random.Next(5, 10)}"
            });
        }
    }

    private string GetRandomCategory(Random random)
    {
        var categories = new[] { "Category A", "Category B", "Category C", "Category D", "Category E" };
        return categories[random.Next(categories.Length)];
    }

    private string GetRandomStatus(Random random)
    {
        var statuses = new[] { "Active", "Pending", "Completed", "Cancelled", "On Hold" };
        return statuses[random.Next(statuses.Length)];
    }
}

public class DataItem
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Value { get; set; }
    public double Percentage { get; set; }
    public string PercentageFormatted => $"{Percentage:F2}%";
    public DateTime Date { get; set; }
    public bool IsActive { get; set; }
    public string Category { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Tags { get; set; } = string.Empty;
}
