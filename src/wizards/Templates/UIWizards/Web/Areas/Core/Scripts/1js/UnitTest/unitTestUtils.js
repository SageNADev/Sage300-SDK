
/*jslint browser: true */
/*global jQuery: false */
(function ($) {
  'use strict';

    this.unitTestUtils = this.unitTestUtils || {};
    const testModule = "pmDefaultTestSuite";

    unitTestUtils.csvToHtmlCode = (viewid) => {
        const func = (htmlCtrl, key) => {
            return `${testModule}.enterToInput("#${htmlCtrl}", obj.${key});`;
        }

        unitTestUtils.csvToCode(false, func, viewid);
    };

    unitTestUtils.csvToGridCode = () => {
        const func = (htmlCtrl, key) => {
            return `${testModule}.enterComplexGridCellValue(index, modelPM0031.${key}, obj.${key}, obj.${key});`;
        }

        unitTestUtils.csvToCode(true, func, "PM00630");
    };

    unitTestUtils.csvToCode = (isGrid, func, viewid = "PM00620") => {

        let input = document.createElement('input');
        input.id = viewid;
        input.type = 'file';

        input.onchange = e => {

            // getting a hold of the file reference
            var file = e.target.files[0];

            // setting up the reader
            var reader = new FileReader();

            // here we tell the reader what to do when it's done reading...
            reader.onload = readerEvent => {
                var content = readerEvent.target.result; // this is the content!
                //console.log(content);
                //document.getElementById(input.id).remove();
                fnCreateCode(content);
            }

            reader.readAsText(file, 'UTF-8');

        }

        const fnCreateCode = (csvString) => {


            //const csvString = "ADJUSTNO:txtAdjustmentNumber,COMPLETE:ddlStatus,TRANSDATE:txtTransactionDate,DATEBUS:txtPostingDatePopup,FISCALYEAR:txtFiscalYear,FISCALPER:txtFiscalPeriod,VALUES:chkHasOptionalFields,DESC:txtDescription,REFERENCE:txtReference\nAAAAAA,10,2/9/2023,2/9/2023,2023,2,TRUE,, ";

            const csvRows = csvString.split("\n");
            const headerRow = csvRows[0];
            const headerFields = headerRow.split(",");

            /*const dataRow = csvRows[1];
            const dataFields = dataRow.split(",");

            const headerMap = {};
            headerFields.forEach((field, i) => {
                const [key, value] = field.split(":");
                //headerMap[key] = value;
                headerMap[value] = key;
            });

            const obj = {};
            dataFields.forEach((field, i) => {
                const key = Object.keys(headerMap)[i];
                obj[key] = field;
            });
            
            const tableArr = [obj];
            */
            let tableArr = [];
            const headerMap = {};
            headerFields.forEach((field, i) => {
                const [key, value] = field.split(":");

                if (isGrid) {
                    headerMap[key] = value;
                } else {
                    headerMap[value] = key;
                }
            });


            //const dataRows = 
            csvRows.shift();
            csvRows.every(dataRow => {
                if (dataRow.length === 0) {
                    return false;
                }

                const dataFields = dataRow.split(",");

                //headerFields.forEach((field, i) => {
                //    const [key, value] = field.split(":");
                //    //headerMap[key] = value;
                //    headerMap[value] = key;
                //});

                const obj = {};
                dataFields.forEach((field, i) => {
                    const key = Object.keys(headerMap)[i];
                    obj[key] = field;
                });

                tableArr.push(obj);

                return true;
            });

            let script = `let tableArr = ${JSON.stringify(tableArr).replaceAll("\\r", "")};\n\n`;

            script += "tableArr.forEach(obj => {\n";

            _.keys(headerMap).forEach(key => {
                //script += `\tunitTestUtilsRC.enterToInput("adjustmentsScreenContainer #${headerMap[key]}", obj.${key});\n`;
                //script += '\t' + func(key, headerMap[key]) + '\n';
                script += '\t' + func(headerMap[key], key) + '\n';
            });

            script += "});\n";

            window.open("data:text/csv;charset=utf-8," + escape(script));
        }

        input.click();

    };

    //use screen to generate automation code
    //no need to specify html ctr id
    unitTestUtils.inputCodeGen = (fileName, humanize, ignore, viewid = "PM00620") => {
        let csv = 'let tableArr = [{';

        //add the header row
        let header = [];
        let steps = [];

        _.keys(baseStaticControlfixtures._controlEvents).forEach(e => {
            if (baseStaticControlfixtures._controlEvents[e][0].viewid === viewid) {
                const field = baseStaticControlfixtures._controlEvents[e][0].field.replace(/"/g, '""');

                let value = baseStaticControlfixtures._controlEvents[e][0].ctrlType === 'dropdownbox'
                    ? $("#" + baseStaticControlfixtures._controlEvents[e][0].ctrlid).data("kendoDropDownList").text()
                    : $("#" + baseStaticControlfixtures._controlEvents[e][0].ctrlid).val();

                if (value === null) {
                    value = "";
                } else if (value instanceof Date) {
                    value = moment(value).format("MM/D/YYYY");
                } else {
                    value = value.toString();
                }

                value = value.replace(/"/g, '""');
                value = '"' + value + '"';

                header.push(`${field}:${value}`);
                steps.push(`\tunitTestUtils.enterToInput("${baseStaticControlfixtures._controlEvents[e][0].ctrlid}", obj.${field}, obj.${field});`);
            }
        });

        csv += header.join(',');

        csv += "}];\n\n";

        csv += "tableArr.forEach(obj => {\n";

        csv += steps.join('\n');

        csv += "\n});\n\n";

        window.open("data:text/csv;charset=utf-8," + escape(csv));
    };

    //use screen to generate csv file
    //and adds html ctr id to csv header
    unitTestUtils.inputToCSV = (fileName, humanize, ignore, viewid = "PM00620") => {
        let csv = '';

        //add the header row
        let header = [];
        _.keys(baseStaticControlfixtures._controlEvents).forEach(e => {
            if (baseStaticControlfixtures._controlEvents[e][0].viewid === viewid) {
                const field = baseStaticControlfixtures._controlEvents[e][0].ctrlid + ":" + baseStaticControlfixtures._controlEvents[e][0].field.replace(/"/g, '""');

                header.push(field);
            }
        });

        csv += header.join(',');
        csv += "\n";

        //add each row of data
        let rows = [];
        _.keys(baseStaticControlfixtures._controlEvents).forEach(e => {
            if (baseStaticControlfixtures._controlEvents[e][0].viewid === viewid) {
                let value = baseStaticControlfixtures._controlEvents[e][0].ctrlType === 'dropdownbox'
                    ? $("#" + baseStaticControlfixtures._controlEvents[e][0].ctrlid).data("kendoDropDownList").text()
                    : $("#" + baseStaticControlfixtures._controlEvents[e][0].ctrlid).val();

                if (value === null) {
                    value = "";
                } else if (value instanceof Date) {
                    value = moment(value).format("MM/D/YYYY");
                } else {
                    value = value.toString();
                }

                value = value.replace(/"/g, '""');
                value = '"' + value + '"';

                rows.push(value);
            }
        });

        csv += rows.join(',');
        csv += "\n";

        window.open("data:text/csv;charset=utf-8," + escape(csv));
    };

    //reference: https://www.telerik.com/forums/export-to-csv
    unitTestUtils.gridToCSV = (gridId, data, fileName, humanize, ignore) => {

        //let grid = $("#" + "adjustmentsDetailGrid").data("kendoGrid");
        //let grid = $("#" + "adjustmentsPopupGrid").data("kendoGrid");

        let grid = $("#" + gridId).data("kendoGrid");

        //data = grid.dataSource.view();
        data = grid.dataSource.data();

        humanize = false;

        var csv = '';
        if (!ignore) {
            ignore = [];
        }

        //ignore added datasource properties
        ignore = _.union(ignore, ["_events", "idField", "_defaultId", "constructor", "init", "get",
            "_set", "wrap", "bind", "one", "first", "trigger",
            "unbind", "uid", "dirty", "id", "parent", "_handlers", "prefixNamespace", "dirtyFields"]);


        //add the header row
        if (data.length > 0) {
            for (let col in data[0]) {
                //do not include inherited properties
                if (!data[0].hasOwnProperty(col) || _.includes(ignore, col)) {
                    continue;
                }

                if (humanize) {
                    col = col.split('_').join(' ').replace(/([A-Z])/g, ' $1');
                }

                col = col.replace(/"/g, '""');
                csv += '"' + col + '"';
                if (col !== data[0].length - 1) {
                    csv += ",";
                }
            }
            csv += "\n";
        }

        //add each row of data
        for (var row in data) {
            for (let col in data[row]) {
                //do not include inherited properties
                if (!data[row].hasOwnProperty(col) || _.includes(ignore, col)) {
                    continue;
                }

                var value = data[row][col];

                if (value === null) {
                    value = "";
                } else if (value instanceof Date) {
                    value = moment(value).format("MM/D/YYYY");
                } else {
                    value = value.toString();

                    //if (value.startsWith("[object") || value.startsWith("function(e)")) {
                    //    continue;
                    //}

                }

                value = value.replace(/"/g, '""');
                csv += '"' + value + '"';
                if (col !== data[row].length - 1) {
                    csv += ",";
                }
            }
            csv += "\n";
        }

        //TODO replace with downloadify so we can get proper file naming
        window.open("data:text/csv;charset=utf-8," + escape(csv));
    };

}).call(this, jQuery);