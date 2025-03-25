using SaveApis.Common.Domains.Core.Domain.VOs;
using SaveApis.Common.Domains.Mediator.Infrastructure.Commands;
using SaveApis.Common.Domains.Tracking.Domain.Types;

namespace SaveApis.Common.Domains.Tracking.Application.Mediator.Commands.CreateTracking;

public record CreateTrackingCommand(Id AffectedEntityId, TrackingType TrackingType) : ICommand<Id>
{
    private readonly ICollection<(string propertyName, string? oldValue, string? newValue)> _values = [];

    public IReadOnlyCollection<(string propertyName, string? oldValue, string? newValue)> Values => _values.ToList();

    public void AddValue(string propertyName, string? oldValue, string? newValue)
    {
        _values.Add((propertyName, oldValue, newValue));
    }
}
