using System;

public static class SpanExtensions
{
    public static ref T GetRef<T>(this Span<T> span, int index)
    {
        return ref span[index];
    }

    public static ref T GetRef<T>(this Span<T> span, Predicate<T> predicate)
    {
        for (var i = 0; i < span.Length; i++)
        {
            if (predicate(span[i]))
            {
                return ref span[i];
            }
        }

        throw new InvalidOperationException("No element found that satisfies the predicate.");
    }
}