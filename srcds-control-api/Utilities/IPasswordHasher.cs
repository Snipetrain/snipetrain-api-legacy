using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace srcds_control_api.Utilities
{
    public interface IPasswordHasher
    {
        HashedPassword CreateUserPassword(string password);
        bool TryMatchPassword(HashedPassword encrypted, string tryPassword);
    }
}
