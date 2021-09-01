using Heus.AspNetCore;

namespace Heus.Web
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            HeusWebApplication.Run(args,typeof(HeusWebModule));
        }
    }
}