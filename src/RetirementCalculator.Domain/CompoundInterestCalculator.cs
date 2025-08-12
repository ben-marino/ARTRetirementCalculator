namespace RetirementCalculator.Domain;

public class CompoundInterestCalculator : ICompoundInterestCalculator
{
    public decimal Calculate(decimal principal, decimal rate, int years, decimal annualContribution)
    {
        var futureValuePrincipal = principal * (decimal)Math.Pow(1 + (double)(rate / 100), years);
        
        var futureValueContributions = 0m;
        if (rate != 0)
        {
            futureValueContributions = annualContribution * 
                (((decimal)Math.Pow(1 + (double)(rate / 100), years) - 1) / (rate / 100));
        }
        else
        {
            futureValueContributions = annualContribution * years;
        }
        
        return futureValuePrincipal + futureValueContributions;
    }
}