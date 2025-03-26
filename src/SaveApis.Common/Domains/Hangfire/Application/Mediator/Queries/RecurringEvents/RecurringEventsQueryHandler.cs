using System.Reflection;
using Autofac;
using FluentResults;
using Hangfire;
using Hangfire.Storage;
using Microsoft.EntityFrameworkCore;
using SaveApis.Common.Domains.Core.Infrastructure.Helper;
using SaveApis.Common.Domains.Hangfire.Domain.Dtos;
using SaveApis.Common.Domains.Hangfire.Domain.Types;
using SaveApis.Common.Domains.Hangfire.Infrastructure.Attributes;
using SaveApis.Common.Domains.Hangfire.Infrastructure.Events;
using SaveApis.Common.Domains.Hangfire.Persistence.Sql.Factories;
using SaveApis.Common.Domains.Mediator.Infrastructure.Queries;

namespace SaveApis.Common.Domains.Hangfire.Application.Mediator.Queries.RecurringEvents;

public class RecurringEventsQueryHandler(IRecurringJobManagerV2 manager, IHangfireDbContextFactory factory, ITypeHelper helper) : IQueryHandler<RecurringEventsQuery, IReadOnlyCollection<RecurringEventGetDto>>
{
    public async Task<Result<IReadOnlyCollection<RecurringEventGetDto>>> Handle(RecurringEventsQuery request, CancellationToken cancellationToken)
    {
        return request.Source switch
        {
            RecurringEventSource.Hangfire => GetHangfireRecurringEvents(),
            RecurringEventSource.MySql => await GetMySqlRecurringEvents().ConfigureAwait(false),
            RecurringEventSource.Code => GetCodeRecurringEvents(),
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
    private async Task<Result<IReadOnlyCollection<RecurringEventGetDto>>> GetMySqlRecurringEvents()
    {
        try
        {
            await using var context = factory.Create();

            var recurringEvents = await context.RecurringEvents.ToListAsync().ConfigureAwait(false);

            return recurringEvents.ConvertAll(input => new RecurringEventGetDto
            {
                Cron = input.Cron,
                Key = input.Key,
            });
        }
        catch (Exception e)
        {
            return Result.Fail("Failed to read recurring events from MySQL").WithError(new ExceptionalError(e));
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
