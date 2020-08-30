using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace srcds_control_api.Utilities
{
    public interface IPasswordEncryptor
    {
        EncryptedPassword CreateUserPassword(string password);
        bool TryMatchPassword(EncryptedPassword encrypted, string tryPassword);
    }
}
