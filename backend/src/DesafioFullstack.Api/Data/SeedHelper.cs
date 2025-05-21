using System;
using System.Security.Cryptography;
using System.Text;

namespace DesafioFullstack.Api.Data
{
    public static class SeedHelper
    {
        public static string GerarHashSenha(string senha)
        {
            if (string.IsNullOrEmpty(senha))
            {
                return string.Empty;
            }

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytesSenha = Encoding.UTF8.GetBytes(senha);
                byte[] bytesHashSenha = sha256.ComputeHash(bytesSenha);
                return BitConverter.ToString(bytesHashSenha).Replace("-", "").ToLower();
            }
        }
    }
}