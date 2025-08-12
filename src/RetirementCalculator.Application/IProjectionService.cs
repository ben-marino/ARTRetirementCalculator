using RetirementCalculator.Domain;

namespace RetirementCalculator.Application;

/// <summary>
/// Application service for retirement projection calculations with validation and error handling.
/// </summary>
public interface IProjectionService
{
    /// <summary>
    /// Calculates retirement savings projections with comprehensive input validation and error handling.
    /// </summary>
    /// <param name="request">The projection request containing user parameters and assumptions.</param>
    /// <returns>A service result containing either the projection results or validation errors.</returns>
    Task<ServiceResult<ProjectionResult>> CalculateProjectionAsync(ProjectionRequest request);
}