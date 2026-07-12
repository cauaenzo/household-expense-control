using GastosResidenciais.Api.Models;

namespace GastosResidenciais.Api.Repositories;

public interface ITransacaoRepository
{
    Task<IEnumerable<Transacao>> ListarAsync(int? pessoaId = null);
    Task<Transacao> CriarAsync(Transacao transacao);
    Task<IEnumerable<Transacao>> ListarPorPessoaAsync(int pessoaId);
}
