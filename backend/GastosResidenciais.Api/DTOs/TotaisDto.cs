namespace GastosResidenciais.Api.DTOs;

public class TotaisPessoaDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public decimal TotalReceitas { get; set; }
    public decimal TotalDespesas { get; set; }
    public decimal Saldo { get; set; }
}

public class TotalGeralDto
{
    public decimal TotalReceitas { get; set; }
    public decimal TotalDespesas { get; set; }
    public decimal SaldoLiquido { get; set; }
}

public class RelatorioTotaisDto
{
    public IEnumerable<TotaisPessoaDto> Pessoas { get; set; } = new List<TotaisPessoaDto>();
    public TotalGeralDto TotalGeral { get; set; } = new TotalGeralDto();
}
