using Microsoft.AspNetCore.Builder;

namespace Registration.Api.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseCustomizedSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/v1/swagger.json", $"v1");

                c.SwaggerEndpoint($"/swagger/v2/swagger.json", $"v2");
            });
        }
    }
}
