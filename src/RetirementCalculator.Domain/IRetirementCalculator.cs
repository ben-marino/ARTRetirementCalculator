namespace RetirementCalculator.Domain;

/// <summary>
/// Defines the contract for retirement savings projection calculations.
/// </summary>
public interface IRetirementCalculator
{
    /// <summary>
    /// Calculates retirement savings projections based on the provided parameters.
    /// </summary>
    /// <param name="request">The projection parameters including current age, retirement age, salary, and investment assumptions.</param>
    /// <returns>A comprehensive projection result including final balance, yearly breakdown, and inflation-adjusted values.</returns>
    ProjectionResult CalculateProjection(ProjectionRequest request);
}