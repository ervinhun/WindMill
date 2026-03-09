using System.Security.Cryptography;
using System.Text;

namespace WindMill.Util;

public static class GenerateHashPass
{

    private const int SaltSize = 16;
    private const int KeySize = 32;
    private const int Iterations = 100_000;
    public static string Generate(string password, string? pepper = null)
    {
        if (!string.IsNullOrEmpty(pepper))
            password += pepper;

        // Generate random salt
        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        
        var key = Pbkdf2(
            password: Encoding.UTF8.GetBytes(password),
            salt: salt,
            iterations: Iterations,
            keyLength: KeySize,
            algorithm: HashAlgorithmName.SHA256
        );

        // Combine salt + key
        var hashBytes = new byte[SaltSize + KeySize];
        Buffer.BlockCopy(salt, 0, hashBytes, 0, SaltSize);
        Buffer.BlockCopy(key, 0, hashBytes, SaltSize, KeySize);

        return Convert.ToBase64String(hashBytes);
    }
    
    public static bool Verify(string password, string hashed, string? pepper = null)
    {
        if (!string.IsNullOrEmpty(pepper))
            password += pepper;

        var hashBytes = Convert.FromBase64String(hashed);

        var salt = new byte[SaltSize];
        Buffer.BlockCopy(hashBytes, 0, salt, 0, SaltSize);

        var storedKey = new byte[KeySize];
        Buffer.BlockCopy(hashBytes, SaltSize, storedKey, 0, KeySize);

        var key = Pbkdf2(
            password: Encoding.UTF8.GetBytes(password),
            salt: salt,
            iterations: Iterations,
            keyLength: KeySize,
            algorithm: HashAlgorithmName.SHA256
        );

        return CryptographicOperations.FixedTimeEquals(storedKey, key);
    }

    
    private static byte[] Pbkdf2(byte[] password, byte[] salt, int iterations, int keyLength, HashAlgorithmName algorithm)
    {
        return Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, algorithm, keyLength);
    }
}