using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using InventarioApi.Models.DTOs;
using InventarioApi.Repository.Interfaces;
using InventarioApi.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace InventarioApi.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IUsuarioRepository usuarioRepository, IConfiguration configuration)
        {
            _usuarioRepository = usuarioRepository;
            _configuration = configuration;
        }

        public LoginResponse? Login(LoginRequest request)
        {
            // 1. Buscar usuario por email y password
            var usuario = _usuarioRepository
                .GetAll()
                .FirstOrDefault(u =>
                    u.Email.Equals(request.Email, StringComparison.OrdinalIgnoreCase)
                    && u.Password == request.Password);

            // 2. Validar que exista y esté activo
            if (usuario == null || !usuario.IsActivo)
                return null;

            // 3. Leer configuración del appsettings.json
            var jwtSettings   = _configuration.GetSection("JwtSettings");
            var secretKey     = jwtSettings["SecretKey"]!;
            var issuer        = jwtSettings["Issuer"]!;
            var audience      = jwtSettings["Audience"]!;
            var expiresInMins = int.Parse(jwtSettings["ExpiresInMinutes"]!);

            // 4. Crear la clave y las credenciales de firma
            var key   = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiracion = DateTime.UtcNow.AddMinutes(expiresInMins);

            // 5. Definir los claims (datos que van dentro del token)
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name,           usuario.Nombre),
                new Claim(ClaimTypes.Email,          usuario.Email),
                new Claim(ClaimTypes.Role,           usuario.TipoRol.ToString())
            };

            // 6. Construir el token
            var token = new JwtSecurityToken(
                issuer:             issuer,
                audience:           audience,
                claims:             claims,
                expires:            expiracion,
                signingCredentials: creds
            );

            // 7. Convertir el token a string y retornar la respuesta
            return new LoginResponse
            {
                Token      = new JwtSecurityTokenHandler().WriteToken(token),
                Nombre     = usuario.Nombre,
                Email      = usuario.Email,
                Rol        = usuario.TipoRol.ToString(),
                Expiracion = expiracion
            };
        }
    }
}