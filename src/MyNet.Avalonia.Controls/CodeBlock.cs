// -----------------------------------------------------------------------
// <copyright file="CodeBlock.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Documents;
using Avalonia.Controls.Primitives;
using Avalonia.Media;

namespace MyNet.Avalonia.Controls;

/// <summary>
/// Formats and display a fragment of the source code.
/// Supports syntax highlighting for C#, XML and XAML.
/// </summary>
[ToolboxItem(true)]
public partial class CodeBlock : ContentControl
{
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

        Entity,

        Keyword,

        Number
    }

    [GeneratedRegex(
        @"(?<comment>//[^\n]*|/\*.*?\*/)"
        + """|(?<string>@"(?:""|[^"])*"|"(?:\\"|[^"\n])*"|'(?:\\'|[^'\n])*')"""
        + """|(?<xmltag></?[\w][\w\-:.]*(?:\s+(?:[^>"']|"[^"]*"|'[^']*')*)?\s*/?>)"""
        + @"|(?<keyword>\b(?:abstract|as|async|await|base|bool|break|byte|case|catch|char|checked|class|const|continue|decimal|default|delegate|do|double|dynamic|else|enum|event|explicit|extern|false|finally|fixed|float|for|foreach|from|get|global|goto|if|implicit|in|init|int|interface|internal|is|lock|long|nameof|namespace|new|not|null|object|operator|out|override|params|partial|private|protected|public|readonly|record|ref|required|return|sbyte|sealed|select|set|short|sizeof|stackalloc|static|string|struct|switch|this|throw|true|try|typeof|uint|ulong|unchecked|unsafe|ushort|using|value|var|virtual|void|volatile|when|where|while|with|yield)\b)"
        + @"|(?<number>\b\d+\.?\d*[fFdDmMlLuU]?\b)"
        + @"|(?<brace>\{[^}]*\})"
        + @"|(?<entity>&[a-zA-Z0-9#]+;)",
        RegexOptions.Singleline | RegexOptions.Compiled)]
    private static partial Regex SyntaxRegex();

    [GeneratedRegex(
        @"(?<tagdelim></?)(?<tagname>[\w][\w\-:.]*)"
        + @"|(?<attrname>[\w][\w\-:.]*)\s*="
        + """|(?<attrvalue>"[^"]*"|'[^']*')"""
        + @"|(?<tagclose>/?>)",
        RegexOptions.Compiled)]
    private static partial Regex TagPartsRegex();

    private SelectableTextBlock? _textBlock;

    static CodeBlock() => ContentProperty.Changed.AddClassHandler<CodeBlock, object?>((x, _) => x.Refresh());

    public CodeBlock() => ActualThemeVariantChanged += (_, _) => Refresh();

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        Refresh();
    }

    public void Refresh()
    {
        if (Presenter is null) return;

        _textBlock ??= new() { TextWrapping = TextWrapping.Wrap, IsTabStop = false };

        if (!Equals(Presenter.Content, _textBlock))
            Presenter.Content = _textBlock;

        _textBlock.Inlines?.Clear();
        PopulateFormattedTextBlock(_textBlock, Clean(Content as string ?? string.Empty));
    }

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

    private void PopulateFormattedTextBlock(SelectableTextBlock textBlock, string code)
    {
        var lastIndex = 0;

        foreach (Match match in SyntaxRegex().Matches(code))
        {
            if (match.Index > lastIndex)
                AddTextSegment(textBlock, code[lastIndex..match.Index], CodeType.Unknown);

            if (match.Groups["comment"].Success)
                AddTextSegment(textBlock, match.Value, CodeType.Comment);
            else if (match.Groups["string"].Success)
                AddTextSegment(textBlock, match.Value, CodeType.Quote);
            else if (match.Groups["xmltag"].Success)
                AddTagInlines(textBlock, match.Value);
            else if (match.Groups["keyword"].Success)
                textBlock.Inlines?.Add(CreateRun(match.Value, CodeType.Keyword));
            else if (match.Groups["number"].Success)
                textBlock.Inlines?.Add(CreateRun(match.Value, CodeType.Number));
            else if (match.Groups["brace"].Success)
                AddTextSegment(textBlock, match.Value, CodeType.Brace);
            else if (match.Groups["entity"].Success)
                textBlock.Inlines?.Add(CreateRun(match.Value, CodeType.Entity));

            lastIndex = match.Index + match.Length;
        }

        if (lastIndex < code.Length)
            AddTextSegment(textBlock, code[lastIndex..], CodeType.Unknown);

        if (textBlock.Inlines?.Count == 0)
            textBlock.Inlines?.Add(CreateRun(code, CodeType.Unknown));
    }

    private void AddTextSegment(SelectableTextBlock textBlock, string text, CodeType type)
    {
        var lines = text.Split('\n');
        for (var i = 0; i < lines.Length; i++)
        {
            var line = lines[i].Replace("\t", "    ", StringComparison.Ordinal);
            if (line.Length > 0)
                textBlock.Inlines?.Add(CreateRun(line, type));

            if (i < lines.Length - 1)
                textBlock.Inlines?.Add(new LineBreak());
        }
    }

    private void AddTagInlines(SelectableTextBlock textBlock, string tag)
    {
        var lastIndex = 0;

        foreach (Match match in TagPartsRegex().Matches(tag))
        {
            if (match.Index > lastIndex)
                AddTagWhitespace(textBlock, tag[lastIndex..match.Index]);

            if (match.Groups["tagdelim"].Success)
            {
                textBlock.Inlines?.Add(CreateRun(match.Groups["tagdelim"].Value, CodeType.Tag));
                textBlock.Inlines?.Add(CreateRun(match.Groups["tagname"].Value, CodeType.Tag));
            }
            else if (match.Groups["attrname"].Success)
            {
                textBlock.Inlines?.Add(CreateRun(match.Groups["attrname"].Value, CodeType.AttributeKey));
                textBlock.Inlines?.Add(CreateRun("=", CodeType.Unknown));
            }
            else if (match.Groups["attrvalue"].Success)
            {
                var value = match.Value;
                var quote = value[..1];
                var inner = value[1..^1];
                textBlock.Inlines?.Add(CreateRun(quote, CodeType.Quote));
                textBlock.Inlines?.Add(CreateRun(inner, inner.Contains('{', StringComparison.Ordinal) ? CodeType.Brace : CodeType.AttributeValue));
                textBlock.Inlines?.Add(CreateRun(quote, CodeType.Quote));
            }
            else if (match.Groups["tagclose"].Success)
            {
                textBlock.Inlines?.Add(CreateRun(match.Value, CodeType.Tag));
            }

            lastIndex = match.Index + match.Length;
        }

        if (lastIndex < tag.Length)
            AddTagWhitespace(textBlock, tag[lastIndex..]);
    }

    private void AddTagWhitespace(SelectableTextBlock textBlock, string text)
    {
        var lines = text.Split('\n');
        for (var i = 0; i < lines.Length; i++)
        {
            var line = lines[i].Replace("\t", "    ", StringComparison.Ordinal);
            if (line.Length > 0)
                textBlock.Inlines?.Add(CreateRun(line, CodeType.Space));

            if (i < lines.Length - 1)
                textBlock.Inlines?.Add(new LineBreak());
        }
    }

    private Run CreateRun(string text, CodeType type) => new(text)
    {
        Foreground = this.TryGetResource<IBrush>($"MyNet.Brush.CodeBlock.{type}", ActualThemeVariant)
    };
}
