using GastosResidenciais.Api.DTOs;
using GastosResidenciais.Api.Models;
using GastosResidenciais.Api.Repositories;

namespace GastosResidenciais.Api.Services;

public class RelatorioService : IRelatorioService
{
    private readonly IPessoaRepository _pessoaRepository;
    private readonly ITransacaoRepository _transacaoRepository;

    public RelatorioService(IPessoaRepository pessoaRepository, ITransacaoRepository transacaoRepository)
    {
        _pessoaRepository = pessoaRepository;
        _transacaoRepository = transacaoRepository;
    }

    public async Task<RelatorioTotaisDto> ObterTotaisAsync()
    {
        var pessoas = await _pessoaRepository.ListarAsync();
        var todasTransacoes = await _transacaoRepository.ListarAsync();

        var totaisPorPessoa = pessoas.Select(p =>
        {
            var transacoesDaPessoa = todasTransacoes.Where(t => t.PessoaId == p.Id);
            var receitas = transacoesDaPessoa.Where(t => t.Tipo == TipoTransacao.Receita).Sum(t => t.Valor);
            var despesas = transacoesDaPessoa.Where(t => t.Tipo == TipoTransacao.Despesa).Sum(t => t.Valor);

            return new TotaisPessoaDto
            {
                Id = p.Id,
                Nome = p.Nome,
                TotalReceitas = receitas,
                TotalDespesas = despesas,
                Saldo = receitas - despesas
            };
        }).ToList();

        var totalReceitas = totaisPorPessoa.Sum(t => t.TotalReceitas);
        var totalDespesas = totaisPorPessoa.Sum(t => t.TotalDespesas);

        return new RelatorioTotaisDto
        {
            Pessoas = totaisPorPessoa,
            TotalGeral = new TotalGeralDto
            {
                TotalReceitas = totalReceitas,
                TotalDespesas = totalDespesas,
                SaldoLiquido = totalReceitas - totalDespesas
            }
        };
    }
}
