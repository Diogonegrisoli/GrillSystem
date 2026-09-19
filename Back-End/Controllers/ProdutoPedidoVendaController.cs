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
        public async Task<ICollection<ProdutoPedidoVenda>> Get()
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
        public async Task<ProdutoPedidoVenda> GetId(int id)
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
        public async Task<ActionResult<ProdutoPedidoVenda>> Create([FromBody] ProdutoPedidoVendaDto data)
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
        public async Task<ActionResult<ProdutoPedidoVenda>> Update(int id, ProdutoPedidoVendaDto data)
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
        public async Task<ActionResult<ProdutoPedidoVenda>> Delete(int id)
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
