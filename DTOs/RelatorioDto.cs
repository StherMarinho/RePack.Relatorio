namespace ApiRelatorio.DTOs
{
    public class RelatorioEnvioDto
    {
        public int TotalEnvios { get; set; }
        public int TotalItens { get; set; }
        public int EnviosConcluidos { get; set; }
        public int EnviosPendentes { get; set; }
        public int EnviosCancelados { get; set; }
    }

    public class RelatorioEmbalagemDto
    {
        public string NomeMaterial { get; set; } = string.Empty;
        public string DescricaoEmbalagem { get; set; } = string.Empty;
        public int QuantidadeRecebida { get; set; }
        public decimal PesoTotalGramas { get; set; }
    }

    public class RelatorioPontuacaoDto
    {
        public double MediaPontosPorUsuario { get; set; }
        public int TotalPontosDistribuidos { get; set; }
        public int UsuariosComPontuacao { get; set; }
        public int MaiorPontuacao { get; set; }
        public int MenorPontuacao { get; set; }
    }

    public class RelatorioUsuarioDto
    {
        public int TotalUsuariosAtivos { get; set; }
        public int TotalUsuariosInativos { get; set; }
        public int NovosUsuariosNoMes { get; set; }
    }

    public class RankingItemDto
    {
        public int Posicao { get; set; }
        public int IdUsuario { get; set; }
        public string NomeUsuario { get; set; } = string.Empty;
        public int TotalPontos { get; set; }
        public int TotalEnvios { get; set; }
    }

    public class RankingPaginadoDto
    {
        public IEnumerable<RankingItemDto> Itens { get; set; } = [];
        public int PaginaAtual { get; set; }
        public int TotalPaginas { get; set; }
        public int TotalUsuarios { get; set; }
        public int ItensPorPagina { get; set; }
    }
}
