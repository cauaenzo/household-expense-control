using Microsoft.AspNetCore.Mvc;
using GastosResidenciais.Api.DTOs;
using GastosResidenciais.Api.Services;

namespace GastosResidenciais.Api.Controllers;

[ApiController]
[Route("api/relatorios")]
public class RelatoriosController : ControllerBase
{
    private readonly IRelatorioService _relatorioService;

    public RelatoriosController(IRelatorioService relatorioService)
    {
        _relatorioService = relatorioService;
    }

    [HttpGet("totais")]
    [ProducesResponseType(typeof(RelatorioTotaisDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObterTotais()
    {
        var relatorio = await _relatorioService.ObterTotaisAsync();
        return Ok(relatorio);
    }
}
