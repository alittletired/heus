using System.Threading.Tasks;

namespace Heus.Ddd.Application.Services
{
    public interface ICreateAppService<TCreateOutput, in TCreateInput>
        : IAppService
    {
        Task<TCreateOutput> CreateAsync(TCreateInput input);
    }
}