using FluentAssertions;
using RetirementCalculator.Domain;

namespace RetirementCalculator.Tests;

public class CompoundInterestCalculatorTests
{
    private readonly ICompoundInterestCalculator _calculator = new CompoundInterestCalculator();
    
    [Fact]
    public void Calculate_WithNoPrincipalAndNoContributions_ReturnsZero()
    {
        // Act
        var result = _calculator.Calculate(principal: 0, rate: 7.5m, years: 10, annualContribution: 0);
        
        // Assert
        result.Should().Be(0);
    }
    
    [Fact]
    public void Calculate_WithPrincipalOnly_ReturnsCompoundedPrincipal()
    {
        // Arrange
        decimal principal = 10000;
        decimal rate = 8.0m; // 8% annual return
        int years = 10;
        
        // Act
        var result = _calculator.Calculate(principal, rate, years, annualContribution: 0);
        
        // Assert
        // Expected: 10000 * (1.08)^10 = 21,589.25
        var expected = 10000 * (decimal)Math.Pow(1.08, 10);
        result.Should().BeApproximately(expected, 0.01m);
    }
    
    [Fact]
    public void Calculate_WithContributionsOnly_ReturnsAnnuityValue()
    {
        // Arrange
        decimal annualContribution = 5000;
        decimal rate = 6.0m; // 6% annual return
        int years = 5;
        
        // Act
        var result = _calculator.Calculate(principal: 0, rate, years, annualContribution);
        
        // Assert
        // Formula: PMT * (((1 + r)^n - 1) / r)
        // Expected: 5000 * (((1.06)^5 - 1) / 0.06) = 28,185.46
        var expected = annualContribution * (((decimal)Math.Pow(1.06, 5) - 1) / 0.06m);
        result.Should().BeApproximately(expected, 0.01m);
    }
    
    [Fact]
    public void Calculate_WithBothPrincipalAndContributions_ReturnsCombinedValue()
    {
        // Arrange
        decimal principal = 25000;
        decimal annualContribution = 8000;
        decimal rate = 7.0m;
        int years = 15;
        
        // Act
        var result = _calculator.Calculate(principal, rate, years, annualContribution);
        
        // Assert
        // Should be greater than principal alone or contributions alone
        var principalOnly = _calculator.Calculate(principal, rate, years, 0);
        var contributionsOnly = _calculator.Calculate(0, rate, years, annualContribution);
        
        result.Should().BeGreaterThan(principalOnly);
        result.Should().BeGreaterThan(contributionsOnly);
        result.Should().BeApproximately(principalOnly + contributionsOnly, 0.01m);
    }
    
    [Fact]
    public void Calculate_WithZeroInterestRate_ReturnsLinearGrowth()
    {
        // Arrange
        decimal principal = 10000;
        decimal annualContribution = 5000;
        decimal rate = 0; // No interest
        int years = 10;
        
        // Act
        var result = _calculator.Calculate(principal, rate, years, annualContribution);
        
        // Assert
        // With 0% interest: principal + (contribution * years)
        var expected = principal + (annualContribution * years);
        result.Should().Be(expected);
    }
    
    [Theory]
    [InlineData(5.0, 10, 16288.95)] // Conservative return
    [InlineData(7.5, 10, 20610.32)] // Balanced return  
    [InlineData(10.0, 10, 25937.42)] // Growth return
    public void Calculate_WithDifferentReturnRates_ReturnsExpectedValues(
        double rate, int years, double expectedApproximate)
    {
        // Arrange
        decimal principal = 10000;
        decimal annualContribution = 0;
        
        // Act
        var result = _calculator.Calculate(principal, (decimal)rate, years, annualContribution);
        
        // Assert
        result.Should().BeApproximately((decimal)expectedApproximate, 1m);
    }
    
    [Theory]
    [InlineData(5)] 
    [InlineData(20)]
    [InlineData(40)]
    public void Calculate_WithDifferentTimeHorizons_ShowsCompoundingEffect(int years)
    {
        // Arrange
        decimal principal = 50000;
        decimal rate = 8.0m;
        decimal annualContribution = 10000;
        
        // Act
        var result = _calculator.Calculate(principal, rate, years, annualContribution);
        
        // Assert
        result.Should().BeGreaterThan(principal);
        
        // Longer time periods should result in exponentially larger values due to compounding
        if (years >= 20)
        {
            var shorterResult = _calculator.Calculate(principal, rate, 10, annualContribution);
            result.Should().BeGreaterThan(shorterResult * 2); // Should be more than double after 20+ years
        }
    }
    
    [Fact]
    public void Calculate_WithHighContributions_ShowsContributionImpact()
    {
        // Arrange
        decimal principal = 25000;
        decimal rate = 7.5m;
        int years = 25;
        
        var lowContribution = 5000m;
        var highContribution = 15000m;
        
        // Act
        var lowResult = _calculator.Calculate(principal, rate, years, lowContribution);
        var highResult = _calculator.Calculate(principal, rate, years, highContribution);
        
        // Assert
        highResult.Should().BeGreaterThan(lowResult);
        
        // The difference should be significant (more than 3x the contribution difference due to compounding)
        var contributionDifference = highContribution - lowContribution;
        var resultDifference = highResult - lowResult;
        resultDifference.Should().BeGreaterThan(contributionDifference * years);
    }
    
    [Fact]
    public void Calculate_WithNegativeValues_HandlesGracefully()
    {
        // These scenarios test edge cases that might occur in real-world data
        
        // Negative principal (debt scenario)
        var result1 = _calculator.Calculate(principal: -5000, rate: 7.5m, years: 10, annualContribution: 8000);
        result1.Should().BeGreaterThan(0); // Should still be positive due to contributions
        
        // This test verifies the calculator handles unusual but mathematically valid scenarios
    }
    
    [Theory]
    [InlineData(1000, 5.0, 1, 0, 1050.00)] // One year simple case
    [InlineData(2000, 10.0, 2, 0, 2420.00)] // Two year compounding
    [InlineData(0, 5.0, 3, 1000, 3152.50)] // Annuity only
    public void Calculate_WithKnownValues_ReturnsExpectedResults(
        decimal principal, decimal rate, int years, decimal contribution, decimal expected)
    {
        // Act
        var result = _calculator.Calculate(principal, rate, years, contribution);
        
        // Assert
        result.Should().BeApproximately(expected, 0.50m); // Allow small rounding differences
    }
}