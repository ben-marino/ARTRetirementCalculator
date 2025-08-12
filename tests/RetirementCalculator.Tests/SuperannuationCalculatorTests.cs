using FluentAssertions;
using RetirementCalculator.Domain;

namespace RetirementCalculator.Tests;

public class SuperannuationCalculatorTests
{
    private readonly IRetirementCalculator _calculator = new SuperannuationCalculator();
    
    [Fact]
    public void CalculateProjection_WithValidInput_ReturnsPositiveBalance()
    {
        // Arrange
        var request = new ProjectionRequest
        {
            CurrentAge = 30,
            RetirementAge = 67,
            CurrentBalance = 50000,
            AnnualSalary = 85000,
            EmployerContributionRate = 11.5m,
            ExpectedReturnRate = 7.5m,
            SalaryGrowthRate = 3.0m,
            InflationRate = 2.5m
        };
        
        // Act
        var result = _calculator.CalculateProjection(request);
        
        // Assert
        result.Should().NotBeNull();
        result.FinalBalance.Should().BeGreaterThan(request.CurrentBalance);
        result.TotalContributions.Should().BePositive();
        result.TotalEarnings.Should().BePositive();
        result.YearlyBreakdown.Should().HaveCount(37); // 67 - 30 = 37 years
        result.InflationAdjustedBalance.Should().BePositive();
        result.InflationAdjustedBalance.Should().BeLessThan(result.FinalBalance);
    }
    
    [Theory]
    [InlineData(30, 67, 50000, 85000, 11.5, 7.5)]
    [InlineData(40, 65, 100000, 100000, 11.5, 6.0)]
    [InlineData(50, 67, 200000, 120000, 11.5, 8.0)]
    [InlineData(25, 60, 25000, 65000, 11.5, 7.0)]
    public void CalculateProjection_WithVariousInputs_ProducesConsistentResults(
        int currentAge, int retirementAge, decimal balance, decimal salary, 
        decimal contributionRate, decimal returnRate)
    {
        // Arrange
        var request = new ProjectionRequest
        {
            CurrentAge = currentAge,
            RetirementAge = retirementAge,
            CurrentBalance = balance,
            AnnualSalary = salary,
            EmployerContributionRate = contributionRate,
            ExpectedReturnRate = returnRate,
            SalaryGrowthRate = 3.0m,
            InflationRate = 2.5m
        };
        
        // Act
        var result = _calculator.CalculateProjection(request);
        
        // Assert
        result.Should().NotBeNull();
        result.TotalContributions.Should().BePositive();
        result.TotalEarnings.Should().BePositive();
        result.FinalBalance.Should().BeGreaterThan(request.CurrentBalance);
        result.YearlyBreakdown.Should().HaveCount(retirementAge - currentAge);
        
        // Verify yearly breakdown consistency
        var firstYear = result.YearlyBreakdown.First();
        var lastYear = result.YearlyBreakdown.Last();
        
        firstYear.OpeningBalance.Should().Be(request.CurrentBalance);
        firstYear.Age.Should().Be(currentAge + 1);
        lastYear.Age.Should().Be(retirementAge);
        lastYear.ClosingBalance.Should().Be(result.FinalBalance);
    }
    
    [Fact]
    public void CalculateProjection_WithContributionCap_EnforcesLimit()
    {
        // Arrange - High salary to trigger contribution cap
        var request = new ProjectionRequest
        {
            CurrentAge = 30,
            RetirementAge = 31, // 1 year only to simplify testing
            CurrentBalance = 0,
            AnnualSalary = 500000, // High salary: 500k * 11.5% = 57.5k (above cap)
            EmployerContributionRate = 11.5m,
            ExpectedReturnRate = 7.5m
        };
        
        // Act
        var result = _calculator.CalculateProjection(request);
        
        // Assert
        // Contribution should be capped at $27,500, then taxed at 15%
        // Expected: 27500 * 0.85 = 23,375
        result.TotalContributions.Should().BeLessOrEqualTo(23375m);
        result.YearlyBreakdown.Should().HaveCount(1);
        
        var yearlyContribution = result.YearlyBreakdown.First().Contributions;
        yearlyContribution.Should().BeLessOrEqualTo(23375m);
    }
    
