using Microsoft.Extensions.Options;

namespace Api.Core.Setups;

public class JwtOptionsSetup(IConfiguration configuration) : IConfigureOptions<JwtOptions>
{
    public void Configure(JwtOptions jwtOptions)
    {
        configuration.GetSection("JwtOptions").Bind(jwtOptions);
    }
}