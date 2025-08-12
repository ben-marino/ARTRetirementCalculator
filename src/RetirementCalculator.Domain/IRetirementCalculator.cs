namespace RetirementCalculator.Domain;

public interface IRetirementCalculator
{
    ProjectionResult CalculateProjection(ProjectionRequest request);
}