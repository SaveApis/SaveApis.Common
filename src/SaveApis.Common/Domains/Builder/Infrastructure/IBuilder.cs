namespace SaveApis.Common.Domains.Builder.Infrastructure;

public interface IBuilder<TResult>
{
    TResult Build();
    Task<TResult> BuildAsync();
}
