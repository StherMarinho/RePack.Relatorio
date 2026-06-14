namespace ApiRelatorio.Models
{
    public class RankingItem
    {
        public int Posicao { get; set; }
        public int IdUsuario { get; set; }
        public string NomeUsuario { get; set; } = string.Empty;
        public int TotalPontos { get; set; }
        public int TotalEnvios { get; set; }
    }
}