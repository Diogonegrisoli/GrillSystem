using GrillSystem.Dto;
using GrillSystem.Models;
using GrillSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GrillSystem.Controllers
{
    [Route("/pedido-compra")]
    [ApiController]
    public class PedidoCompraController : ControllerBase
    {
        private readonly PedidoCompraService _service;

        public PedidoCompraController(PedidoCompraService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ResultadoPaginadoDto<PedidoCompra>> Get(
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
        public async Task<PedidoCompra> GetId(int id)
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
        public async Task<ActionResult<PedidoCompra>> Create(
            [FromBody] PedidoCompraDto data,
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
        public async Task<ActionResult<PedidoCompra>> Update(
            int id,
            PedidoCompraUpdateDto data,
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
        public async Task<ActionResult<PedidoCompra>> Delete(int id, CancellationToken cancellationToken)
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
