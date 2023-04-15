using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkStar.Api.Utils;

public class BufferUtils
{
    public const int LengthHeaderSize = 4;

    public static byte[] Combine(byte[] first, byte[] second)
    {
        var ret = new byte[first.Length + second.Length];
        Buffer.BlockCopy(first, 0, ret, 0, first.Length);
        Buffer.BlockCopy(second, 0, ret, first.Length, second.Length);
        return ret;
    }

    public static byte[] GetByteArrayFromInt(int length)
    {
        var bytes = new byte[4];

        bytes[0] = (byte)(length >> 24);
        bytes[1] = (byte)(length >> 16);
        bytes[2] = (byte)(length >> 8);
        bytes[3] = (byte)length;

        return bytes;
    }

    public static int GetIntFromByteArray(byte[] bytes)
    {
        return BitConverter.ToInt32(bytes[..LengthHeaderSize].Reverse().ToArray(), 0);
    }
}
