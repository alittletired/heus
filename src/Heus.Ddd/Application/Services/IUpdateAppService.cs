using System.Threading.Tasks;

namespace Heus.Ddd.Application.Services
{
    public interface IUpdateAppService<TUpdateOutput,  in TUpdateInput>
        : IAppService
    {
        Task<TUpdateOutput> UpdateAsync(TUpdateInput input);
    }
}