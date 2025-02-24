namespace SaveApis.Common.Infrastructure.Builder;

public interface IBuilder<TResult>
{
    TResult Build();
    Task<TResult> BuildAsync();
}
