using Entities;

namespace Services
{
    public interface IPasswordService
    {
        Password CheckPassword(string password);
    }
}