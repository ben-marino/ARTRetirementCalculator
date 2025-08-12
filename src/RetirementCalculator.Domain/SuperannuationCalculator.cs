namespace RetirementCalculator.Domain;

/// <summary>
/// Implements Australian superannuation retirement savings calculations with accurate tax and contribution modeling.
/// </summary>
public class SuperannuationCalculator : IRetirementCalculator
{
    /// <summary>
    /// Tax rate applied to concessional superannuation contributions (15%).
    /// </summary>
    private const decimal CONTRIBUTION_TAX = 0.15m;
    
    /// <summary>
    /// Annual cap for concessional superannuation contributions in Australian dollars.
    /// </summary>
    private const decimal CONCESSIONAL_CAP = 27500m;
    
    /// <summary>
    /// Calculates comprehensive retirement savings projections using Australian superannuation rules.
    /// Includes employer contributions, contribution caps, tax implications, and compound growth.
    /// </summary>
    /// <param name="request">The projection parameters including demographics, financial details, and growth assumptions.</param>
    /// <returns>Detailed projection results with yearly breakdown and inflation-adjusted values.</returns>
    public ProjectionResult CalculateProjection(ProjectionRequest request)
    {
        var yearlyProjections = new List<YearlyProjection>();
        var currentBalance = request.CurrentBalance;
        var currentSalary = request.AnnualSalary;
        var totalContributions = 0m;
        
        for (int year = 1; year <= request.RetirementAge - request.CurrentAge; year++)
        {
            // Calculate employer contribution
            var employerContribution = currentSalary * (request.EmployerContributionRate / 100);
            
            // Apply contribution cap
            employerContribution = Math.Min(employerContribution, CONCESSIONAL_CAP);
            
            // Apply contributions tax
            var netContribution = employerContribution * (1 - CONTRIBUTION_TAX);
            
            // Calculate earnings
            var earnings = (currentBalance + netContribution / 2) * (request.ExpectedReturnRate / 100);
            
            // Update balance
            var closingBalance = currentBalance + netContribution + earnings;
            
            yearlyProjections.Add(new YearlyProjection
            {
                Year = year,
                Age = request.CurrentAge + year,
                OpeningBalance = currentBalance,
                Contributions = netContribution,
                Earnings = earnings,
                ClosingBalance = closingBalance
            });
            
            totalContributions += netContribution;
            currentBalance = closingBalance;
            currentSalary *= (1 + request.SalaryGrowthRate / 100);
        }
        
        // Calculate inflation-adjusted balance
        var yearsToRetirement = request.RetirementAge - request.CurrentAge;
        var inflationAdjustedBalance = currentBalance / (decimal)Math.Pow(1 + (double)(request.InflationRate / 100), yearsToRetirement);
        
        return new ProjectionResult
        {
            FinalBalance = currentBalance,
            TotalContributions = totalContributions,
            TotalEarnings = currentBalance - request.CurrentBalance - totalContributions,
            InflationAdjustedBalance = inflationAdjustedBalance,
            YearlyBreakdown = yearlyProjections
        };
    }
}