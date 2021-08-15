using Microsoft.Extensions.DependencyInjection;

namespace TraditionalBowlingServices;

public static class ServiceRegistration
{
    public static IServiceCollection RegisterGameServices(this IServiceCollection services)
    {
        services.AddSingleton<IGameService, GameService>();
        services.AddSingleton<IScoreService, ScoreService>();
        services.AddSingleton<IFrameService, FrameService>();
        return services;
    }
}