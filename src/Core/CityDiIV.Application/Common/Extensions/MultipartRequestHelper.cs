using Microsoft.Net.Http.Headers;

namespace CityDiIV.Application.Common.Extensions;
public static class MultipartRequestHelper
{
    // Parses the boundary from the Content-Type header
    public static string GetBoundary(MediaTypeHeaderValue contentType, int lengthLimit)
    {
        var boundary = HeaderUtilities.RemoveQuotes(contentType.Boundary).Value;

        if (string.IsNullOrWhiteSpace(boundary))
            throw new InvalidDataException("Missing content-type boundary.");

        if (boundary.Length > lengthLimit)
            throw new InvalidDataException($"Multipart boundary length limit {lengthLimit} exceeded.");

        return boundary;
    }

    // Checks if a Content-Disposition header represents a file
    public static bool HasFileContentDisposition(ContentDispositionHeaderValue contentDisposition)
    {
        return contentDisposition != null &&
               contentDisposition.DispositionType.Equals("form-data") &&
               !string.IsNullOrEmpty(contentDisposition.FileName.Value);
    }
}
