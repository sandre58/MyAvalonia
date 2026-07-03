// -----------------------------------------------------------------------

// <copyright file="RatingItemStateCalculator.cs" company="Stéphane ANDRE">

// Copyright (c) Stéphane ANDRE. All rights reserved.

// </copyright>

// -----------------------------------------------------------------------



using MyNet.Avalonia.Controls;



namespace MyNet.Avalonia.Controls.Internals.Rating;



/// <summary>

/// Computes <see cref="RatingItemVisualState"/> from committed and preview values.

/// </summary>

internal static class RatingItemStateCalculator

{

    public static RatingItemVisualState Calculate(int index, double value, double? previewValue)

    {

        var committedRatio = RatingValueHelper.GetFillRatio(value, index);



        if (previewValue is not { } preview)

            return new RatingItemVisualState(committedRatio, 0, 0, false, false, false, false);



        var previewRatio = RatingValueHelper.GetFillRatio(preview, index);



        if (preview > value)

        {

            var isPreviewExtend = previewRatio > committedRatio + double.Epsilon;

            return new RatingItemVisualState(committedRatio, previewRatio, 0, isPreviewExtend, false, false, false);

        }



        if (preview < value)

        {

            if (previewRatio > double.Epsilon && previewRatio < committedRatio - double.Epsilon)

            {

                return new RatingItemVisualState(

                    committedRatio,

                    previewRatio,

                    committedRatio - previewRatio,

                    false,

                    false,

                    false,

                    true);

            }



            if (previewRatio >= committedRatio - double.Epsilon && previewRatio > double.Epsilon)

            {

                return new RatingItemVisualState(committedRatio, previewRatio, 0, false, true, false, false);

            }



            if (previewRatio < committedRatio - double.Epsilon)

            {

                return new RatingItemVisualState(committedRatio, previewRatio, 0, false, false, true, false);

            }

        }



        return new RatingItemVisualState(committedRatio, previewRatio, 0, false, false, false, false);

    }

}


