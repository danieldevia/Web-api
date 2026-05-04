using InventarioApi.Models.DTOs;

namespace InventarioApi.Services.Interfaces
{
    public interface IAuthService
    {
        LoginResponse? Login(LoginRequest request);
    }
}