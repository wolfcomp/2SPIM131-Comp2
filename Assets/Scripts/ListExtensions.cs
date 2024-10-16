using System.Collections.Generic;

public static class ListExtensions
{
    public static bool TryGetValue<T>(this List<T> list, int index, out T value)
    {
        if (index < 0 || index >= list.Count)
        {
            value = default;
            return false;
        }
        value = list[index];
        return true;
    }
}
