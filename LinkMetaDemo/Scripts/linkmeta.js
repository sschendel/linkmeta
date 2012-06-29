var linkmeta = (function () {
    var loadingLinkPreview = false;

    var setPuLinkFormFields = function (data) {
        $('#linkUrl').val(data.Url);
        $('#linkTitle').val(data.Title);
        $('#linkDescription').val(data.Description);
        if (data.ImgUrl != null) {
            $('#linkImgUrl').val(data.ImgUrl);
        }
    }

    var clearPuLinkFormFields = function () {
        $('#linkUrl').val('');
        $('#linkTitle').val('');
        $('#linkDescription').val('');
        $('#linkImgUrl').val('');
    }

    var updateLinkPreview = function (data) {
        if (data.ImgUrl != null) {
            $('#puLinkImg').attr('src', data.ImgUrl);
        }
        $('#puLinkTitle').html('<a href="' + data.Url + '">' + data.Title + '</a>');
        $('#puLinkUrl').html(data.Url);
        $('#puLinkDescription').html(data.Description);
        setPuLinkFormFields(data);
        $('#puLinkDisplay').show();
        $('#puLinkRemoveLink').click(function () {
            clearPuLinkFormFields();
            $('#puLinkDisplay').hide();
        });
    };


    var getLinkPreview = function (url) {
        console.log("getLinkPreview: " + url);
        if (!$('#puLinkDisplay').is(":visible")) {
            if (!loadingLinkPreview) {
                loadingLinkPreview = true;
                $('#puLinkLoading').show();
                $.get('/home/linkmeta/', { url: url }, function (data) {
                    //alert(data);
                    if (data) {
                        updateLinkPreview(data);
                    }
                }).complete(function () {
                    loadingLinkPreview = false;
                    $('#puLinkLoading').hide();
                });
            } else {
                console.log("loading... skip link lookup url: " + url);
            }
        }
        else {
            console.log("skip link lookup url: " + url);
        }
    }

    return {
        getLinkPreview: getLinkPreview
    };

})();