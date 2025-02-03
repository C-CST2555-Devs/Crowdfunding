// Initialize form handlers based on current page
document.addEventListener('DOMContentLoaded', function() {
    // Handle basics form
    const basicsForm = document.getElementById('campaign-basics');
    if (basicsForm) {
        basicsForm.addEventListener('submit', function(e) {
            e.preventDefault();
            
            // Store form data in sessionStorage
            const formData = new FormData(this);
            const formObject = {};
            formData.forEach((value, key) => {
                formObject[key] = value;
            });
            sessionStorage.setItem('campaign-basics', JSON.stringify(formObject));
            
            // Navigate to details page
            window.location.href = 'campaign-details.html';
        });
    }

    // Handle details form
    const detailsForm = document.getElementById('campaign-details');
    if (detailsForm) {
        // Features container functionality
        const featuresContainer = document.getElementById('features-container');
        if (featuresContainer) {
            const addFeature = () => {
                const featureInput = document.createElement('div');
                featureInput.className = 'feature-input';
                featureInput.innerHTML = `
                    <input type="text" name="features[]" placeholder="Add a key feature">
                    <button type="button" class="btn-add-feature">+</button>
                `;
                featuresContainer.appendChild(featureInput);
            };

            featuresContainer.addEventListener('click', (e) => {
                if (e.target.classList.contains('btn-add-feature')) {
                    addFeature();
                }
            });
        }

        // Form submission
        detailsForm.addEventListener('submit', function(e) {
            e.preventDefault();
            
            // Store form data in sessionStorage
            const formData = new FormData(this);
            const formObject = {};
            formData.forEach((value, key) => {
                formObject[key] = value;
            });
            sessionStorage.setItem('campaign-details', JSON.stringify(formObject));
            
            // Navigate to rewards page
            window.location.href = 'campaign-rewards.html';
        });
    }

    // Handle rewards form
    const rewardsForm = document.getElementById('campaign-rewards');
    if (rewardsForm) {
        const rewardsContainer = document.getElementById('rewards-container');
        let rewardCount = 1;

        // Add new reward tier button
        const addRewardButton = document.getElementById('add-reward-tier');
        if (addRewardButton) {
            addRewardButton.addEventListener('click', () => {
                rewardCount++;
                const newTier = document.createElement('div');
                newTier.className = 'reward-tier';
                newTier.innerHTML = `
                    <div class="reward-header">
                        <h3>Reward Tier ${rewardCount}</h3>
                        <button type="button" class="btn-remove-tier">×</button>
                    </div>
                    <div class="form-group">
                        <label for="tier-name-${rewardCount}">Tier Name</label>
                        <input type="text" id="tier-name-${rewardCount}" name="rewards[${rewardCount-1}][name]" required
                               placeholder="e.g., Early Bird Access">
                    </div>
                    <div class="form-group">
                        <label for="tier-amount-${rewardCount}">Pledge Amount</label>
                        <div class="input-with-prefix">
                            <span class="prefix">$</span>
                            <input type="number" id="tier-amount-${rewardCount}" name="rewards[${rewardCount-1}][amount]" required
                                   placeholder="e.g., 99" min="1">
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="tier-description-${rewardCount}">Description</label>
                        <textarea id="tier-description-${rewardCount}" name="rewards[${rewardCount-1}][description]" required
                                placeholder="What backers will receive..." rows="3"></textarea>
                    </div>
                    <div class="form-group">
                        <label for="tier-limit-${rewardCount}">Quantity Available (Optional)</label>
                        <input type="number" id="tier-limit-${rewardCount}" name="rewards[${rewardCount-1}][limit]"
                               placeholder="Leave blank for unlimited" min="1">
                    </div>
                    <div class="form-group">
                        <label for="tier-delivery-${rewardCount}">Estimated Delivery</label>
                        <input type="month" id="tier-delivery-${rewardCount}" name="rewards[${rewardCount-1}][delivery]" required>
                    </div>
                `;
                rewardsContainer.appendChild(newTier);
            });
        }

        // Remove reward tier functionality
        if (rewardsContainer) {
            rewardsContainer.addEventListener('click', (e) => {
                if (e.target.classList.contains('btn-remove-tier')) {
                    const tier = e.target.closest('.reward-tier');
                    if (rewardsContainer.children.length > 1) {
                        tier.remove();
                        rewardCount--;
                    }
                }
            });
        }

        // Form submission
        rewardsForm.addEventListener('submit', function(e) {
            e.preventDefault();
            
            // Store form data in sessionStorage
            const formData = new FormData(this);
            const formObject = {
                rewards: []
            };
            
            // Group reward data
            for (let [key, value] of formData.entries()) {
                if (key.startsWith('rewards[')) {
                    const matches = key.match(/rewards\[(\d+)\]\[(\w+)\]/);
                    if (matches) {
                        const [, index, field] = matches;
                        if (!formObject.rewards[index]) {
                            formObject.rewards[index] = {};
                        }
                        formObject.rewards[index][field] = value;
                    }
                }
            }
            
            sessionStorage.setItem('campaign-rewards', JSON.stringify(formObject));
            
            // Navigate to preview page
            window.location.href = 'campaign-preview.html';
        });
    }

    // Handle launch button in preview page
    if (document.getElementById('launch-campaign')) {
        document.getElementById('launch-campaign').addEventListener('click', function() {
            // Get all campaign data
            const campaignData = {
                basics: JSON.parse(sessionStorage.getItem('campaign-basics') || '{}'),
                details: JSON.parse(sessionStorage.getItem('campaign-details') || '{}'),
                rewards: JSON.parse(sessionStorage.getItem('campaign-rewards') || '{"rewards":[]}'),
                stats: {
                    backers: 0,
                    daysLeft: parseInt(sessionStorage.getItem('campaign-basics').duration) || 30,
                    raisedAmount: 0,
                    createdAt: new Date().toISOString()
                }
            };

            // Get existing campaigns or initialize empty array
            const existingCampaigns = JSON.parse(localStorage.getItem('campaigns') || '[]');
            
            // Add new campaign to the beginning of the array
            existingCampaigns.unshift(campaignData);
            
            // Save to localStorage
            localStorage.setItem('campaigns', JSON.stringify(existingCampaigns));
            
            alert('Campaign submitted successfully!');
            // Clear session storage
            sessionStorage.clear();
            // Redirect to home
            window.location.href = 'index.html';
        });
    }
}); 