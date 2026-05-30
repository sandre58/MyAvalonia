using System;
using System.Collections.ObjectModel;

namespace MyNet.Avalonia.PerfTest.ViewModels;

public class ListViewModel : ViewModelBase
{
    public ObservableCollection<ListItem> Items { get; } = new();

    public ListViewModel()
    {
        var random = new Random(42);
        
        // Generate 1000 items for stress testing
        for (int i = 0; i < 1000; i++)
        {
            Items.Add(new()
            {
                Id = i + 1,
                Title = $"List Item {i + 1}",
                Subtitle = $"Subtitle for item {i + 1}",
                Description = $"This is a longer description for item {i + 1} to test text rendering performance",
                ImageUrl = $"https://picsum.photos/seed/{i}/48/48",
                Rating = random.Next(1, 6),
                Price = random.Next(10, 1000) + 0.99m,
                InStock = random.Next(0, 2) == 1
            });
        }
    }
}

public class ListItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Subtitle { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public int Rating { get; set; }
    public decimal Price { get; set; }
    public bool InStock { get; set; }
}
