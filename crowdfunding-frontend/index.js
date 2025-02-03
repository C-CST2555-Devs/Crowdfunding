document.addEventListener('DOMContentLoaded', function() {
    // Pre-existing campaigns
    const preExistingCampaigns = [
        {
            basics: {
                title: "CodeAI - Intelligent Code Completion",
                category: "AI/ML",
                goal: 200000
            },
            details: {
                description: "Next-gen AI code assistant with real-time collaboration features.",
                image: "assets/images/ai-project.png"
            },
            stats: {
                raisedAmount: 157500,
                backers: 324,
                daysLeft: 15
            }
        },
        {
            basics: {
                title: "DataFlow - Cloud Analytics Suite",
                category: "SaaS",
                goal: 100000
            },
            details: {
                description: "Enterprise-grade analytics platform for modern data teams.",
                image: "assets/images/saas-project.jpg"
            },
            stats: {
                raisedAmount: 45000,
                backers: 156,
                daysLeft: 28
            }
        },
        {
            basics: {
                title: "HomeHub - Smart Home Controller",
                category: "Hardware",
                goal: 100000
            },
            details: {
                description: "Open-source smart home hub with Matter protocol support.",
                image: "assets/images/hardware-project.jpg"
            },
            stats: {
                raisedAmount: 90000,
                backers: 412,
                daysLeft: 7
            }
        }
    ];

    // Load user-created campaigns from localStorage
    const userCampaigns = JSON.parse(localStorage.getItem('campaigns') || '[]');
    
    // Combine pre-existing and user campaigns
    const allCampaigns = [...userCampaigns, ...preExistingCampaigns];
    const campaignsGrid = document.querySelector('.campaigns-grid');

    function renderCampaigns(campaigns) {
        campaignsGrid.innerHTML = '';
        
        campaigns.forEach(campaign => {
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
    renderCampaigns(allCampaigns);

    // Search functionality
    const searchInput = document.getElementById('campaign-search');
    let activeCategory = 'All';

    function filterCampaigns() {
        const searchTerm = searchInput.value.toLowerCase();
        const filteredCampaigns = allCampaigns.filter(campaign => {
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

    // Filter functionality
    const filterButtons = document.querySelectorAll('.filter-btn');
    filterButtons.forEach(button => {
        button.addEventListener('click', function() {
            filterButtons.forEach(btn => btn.classList.remove('active'));
            this.classList.add('active');

            const category = this.textContent.trim();
            activeCategory = category;

            filterCampaigns();
        });
    });
}); 