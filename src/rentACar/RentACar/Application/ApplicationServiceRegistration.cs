﻿using Core.Application.Pipelines.Caching;
using Core.Application.Pipelines.Transaction;
using Core.Application.Pipelines.Validation;
using Core.Application.Rules;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddSubClassesOfType(Assembly.GetExecutingAssembly(),typeof(BaseBusinessRules));
        services.AddMediatR(configuation =>
        {
            configuation.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            configuation.AddOpenBehavior(typeof(RequstValidationBehavior<,>)); 
            configuation.AddOpenBehavior(typeof(TransactionScoperBehavior<,>)); 
            configuation.AddOpenBehavior(typeof(CachingBehavior<,>)); 
        });
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;    
    }
    public static IServiceCollection AddSubClassesOfType(
       this IServiceCollection services,
       Assembly assembly,
       Type type,
       Func<IServiceCollection, Type, IServiceCollection>? addWithLifeCycle = null
   )
    {
        var types = assembly.GetTypes().Where(t => t.IsSubclassOf(type) && type != t).ToList();
        foreach (var item in types)
            if (addWithLifeCycle == null)
                services.AddScoped(item);

            else
                addWithLifeCycle(services, type);
        return services;
    }
}
