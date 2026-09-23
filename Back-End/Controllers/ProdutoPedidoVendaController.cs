using GrillSystem.Dto;
using GrillSystem.Models;
using GrillSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GrillSystem.Controllers
{
    [Route("produto-pedido-venda")]
    [ApiController]
    public class ProdutoPedidoVendaController : ControllerBase
    {
        private readonly ProdutoPedidoVendaServices _service;
        public ProdutoPedidoVendaController(ProdutoPedidoVendaServices service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ResultadoPaginadoDto<ProdutoPedidoVenda>> Get(
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
        public async Task<ProdutoPedidoVenda> GetId(int id)
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
        public async Task<ActionResult<ProdutoPedidoVenda>> Create(
            [FromBody] ProdutoPedidoVendaDto data,
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
        public async Task<ActionResult<ProdutoPedidoVenda>> Update(
            int id,
            ProdutoPedidoVendaDto data,
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
        public async Task<ActionResult<ProdutoPedidoVenda>> Delete(int id, CancellationToken cancellationToken)
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
