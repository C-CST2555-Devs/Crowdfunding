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
}); 