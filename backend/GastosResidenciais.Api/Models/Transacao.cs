using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GastosResidenciais.Api.Models;

public enum TipoTransacao
{
    Receita,
    Despesa
}

public class Transacao
{
    public int Id { get; set; }

    [Required]
    public string Descricao { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18,2)")]
    public decimal Valor { get; set; }

    public TipoTransacao Tipo { get; set; }

    public int PessoaId { get; set; }

    public Pessoa Pessoa { get; set; } = null!;
}
