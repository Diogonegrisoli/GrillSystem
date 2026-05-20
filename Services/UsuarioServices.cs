using GrillSystem.Data;
using GrillSystem.Dto;
using GrillSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GrillSystem.Services
{
    public class UsuarioServices
    {
        private readonly AppDbContext _context;
        public UsuarioServices(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<Usuario>> ListAll()
        {
            var usuario = await _context.Usuarios.ToListAsync();
            if(usuario is null)
            {
                throw new Exception("Não foi possível retornar nenhum usuário!");
            }

            return usuario;
        }

        public async Task<Usuario> GetId(int id)
        {
            var  usuario  = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id == id);
            if (usuario is null)
            {
                throw new Exception("Não foi possível retornar nenhum usuário!");
            }

            return usuario;
        }

        public async Task<Usuario> Create([FromBody] UsuarioDto data)
        {
            var usuario = new Usuario
                (data.Email, data.SenhaHash, data.FuncionarioId);

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return usuario;
        }

        public async Task<Usuario> Update(int id, [FromBody] UsuarioDto data)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id == id);
            if (usuario is null)
            {
                throw new Exception("Não foi possível retornar nenhum usuário!");
            }

            usuario.Email = data.Email;
            usuario.SenhaHash = data.SenhaHash;
            usuario.FuncionarioId = data.FuncionarioId;

            await _context.SaveChangesAsync();

            return usuario;
        }

        public async Task Delete(int id)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id == id);
            if (usuario is null)
            {
                throw new Exception("Não foi possível retornar nenhum usuário!");
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
        }
    }
}
