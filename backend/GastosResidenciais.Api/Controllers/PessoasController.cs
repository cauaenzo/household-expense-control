using Microsoft.AspNetCore.Mvc;
using GastosResidenciais.Api.DTOs;
using GastosResidenciais.Api.Services;

namespace GastosResidenciais.Api.Controllers;

[ApiController]
[Route("api/pessoas")]
public class PessoasController : ControllerBase
{
    private readonly IPessoaService _pessoaService;

    public PessoasController(IPessoaService pessoaService)
    {
        _pessoaService = pessoaService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PessoaDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Listar()
    {
        var pessoas = await _pessoaService.ListarAsync();
        return Ok(pessoas);
    }

    [HttpPost]
    [ProducesResponseType(typeof(PessoaDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Criar([FromBody] CriarPessoaDto dto)
    {
        var criada = await _pessoaService.CriarAsync(dto);
        return CreatedAtAction(nameof(Listar), new { id = criada.Id }, criada);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Excluir(int id)
    {
        await _pessoaService.ExcluirAsync(id);
        return NoContent();
    }
}
