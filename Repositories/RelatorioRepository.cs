using Dapper;
using MySql.Data.MySqlClient;
using ApiRelatorio.Models;
using ApiRelatorio.Repositories.Interfaces;

namespace ApiRelatorio.Repositories
{
    public class RelatorioRepository : IRelatorioRepository
    {
        private readonly string _connectionString;

        public RelatorioRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("String de conexão 'DefaultConnection' não configurada.");
        }

        public async Task<RelatorioEnvio> GetEstatisticasEnviosAsync()
        {
            using var connection = new MySqlConnection(_connectionString);

            const string sql = @"
                SELECT
                    COUNT(*) AS TotalEnvios,
                    COALESCE(SUM(quantidade_itens), 0) AS TotalItens,
                    SUM(CASE WHEN id_status_envio = (SELECT id FROM status_envio WHERE nome_status = 'Concluído') THEN 1 ELSE 0 END) AS EnviosConcluidos,
                    SUM(CASE WHEN id_status_envio = (SELECT id FROM status_envio WHERE nome_status = 'Pendente')  THEN 1 ELSE 0 END) AS EnviosPendentes,
                    SUM(CASE WHEN id_status_envio = (SELECT id FROM status_envio WHERE nome_status = 'Cancelado') THEN 1 ELSE 0 END) AS EnviosCancelados
                FROM envio;";

            return await connection.QuerySingleAsync<RelatorioEnvio>(sql);
        }

        public async Task<IEnumerable<RelatorioEmbalagem>> GetEmbalagensMaisRecebidasAsync(int top)
        {
            using var connection = new MySqlConnection(_connectionString);

            const string sql = @"
                SELECT
                    me.nome          AS NomeMaterial,
                    e.descricao      AS DescricaoEmbalagem,
                    COUNT(ie.id)     AS QuantidadeRecebida,
                    COALESCE(SUM(e.peso_medio), 0) AS PesoTotalGramas
                FROM item_envio ie
                INNER JOIN embalagem e ON ie.id_embalagem = e.id
                INNER JOIN material_embalagem me ON e.id_tipo = me.id
                GROUP BY e.id, me.nome, e.descricao
                ORDER BY QuantidadeRecebida DESC
                LIMIT @Top;";

            return await connection.QueryAsync<RelatorioEmbalagem>(sql, new { Top = top });
        }

        public async Task<RelatorioPontuacao> GetEstatisticasPontuacaoAsync()
        {
            using var connection = new MySqlConnection(_connectionString);

            const string sql = @"
                SELECT
                    COALESCE(AVG(pontos), 0)           AS MediaPontosPorUsuario,
                    COALESCE(SUM(pontos), 0)           AS TotalPontosDistribuidos,
                    COUNT(DISTINCT id_usuario)         AS UsuariosComPontuacao,
                    COALESCE(MAX(pontos), 0)           AS MaiorPontuacao,
                    COALESCE(MIN(pontos), 0)           AS MenorPontuacao
                FROM pontuacao;";

            return await connection.QuerySingleAsync<RelatorioPontuacao>(sql);
        }

        public async Task<RelatorioUsuario> GetEstatisticasUsuariosAsync()
        {
            using var connection = new MySqlConnection(_connectionString);

            const string sql = @"
                SELECT
                    SUM(CASE WHEN ativo = TRUE  THEN 1 ELSE 0 END) AS TotalUsuariosAtivos,
                    SUM(CASE WHEN ativo = FALSE THEN 1 ELSE 0 END) AS TotalUsuariosInativos,
                    SUM(CASE WHEN MONTH(data_cadastro) = MONTH(NOW())
                              AND YEAR(data_cadastro)  = YEAR(NOW())  THEN 1 ELSE 0 END) AS NovosUsuariosNoMes
                FROM usuario;";

            return await connection.QuerySingleAsync<RelatorioUsuario>(sql);
        }

        public async Task<(IEnumerable<RankingItem> Itens, int Total)> GetRankingAsync(int pagina, int itensPorPagina)
        {
            using var connection = new MySqlConnection(_connectionString);

            // Total de clientes ativos com pelo menos uma pontuação
            const string sqlTotal = @"
                SELECT COUNT(DISTINCT u.id)
                FROM usuario u
                INNER JOIN tipo_usuario tu ON u.id_tipo_usuario = tu.id
                INNER JOIN pontuacao p ON p.id_usuario = u.id
                WHERE tu.descricao = 'Comum'
                  AND u.ativo = TRUE;";

            int total = await connection.ExecuteScalarAsync<int>(sqlTotal);

            // Página do ranking com posição calculada via ROW_NUMBER
            const string sqlRanking = @"
                SELECT
                    ROW_NUMBER() OVER (ORDER BY SUM(p.pontos) DESC) AS Posicao,
                    u.id                                             AS IdUsuario,
                    u.nome                                           AS NomeUsuario,
                    COALESCE(SUM(p.pontos), 0)                       AS TotalPontos,
                    COUNT(DISTINCT p.id_envio)                       AS TotalEnvios
                FROM usuario u
                INNER JOIN tipo_usuario tu ON u.id_tipo_usuario = tu.id
                INNER JOIN pontuacao p ON p.id_usuario = u.id
                WHERE tu.descricao = 'Comum'
                  AND u.ativo = TRUE
                GROUP BY u.id, u.nome
                ORDER BY TotalPontos DESC
                LIMIT @Limit OFFSET @Offset;";

            int offset = (pagina - 1) * itensPorPagina;

            var itens = await connection.QueryAsync<RankingItem>(sqlRanking, new
            {
                Limit = itensPorPagina,
                Offset = offset
            });

            return (itens, total);
        }
    }
}