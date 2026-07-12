using Microsoft.EntityFrameworkCore;
using GastosResidenciais.Api.Data;
using GastosResidenciais.Api.Models;

namespace GastosResidenciais.Api.Repositories;

public class TransacaoRepository : ITransacaoRepository
{
    private readonly AppDbContext _db;

    public TransacaoRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Transacao>> ListarAsync(int? pessoaId = null)
    {
        var query = _db.Transacoes.Include(t => t.Pessoa).AsNoTracking();

        if (pessoaId.HasValue)
            query = query.Where(t => t.PessoaId == pessoaId.Value);

        return await query.ToListAsync();
    }

    public async Task<Transacao> CriarAsync(Transacao transacao)
    {
        _db.Transacoes.Add(transacao);
        await _db.SaveChangesAsync();
        await _db.Entry(transacao).Reference(t => t.Pessoa).LoadAsync();
        return transacao;
    }

    public async Task<IEnumerable<Transacao>> ListarPorPessoaAsync(int pessoaId)
    {
        return await _db.Transacoes
            .AsNoTracking()
            .Where(t => t.PessoaId == pessoaId)
            .ToListAsync();
    }
}
