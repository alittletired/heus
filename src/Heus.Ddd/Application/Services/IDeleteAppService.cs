using System.Threading.Tasks;

namespace Heus.Ddd.Application.Services
{
    public interface IDeleteAppService<in TKey> : IAppService
    {
        Task DeleteAsync(TKey id);
    }
}