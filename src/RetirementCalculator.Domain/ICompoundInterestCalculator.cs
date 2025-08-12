namespace RetirementCalculator.Domain;

public interface ICompoundInterestCalculator
{
    decimal Calculate(decimal principal, decimal rate, int years, decimal annualContribution);
}