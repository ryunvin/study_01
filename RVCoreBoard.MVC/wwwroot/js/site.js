// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// summernote
$(function () {
    $('#summernote').summernote({
        lang: 'ko-kr',               // default: 'en-us'
        placeholder: '내용 입력',
        tabsize: 2,
        height: 400,                 // set editor height
        minheight: null,             // set minimum height of editor
        maxheight: null,             // set maximum height of editor
        focus: true,                 // set focus to editable area after initializing summernote
        //callbacks: {
        //    // 이미지 업로드시 처리, 해당 처리가 없는 경우 기본적으로 이미지 업로드시 base64인코딩 되어 처리 된다.
        //    onimageupload: function (files, editor, weleditable) {
        //        for (var i = files.length - 1; i >= 0; i--) {
        //            sendfile(files[i], this);
        //        }
        //    }
        //},
        toolbar: [
            // [groupname, [list of button]]
            ['fontname', ['fontname']],
            ['fontsize', ['fontsize']],
            ['style', ['bold', 'italic', 'underline', 'strikethrough', 'clear']],
            ['color', ['forecolor', 'color']],
            ['table', ['table']],
            ['para', ['ul', 'ol', 'paragraph']],
            ['height', ['height']],
            ['insert', ['picture', 'link', 'video']],
            ['view', ['fullscreen', 'help']]
        ],
        fontnames: ['arial', 'arial black', 'comic sans ms', 'courier new', '맑은 고딕', '궁서', '굴림체', '굴림', '돋음체', '바탕체'],
        fontsizes: ['8', '9', '10', '11', '12', '14', '16', '18', '20', '22', '24', '28', '30', '36', '50', '72'],
    });
})
