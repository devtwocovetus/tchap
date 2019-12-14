namespace TheCloudHealth.Lib
{
    interface ICryptoEngine
    {
        string Encrypt(string input, string key);
        string Decrypt(string input, string key);
    }
}
