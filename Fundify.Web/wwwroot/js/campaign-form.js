document.addEventListener('DOMContentLoaded', function() {
    // Rich Text Editor
    const toolbar = document.querySelector('.rich-text-toolbar');
    const editor = document.querySelector('.rich-text-editor');
    const descriptionInput = document.querySelector('#description-input');

    if (toolbar && editor) {
        toolbar.querySelectorAll('button').forEach(button => {
            button.addEventListener('click', function(e) {
                e.preventDefault();
                const command = this.dataset.command;
                if (command === 'createLink') {
                    const url = prompt('Enter the URL:');
                    if (url) document.execCommand(command, false, url);
                } else {
                    document.execCommand(command, false, null);
                }
                editor.focus();
            });
        });

        editor.addEventListener('input', function() {
            descriptionInput.value = this.innerHTML;
        });
    }

    // Image Upload
    const imageUploadContainer = document.querySelector('.image-upload-container');
    const imageUpload = document.querySelector('#image-upload');
    const imagePreview = document.querySelector('#image-preview');
    const imageUrlInput = document.querySelector('input[name="ImageUrl"]');

    if (imageUploadContainer && imageUpload) {
        imageUploadContainer.addEventListener('click', () => imageUpload.click());
        
        imageUploadContainer.addEventListener('dragover', e => {
            e.preventDefault();
            imageUploadContainer.classList.add('dragover');
        });

        imageUploadContainer.addEventListener('dragleave', () => {
            imageUploadContainer.classList.remove('dragover');
        });

        imageUploadContainer.addEventListener('drop', e => {
            e.preventDefault();
            imageUploadContainer.classList.remove('dragover');
            const file = e.dataTransfer.files[0];
            handleImage(file);
        });

        imageUpload.addEventListener('change', e => {
            const file = e.target.files[0];
            handleImage(file);
        });

        function handleImage(file) {
            if (file && file.type.startsWith('image/')) {
                const reader = new FileReader();
                reader.onload = e => {
                    imagePreview.innerHTML = `<img src="${e.target.result}" alt="Preview">`;
                    imageUrlInput.value = e.target.result;
                };
                reader.readAsDataURL(file);
            }
        }
    }

    // Rewards form
    const rewardsContainer = document.getElementById('rewards-container');
    const addRewardButton = document.getElementById('add-reward-tier');

    if (rewardsContainer && addRewardButton) {
        let rewardCount = 1;

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
                    <label>Tier Name</label>
                    <input type="text" name="Rewards[${rewardCount-1}].Name" class="form-control" 
                           placeholder="e.g., Early Bird Access" required>
                </div>
                <div class="form-group">
                    <label>Pledge Amount</label>
                    <div class="input-with-prefix">
                        <span class="prefix">$</span>
                        <input type="number" name="Rewards[${rewardCount-1}].Amount" class="form-control" 
                               placeholder="e.g., 99" min="1" required>
                    </div>
                </div>
                <div class="form-group">
                    <label>Description</label>
                    <textarea name="Rewards[${rewardCount-1}].Description" class="form-control" rows="3" 
                             placeholder="What backers will receive..." required></textarea>
                </div>
                <div class="form-group">
                    <label>Quantity Available (Optional)</label>
                    <input type="number" name="Rewards[${rewardCount-1}].Limit" class="form-control" 
                           placeholder="Leave blank for unlimited" min="1">
                </div>
                <div class="form-group">
                    <label>Estimated Delivery</label>
                    <input type="month" name="Rewards[${rewardCount-1}].EstimatedDelivery" class="form-control" required>
                </div>
            `;
            rewardsContainer.appendChild(newTier);
        });

        rewardsContainer.addEventListener('click', e => {
            if (e.target.classList.contains('btn-remove-tier')) {
                const tier = e.target.closest('.reward-tier');
                if (rewardsContainer.children.length > 1) {
                    tier.remove();
                    rewardCount--;
                }
            }
        });
    }
});

function updateFileName(input) {
    const fileName = input.files[0]?.name ?? "No file chosen";
    input.closest('.upload-content').querySelector('.file-name').textContent = fileName;
} 