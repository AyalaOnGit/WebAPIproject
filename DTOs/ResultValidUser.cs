using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    public record ResultValidUser<T>(bool InvalidPassword, bool UserAlreadyExists,bool IsValidEmail, T data);

}
