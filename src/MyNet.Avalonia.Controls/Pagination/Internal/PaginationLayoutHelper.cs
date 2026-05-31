// -----------------------------------------------------------------------
// <copyright file="PaginationLayoutHelper.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.Primitives;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls.Internals;
#pragma warning restore IDE0130 // Namespace does not match folder structure

internal static class PaginationLayoutHelper
{
    internal const int ButtonSlotCount = 7;

    internal const int CompactPageThreshold = 7;

    public static PaginationButtonState[] BuildButtonStates(int? currentPage, int pageCount)
    {
        var states = new PaginationButtonState[ButtonSlotCount];
        switch (pageCount)
        {
            case <= 0:
                return states;
            case <= CompactPageThreshold:
                {
                    for (var i = 0; i < ButtonSlotCount; i++)
                    {
                        if (i < pageCount)
                            states[i] = new(i + 1, true, currentPage == i + 1, false, false);
                    }

                    return states;
                }
        }

        var mid = (currentPage ?? 0).SafeClamp(4, pageCount - 3);

        states[0] = new(1, true, currentPage == 1, false, false);
        states[6] = new(pageCount, true, currentPage == pageCount, false, false);
        states[3] = new(mid, true, currentPage == mid, false, false);
        states[2] = new(mid - 1, true, currentPage == mid - 1, false, false);
        states[4] = new(mid + 1, true, currentPage == mid + 1, false, false);

        states[1] = mid > 4
            ? new(-1, true, false, true, false)
            : new(mid - 2, true, currentPage == mid - 2, false, false);

        states[5] = mid < pageCount - 3
            ? new(-1, true, false, false, true)
            : new(mid + 2, true, currentPage == mid + 2, false, false);

        return states;
    }
}
