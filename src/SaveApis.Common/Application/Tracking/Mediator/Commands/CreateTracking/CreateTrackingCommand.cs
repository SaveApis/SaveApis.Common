using SaveApis.Common.Domain.Tracking.Types;
using SaveApis.Common.Domain.VOs;
using SaveApis.Common.Infrastructure.Mediator;

namespace SaveApis.Common.Application.Tracking.Mediator.Commands.CreateTracking;

public record CreateTrackingCommand(Id AffectedEntityId, TrackingType TrackingType) : ICommand<Id>
{
    private readonly ICollection<(string propertyName, string? oldValue, string? newValue)> _values = [];

    public IReadOnlyCollection<(string propertyName, string? oldValue, string? newValue)> Values => _values.ToList();

    public void AddValue(string propertyName, string? oldValue, string? newValue)
    {
        _values.Add((propertyName, oldValue, newValue));
    }
}
