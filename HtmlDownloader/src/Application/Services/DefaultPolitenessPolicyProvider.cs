using Domain.Interfaces;

namespace Application.Services;

public class DefaultPolitenessPolicyProvider : IPolitenessPolicyProvider
{
    public async Task ApplyPolicy()
    {
        await Task.Delay(1000);
    }
}
