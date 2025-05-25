using FluentValidation;
using FluentValidation.AspNetCore;
using SmartCharging.Api.Dtos.ChargeStation;
using SmartCharging.Api.Dtos.Connector;
using SmartCharging.Api.Dtos.Group;
using SmartCharging.Api.Models;
using SmartCharging.Api.Repositories;
using SmartCharging.Api.Services.ChargeStations;
using SmartCharging.Api.Services.Connectors;
using SmartCharging.Api.Services.Groups;
using SmartCharging.Api.Validators;

namespace SmartCharging.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            services
                .AddScoped<IValidator<CreateGroup>, CreateGroupValidator>()
                .AddScoped<IValidator<CreateChargeStation>, CreateChargeStationValidator>()
                .AddScoped<IValidator<CreateConnector>, CreateConnectorValidator>()
                .AddScoped<IValidator<UpdateGroup>, UpdateGroupValidator>()
                .AddScoped<IValidator<UpdateChargeStation>, UpdateChargeStationValidator>()
                .AddScoped<IValidator<UpdateConnector>, UpdateConnectorValidator>()
                .AddFluentValidationAutoValidation();

            return services;
        }

        public static IServiceCollection AddAppServices(this IServiceCollection services)
        {
            services
                .AddTransient<IGroupService, GroupService>()
                .AddTransient<IChargeStationService, ChargeStationService>()
                .AddTransient<IConnectorService, ConnectorService>();

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services
                .AddTransient<IRepository<Group>, Repository<Group>>()
                .AddTransient<IRepository<ChargeStation>, Repository<ChargeStation>>()
                .AddTransient<IRepository<Connector>, Repository<Connector>>();

            return services;
        }
    }
}
