using ApiRelatorio.DTOs;

namespace ApiRelatorio.Services.Interfaces
{
    public interface IRelatorioService
    {
        Task<RelatorioEnvioDto> GetEstatisticasEnviosAsync();
        Task<IEnumerable<RelatorioEmbalagemDto>> GetEmbalagensMaisRecebidasAsync(int top);
        Task<RelatorioPontuacaoDto> GetEstatisticasPontuacaoAsync();
        Task<RelatorioUsuarioDto> GetEstatisticasUsuariosAsync();
        Task<RankingPaginadoDto> GetRankingAsync(int pagina, int itensPorPagina);
    }
}