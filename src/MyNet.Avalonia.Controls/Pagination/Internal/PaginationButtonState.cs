// -----------------------------------------------------------------------
// <copyright file="PaginationButtonState.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MyNet.Avalonia.Controls.Internals.Pagination;

internal readonly record struct PaginationButtonState(
    int Page,
    bool IsVisible,
    bool IsSelected,
    bool IsLeftEllipsis,
    bool IsRightEllipsis);
