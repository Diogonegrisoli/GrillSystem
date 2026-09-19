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
        public async Task<ICollection<PedidoVenda>> Get()
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
        public async Task<PedidoVenda> GetId(int id)
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
        public async Task<ActionResult<PedidoVenda>> Create([FromBody] PedidoVendaDto data)
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
        public async Task<ActionResult<PedidoVenda>> Update(int id, PedidoVendaUpdateDto data)
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
        public async Task<ActionResult<PedidoVenda>> Delete(int id)
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
