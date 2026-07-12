using System.ComponentModel.DataAnnotations;
using GastosResidenciais.Api.Models;

namespace GastosResidenciais.Api.DTOs;

public class TransacaoDto
{
    public int Id { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public decimal Valor { get; set; }
    public TipoTransacao Tipo { get; set; }
    public int PessoaId { get; set; }
    public string NomePessoa { get; set; } = string.Empty;
}

public class CriarTransacaoDto
{
    [Required(ErrorMessage = "O campo Descricao e obrigatorio.")]
    [MinLength(1, ErrorMessage = "A descricao nao pode ser vazia.")]
    public string Descricao { get; set; } = string.Empty;

    [Required(ErrorMessage = "O campo Valor e obrigatorio.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "O valor deve ser maior que zero.")]
    public decimal Valor { get; set; }

    [Required(ErrorMessage = "O campo Tipo e obrigatorio.")]
    public TipoTransacao Tipo { get; set; }

    [Required(ErrorMessage = "O campo PessoaId e obrigatorio.")]
    public int PessoaId { get; set; }
}
