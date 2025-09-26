using Microsoft.EntityFrameworkCore;
using Programacion3MovilCrudAPI.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Programacion3MovilCrudAPI.Services
{
    public class SesionService
    {
        private readonly plantasDBContext _context;

        public Usuario UsuarioActual { get; private set; }

        public bool EstaAutenticado => UsuarioActual != null;

        public SesionService(plantasDBContext context)
        {
            _context = context;
        }

        public async Task<bool> LoginAsync(string nombre, string contrasena)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Name == nombre && u.Password == contrasena);

            if (usuario != null)
            {
                UsuarioActual = usuario;
                return true;
            }

            return false;
        }

        public void Logout()
        {
            UsuarioActual = null;
        }
    }
}
