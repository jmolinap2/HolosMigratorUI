using System.Security.Cryptography;

namespace HolosMigratorUI.Core;

public static class SecretGenerator
{
    private const string Alphabet = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz23456789";

    /// <summary>
    /// Genera una cadena alfanumerica aleatoria (sin caracteres ambiguos ni simbolos que
    /// puedan romper un archivo .env o una connection string). Longitud minima 16.
    /// </summary>
    public static string Generate(int length)
    {
        var len = Math.Max(16, length);
        var bytes = new byte[len];
        RandomNumberGenerator.Fill(bytes);

        var chars = new char[len];
        for (var i = 0; i < len; i++)
        {
            chars[i] = Alphabet[bytes[i] % Alphabet.Length];
        }

        return new string(chars);
    }
}
