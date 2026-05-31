// -----------------------------------------------------------------------
// <copyright file="PaginationHelper.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.Primitives;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls.Internals;
#pragma warning restore IDE0130 // Namespace does not match folder structure

internal static class PaginationHelper
{
    public static int CalculatePageCount(int totalCount, int pageSize)
    {
        if (pageSize <= 0)
            return 0;

        var pageCount = totalCount / pageSize;
        if (totalCount % pageSize > 0)
            pageCount++;

        return pageCount;
    }

    public static int? CoerceCurrentPage(int? page, int pageCount) => page?.SafeClamp(1, pageCount);

    public static int AddPageOffset(int currentPageOrZero, int offset, int pageCount) =>
        (currentPageOrZero + offset).SafeClamp(1, pageCount);

    public static int ClampQuickJump(decimal value, int pageCount) =>
        (int)value.SafeClamp(1, pageCount);

    public static (bool PreviousEnabled, bool NextEnabled) GetNavigationState(int? currentPage, int pageCount) =>
        ((currentPage ?? int.MaxValue) > 1, (currentPage ?? 0) < pageCount);
}
