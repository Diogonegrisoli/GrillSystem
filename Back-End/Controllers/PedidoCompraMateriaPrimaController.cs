using GrillSystem.Dto;
using GrillSystem.Models;
using GrillSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GrillSystem.Controllers
{
    [Route("/pedido-compra-materia-prima")]
    [ApiController]
    public class PedidoCompraMateriaPrimaController : ControllerBase
    {
        private readonly PedidoCompraMateriaPrimaService _service;
        public PedidoCompraMateriaPrimaController(PedidoCompraMateriaPrimaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ResultadoPaginadoDto<PedidoCompraMateriaPrima>> Get(
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
        public async Task<PedidoCompraMateriaPrima> GetId(int id)
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
        public async Task<ActionResult<PedidoCompraMateriaPrima>> Create(
            [FromBody] PedidoCompraMateriaPrimaDto data,
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
        public async Task<ActionResult<PedidoCompraMateriaPrima>> Update(
            int id,
            PedidoCompraMateriaPrimaDto data,
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
        public async Task<ActionResult<PedidoCompraMateriaPrima>> Delete(int id, CancellationToken cancellationToken)
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
