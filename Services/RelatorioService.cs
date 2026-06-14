using ApiRelatorio.DTOs;
using ApiRelatorio.Repositories.Interfaces;
using ApiRelatorio.Services.Interfaces;

namespace ApiRelatorio.Services
{
    public class RelatorioService : IRelatorioService
    {
        private readonly IRelatorioRepository _repository;

        public RelatorioService(IRelatorioRepository repository)
        {
            _repository = repository;
        }

        public async Task<RelatorioEnvioDto> GetEstatisticasEnviosAsync()
        {
            var dados = await _repository.GetEstatisticasEnviosAsync();

            return new RelatorioEnvioDto
            {
                TotalEnvios = dados.TotalEnvios,
                TotalItens = dados.TotalItens,
                EnviosConcluidos = dados.EnviosConcluidos,
                EnviosPendentes = dados.EnviosPendentes,
                EnviosCancelados = dados.EnviosCancelados
            };
        }

        public async Task<IEnumerable<RelatorioEmbalagemDto>> GetEmbalagensMaisRecebidasAsync(int top)
        {
            var dados = await _repository.GetEmbalagensMaisRecebidasAsync(top);

            return dados.Select(d => new RelatorioEmbalagemDto
            {
                NomeMaterial = d.NomeMaterial,
                DescricaoEmbalagem = d.DescricaoEmbalagem,
                QuantidadeRecebida = d.QuantidadeRecebida,
                PesoTotalGramas = d.PesoTotalGramas
            });
        }

        public async Task<RelatorioPontuacaoDto> GetEstatisticasPontuacaoAsync()
        {
            var dados = await _repository.GetEstatisticasPontuacaoAsync();

            return new RelatorioPontuacaoDto
            {
                MediaPontosPorUsuario = Math.Round(dados.MediaPontosPorUsuario, 2),
                TotalPontosDistribuidos = dados.TotalPontosDistribuidos,
                UsuariosComPontuacao = dados.UsuariosComPontuacao,
                MaiorPontuacao = dados.MaiorPontuacao,
                MenorPontuacao = dados.MenorPontuacao
            };
        }

        public async Task<RelatorioUsuarioDto> GetEstatisticasUsuariosAsync()
        {
            var dados = await _repository.GetEstatisticasUsuariosAsync();

            return new RelatorioUsuarioDto
            {
                TotalUsuariosAtivos = dados.TotalUsuariosAtivos,
                TotalUsuariosInativos = dados.TotalUsuariosInativos,
                NovosUsuariosNoMes = dados.NovosUsuariosNoMes
            };
        }

        public async Task<RankingPaginadoDto> GetRankingAsync(int pagina, int itensPorPagina)
        {
            if (pagina < 1) pagina = 1;
            if (itensPorPagina < 1) itensPorPagina = 20;

            var (itens, total) = await _repository.GetRankingAsync(pagina, itensPorPagina);

            int totalPaginas = (int)Math.Ceiling((double)total / itensPorPagina);

            return new RankingPaginadoDto
            {
                Itens = itens.Select(i => new RankingItemDto
                {
                    Posicao = (int)i.Posicao,
                    IdUsuario = i.IdUsuario,
                    NomeUsuario = i.NomeUsuario,
                    TotalPontos = i.TotalPontos,
                    TotalEnvios = i.TotalEnvios
                }),
                PaginaAtual = pagina,
                TotalPaginas = totalPaginas,
                TotalUsuarios = total,
                ItensPorPagina = itensPorPagina
            };
        }
    }
}