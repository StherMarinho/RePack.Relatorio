namespace ApiRelatorio.Models
{
    public class RelatorioPontuacao
    {
        public double MediaPontosPorUsuario { get; set; }
        public int TotalPontosDistribuidos { get; set; }
        public int UsuariosComPontuacao { get; set; }
        public int MaiorPontuacao { get; set; }
        public int MenorPontuacao { get; set; }
    }
}
