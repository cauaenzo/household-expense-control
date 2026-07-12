using GastosResidenciais.Api.DTOs;

namespace GastosResidenciais.Api.Services;

public interface ITransacaoService
{
    Task<IEnumerable<TransacaoDto>> ListarAsync(int? pessoaId = null);
    Task<TransacaoDto> CriarAsync(CriarTransacaoDto dto);
}
