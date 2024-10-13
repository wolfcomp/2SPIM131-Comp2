using System;
using System.Runtime.InteropServices;

public unsafe struct Utf16StringComponent
{
    public char* StringPointer;
    public int BufferSize;
    public int Length;
    public bool IsEmpty;
    public bool IsUsingInlineArray;
    public fixed char InlineArray[64];

    public override string ToString() => new(new ReadOnlySpan<char>(StringPointer, Length));

    public void SetString(string s)
    {
        var chars = s.ToCharArray();
        if (!IsUsingInlineArray)
            Marshal.FreeHGlobal((nint)StringPointer);
        if (chars.Length > BufferSize)
        {
            var ptr = (char*)Marshal.AllocHGlobal(chars.Length);
            chars.CopyTo(new Span<char>(ptr, chars.Length));
            StringPointer = ptr;
        }
        else
        {
            fixed (char* inline = InlineArray)
            {
                chars.CopyTo(new Span<char>(inline, 64));
                StringPointer = inline;
            }
        }
    }

    public void Destroy()
    {
        if (!IsUsingInlineArray)
            Marshal.FreeHGlobal((nint)StringPointer);
    }
}
