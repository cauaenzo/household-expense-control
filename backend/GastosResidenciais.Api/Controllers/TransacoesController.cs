using Microsoft.AspNetCore.Mvc;
using GastosResidenciais.Api.DTOs;
using GastosResidenciais.Api.Services;

namespace GastosResidenciais.Api.Controllers;

[ApiController]
[Route("api/transacoes")]
public class TransacoesController : ControllerBase
{
    private readonly ITransacaoService _transacaoService;

    public TransacoesController(ITransacaoService transacaoService)
    {
        _transacaoService = transacaoService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<TransacaoDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Listar([FromQuery] int? pessoaId)
    {
        var transacoes = await _transacaoService.ListarAsync(pessoaId);
        return Ok(transacoes);
    }

    [HttpPost]
    [ProducesResponseType(typeof(TransacaoDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Criar([FromBody] CriarTransacaoDto dto)
    {
        var criada = await _transacaoService.CriarAsync(dto);
        return CreatedAtAction(nameof(Listar), new { id = criada.Id }, criada);
    }
}
