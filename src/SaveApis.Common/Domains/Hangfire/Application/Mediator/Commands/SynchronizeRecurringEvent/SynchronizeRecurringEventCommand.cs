using SaveApis.Common.Domains.Hangfire.Domain.Dtos;
using SaveApis.Common.Domains.Mediator.Infrastructure.Commands;

namespace SaveApis.Common.Domains.Hangfire.Application.Mediator.Commands.SynchronizeRecurringEvent;

public record SynchronizeRecurringEventCommand(RecurringEventSummaryDto Summary) : ICommand;
