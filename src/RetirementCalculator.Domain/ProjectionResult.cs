namespace RetirementCalculator.Domain;

public class ProjectionResult
{
    public decimal FinalBalance { get; init; }
    public decimal TotalContributions { get; init; }
    public decimal TotalEarnings { get; init; }
    public decimal InflationAdjustedBalance { get; init; }
    public List<YearlyProjection> YearlyBreakdown { get; init; } = new List<YearlyProjection>();
}