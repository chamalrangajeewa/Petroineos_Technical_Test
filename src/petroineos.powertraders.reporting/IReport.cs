namespace petroineos.powertraders.reporting
{
    public interface IReport
    {
        Task GenerateAsync(CancellationToken cancellationToken = default);
    }
}