using System.Security.Cryptography;

namespace CampusCourses.Services
{
    public class PasswordService
    {
        public async Task<string> HashPassword(string password)
        {
            byte[] salt;
            byte[] buffer2;

            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            using (var bytes = new Rfc2898DeriveBytes(password, 16, 10000))
            {
                salt = bytes.Salt;
                buffer2 = bytes.GetBytes(32);
            }
            byte[] dst = new byte[49];
            dst[0] = 0;

            Buffer.BlockCopy(salt, 0, dst, 1, 16);
            Buffer.BlockCopy(buffer2, 0, dst, 17, 32);

            return Convert.ToBase64String(dst);
        }

        public async Task<bool> VerifyHashedPassword(string hashedPassword, string password)
        {
            if (hashedPassword == null)
            {
                return false;
            }

            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            byte[] src;

            try
            {
                src = Convert.FromBase64String(hashedPassword);
            }
            catch
            {
                return false;
            }

            if (src.Length != 49 || src[0] != 0)
            {
                return false;
            }

            byte[] salt = new byte[16];
            Buffer.BlockCopy(src, 1, salt, 0, 16);

            byte[] storedHash = new byte[32];
            Buffer.BlockCopy(src, 17, storedHash, 0, 32);

            using (var bytes = new Rfc2898DeriveBytes(password, salt, 10000))
            {
                byte[] computedHash = bytes.GetBytes(32);

                return CryptographicOperations.FixedTimeEquals(storedHash, computedHash);
            }
        }
    }
}
