using System.Threading.Tasks;

namespace Heus.DynamicProxy
{
	public abstract class Interceptor : IInterceptor
    {
        public abstract Task InterceptAsync(IAbpMethodInvocation invocation);
    }
}