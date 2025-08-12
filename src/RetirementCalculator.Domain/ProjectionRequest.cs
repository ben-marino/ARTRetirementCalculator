namespace RetirementCalculator.Domain;

/// <summary>
/// Represents the input parameters for a retirement savings projection calculation.
/// Contains demographic information, financial details, and economic assumptions.
/// </summary>
public class ProjectionRequest
{
    /// <summary>
    /// The individual's current age in years. Must be between 18 and 100.
    /// </summary>
    public int CurrentAge { get; init; }
    
    /// <summary>
    /// The planned retirement age in years. Must be greater than current age and between 55 and 100.
    /// </summary>
    public int RetirementAge { get; init; }
    
    /// <summary>
    /// The current superannuation balance in Australian dollars. Must be non-negative.
    /// </summary>
    public decimal CurrentBalance { get; init; }
    
    /// <summary>
    /// The current annual salary in Australian dollars. Must be positive.
    /// </summary>
    public decimal AnnualSalary { get; init; }
    
    /// <summary>
    /// The employer contribution rate as a percentage of salary. Defaults to 11.5% (current Super Guarantee rate).
    /// </summary>
    public decimal EmployerContributionRate { get; init; } = 11.5m;
    
    /// <summary>
    /// The expected annual salary growth rate as a percentage. Defaults to 3.0%.
    /// </summary>
    public decimal SalaryGrowthRate { get; init; } = 3.0m;
    
    /// <summary>
    /// The expected annual investment return rate as a percentage. Defaults to 7.5% (balanced option).
    /// </summary>
    public decimal ExpectedReturnRate { get; init; } = 7.5m;
    
    /// <summary>
    /// The expected annual inflation rate as a percentage. Defaults to 2.5% (RBA target).
    /// </summary>
    public decimal InflationRate { get; init; } = 2.5m;
}