namespace RetirementCalculator.Domain;

public class YearlyProjection
{
    public int Year { get; init; }
    public int Age { get; init; }
    public decimal OpeningBalance { get; init; }
    public decimal Contributions { get; init; }
    public decimal Earnings { get; init; }
    public decimal ClosingBalance { get; init; }
}