using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Programacion3MovilCrudAPI.Models;

[Route("api/[controller]")]
[ApiController]
public class UsuariosController : ControllerBase
{
    private readonly plantasDBContext _context;

    public UsuariosController(plantasDBContext context)
    {
        _context = context;
    }

    [HttpGet("listar")]
    public async Task<ActionResult<IEnumerable<Usuario>>> Listar()
    {
        return await _context.Usuarios.ToListAsync();
    }

    [HttpGet("buscar/{id}")]
    public async Task<ActionResult<Usuario>> Buscar(int id)
    {
        var user = await _context.Usuarios.FindAsync(id);
        if (user == null) return NotFound();
        return user;
    }

    [HttpPost("crear")]
    public async Task<ActionResult> Crear([FromBody] Usuario usuario)
    {
        if (_context.Usuarios.Any(u => u.Id == usuario.Id))
            return BadRequest("El usuario ya existe.");

        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();
        return Ok(new { message = "Usuario creado correctamente" });
    }

    [HttpPut("editar/{id}")]
    public async Task<ActionResult> Editar(int id, [FromBody] Usuario usuario)
    {
        var userDb = await _context.Usuarios.FindAsync(id);
        if (userDb == null) return NotFound();

        userDb.Name = usuario.Name;
        userDb.Password = usuario.Password;
        userDb.Role = usuario.Role;
        userDb.Imagen = usuario.Imagen;

        await _context.SaveChangesAsync();
        return Ok(new { message = "Usuario actualizado correctamente" });
    }

    [HttpDelete("eliminar/{id}")]
    public async Task<ActionResult> Eliminar(int id)
    {
        var user = await _context.Usuarios.FindAsync(id);
        if (user == null) return NotFound();

        _context.Usuarios.Remove(user);
        await _context.SaveChangesAsync();
        return Ok(new { message = "Usuario eliminado correctamente" });
    }
}
