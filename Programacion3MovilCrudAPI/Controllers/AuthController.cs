using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Programacion3MovilCrudAPI.Models;
using System.Security.Claims;

namespace Programacion3MovilCrudAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly plantasDBContext _context;

        public AuthController(plantasDBContext context)
        {
            _context = context;
        }

        /*
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u =>
                u.Name == request.Nombre && u.Password == request.Contrasena);

            if (usuario == null)
                return Unauthorized(new { message = "Credenciales inválidas" });

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.Name),
                new Claim(ClaimTypes.Role, usuario.Role),
                new Claim("UsuarioId", usuario.Id.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, "MiCookieAuth");

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true, // hace que la cookie persista entre reinicios del navegador
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(2)
            };

            await HttpContext.SignInAsync("MiCookieAuth",
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return Ok(new { message = "Login exitoso", usuario });
        }
        */

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest login)
        {
            if (login == null || string.IsNullOrWhiteSpace(login.Nombre) || string.IsNullOrWhiteSpace(login.Contrasena))
                return BadRequest("Datos inválidos");

            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u =>
                u.Name == login.Nombre && u.Password == login.Contrasena);

            if (usuario == null)
                return Unauthorized("Credenciales inválidas");

            // Crear la cookie
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, usuario.Name),
        new Claim(ClaimTypes.Role, usuario.Role)
    };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return Ok("Login exitoso");
        }


        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok("Sesión cerrada correctamente.");
        }


        [HttpGet("estado")]
        public IActionResult Estado()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return Ok(new
                {
                    autenticado = true,
                    nombre = User.Identity.Name,
                    rol = User.FindFirst(ClaimTypes.Role)?.Value,
                    id = User.FindFirst("UsuarioId")?.Value
                });
            }

            return Ok(new { autenticado = false });
        }
    }

    public class LoginRequest
    {
        public string Nombre { get; set; }
        public string Contrasena { get; set; }
    }
}
