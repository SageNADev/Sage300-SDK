$(document).ready(function () {
     
    var iframeResizer = new Object();
    iframeResizer.MessageType = "iFrameHeight";
    setTimeout(function () {
        iframeResizer.Height = $(document).height();
        iframeResizer.Width = $(document).width();
        window.parent.postMessage(JSON.stringify(iframeResizer), "*");
    }, 2000);
    $(document).click(function () {
       
        setTimeout(function () {
            iframeResizer.MessageType = "iFrameHeight";
            iframeResizer.Height = $(document).height();
            iframeResizer.Width = $(document).width();
            window.parent.postMessage(JSON.stringify(iframeResizer), "*");
        }, 2000);

    });
});