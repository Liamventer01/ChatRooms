// Initializes the tinymce service
tinymce.init({
    selector: '#messageContent',
    plugins: [
        'advlist', 'autolink',
        'lists', 'link', 'image', 'charmap', 'preview', 'anchor', 'searchreplace', 'visualblocks',
        'fullscreen', 'insertdatetime', 'media', 'table', 'help', 'wordcount'
    ],
    toolbar: 'undo redo | casechange blocks | bold italic backcolor | alignleft aligncenter alignright alignjustify |' +
        'bullist numlist checklist outdent indent | removeformat | help',
    skin: "oxide-dark",
    content_css: "dark, /css/tinymce.css",
    height: '260px',
});