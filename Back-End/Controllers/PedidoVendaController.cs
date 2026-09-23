using GrillSystem.Dto;
using GrillSystem.Models;
using GrillSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GrillSystem.Controllers
{
    [Route("/pedido-venda")]
    [ApiController]
    public class PedidoVendaController : ControllerBase
    {
        private readonly PedidoVendaServices _service;
        public PedidoVendaController(PedidoVendaServices service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ResultadoPaginadoDto<PedidoVenda>> Get(
            [FromQuery] PaginacaoDto paginacao,
            CancellationToken cancellationToken)
        {
            try
            {
                return await _service.ListAll(paginacao, cancellationToken);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("{id}")]
        public async Task<PedidoVenda> GetId(int id)
        {
            try
            {
                return await _service.GetId(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<ActionResult<PedidoVenda>> Create(
            [FromBody] PedidoVendaDto data,
            CancellationToken cancellationToken)
        {
            try
            {
                return await _service.Create(data, cancellationToken);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<PedidoVenda>> Update(
            int id,
            PedidoVendaUpdateDto data,
            CancellationToken cancellationToken)
        {
            try
            {
                return await _service.Update(id, data, cancellationToken);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize(Policy = Politicas.GerenciarSistema)]
        [HttpDelete("{id}")]
        public async Task<ActionResult<PedidoVenda>> Delete(int id, CancellationToken cancellationToken)
        {
            try
            {
                return await _service.Delete(id, cancellationToken);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
