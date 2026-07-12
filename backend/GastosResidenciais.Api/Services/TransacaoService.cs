using GastosResidenciais.Api.DTOs;
using GastosResidenciais.Api.Models;
using GastosResidenciais.Api.Repositories;

namespace GastosResidenciais.Api.Services;

public class TransacaoService : ITransacaoService
{
    private readonly ITransacaoRepository _transacaoRepository;
    private readonly IPessoaRepository _pessoaRepository;

    public TransacaoService(ITransacaoRepository transacaoRepository, IPessoaRepository pessoaRepository)
    {
        _transacaoRepository = transacaoRepository;
        _pessoaRepository = pessoaRepository;
    }

    public async Task<IEnumerable<TransacaoDto>> ListarAsync(int? pessoaId = null)
    {
        var transacoes = await _transacaoRepository.ListarAsync(pessoaId);
        return transacoes.Select(t => new TransacaoDto
        {
            Id = t.Id,
            Descricao = t.Descricao,
            Valor = t.Valor,
            Tipo = t.Tipo,
            PessoaId = t.PessoaId,
            NomePessoa = t.Pessoa?.Nome ?? string.Empty
        });
    }

    public async Task<TransacaoDto> CriarAsync(CriarTransacaoDto dto)
    {
        var pessoa = await _pessoaRepository.ObterPorIdAsync(dto.PessoaId);

        if (pessoa is null)
            throw new KeyNotFoundException($"Pessoa com id {dto.PessoaId} nao encontrada.");

        // Menores de idade nao podem registrar receitas
        if (pessoa.Idade < 18 && dto.Tipo == TipoTransacao.Receita)
            throw new InvalidOperationException("Menores de idade nao podem cadastrar receitas.");

        var transacao = new Transacao
        {
            Descricao = dto.Descricao.Trim(),
            Valor = dto.Valor,
            Tipo = dto.Tipo,
            PessoaId = dto.PessoaId
        };

        var criada = await _transacaoRepository.CriarAsync(transacao);

        return new TransacaoDto
        {
            Id = criada.Id,
            Descricao = criada.Descricao,
            Valor = criada.Valor,
            Tipo = criada.Tipo,
            PessoaId = criada.PessoaId,
            NomePessoa = criada.Pessoa?.Nome ?? string.Empty
        };
    }
}
