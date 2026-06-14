using Microsoft.AspNetCore.Mvc;
using ApiRelatorio.Services.Interfaces;

namespace ApiRelatorio.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RelatorioController : ControllerBase
    {
        private readonly IRelatorioService _service;

        public RelatorioController(IRelatorioService service)
        {
            _service = service;
        }

        // Resumo geral: total de envios, itens e distribuição por status
        // GET api/relatorio/envios
        [HttpGet("envios")]
        public async Task<IActionResult> GetEstatisticasEnvios()
        {
            var relatorio = await _service.GetEstatisticasEnviosAsync();
            return Ok(relatorio);
        }

        // Tipos de embalagem mais recebidos em ordem decrescente
        // GET api/relatorio/embalagens?top=10
        [HttpGet("embalagens")]
        public async Task<IActionResult> GetEmbalagensMaisRecebidas([FromQuery] int top = 10)
        {
            if (top <= 0)
                return BadRequest(new { mensagem = "O parâmetro 'top' deve ser maior que zero." });

            var relatorio = await _service.GetEmbalagensMaisRecebidasAsync(top);
            return Ok(relatorio);
        }

        // Estatísticas de pontuação: média, total distribuído, maior e menor pontuação
        // GET api/relatorio/pontuacao
        [HttpGet("pontuacao")]
        public async Task<IActionResult> GetEstatisticasPontuacao()
        {
            var relatorio = await _service.GetEstatisticasPontuacaoAsync();
            return Ok(relatorio);
        }

        // Estatísticas de usuários: ativos, inativos e novos no mês
        // GET api/relatorio/usuarios
        [HttpGet("usuarios")]
        public async Task<IActionResult> GetEstatisticasUsuarios()
        {
            var relatorio = await _service.GetEstatisticasUsuariosAsync();
            return Ok(relatorio);
        }

        // Ranking paginado de clientes ordenado por pontuação
        // GET api/relatorio/ranking?pagina=1&itensPorPagina=20
        [HttpGet("ranking")]
        public async Task<IActionResult> GetRanking(
            [FromQuery] int pagina = 1,
            [FromQuery] int itensPorPagina = 20)
        {
            if (pagina < 1)
                return BadRequest(new { mensagem = "O parâmetro 'pagina' deve ser maior que zero." });

            if (itensPorPagina < 1 || itensPorPagina > 100)
                return BadRequest(new { mensagem = "O parâmetro 'itensPorPagina' deve estar entre 1 e 100." });

            var ranking = await _service.GetRankingAsync(pagina, itensPorPagina);
            return Ok(ranking);
        }
    }
}