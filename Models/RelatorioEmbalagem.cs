namespace ApiRelatorio.Models
{
    public class RelatorioEmbalagem
    {
        public string NomeMaterial { get; set; } = string.Empty;
        public string DescricaoEmbalagem { get; set; } = string.Empty;
        public int QuantidadeRecebida { get; set; }
        public decimal PesoTotalGramas { get; set; }
    }
}
