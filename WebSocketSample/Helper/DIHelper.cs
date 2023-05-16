using WebSocketSample.Handler;
using WebSocketSample.Interface;
using WebSocketSample.Services;

namespace WebSocketSample.Helper
{
    public static class DIHelper
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient<IWebSocketService, WebSocketService>();
            services.AddSingleton<WebSocketHandler>();

            return services;
        }
    }
}
