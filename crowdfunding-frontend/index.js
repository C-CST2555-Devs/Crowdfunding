document.addEventListener('DOMContentLoaded', async function() {
    // Fetch campaigns from API
    async function fetchCampaigns() {
        try {
            const response = await fetch('https://localhost:7000/api/campaigns');
            if (!response.ok) throw new Error('Failed to fetch campaigns');
            const campaigns = await response.json();
            
            // Transform API data to match frontend format
            return campaigns.map(campaign => ({
                basics: {
                    title: campaign.title,
                    category: campaign.category,
                    goal: campaign.goal
                },
                details: {
                    description: campaign.description,
                    image: campaign.imageUrl
                },
                stats: {
                    raisedAmount: campaign.raisedAmount,
                    backers: campaign.backerCount,
                    daysLeft: campaign.durationInDays
                }
            }));
        } catch (error) {
            console.error('Error fetching campaigns:', error);
            return [];
        }
    }

    const campaigns = await fetchCampaigns();
    const campaignsGrid = document.querySelector('.campaigns-grid');

    function renderCampaigns(campaignsToRender) {
        campaignsGrid.innerHTML = '';
        
        campaignsToRender.forEach(campaign => {
            const progressPercentage = (campaign.stats.raisedAmount / campaign.basics.goal) * 100;

            const campaignCard = `
                <article class="campaign-card">
                    <div class="campaign-image">
                        ${campaign.details.image ? 
                            `<img src="${campaign.details.image}" alt="${campaign.basics.title}">` :
                            `<div class="placeholder-image"></div>`
                        }
                        <span class="category">${campaign.basics.category}</span>
                    </div>
                    <div class="campaign-content">
                        <h3>${campaign.basics.title}</h3>
                        <p class="description">${campaign.details.description}</p>
                        <div class="progress-wrapper">
                            <div class="progress-bar">
                                <div class="progress" style="width: ${progressPercentage}%"></div>
                            </div>
                            <div class="campaign-stats">
                                <div class="amount">$${campaign.stats.raisedAmount.toLocaleString()} raised</div>
                                <div class="target">of $${campaign.basics.goal.toLocaleString()}</div>
                            </div>
                            <div class="campaign-meta">
                                <span>${campaign.stats.backers} backers</span>
                                <span>${campaign.stats.daysLeft} days left</span>
                            </div>
                        </div>
                    </div>
                </article>
            `;

            campaignsGrid.insertAdjacentHTML('beforeend', campaignCard);
        });
    }

    // Initial render
    renderCampaigns(campaigns);

    // Search and filter functionality
    const searchInput = document.getElementById('campaign-search');
    let activeCategory = 'All';

    function filterCampaigns() {
        const searchTerm = searchInput.value.toLowerCase();
        const filteredCampaigns = campaigns.filter(campaign => {
            const matchesSearch = campaign.basics.title.toLowerCase().includes(searchTerm);
            const matchesCategory = activeCategory === 'All' || campaign.basics.category === activeCategory;
            return matchesSearch && matchesCategory;
        });
        renderCampaigns(filteredCampaigns);
    }

    // Debounce function to limit how often the search is performed
    function debounce(func, wait) {
        let timeout;
        return function executedFunction(...args) {
            const later = () => {
                clearTimeout(timeout);
                func(...args);
            };
            clearTimeout(timeout);
            timeout = setTimeout(later, wait);
        };
    }

    // Add search event listener with debounce
    searchInput.addEventListener('input', debounce(filterCampaigns, 300));

    // Filter buttons
    const filterButtons = document.querySelectorAll('.filter-btn');
    filterButtons.forEach(button => {
        button.addEventListener('click', function() {
            filterButtons.forEach(btn => btn.classList.remove('active'));
            this.classList.add('active');
            activeCategory = this.textContent.trim();
            filterCampaigns();
        });
    });
}); 