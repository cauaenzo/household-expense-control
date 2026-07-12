using GastosResidenciais.Api.DTOs;
using GastosResidenciais.Api.Models;
using GastosResidenciais.Api.Repositories;

namespace GastosResidenciais.Api.Services;

public class PessoaService : IPessoaService
{
    private readonly IPessoaRepository _pessoaRepository;

    public PessoaService(IPessoaRepository pessoaRepository)
    {
        _pessoaRepository = pessoaRepository;
    }

    public async Task<IEnumerable<PessoaDto>> ListarAsync()
    {
        var pessoas = await _pessoaRepository.ListarAsync();
        return pessoas.Select(p => new PessoaDto
        {
            Id = p.Id,
            Nome = p.Nome,
            Idade = p.Idade
        });
    }

    public async Task<PessoaDto> CriarAsync(CriarPessoaDto dto)
    {
        var pessoa = new Pessoa
        {
            Nome = dto.Nome.Trim(),
            Idade = dto.Idade
        };

        var criada = await _pessoaRepository.CriarAsync(pessoa);

        return new PessoaDto
        {
            Id = criada.Id,
            Nome = criada.Nome,
            Idade = criada.Idade
        };
    }

    public async Task ExcluirAsync(int id)
    {
        var pessoa = await _pessoaRepository.ObterPorIdAsync(id);

        if (pessoa is null)
            throw new KeyNotFoundException($"Pessoa com id {id} nao encontrada.");

        await _pessoaRepository.ExcluirAsync(pessoa);
    }
}
