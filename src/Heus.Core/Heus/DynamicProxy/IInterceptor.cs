using System.Threading.Tasks;

namespace Heus.DynamicProxy
{
	public interface IInterceptor
    {
        Task InterceptAsync(IAbpMethodInvocation invocation);
	}
}
