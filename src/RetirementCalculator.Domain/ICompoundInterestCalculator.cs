namespace RetirementCalculator.Domain;

/// <summary>
/// Provides mathematical calculations for compound interest with regular contributions.
/// </summary>
public interface ICompoundInterestCalculator
{
    /// <summary>
    /// Calculates the future value of an investment with compound interest and regular annual contributions.
    /// </summary>
    /// <param name="principal">The initial investment amount.</param>
    /// <param name="rate">The annual interest rate as a percentage (e.g., 7.5 for 7.5%).</param>
    /// <param name="years">The number of years for the investment to grow.</param>
    /// <param name="annualContribution">The amount contributed annually to the investment.</param>
    /// <returns>The total future value of the investment including principal, compound interest, and contributions.</returns>
    decimal Calculate(decimal principal, decimal rate, int years, decimal annualContribution);
}