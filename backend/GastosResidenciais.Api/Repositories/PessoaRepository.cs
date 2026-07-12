using Microsoft.EntityFrameworkCore;
using GastosResidenciais.Api.Data;
using GastosResidenciais.Api.Models;

namespace GastosResidenciais.Api.Repositories;

public class PessoaRepository : IPessoaRepository
{
    private readonly AppDbContext _db;

    public PessoaRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Pessoa>> ListarAsync()
    {
        return await _db.Pessoas.AsNoTracking().ToListAsync();
    }

    public async Task<Pessoa?> ObterPorIdAsync(int id)
    {
        return await _db.Pessoas.FindAsync(id);
    }

    public async Task<Pessoa> CriarAsync(Pessoa pessoa)
    {
        _db.Pessoas.Add(pessoa);
        await _db.SaveChangesAsync();
        return pessoa;
    }

    public async Task ExcluirAsync(Pessoa pessoa)
    {
        _db.Pessoas.Remove(pessoa);
        await _db.SaveChangesAsync();
    }
}
