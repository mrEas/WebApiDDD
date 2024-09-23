using App.Application.Common.Validation;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace App.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            //Регистрируем все обработчики запросов Request Handlers находящиеся в сборке
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssemblyContaining<ApplicationAssemblyReference>();
            });

            //Перед тем как в Application.Customers.Commands метод Handle() придет Request,
            //он будет проверен валидатором Application.Customers.Commands.CreateCustomerCommandValidator

            //Подключили кастомные пайплайны
            services.AddScoped(typeof(IPipelineBehavior<,>),typeof(ValidationBehavior<,>));

            //Подключили валидаторы сборки
            services.AddValidatorsFromAssemblyContaining<ApplicationAssemblyReference>();
            
            return services;
        }
    }
}
