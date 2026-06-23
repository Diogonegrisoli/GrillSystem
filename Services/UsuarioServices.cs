using GrillSystem.Data;
using GrillSystem.Dto;
using GrillSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GrillSystem.Services
{
    public class UsuarioServices
    {
        private readonly AppDbContext _context;
        private readonly PasswordHasher<Usuario> _passwordHasher;

        public UsuarioServices(AppDbContext context, PasswordHasher<Usuario> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public Task<bool> HasUsers()
        {
            return _context.Usuarios.AnyAsync();
        }

        public async Task<Usuario?> Authenticate(LoginDto data)
        {
            string email = data.Email.Trim().ToLowerInvariant();
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(x => x.Email == email);

            if (usuario is null)
            {
                return null;
            }

            var resultado = _passwordHasher.VerifyHashedPassword(usuario, usuario.SenhaHash, data.Senha);
            if (resultado == PasswordVerificationResult.Failed)
            {
                // Migra usuários antigos que ainda estejam com a senha salva em texto puro.
                if (usuario.SenhaHash.StartsWith("AQAAAA") || usuario.SenhaHash != data.Senha)
                {
                    return null;
                }

                usuario.SenhaHash = _passwordHasher.HashPassword(usuario, data.Senha);
                await _context.SaveChangesAsync();
            }
            else if (resultado == PasswordVerificationResult.SuccessRehashNeeded)
            {
                usuario.SenhaHash = _passwordHasher.HashPassword(usuario, data.Senha);
                await _context.SaveChangesAsync();
            }

            return usuario;
        }

        public async Task<ICollection<Usuario>> ListAll()
        {
            try
            {
                var usuario = await _context.Usuarios.ToListAsync();
                if(usuario is null)
                {
                    throw new Exception("Não foi possível retornar nenhum usuário!");
                }

                return usuario;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Usuario> GetId(int id)
        {
            try
            {
                var  usuario  = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id == id);
                if (usuario is null)
                {
                    throw new Exception("Não foi possível retornar nenhum usuário!");
                }

                return usuario;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Usuario> Create([FromBody] UsuarioDto data)
        {
            try
            {
                string email = data.Email.Trim().ToLowerInvariant();
                if (await _context.Usuarios.AnyAsync(x => x.Email == email))
                {
                    throw new Exception("Já existe um usuário com esse e-mail!");
                }

                var usuario = new Usuario
                    (email, string.Empty, data.FuncionarioId);

                usuario.SenhaHash = _passwordHasher.HashPassword(usuario, data.Senha);

                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                return usuario;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Usuario> Update(int id, [FromBody] UsuarioDto data)
        {
            try
            {
                var usuario = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id == id);
                if (usuario is null)
                {
                    throw new Exception("Não foi possível retornar nenhum usuário!");
                }

                usuario.Email = data.Email.Trim().ToLowerInvariant();
                usuario.SenhaHash = _passwordHasher.HashPassword(usuario, data.Senha);
                usuario.FuncionarioId = data.FuncionarioId;

                await _context.SaveChangesAsync();

                return usuario;
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível atualizar o usuário.", ex);
            }
        }

        public async Task<Usuario> Delete(int id)
        {
            try
            {
                var usuario = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id == id);
                if (usuario is null)
                {
                    throw new Exception("Não foi possível retornar nenhum usuário!");
                }

                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();

                return usuario;
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível deletar o usuário.", ex);
            }
        }
    }
}
