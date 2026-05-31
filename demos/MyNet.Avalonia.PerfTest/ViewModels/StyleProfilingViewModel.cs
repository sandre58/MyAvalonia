using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia;
using Avalonia.Markup.Xaml.Styling;
using Avalonia.Styling;

namespace MyNet.Avalonia.PerfTest.ViewModels;

public class StyleProfilingViewModel : ViewModelBase
{
    private string _status = "";
    private string _report = "";

    public StyleProfilingViewModel()
    {
        ProfileStylesCommand = new RelayCommand(async () => await ProfileStylesAsync());
        Results = new();
    }

    public ObservableCollection<StyleProfileResult> Results { get; }

    public string Status
    {
        get => _status;
        set => SetProperty(ref _status, value);
    }

    public string Report
    {
        get => _report;
        set => SetProperty(ref _report, value);
    }

    public ICommand ProfileStylesCommand { get; }

    private async Task ProfileStylesAsync()
    {
        Results.Clear();
        Status = "Profiling des styles en cours...";

        await Task.Delay(100); // let UI update

        var app = Application.Current;
        if (app is null) return;

        // Profile each top-level style by counting the total number of child styles/selectors
        var sb = new System.Text.StringBuilder();
        sb.AppendLine("========== STYLE PROFILING REPORT ==========");
        sb.AppendLine();

        var totalStyles = 0;

        foreach (var style in app.Styles)
        {
            var name = style.GetType().Name;
            var count = CountStyles(style);
            totalStyles += count;

            var result = new StyleProfileResult
            {
                Name = name,
                StyleCount = count,
                Description = style is Styles s ? $"{s.Count} child groups" : style.GetType().FullName ?? ""
            };
            Results.Add(result);

            sb.AppendLine($"[STYLE] {name}: {count} styles/selectors");
            Debug.WriteLine($"[STYLE-PROFILE] {name}: {count} styles/selectors");
        }

        // Deep dive into IStyleHost resources
        sb.AppendLine();
        sb.AppendLine($"Total styles/selectors: {totalStyles}");
        sb.AppendLine();

        // Analyze the MyTheme specifically
        var myTheme = app.Styles.OfType<Styles>().FirstOrDefault(s => s.GetType().Name == "MyTheme");
        if (myTheme is not null)
        {
            sb.AppendLine("--- MyTheme breakdown ---");
            AnalyzeStylesRecursive(myTheme, sb, 0);
        }

        sb.AppendLine();
        sb.AppendLine("========== KEY FINDINGS ==========");
        sb.AppendLine();
        sb.AppendLine("P0 optimizations applied:");
        sb.AppendLine("1. Theme markup uses Binding (not ReflectionBinding) via ThemeBindingHelper");
        sb.AppendLine("2. UseRegisteredClasses is lazy (no global :is(Control) style)");
        sb.AppendLine("3. Compare 'List (1000)' vs 'Theme List (1000)' for theme binding cost");
        sb.AppendLine();
        sb.AppendLine("Remaining hotspots to watch:");
        sb.AppendLine("- TextBlock.has-role still uses ThemeRole MultiBinding per control");
        sb.AppendLine("- Variant styles still apply when variant-* classes are present");
        sb.AppendLine("- Foreground ancestor binding on has-role TextBlocks in large lists");
        sb.AppendLine();
        sb.AppendLine("==========================================");

        Report = sb.ToString();
        Status = "Profiling terminé";

        Debug.WriteLine(sb.ToString());
    }

    private static int CountStyles(IStyle style)
    {
        var count = 0;
        if (style is Styles styles)
        {
            count += styles.Count;
            foreach (var child in styles)
                count += CountStyles(child);
        }
        else if (style is Style s)
        {
            count = 1;
            if (s.Children.Count > 0)
            {
                foreach (var child in s.Children)
                    count += CountStyles(child);
            }
        }
        return count;
    }

    private static void AnalyzeStylesRecursive(IStyle style, System.Text.StringBuilder sb, int depth)
    {
        var indent = new string(' ', depth * 2);

        if (style is Styles styles)
        {
            foreach (var child in styles)
            {
                if (child is StyleInclude inc)
                {
                    var childCount = CountStyles(child);
                    sb.AppendLine($"{indent}StyleInclude ({inc.Source}): {childCount} styles");
                }
                else
                {
                    var typeName = child.GetType().Name;
                    var childCount = CountStyles(child);
                    sb.AppendLine($"{indent}{typeName}: {childCount} styles");
                }

                if (depth < 2)
                    AnalyzeStylesRecursive(child, sb, depth + 1);
            }
        }
        else if (style is StyleInclude inc && inc.Loaded is { } loaded)
        {
            AnalyzeStylesRecursive(loaded, sb, depth);
        }
    }
}

public class StyleProfileResult
{
    public string Name { get; set; } = string.Empty;
    public int StyleCount { get; set; }
    public string Description { get; set; } = string.Empty;

    public override string ToString() => $"{Name}: {StyleCount} styles ({Description})";
}
