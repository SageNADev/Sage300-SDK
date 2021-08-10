/* Copyright (c) 1994-2014 Sage Software, Inc.  All rights reserved. */

var uplodeUI =
{
    maxBlockSize: 256 * 1024, //Each file will be split in 256 KB.
    numberOfBlocks: 1,
    selectedFile: null,
    currentFilePointer: 0,
    totalBytesRemaining: 0,
    blockIds: new Array(),
    blockIdPrefix: "block-",
    submitUri: null,
    bytesUploaded: 0,
    fileName: "",
    fileType: "",
    supportFileType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
    supportFileExtension: "xlsx",
    reader: new FileReader(),

    //Read the file and find out how many blocks we would need to split it.
    handleFileSelect: function (e) {
        $("#importValidationMessageDiv").hide();
        sg.utls.progressBarControl("#progressBarForUpload", 0);
        uplodeUI.maxBlockSize = 256 * 1024;
        uplodeUI.currentFilePointer = 0;
        uplodeUI.totalBytesRemaining = 0;
        var files = e.target.files;
        uplodeUI.selectedFile = files[0];
        var fileName = uplodeUI.selectedFile.name;
        var guid = sg.importHelper.importModel.ImportRequest.FileName();
        uplodeUI.fileName = guid + fileName;
        uplodeUI.fileType = uplodeUI.selectedFile.type;
        $("#fileName").text(fileName);
        $("#fileSize").text(uplodeUI.selectedFile.size);
        $("#fileType").text(uplodeUI.selectedFile.type);
        var fileSize = uplodeUI.selectedFile.size;
        if (fileSize < uplodeUI.maxBlockSize) {
            uplodeUI.maxBlockSize = fileSize;
        }
        uplodeUI.totalBytesRemaining = fileSize;
        if (fileSize % uplodeUI.maxBlockSize == 0) {
            uplodeUI.numberOfBlocks = fileSize / uplodeUI.maxBlockSize;
        } else {
            uplodeUI.numberOfBlocks = parseInt(fileSize / uplodeUI.maxBlockSize, 10) + 1;
        }
    },
    uploadFileInBlocks: function () {
        $("#uploadDiv").hide();
        $("#uploadResult").show();
        if (uplodeUI.totalBytesRemaining > 0) {
            var fileContent = uplodeUI.selectedFile.slice(uplodeUI.currentFilePointer, uplodeUI.currentFilePointer + uplodeUI.maxBlockSize);
            var blockId = uplodeUI.blockIdPrefix + uplodeUI.pad(uplodeUI.blockIds.length, 6);
            uplodeUI.blockIds.push(btoa(blockId));
            uplodeUI.reader.readAsArrayBuffer(fileContent);
            uplodeUI.currentFilePointer += uplodeUI.maxBlockSize;
            uplodeUI.totalBytesRemaining -= uplodeUI.maxBlockSize;
            if (uplodeUI.totalBytesRemaining < uplodeUI.maxBlockSize) {
                uplodeUI.maxBlockSize = uplodeUI.totalBytesRemaining;
            }
        } else {
            uplodeUI.commitBlockList();
        }
    },
    commitBlockList: function () {
        var uri = uplodeUI.submitUri + '&comp=blocklist';
        var requestBody = '<?xml version="1.0" encoding="utf-8"?><BlockList>';
        for (var i = 0; i < uplodeUI.blockIds.length; i++) {
            requestBody += '<Latest>' + uplodeUI.blockIds[i] + '</Latest>';
        }
        requestBody += '</BlockList>';
        $.ajax({
            url: uri,
            type: "PUT",
            data: requestBody,
            beforeSend: function (xhr) {
                xhr.setRequestHeader('x-ms-blob-content-type', uplodeUI.selectedFile.type);
                xhr.setRequestHeader('Content-Length', requestBody.length);
            },
            success: function (data, status) {
                $("#hiddenFileUploadButton").click();//.trigger('click');
            },
            error: function (xhr, desc, err) {
                console.log(desc);
                console.log(err);
            }
        });
    },
    pad: function (number, length) {
        var str = '' + number;
        while (str.length < length) {
            str = '0' + str;
        }
        return str;
    }
};


$("#btnFileImport").bind('change', uplodeUI.handleFileSelect);
if (window.File && window.FileReader && window.FileList && window.Blob) {
} else {
    alert('The File APIs are not fully supported in this browser.');
}
$("#btnSelect").bind('click', function () {
    $("#importValidationMessageDiv").hide();
    sg.utls.clearValidations("frmImport");
    if (uplodeUI.fileName == "" || uplodeUI.fileName == sg.importHelper.importModel.ImportRequest.FileName()) {
        window.sg.utls.showMessageInfoInCustomDivWithoutClose(window.sg.utls.msgType.ERROR, globalResource.ImportFileSelectError, "importValidationMessageDiv");
    } else if (uplodeUI.fileType == uplodeUI.supportFileType || uplodeUI.fileName.split('.').pop().toLowerCase() == uplodeUI.supportFileExtension) {
        uplodeUI.uploadFileInBlocks();
    } else {
        window.sg.utls.showMessageInfoInCustomDivWithoutClose(window.sg.utls.msgType.ERROR, globalResource.ImportFileTypeSelectError, "importValidationMessageDiv");
    }
});

uplodeUI.reader.onloadend = function (evt) {
    if (evt.target.readyState == FileReader.DONE) { // DONE == 2
        var data = { blobName: uplodeUI.fileName, directoryName: "import" };

        // This is to set OFX as directory in the blob for Importing OFX statements
        if (sg.importHelper.importModel.ImportRequest.Name() === "importofxstatement") {
            data.directoryName = "importofxstatement"
        }

        sg.utls.ajaxPost(sg.utls.url.buildUrl("Core", "ExportImport", "GetImportBlobReference"), data, function (result) {

            //This wil only happen if directory name is invalid.
            if (result === "") {
                return;
            }
            //Update Sas Url, hide file information from screen and show upload related information
            uplodeUI.submitUri = result;
            $("#uploadDiv").hide();
            $("#uploadResult").show();

            var uri = uplodeUI.submitUri + '&comp=block&blockid=' + uplodeUI.blockIds[uplodeUI.blockIds.length - 1];
            var requestData = new Uint8Array(evt.target.result);
            $.ajax({
                url: uri,
                type: "PUT",
                data: requestData,
                processData: false,
                beforeSend: function (xhr) {
                    xhr.setRequestHeader('x-ms-blob-type', 'BlockBlob');
                    xhr.setRequestHeader('Content-Length', requestData.length);
                },
                success: function (data, status) {
                    uplodeUI.bytesUploaded += requestData.length;
                    var percentComplete = ((parseFloat(uplodeUI.bytesUploaded) / parseFloat(uplodeUI.selectedFile.size)) * 100).toFixed(2);
                    $("#fileUploadProgress").text(percentComplete + " %");
                    sg.utls.progressBarControl("#progressBarForUpload", percentComplete);
                    uplodeUI.uploadFileInBlocks();
                },
                error: function (xhr, desc, err) {
                    console.log(desc);
                    console.log(err);
                }
            });
        });
    }
};
