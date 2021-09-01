using System.Threading.Tasks;
using Heus.Ddd.Application.Dtos;

namespace Heus.Ddd.Application.Services
{

    public interface IReadOnlyAppService<TEntityDto, in TKey, in TGetListInput>
        : IReadOnlyAppService<TEntityDto, TKey, TGetListInput, TEntityDto>
    {

    }

    public interface IReadOnlyAppService<TGetOutputDto, in TKey, in TGetListInput, TGetListOutputDto>
        : IAppService
    {
        Task<TGetOutputDto> GetAsync(TKey id);
        Task<PagedResultDto<TGetListOutputDto>> GetListAsync(TGetListInput input);
    }
}