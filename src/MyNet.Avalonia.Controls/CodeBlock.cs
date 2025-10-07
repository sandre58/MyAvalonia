// -----------------------------------------------------------------------
// <copyright file="CodeBlock.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Documents;
using Avalonia.Controls.Primitives;
using Avalonia.Media;

namespace MyNet.Avalonia.Controls;

/// <summary>
/// Formats and display a fragment of the source code.
/// </summary>
[ToolboxItem(true)]
public class CodeBlock : ContentControl
{
    private const string EndlinePattern = /* language=regex */ "(\n)";

    private const string TabPattern = /* language=regex */ "(\t)";

    private const string QuotePattern = /* language=regex */ "(\"(?:\\\"|[^\"])*\")|('(?:\\'|[^'])*')";

    private const string CommentPattern = /* language=regex */ @"(\/\/.*?(?:\n|$)|\/\*.*?\*\/)";

    private const string TagPattern = /* language=regex */ @"(<\/?)([a-zA-Z\-:]+)(.*?)(\/?>)";

    private const string BracePattern = /* language=regex */ @"(\{.*?\})";

    private const string EntityPattern = /* language=regex */ "(&[a-zA-Z0-9#]+;)";

    private enum CodeType
    {
        Unknown,

        Space,

        Comment,

        Tag,

        Quote,

        AttributeValue,

        AttributeKey,

        Brace,

        Entity
    }

    static CodeBlock() => ContentProperty.Changed.AddClassHandler<CodeBlock, object?>((x, _) => x.Refresh());

    public CodeBlock() => ActualThemeVariantChanged += (_, _) => Refresh();

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        Refresh();
    }

    public virtual void Refresh() => Presenter?.Content = CreateFormattedTextBlock(Clean(Content as string ?? string.Empty));

    private static string Clean(string code)
    {
        code = code.Replace(@"\n", "\n", StringComparison.OrdinalIgnoreCase);
        code = code.Replace(@"\t", "\t", StringComparison.OrdinalIgnoreCase);
        code = code.Replace("&lt;", "<", StringComparison.OrdinalIgnoreCase);
        code = code.Replace("&gt;", ">", StringComparison.OrdinalIgnoreCase);
        code = code.Replace("&amp;", "&", StringComparison.OrdinalIgnoreCase);
        code = code.Replace("&quot;", "\"", StringComparison.OrdinalIgnoreCase);
        code = code.Replace("&apos;", "'", StringComparison.OrdinalIgnoreCase);

        return code;
    }

    private SelectableTextBlock CreateFormattedTextBlock(string code)
    {
        var returnText = new SelectableTextBlock { TextWrapping = TextWrapping.Wrap, IsTabStop = false };

        var pattern = string.Empty;
        pattern += EndlinePattern;
        pattern += "|" + TabPattern;
        pattern += "|" + QuotePattern;
        pattern += "|" + CommentPattern;
        pattern += "|" + EntityPattern;
        pattern += "|" + BracePattern;
        pattern += "|" + TagPattern;

        Regex rgx = new(pattern);

        foreach (var match in rgx.Matches(code).OfType<Match>())
        {
            foreach (var group in match.Groups)
            {
                // Remove whole matches
                if (group is Match)
                    continue;

                // Cast to group
                var codeMatched = (Group)group;

                // Remove empty groups
                if (string.IsNullOrEmpty(codeMatched.Value))
                    continue;

                if (codeMatched.Value.Contains('\t', StringComparison.OrdinalIgnoreCase))
                {
                    returnText.Inlines?.Add(Line("  ", CodeType.Space));
                }
                else if (codeMatched.Value.Contains("/*", StringComparison.OrdinalIgnoreCase) || codeMatched.Value.Contains("//", StringComparison.OrdinalIgnoreCase))
                {
                    returnText.Inlines?.Add(Line(codeMatched.Value, CodeType.Comment));
                }
                else if (codeMatched.Value.Contains('<', StringComparison.OrdinalIgnoreCase) || codeMatched.Value.Contains('>', StringComparison.OrdinalIgnoreCase))
                {
                    returnText.Inlines?.Add(Line(codeMatched.Value, CodeType.Tag));
                }
                else if (codeMatched.Value.Contains('"', StringComparison.OrdinalIgnoreCase))
                {
                    var attributeArray = codeMatched.Value.Split('"');
                    attributeArray = [.. attributeArray.Where(x => !string.IsNullOrEmpty(x.Trim()))];

                    if (attributeArray.Length % 2 == 0)
                    {
                        for (var i = 0; i < attributeArray.Length; i += 2)
                        {
                            returnText.Inlines?.Add(Line(attributeArray[i], CodeType.AttributeKey));
                            returnText.Inlines?.Add(Line("\"", CodeType.Quote));
                            returnText.Inlines?.Add(Line(attributeArray[i + 1], attributeArray[i + 1].Contains('{', StringComparison.OrdinalIgnoreCase) ? CodeType.Brace : CodeType.AttributeValue));
                            returnText.Inlines?.Add(Line("\"", CodeType.Quote));
                        }
                    }
                    else
                    {
                        returnText.Inlines?.Add(Line(codeMatched.Value, CodeType.Unknown));
                    }
                }
                else if (codeMatched.Value.Contains('\'', StringComparison.OrdinalIgnoreCase))
                {
                    var attributeArray = codeMatched.Value.Split('\'');
                    attributeArray = [.. attributeArray.Where(x => !string.IsNullOrEmpty(x.Trim()))];

                    if (attributeArray.Length % 2 == 0)
                    {
                        for (var i = 0; i < attributeArray.Length; i += 2)
                        {
                            returnText.Inlines?.Add(Line(attributeArray[i], CodeType.AttributeKey));
                            returnText.Inlines?.Add(Line("'", CodeType.Quote));
                            returnText.Inlines?.Add(Line(attributeArray[i + 1], CodeType.AttributeValue));
                            returnText.Inlines?.Add(Line("'", CodeType.Quote));
                        }
                    }
                    else
                    {
                        returnText.Inlines?.Add(Line(codeMatched.Value, CodeType.Unknown));
                    }
                }
                else if (codeMatched.Value.Contains('{', StringComparison.OrdinalIgnoreCase) || codeMatched.Value.Contains('}', StringComparison.OrdinalIgnoreCase))
                {
                    returnText.Inlines?.Add(Line(codeMatched.Value, CodeType.Brace));
                }
                else
                {
                    returnText.Inlines?.Add(Line(codeMatched.Value, CodeType.Entity));
                }
            }
        }

        if (returnText.Inlines?.Count == 0)
        {
            returnText.Inlines?.Add(Line(code, CodeType.Unknown));
        }

        return returnText;
    }

    private Run Line(string line, CodeType type)
    {
        var result = new Run(line)
        {
            Foreground = Avalonia.ResourceLocator.TryGetResource<IBrush>($"MyNet.Brush.Code.{type}", ActualThemeVariant)
        };

        return result;
    }
}
