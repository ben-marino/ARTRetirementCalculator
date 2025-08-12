namespace RetirementCalculator.Domain;

public class ProjectionRequest
{
    public int CurrentAge { get; init; }
    public int RetirementAge { get; init; }
    public decimal CurrentBalance { get; init; }
    public decimal AnnualSalary { get; init; }
    public decimal EmployerContributionRate { get; init; } = 11.5m; // Current SG rate
    public decimal SalaryGrowthRate { get; init; } = 3.0m;
    public decimal ExpectedReturnRate { get; init; } = 7.5m;
    public decimal InflationRate { get; init; } = 2.5m;
}