namespace Application.Organizer;

using System;
using System.Collections.Generic;
using System.IO;
using Application.Model;
using SkiaSharp;

public class ImageGenerator
{
    public static void GenerateCompositeImageForLinkedIn(List<Blog> blogs, string outputPath, string logoPath, string imageDirectory)
    {
        string outputFilePath = Path.Combine(outputPath, "LinkedInCard.webp");
        GenerateCompositeImage(blogs, outputFilePath, logoPath, imageDirectory, 400, 314);
    }
    public static void GenerateCompositeImageForTwitter(List<Blog> blogs, string outputPath, string logoPath, string imageDirectory)
    {
        string outputFilePath = Path.Combine(outputPath, "TwitterCard.webp");
        GenerateCompositeImage(blogs, outputFilePath, logoPath, imageDirectory, 300, 157);
    }

    private static void GenerateCompositeImage(List<Blog> blogs, string outputPath, string logoPath, string imageDirectory, int singleWidth, int singleHeight)
    {
        if (blogs.Count > 6)
        {
            blogs = blogs.GetRange(0, 6);
        }

        int resultWidth = singleWidth * 3; // three images per row
        int resultHeight = singleHeight * 2; // two rows of images

        using var resultImage = new SKBitmap(resultWidth, resultHeight + 50); // add 50 for footer
        using var canvas = new SKCanvas(resultImage);

        var paint = PreparePaint();

        DrawImages(blogs, imageDirectory, singleWidth, singleHeight, canvas, paint);
        //DrawLogo(logoPath, canvas);
        DrawFooter(resultImage, canvas, paint);

        SaveFinalImage(resultImage, outputPath);
    }

    private static SKPaint PreparePaint()
    {
        return new SKPaint
        {
            IsAntialias = true,
            Color = SKColors.Black,
            TextSize = 20
        };
    }

    private static void DrawImages(List<Blog> blogs, string imageDirectory, int singleWidth, int singleHeight, SKCanvas canvas, SKPaint paint)
    {
        for (int i = 0; i < blogs.Count; i++)
        {
            var localPath = GetLocalPathFromBlog(blogs[i], imageDirectory);

            SKBitmap bitmap;
            if (!File.Exists(localPath))
            {
                using var stream = File.OpenRead(Path.Combine(imageDirectory,"default.png"));
                bitmap = SKBitmap.Decode(stream);
            }
            else
            {
                using var stream = File.OpenRead(localPath);
                bitmap = SKBitmap.Decode(stream);
            }

            // resize the image
            var resizedImage = bitmap.Resize(new SKImageInfo(singleWidth, singleHeight), SKFilterQuality.High);

            int x = (i % 3) * singleWidth;
            int y = (i / 3) * singleHeight + 50; // offset by 50 for the logo

            canvas.DrawBitmap(resizedImage, x, y);
        }
    }

    private static string GetLocalPathFromBlog(Blog blog, string imageDirectory)
    {
        if (string.IsNullOrEmpty(blog.FeaturedImageUrl))
            blog.FeaturedImageUrl="https://www.abc.com/default.png";
        var uri = new Uri(blog.FeaturedImageUrl);
        string localPath = Path.Combine(imageDirectory, string.Join(Path.DirectorySeparatorChar.ToString(), uri.Segments.Skip(1)));
        return Uri.UnescapeDataString(localPath); // Remove any URL-encoded characters from the path
    }

    private static void DrawLogo(string logoPath, SKCanvas canvas)
    {
        // Add company logo at the top left corner
        using var logoStream = File.OpenRead(logoPath);
        using var logoBitmap = SKBitmap.Decode(logoStream);
        canvas.DrawBitmap(logoBitmap, new SKPoint(10, 10)); // offset by 10 for a bit of margin
    }

    private static void DrawFooter(SKBitmap resultImage, SKCanvas canvas, SKPaint paint)
    {
        string footerText = "© " + DateTime.Now.Year + " AwsConcepts.com";
        float textWidth = paint.MeasureText(footerText);
        float textHeight = paint.FontMetrics.CapHeight;

        // Create a semi-transparent background
        SKPaint bgPaint = new SKPaint
        {
            Color = new SKColor(218, 165, 32, 178), // 50% transparent grey
            Style = SKPaintStyle.Fill
        };

        // Draw the background first, adjust rectangle as necessary
        SKRect bgRect = new SKRect(resultImage.Width - textWidth - 20, resultImage.Height - textHeight - 20, resultImage.Width, resultImage.Height);
        canvas.DrawRect(bgRect, bgPaint);

        // Update the text paint color to black
        paint.Color = SKColors.Black;

        // Now draw the text over the background
        canvas.DrawText(footerText, resultImage.Width - textWidth - 10, resultImage.Height - 10, paint);
    }

    private static void SaveFinalImage(SKBitmap resultImage, string outputPath)
    {
        using var resultData = SKImage.FromBitmap(resultImage).Encode(SKEncodedImageFormat.Webp, 80);
        using var resultStream = File.OpenWrite(outputPath);
        resultData.SaveTo(resultStream);
    }
}

