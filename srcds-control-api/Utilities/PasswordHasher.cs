using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using srcds_control_api.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace srcds_control_api.Utilities
{
    public class HashedPassword
    {
        public HashedPassword()
        { }

        public HashedPassword(string salt, string hashedPassword)
        {
            Salt = salt.ToByteArray();
            HashedPasswordString = hashedPassword.ToByteArray();
        }

        public byte[] Salt { get; set; }
        public byte[] HashedPasswordString { get; set; }

    }

    public class PasswordHasher : IPasswordHasher
    {
        public PasswordHasher()
        { }

        public HashedPassword CreateUserPassword(string password)
        {
            try
            {
                // generate a 128-bit salt using a secure PRNG
                byte[] salt = new byte[128 / 8];
                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(salt);
                }

                // derive a 256-bit subkey (use HMACSHA512 with 10,000 iterations)
                var hashed = KeyDerivation.Pbkdf2(password: password, salt: salt, prf: KeyDerivationPrf.HMACSHA512, iterationCount: 10000, numBytesRequested: 256 / 8);

                return new HashedPassword()
                {
                    HashedPasswordString = hashed,
                    Salt = salt
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool TryMatchPassword(HashedPassword encrypted, string tryPassword)
        {
            try
            {
                var hashed = KeyDerivation.Pbkdf2(password: tryPassword, salt: encrypted.Salt, prf: KeyDerivationPrf.HMACSHA512, iterationCount: 10000, numBytesRequested: 256 / 8);

                return encrypted.HashedPasswordString.SequenceEqual(hashed);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
