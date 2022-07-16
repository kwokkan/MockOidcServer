using System.Text;
using System.Text.Json;

namespace System;

public static class StringExtensions
{
    public static byte[]? ToByteArray(this string? value)
    {
        if (value == null)
        {
            return default;
        }

        return Encoding.ASCII.GetBytes(value);
    }
}
