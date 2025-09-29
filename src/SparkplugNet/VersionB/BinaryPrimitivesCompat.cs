namespace SparkplugNet.VersionB;
using System;
using System.Buffers.Binary;

/// <summary>
/// Helper class to provide compatibility for BinaryPrimitives methods not available in .NET Standard 2.0.
/// </summary>
public static class BinaryPrimitivesCompat
{
    /// <summary>
    /// Reads a single-precision floating-point value from a byte span in little-endian format.
    /// </summary>
    /// <param name="source">The source span containing the bytes to read.</param>
    /// <returns>The single-precision floating-point value.</returns>
    public static float ReadSingleLittleEndian(ReadOnlySpan<byte> source)
    {
        int intVal = BinaryPrimitives.ReadInt32LittleEndian(source);
        var bytes = BitConverter.GetBytes(intVal); // machine-endian bytes for that int
        return BitConverter.ToSingle(bytes, 0);    // reinterpret the bytes as float
    }

    /// <summary>
    /// Reads a double-precision floating-point value from a byte span in little-endian format.
    /// </summary>
    /// <param name="source">The source span containing the bytes to read.</param>
    /// <returns>The double-precision floating-point value.</returns>
    public static double ReadDoubleLittleEndian(ReadOnlySpan<byte> source)
    {
        long longVal = BinaryPrimitives.ReadInt64LittleEndian(source);
        return BitConverter.Int64BitsToDouble(longVal);
    }

    /// <summary>
    /// Writes a single-precision floating-point value to a byte span in little-endian format.
    /// </summary>
    /// <param name="destination">The destination span to write the bytes to.</param>
    /// <param name="value">The single-precision floating-point value to write.</param>
    public static void WriteSingleLittleEndian(Span<byte> destination, float value)
    {
        var bytes = BitConverter.GetBytes(value);
        int intVal = BitConverter.ToInt32(bytes, 0);
        BinaryPrimitives.WriteInt32LittleEndian(destination, intVal);
    }

    /// <summary>
    /// Writes a double-precision floating-point value to a byte span in little-endian format.
    /// </summary>
    /// <param name="destination">The destination span to write the bytes to.</param>
    /// <param name="value">The double-precision floating-point value to write.</param>
    public static void WriteDoubleLittleEndian(Span<byte> destination, double value)
    {
        BinaryPrimitives.WriteInt64LittleEndian(destination, BitConverter.DoubleToInt64Bits(value));
    }
}
