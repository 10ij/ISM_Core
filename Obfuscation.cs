using System;

namespace ISM.Core;

public class Obfuscation
{   
    // Doesnt need to be a huge key, just something more secure than base64
    private static readonly int SecretKey = 0x5F3759DF;

    /// <summary>
    /// Run this through every time you send a primary or foreign key client side
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static string ObfuscateID(int id)
    {
        uint obfuscated = Encrypt32((uint)id, SecretKey);
        return obfuscated.ToString("X8");
    }

    /// <summary>
    /// Run this through every time you receive a primary or foreign key from the client side
    /// </summary>
    /// <param name="obfuscatedId"></param>
    /// <returns></returns>
    public static int DeobfuscateID(string obfuscatedId)
    {
        uint obfuscated = uint.Parse(obfuscatedId, System.Globalization.NumberStyles.HexNumber);
        uint original = Decrypt32(obfuscated, SecretKey);
        return (int)original;
    }

    private static uint Encrypt32(uint val, int key)
    {
        uint l = val >> 16;
        uint r = val & 0xFFFF; 

        // 4 rounds of Feistel network mixing
        for (int i = 0; i < 4; i++)
        {
            uint temp = r;
            r = l ^ F(r, key, i);
            l = temp;
        }

        return (l << 16) | (r & 0xFFFF);
    }

    private static uint Decrypt32(uint val, int key)
    {
        uint l = val >> 16;
        uint r = val & 0xFFFF;

        // 4 rounds of Feistel network mixing (in reverse)
        for (int i = 3; i >= 0; i--)
        {
            uint temp = l;
            l = r ^ F(l, key, i);
            r = temp;
        }

        return (l << 16) | (r & 0xFFFF);
    }

    private static uint F(uint r, int key, int round)
    {
        // Lightweight non-linear mixing function using the key and round index
        uint hash = r * 2654435761u ^ (uint)key ^ (uint)(round * 0x9e3779b9);
        return (hash ^ (hash >> 16)) & 0xFFFF;
    }
}