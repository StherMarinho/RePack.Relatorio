using ApiRelatorio.Models;

namespace ApiRelatorio.Repositories.Interfaces
{
    public interface IRelatorioRepository
    {
        Task<RelatorioEnvio> GetEstatisticasEnviosAsync();
        Task<IEnumerable<RelatorioEmbalagem>> GetEmbalagensMaisRecebidasAsync(int top);
        Task<RelatorioPontuacao> GetEstatisticasPontuacaoAsync();
        Task<RelatorioUsuario> GetEstatisticasUsuariosAsync();
        Task<(IEnumerable<RankingItem> Itens, int Total)> GetRankingAsync(int pagina, int itensPorPagina);
    }
}