namespace RetirementCalculator.Domain;

public class SuperannuationCalculator : IRetirementCalculator
{
    private const decimal CONTRIBUTION_TAX = 0.15m;
    private const decimal CONCESSIONAL_CAP = 27500m;
    
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