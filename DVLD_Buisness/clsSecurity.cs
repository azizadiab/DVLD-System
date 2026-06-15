using System;
using System.Security.Cryptography;
using System.Text;


namespace DVLD_Buisness
{
    public class clsSecurity
    {
        public static string GenerateSalt(int Size = 16)
        {
            byte[] ByteSalt = new byte[Size];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(ByteSalt);
            }
            return Convert.ToBase64String(ByteSalt);
        }

        
        public static string Has256Password(string Password, string Salt)
        {
            string PasswordSalt = Password + Salt;

            using (SHA256 sha256 = SHA256.Create())
            {

                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(PasswordSalt));
                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
        }


    }
}
