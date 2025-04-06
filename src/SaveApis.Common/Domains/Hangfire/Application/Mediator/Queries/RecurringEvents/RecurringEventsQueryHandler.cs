using System.Reflection;
using Autofac;
using FluentResults;
using Hangfire;
using Hangfire.Storage;
using SaveApis.Common.Domains.Core.Infrastructure.Helper;
using SaveApis.Common.Domains.Hangfire.Domain.Dtos;
using SaveApis.Common.Domains.Hangfire.Domain.Types;
using SaveApis.Common.Domains.Hangfire.Infrastructure.Attributes;
using SaveApis.Common.Domains.Hangfire.Infrastructure.Events;
using SaveApis.Common.Domains.Mediator.Infrastructure.Queries;

namespace SaveApis.Common.Domains.Hangfire.Application.Mediator.Queries.RecurringEvents;

public class RecurringEventsQueryHandler(IRecurringJobManagerV2 manager, ITypeHelper helper) : IQueryHandler<RecurringEventsQuery, IReadOnlyCollection<RecurringEventGetDto>>
{
    public Task<Result<IReadOnlyCollection<RecurringEventGetDto>>> Handle(RecurringEventsQuery request, CancellationToken cancellationToken)
    {
        return request.Source switch
        {
            RecurringEventSource.Hangfire => Task.FromResult(GetHangfireRecurringEvents()),
            RecurringEventSource.Code => Task.FromResult(GetCodeRecurringEvents()),
            _ => throw new NotSupportedException($"Unsupported source: {request.Source}"),
        };
    }

    private Result<IReadOnlyCollection<RecurringEventGetDto>> GetHangfireRecurringEvents()
    {
        try
        {
            var recurringJobs = manager.Storage.GetConnection().GetRecurringJobs() ?? [];
            return recurringJobs.ConvertAll(input => new RecurringEventGetDto()
            {
                Cron = input.Cron,
                Key = input.Id,
            });
        }
        catch (Exception e)
        {
            return Result.Fail("Failed to read recurring events from Hangfire").WithError(new ExceptionalError(e));
        }
    }

    private Result<IReadOnlyCollection<RecurringEventGetDto>> GetCodeRecurringEvents()
    {
        try
        {
            var types = helper.GetTypesByAttribute<HangfireRecurringEventAttribute>()
                .Where(it => it.IsAssignableTo<IEvent>())
                .ToList();

            return (from type in types
                    let instance = (IEvent)Activator.CreateInstance(type)!
                    let attribute = type.GetCustomAttribute<HangfireRecurringEventAttribute>()!
                    select new RecurringEventGetDto
                    {
                        Cron = attribute.Cron,
                        Key = attribute.Key,
                        Event = instance,
                    })
                .ToList();
        }
        catch (Exception e)
        {
            return Result.Fail("Failed to read recurring events from code").WithError(new ExceptionalError(e));
        }
    }
}
