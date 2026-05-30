using System.Collections.ObjectModel;

namespace MyNet.Avalonia.PerfTest.ViewModels;

public class ComplexLayoutViewModel : ViewModelBase
{
    public ObservableCollection<Section> Sections { get; } = new();

    public ComplexLayoutViewModel()
    {
        // Generate multiple sections with nested items
        for (int i = 0; i < 20; i++)
        {
            var section = new Section
            {
                Title = $"Section {i + 1}",
                Description = $"This is a complex section with multiple nested elements"
            };

            for (int j = 0; j < 15; j++)
            {
                section.Items.Add(new()
                {
                    Title = $"Item {j + 1}",
                    Content = $"Content for item {j + 1} in section {i + 1}",
                    Icon = j % 2 == 0 ? "📄" : "📁",
                    Color = GetColor(j % 5)
                });
            }

            Sections.Add(section);
        }
    }

    private string GetColor(int index)
    {
        var colors = new[] { "#FF0000", "#00FF00", "#0000FF", "#FFFF00", "#FF00FF" };
        return colors[index];
    }
}

public class Section
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ObservableCollection<SectionItem> Items { get; } = new();
}

public class SectionItem
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
}
