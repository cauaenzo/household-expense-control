using GastosResidenciais.Api.Models;

namespace GastosResidenciais.Api.Repositories;

public interface IPessoaRepository
{
    Task<IEnumerable<Pessoa>> ListarAsync();
    Task<Pessoa?> ObterPorIdAsync(int id);
    Task<Pessoa> CriarAsync(Pessoa pessoa);
    Task ExcluirAsync(Pessoa pessoa);
}
