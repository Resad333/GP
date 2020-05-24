using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Registration.Core.Abstraction;
using Registration.Core.Database;
using Registration.Core.Logging;
using Registration.Core.Mapper;
using Registration.Core.Repository;
using Registration.Core.Service;
using Registration.Core.Validator;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

namespace Registration.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddBackendComponents(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddSingleton<ILogger, NLogFileLogger>();

            //services.AddDbContext<CustomerDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("CustomerDbConnection")));
            services.AddDbContext<CustomerDbContext>(opt => opt.UseInMemoryDatabase());
            services.AddTransient<ICustomerRepository, CustomerRepository>();
            services.AddTransient<ICustomerService, CustomerService>();

            services.AddSingleton<IRedbetCustomerRequestMapper, RedbetCustomerRequestMapper>();
            services.AddSingleton<IMrgreenCustomerRequestMapper, MrgreenCustomerRequestMapper>();

            services.AddSingleton<IRedbetCustomerRequestValidator, RedbetCustomerRequestValidator>();
            services.AddSingleton<IMrgreenCustomerRequestValidator, MrgreenCustomerRequestValidator>();
        }

        public static void AddCustomizedSwagger(this IServiceCollection services)
        {

            services.AddApiVersioning(o =>
            {
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new ApiVersion(1, 0);
            });

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1",
                    new Info
                    {
                        Version = "v1",
                        Title = "v1 API",
                        Description = "v1 API Description",
                        TermsOfService = "Terms of usage v1"
                    });

                // Add a SwaggerDoc for v2 
                options.SwaggerDoc("v2",
                    new Info
                    {
                        Version = "v2",
                        Title = "v2 API",
                        Description = "v2 API Description",
                        TermsOfService = "Terms of usage v3"
                    });

                // Apply the filters
                options.OperationFilter<RemoveVersionFromParameter>();
                options.DocumentFilter<ReplaceVersionWithExactValueInPath>();

                // Ensure the routes are added to the right Swagger doc
                options.DocInclusionPredicate((version, desc) =>
                {
                    var versions = desc.ControllerAttributes()
                        .OfType<ApiVersionAttribute>()
                        .SelectMany(attr => attr.Versions);

                    var maps = desc.ActionAttributes()
                        .OfType<MapToApiVersionAttribute>()
                        .SelectMany(attr => attr.Versions)
                        .ToArray();

                    return versions.Any(v => $"v{v.ToString()}" == version) && (!maps.Any() || maps.Any(v => $"v{v.ToString()}" == version)); ;
                });

            });
        }
    }


    public class RemoveVersionFromParameter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            var versionParameter = operation.Parameters.Single(p => p.Name == "version");
            operation.Parameters.Remove(versionParameter);
        }
    }
    public class ReplaceVersionWithExactValueInPath : IDocumentFilter
    {
        public void Apply(SwaggerDocument swaggerDoc, DocumentFilterContext context)
        {
            swaggerDoc.Paths = swaggerDoc.Paths
                .ToDictionary(
                    path => path.Key.Replace("v{version}", swaggerDoc.Info.Version),
                    path => path.Value
                );
        }
    }
}
