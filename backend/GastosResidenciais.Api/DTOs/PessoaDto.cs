using System.ComponentModel.DataAnnotations;

namespace GastosResidenciais.Api.DTOs;

public class PessoaDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public int Idade { get; set; }
}

public class CriarPessoaDto
{
    [Required(ErrorMessage = "O campo Nome e obrigatorio.")]
    [MinLength(1, ErrorMessage = "O nome nao pode ser vazio.")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "O campo Idade e obrigatorio.")]
    [Range(0, int.MaxValue, ErrorMessage = "A idade deve ser um valor nao negativo.")]
    public int Idade { get; set; }
}
