document.addEventListener('DOMContentLoaded', function() {
    const form = document.getElementById('calculatorForm');
    const calculateBtn = document.getElementById('calculateBtn');
    const resultsDiv = document.getElementById('results');
    
    // Format currency for Australian dollars
    const formatCurrency = (amount) => {
        return new Intl.NumberFormat('en-AU', {
            style: 'currency',
            currency: 'AUD',
            minimumFractionDigits: 0,
            maximumFractionDigits: 0
        }).format(amount);
    };
    
    // Format percentage
    const formatPercentage = (rate) => {
        return new Intl.NumberFormat('en-AU', {
            style: 'percent',
            minimumFractionDigits: 1,
            maximumFractionDigits: 1
        }).format(rate / 100);
    };
    
    // Show loading state
    const showLoading = () => {
        calculateBtn.disabled = true;
        calculateBtn.textContent = 'Calculating...';
        resultsDiv.innerHTML = '<div class="loading">Calculating your retirement projection...</div>';
        resultsDiv.style.display = 'block';
    };
    
    // Hide loading state
    const hideLoading = () => {
        calculateBtn.disabled = false;
        calculateBtn.textContent = 'Calculate My Retirement Projection';
    };
    
    // Display error message
    const showError = (message) => {
        resultsDiv.innerHTML = `<div class="error">Error: ${message}</div>`;
        resultsDiv.style.display = 'block';
        hideLoading();
    };
    
    // Display results
    const displayResults = (data) => {
        if (!data.isSuccess) {
            showError(data.error || 'An error occurred during calculation');
            return;
        }
        
        const result = data.value;
        const yearsToRetirement = parseInt(document.getElementById('retirementAge').value) - 
                                parseInt(document.getElementById('currentAge').value);
        
        resultsDiv.innerHTML = `
            <div class="results">
                <h2>Your Retirement Projection</h2>
                <div class="result-grid">
                    <div class="result-item">
                        <div class="result-value">${formatCurrency(result.finalBalance)}</div>
                        <div class="result-label">Final Balance</div>
                    </div>
                    <div class="result-item">
                        <div class="result-value">${formatCurrency(result.totalContributions)}</div>
                        <div class="result-label">Total Contributions</div>
                    </div>
                    <div class="result-item">
                        <div class="result-value">${formatCurrency(result.totalEarnings)}</div>
                        <div class="result-label">Investment Earnings</div>
                    </div>
                    <div class="result-item">
                        <div class="result-value">${formatCurrency(result.inflationAdjustedBalance)}</div>
                        <div class="result-label">Today's Purchasing Power</div>
                    </div>
                </div>
                <div style="margin-top: 20px; padding: 15px; background: white; border-radius: 6px;">
                    <h3 style="color: #2c3e50; margin-bottom: 10px;">Projection Summary</h3>
                    <p><strong>Years to retirement:</strong> ${yearsToRetirement} years</p>
                    <p><strong>Growth rate:</strong> ${formatPercentage(parseFloat(document.getElementById('expectedReturnRate').value))}</p>
                    <p><strong>Super Guarantee rate:</strong> 11.5%</p>
                    <p><strong>Contributions tax:</strong> 15%</p>
                </div>
            </div>
        `;
        
        resultsDiv.style.display = 'block';
        hideLoading();
    };
    
    // Handle form submission
    form.addEventListener('submit', async (e) => {
        e.preventDefault();
        
        showLoading();
        
        // Gather form data
        const request = {
            currentAge: parseInt(document.getElementById('currentAge').value),
            retirementAge: parseInt(document.getElementById('retirementAge').value),
            currentBalance: parseFloat(document.getElementById('currentBalance').value),
            annualSalary: parseFloat(document.getElementById('annualSalary').value),
            employerContributionRate: 11.5, // Fixed Super Guarantee rate
            expectedReturnRate: parseFloat(document.getElementById('expectedReturnRate').value),
            salaryGrowthRate: parseFloat(document.getElementById('salaryGrowthRate').value),
            inflationRate: 2.5 // Fixed inflation rate
        };
        
        try {
            const response = await fetch('/api/projection/calculate', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(request)
            });
            
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            
            const data = await response.json();
            displayResults(data);
            
        } catch (error) {
            console.error('Calculation error:', error);
            showError('Unable to calculate projection. Please check your inputs and try again.');
        }
    });
    
    // Load scenarios on page load
    const loadScenarios = async () => {
        try {
            const response = await fetch('/api/projection/scenarios');
            if (response.ok) {
                const scenarios = await response.json();
                console.log('Available investment scenarios:', scenarios);
            }
        } catch (error) {
            console.warn('Could not load investment scenarios:', error);
        }
    };
    
    // Input validation
    document.getElementById('currentAge').addEventListener('change', function() {
        const currentAge = parseInt(this.value);
        const retirementAge = parseInt(document.getElementById('retirementAge').value);
        
        if (currentAge >= retirementAge) {
            document.getElementById('retirementAge').value = currentAge + 1;
        }
    });
    
    document.getElementById('retirementAge').addEventListener('change', function() {
        const retirementAge = parseInt(this.value);
        const currentAge = parseInt(document.getElementById('currentAge').value);
        
        if (retirementAge <= currentAge) {
            this.value = currentAge + 1;
        }
    });
    
    // Load scenarios when page loads
    loadScenarios();
});