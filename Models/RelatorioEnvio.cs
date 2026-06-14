namespace ApiRelatorio.Models
{
    public class RelatorioEnvio
    {
        public int TotalEnvios { get; set; }
        public int TotalItens { get; set; }
        public int EnviosConcluidos { get; set; }
        public int EnviosPendentes { get; set; }
        public int EnviosCancelados { get; set; }
    }
}