    [Fact]
    public void CalculateProjection_WithNoContributionCap_AppliesTaxCorrectly()
    {
        // Arrange - Moderate salary that won't hit the cap
        var request = new ProjectionRequest
        {
            CurrentAge = 30,
            RetirementAge = 31, // 1 year only
            CurrentBalance = 0,
            AnnualSalary = 80000, // 80k * 11.5% = 9,200 (below cap)
            EmployerContributionRate = 11.5m,
            ExpectedReturnRate = 7.5m
        };
        
        // Act
        var result = _calculator.CalculateProjection(request);
        
        // Assert
        // Expected: 80000 * 0.115 * 0.85 = 7,820
        result.TotalContributions.Should().BeApproximately(7820m, 1m);
    }
    
    [Fact]
    public void CalculateProjection_WithZeroCurrentBalance_StartsFromZero()
    {
        // Arrange
        var request = new ProjectionRequest
        {
            CurrentAge = 25,
            RetirementAge = 30, // 5 years
            CurrentBalance = 0,
            AnnualSalary = 70000,
            EmployerContributionRate = 11.5m,
            ExpectedReturnRate = 7.5m,
            SalaryGrowthRate = 3.0m
        };
        
        // Act
        var result = _calculator.CalculateProjection(request);
        
        // Assert
        result.YearlyBreakdown.First().OpeningBalance.Should().Be(0);
        result.FinalBalance.Should().BeGreaterThan(0);
        result.TotalEarnings.Should().BeGreaterThan(0);
    }
    
    [Fact]
    public void CalculateProjection_WithSalaryGrowth_IncreaseContributionsOverTime()
    {
        // Arrange
        var request = new ProjectionRequest
        {
            CurrentAge = 30,
            RetirementAge = 35, // 5 years
            CurrentBalance = 50000,
            AnnualSalary = 80000,
            EmployerContributionRate = 11.5m,
            ExpectedReturnRate = 7.5m,
            SalaryGrowthRate = 5.0m // Higher growth to see effect
        };
        
        // Act
        var result = _calculator.CalculateProjection(request);
        
        // Assert
        var years = result.YearlyBreakdown.ToList();
        
        // Contributions should increase each year due to salary growth
        for (int i = 1; i < years.Count; i++)
        {
            years[i].Contributions.Should().BeGreaterThan(years[i - 1].Contributions);
        }
    }
    
    [Fact]
    public void CalculateProjection_WithHighReturnRate_GeneratesMoreEarnings()
    {
        // Arrange
        var baseRequest = new ProjectionRequest
        {
            CurrentAge = 30,
            RetirementAge = 40,
            CurrentBalance = 100000,
            AnnualSalary = 80000,
            EmployerContributionRate = 11.5m,
            SalaryGrowthRate = 3.0m
        };
        
        var lowReturnRequest = new ProjectionRequest
        {
            CurrentAge = baseRequest.CurrentAge,
            RetirementAge = baseRequest.RetirementAge,
            CurrentBalance = baseRequest.CurrentBalance,
            AnnualSalary = baseRequest.AnnualSalary,
            EmployerContributionRate = baseRequest.EmployerContributionRate,
            SalaryGrowthRate = baseRequest.SalaryGrowthRate,
            ExpectedReturnRate = 5.0m
        };
        var highReturnRequest = new ProjectionRequest
        {
            CurrentAge = baseRequest.CurrentAge,
            RetirementAge = baseRequest.RetirementAge,
            CurrentBalance = baseRequest.CurrentBalance,
            AnnualSalary = baseRequest.AnnualSalary,
            EmployerContributionRate = baseRequest.EmployerContributionRate,
            SalaryGrowthRate = baseRequest.SalaryGrowthRate,
            ExpectedReturnRate = 10.0m
        };
        
        // Act
        var lowReturnResult = _calculator.CalculateProjection(lowReturnRequest);
        var highReturnResult = _calculator.CalculateProjection(highReturnRequest);
        
        // Assert
        highReturnResult.TotalEarnings.Should().BeGreaterThan(lowReturnResult.TotalEarnings);
        highReturnResult.FinalBalance.Should().BeGreaterThan(lowReturnResult.FinalBalance);
    }
    
