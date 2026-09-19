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
        public async Task<ICollection<PedidoCompraMateriaPrima>> Get()
        {
            try
            {
                return await _service.ListAll();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<PedidoCompraMateriaPrima> GetId(int id)
        {
            try
            {
                return await _service.GetId(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<PedidoCompraMateriaPrima>> Create([FromBody] PedidoCompraMateriaPrimaDto data)
        {
            try
            {
                return await _service.Create(data);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<PedidoCompraMateriaPrima>> Update(int id, PedidoCompraMateriaPrimaDto data)
        {
            try
            {
                return await _service.Update(id, data);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [Authorize(Policy = Politicas.GerenciarSistema)]
        [HttpDelete("{id}")]
        public async Task<ActionResult<PedidoCompraMateriaPrima>> Delete(int id)
        {
            try
            {
                return await _service.Delete(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
