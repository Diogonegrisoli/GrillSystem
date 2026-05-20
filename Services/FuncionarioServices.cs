using GrillSystem.Data;
using GrillSystem.Dto;
using GrillSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.EntityFrameworkCore;

namespace GrillSystem.Services
{
    public class FuncionarioServices
    {
        private readonly AppDbContext _context;
        public FuncionarioServices(AppDbContext context)
        {
            _context = context;
        }


        public async Task<ICollection<Funcionario>> ListAll()
        {
            try
            {
                var funcionario = await _context.Funcionarios.ToListAsync();
                if (funcionario is null)
                {
                    throw new Exception("Não foi possível retornar nenhum funcionário!");
                }

                return funcionario;
            }
            catch (Exception)
            {
                throw new Exception("Ocorreu um erro ao executar a ação!");
            }
        }

        public async Task<Funcionario> GetId(int id)
        {
            try
            {
                var funcionario = await _context.Funcionarios.FirstOrDefaultAsync(x => x.Id == id);
                if (funcionario is null)
                {
                    throw new Exception($"O funcionário com o id {id}# não foi localizado!");
                }
                return funcionario;
            }
            catch (Exception)
            {
                throw new Exception("Ocorreu um erro ao executar a ação!");
            }
        }

        public async Task<Funcionario> Create([FromBody] FuncionarioDto data)
        {
            var funcionario = new Funcionario
            (data.Nome, data.Cpf);

            _context.Funcionarios.Add(funcionario);
            await _context.SaveChangesAsync();

            return funcionario;
        }

        public async Task<Funcionario> Update(int id, [FromBody] FuncionarioDto data)
        {
            var funcionario = await _context.Funcionarios.FirstOrDefaultAsync(x => x.Id == id);
            if (funcionario is null)
            {
                throw new Exception($"O funcionário com o id {id}# não foi localizado!");
            }

            funcionario.Nome = data.Nome;
            funcionario.Cpf = data.Cpf;

            await _context.SaveChangesAsync();


            return funcionario;
        }

        public async Task<Funcionario> Delete(int id)
        {
            var funcionario = await _context.Funcionarios.FirstOrDefaultAsync(x => x.Id == id);
            if (funcionario is null)
            {
                throw new Exception($"O funcionário com o id {id}# não foi localizado!");
            }

            _context.Funcionarios.Remove(funcionario);
            await _context.SaveChangesAsync();

            return funcionario;
        }
    }
}