    [Fact]
    public void CalculateProjection_YearlyBreakdown_MaintainsConsistency()
    {
        // Arrange
        var request = new ProjectionRequest
        {
            CurrentAge = 30,
            RetirementAge = 35,
            CurrentBalance = 50000,
            AnnualSalary = 80000,
            EmployerContributionRate = 11.5m,
            ExpectedReturnRate = 7.5m,
            SalaryGrowthRate = 3.0m
        };
        
        // Act
        var result = _calculator.CalculateProjection(request);
        
        // Assert
        var years = result.YearlyBreakdown.ToList();
        
        // First year opening balance should match starting balance
        years[0].OpeningBalance.Should().Be(request.CurrentBalance);
        
        // Each year's opening balance should equal previous year's closing balance
        for (int i = 1; i < years.Count; i++)
        {
            years[i].OpeningBalance.Should().Be(years[i - 1].ClosingBalance);
        }
        
        // Each year's closing balance should equal opening + contributions + earnings
        foreach (var year in years)
        {
            year.ClosingBalance.Should().BeApproximately(
                year.OpeningBalance + year.Contributions + year.Earnings, 0.01m);
        }
        
        // Final balance should match last year's closing balance
        result.FinalBalance.Should().Be(years.Last().ClosingBalance);
    }
    
    [Fact]
    public void CalculateProjection_InflationAdjustment_ReducesPurchasingPower()
    {
        // Arrange
        var request = new ProjectionRequest
        {
            CurrentAge = 30,
            RetirementAge = 67,
            CurrentBalance = 50000,
            AnnualSalary = 80000,
            EmployerContributionRate = 11.5m,
            ExpectedReturnRate = 7.5m,
            SalaryGrowthRate = 3.0m,
            InflationRate = 2.5m
        };
        
        // Act
        var result = _calculator.CalculateProjection(request);
        
        // Assert
        result.InflationAdjustedBalance.Should().BePositive();
        result.InflationAdjustedBalance.Should().BeLessThan(result.FinalBalance);
        
        // With 37 years at 2.5% inflation, purchasing power should be significantly reduced
        var expectedInflationFactor = Math.Pow(1.025, 37);
        var expectedAdjustedBalance = (double)result.FinalBalance / expectedInflationFactor;
        
        result.InflationAdjustedBalance.Should().BeApproximately((decimal)expectedAdjustedBalance, 1000m);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(30)]
    [InlineData(67)]
    public void CalculateProjection_WithZeroYears_HandlesEdgeCases(int retirementAge)
    {
        // Arrange
        var request = new ProjectionRequest
        {
            CurrentAge = 30,
            RetirementAge = retirementAge,
            CurrentBalance = 50000,
            AnnualSalary = 80000,
            EmployerContributionRate = 11.5m,
            ExpectedReturnRate = 7.5m
        };
        
        // Act & Assert
        if (retirementAge <= 30)
        {
            // When retirement age is same or before current age, should handle gracefully
            var result = _calculator.CalculateProjection(request);
            result.Should().NotBeNull();
            result.YearlyBreakdown.Should().BeEmpty();
            result.FinalBalance.Should().Be(request.CurrentBalance);
        }
        else
        {
            var result = _calculator.CalculateProjection(request);
            result.YearlyBreakdown.Should().HaveCount(retirementAge - 30);
        }
    }
}