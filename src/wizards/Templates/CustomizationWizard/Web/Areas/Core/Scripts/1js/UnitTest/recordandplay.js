(function () {
    'use strict';

    this.RecAndPlay = this.RecAndPlay || {};

    RecAndPlay.playing = false;
    RecAndPlay.recording = false;
    RecAndPlay.testComplete = false;

    this.RecAndPlay.stopPlaying = function(){
        RecAndPlay.playing = false;
    }

    this.RecAndPlay.stopRecording = function(){
        RecAndPlay.recording = false;
    }

    this.RecAndPlay.getTestComplete = function () {
        return RecAndPlay.testComplete;
    }

    this.RecAndPlay.setTestComplete = function () {
        RecAndPlay.testComplete = false; 
    }

    $(document).ready(function () {

  //      $.subscribe("play", function (obj, results) {
		//	console.log(results.data);
  //          //alert(results);
  //          //return 'results.data';
  //          RecAndPlay.play({});
		//}); 

        //RecAndPlay.execute();

        
    });

    this.RecAndPlay.fnc = function(){
            return 'ADMIN';
    }

    this.RecAndPlay.fnc2 = function(param){
            return param.data;
    }

    this.RecAndPlay.performTest = function(param){
            param.Module = 'login';
            param.TestCase = 'canLogin';
            let testModule = baseStaticTestModule.findModule(param.Module); //login.get(param.obj);

            return (testModule[param.TestCase])(); //(login[fn])(); //login.canLogin();
    }

    this.RecAndPlay.performTest2 = function(param){
            //let testModule = baseStaticTestModule.findModule(param.Module); //login.get(param.obj);

            //testModule.id = param.id;
            //return (testModule[param.TestCase])(); //(login[fn])(); //login.canLogin();
            (login[param.TestCase])();
    }

    this.RecAndPlay.play2 = function(){
        let param = {};
        
        
            //param.TestCase = 'canRedirectAfterLogin';

            //RecAndPlay.performTest2(param);
        
        param.Module = login.testModule
        param.TestCase = 'canLogin';
        RecAndPlay.performTest2(param);

        param.Module = ICBOM.testModule;
        param.TestCase = 'canRedirectAfterLogin';
        RecAndPlay.performTest2(param);

        /*
        param.Module = ICBOM.testModule;
        param.TestCase = 'canCreateNew';
        RecAndPlay.performTest2(param);
        */
    }

    let tests = [];

    this.RecAndPlay.addTest = function(module, testCase){
        let param = {};
        param.id = tests.length;
        param.Module = module;
        param.TestCase = testCase;

        tests.push(param); 
    }

    this.RecAndPlay.showResults = function(){
        $("#results").css("display", "block");
        $("#results").css("overflow-y", "scroll");
        $("#closeResult").removeAttr('disabled');
        
        return "showing results...";
    }

    this.RecAndPlay.hideResults = function(){
        $("#results, #closeResult").hide();
    }

    this.RecAndPlay.play3 = function(){
        RecAndPlay.playing = false;
        //login.canLogin();
        RecAndPlay.reset();

        RecAndPlay.addTest(login.testModule, 'canLogin');
        //RecAndPlay.addTest(ICBOM.testModule, 'canRedirectAfterLogin');
        
        /*this works
        RecAndPlay.addTest(ICBOM.testModule, 'canCreateNew');
        RecAndPlay.addTest(ICBOM.testModule, 'canInputDataByServerMsgEvent');
        RecAndPlay.addTest(ICBOM.testModule, 'canInputData');
        */

        RecAndPlay.addTest(ICBOM.testModule, 'canOpenICBOMFinder');
        RecAndPlay.addTest(ICBOM.testModule, 'canICBOMFinderSearchbyBOMNumber');
        
        
        //RecAndPlay.addTest(ICBOM.testModule, 'canSelectICBOMrGrid');
        
        
        /*
        let param = {};
        param.Module = login.testModule
        param.TestCase = 'canLogin';
        tests.push(param);

        let param1 = {};
        param1.Module = ICBOM.testModule;
        param1.TestCase = 'canRedirectAfterLogin';
        tests.push(param1);

        let param2 = {};
        param2.Module = ICBOM.testModule;
        param2.TestCase = 'canCreateNew';
        tests.push(param2);
        */

        sessionStorage.setItem("tests", JSON.stringify(tests));

        RecAndPlay.playing = true;
        login.process();
        //ICBOM.process();

        //RecAndPlay.execute();
        // * /
    }

    this.RecAndPlay.play4 = function(){
        RecAndPlay.playing = false;
        RecAndPlay.reset();

        RecAndPlay.addTest(login.testModule, 'canLogin');
        
        sessionStorage.setItem("tests", JSON.stringify(tests));

        RecAndPlay.playing = true;
        login.process();
        
        return  window.jQuery.active;
    }

    this.RecAndPlay.play5 = function(){
        RecAndPlay.playing = false;
        RecAndPlay.reset();

        RecAndPlay.addTest(ICBOM.testModule, 'canOpenICBOMFinder');
        RecAndPlay.addTest(ICBOM.testModule, 'canICBOMFinderSearchbyBOMNumber');
        
        
        sessionStorage.setItem("tests", JSON.stringify(tests));

        RecAndPlay.playing = true;
        ICBOM.process();

        return  window.jQuery.active;
    }

    this.RecAndPlay.play6 = function(){
        RecAndPlay.playing = false;
        loginTestSuite();
        QUnit.start();

        //icbomTestSuite();
        //QUnit.start();

        //$("#qunit-testresult-display")[0].innerText = "0 tests completed in 4 milliseconds, with 0 failed, 0 skipped, and 0 todo. 0 assertions of 0 passed, 0 failed.";
        //$("#qunit-tests")[0].innerText = "";

        return ""; //$("#qunit-testresult-display")[0].innerText;
    }

    this.RecAndPlay.play7 = function(){
        RecAndPlay.playing = false;
        icbomTestSuite();
        QUnit.start();

        return ""; //$("#qunit-testresult-display")[0].innerText;
    }

    this.RecAndPlay.play8 = function(){
        this.showResults();

        return ""; //$("#qunit-testresult-display")[0].innerText;
    }

    this.RecAndPlay.getLog = function(){
        QUnit.jUnitDone(function(data) {
			return data.xml;
			
		});
    }

    this.RecAndPlay.testy = (d) => {
        let fName = $("<input id='xzy' type='text'  />");
        $("#qunit-fixture").append(fName);
        console.log("all done");
    };

    this.RecAndPlay.idly = function(){

        $("#xzy").remove();

        Keeler.listenTo(MessageBus.msg, "testcomplete", ()=>{
            console.log("testcomplete done");
            this.testy("ABC");
        });
    }
    /*
    this.RecAndPlay.idly = () => $(document).idle({
            onIdle: function(){

                console.log("proceed idly ");
                    
            },
            idle: 2000,
            keepTracking: false
        });
    */	

    this.RecAndPlay.execute = function(){
        let tmp;
        try{
            let param = sessionStorage.getItem("tests");
            console.log("execute " + param);

            if (param === "undefined" || param === ",,"){
                return;
            }

            let tests = JSON.parse(param);
            //tmp = JSON.parse(param);
            if (tests === null){
                return;
            }

            //RecAndPlay.performTest2(tests[0]);

            /*
            let i = 0;
            apputils.each(tests, (test)=>{
                RecAndPlay.performTest2(test);
                //delete tmp[i++];
            });
            */
            
        }
        finally{
            //sessionStorage.setItem("tests", tmp);
        }
    }

    /*
    this.RecAndPlay.refreshScript = function(src) {
      var scriptElement = document.createElement('script');
      scriptElement.type = 'text/javascript';
      scriptElement.src = src + '?' + (new Date).getTime();
      document.getElementsByTagName('head')[0].appendChild(scriptElement);
    }
    */

    this.RecAndPlay.reset = function(){
        this.idly();

        tests = [];
        //QUnit.config.currentModule.tests = [];
        //QUnit.config.modules = [];

        //sessionStorage.setItem("tests", "[]");
        sessionStorage.removeItem('tests');
        sessionStorage.clear();

        $("#qunit-testresult-display")[0].innerText = "0 tests completed in 4 milliseconds, with 0 failed, 0 skipped, and 0 todo. 0 assertions of 0 passed, 0 failed.";
        $("#qunit-tests")[0].innerText = "";

        //this.refreshScript("/POC/Scripts/tests/lib/qunit.js");

        //But cannot call it here; run manually from console
        //reload page to flush the session storage & QUnit objects
        //location.reload();
    }

    this.RecAndPlay.play = /*async*/ function(dataRows){
        
        if (recording){
            console.log("Cannot play while another session is recording");
            return;
        }

        playing = true;

        try{

            dataRows = [
                {viewid: "IC0200", rowIndex:0, field:"UnformattedItemNumber", value:"123"},
                {viewid: "IC0200", rowIndex:0, field:"BOMNumber", value:"1"}
            ] 

            //since this click makes server calls and updates same fields which will be played, we need to wait and check this field is updated by server first before proceeding.
            //add 'once' event listener to the field and after update
            $("#fixedCost").on("blur", function (data) {
                alert(data);
            });

            let data = dataRows[0];
            let msg = data.viewid + data.rowIndex + data.field;
            this.play.waitForServerCallsToComplete(msg, ()=>{
                console.log("proceeding");

                apputils.each(dataRows, (data)=>{
                    msg = data.viewid + data.rowIndex + data.field;

                    MessageBus.msg.trigger(msg + apputils.EventMsgTags.svrUpdate, data.value, msg + apputils.EventMsgTags.svrUpdate);
                });
            });

            $("#btnPlusAddNewButton").click();

            /*
            let callback = (data)=>{
                console.log("server done " + data);

                Keeler.stopListening(MessageBus.msg, msg + apputils.EventMsgTags.svrUpdate);
                
            };

            let data = dataRows[0];
            let msg = data.viewid + data.rowIndex + data.field;
            Keeler.listenTo(MessageBus.msg, msg + apputils.EventMsgTags.svrUpdate, callback);
            */

            /*left as an example
            $(document).idle({
              onIdle: function(){
                alert('You did nothing for 5 seconds');
              },
              idle: 5000
            });
            */

            //$("#btnPlusAddNewButton").click();

            /*
            
            apputils.each(dataRows, (data)=>{
                let msg = data.viewid + data.rowIndex + data.field;

                MessageBus.msg.trigger(msg + apputils.EventMsgTags.svrUpdate, data.value, msg + apputils.EventMsgTags.svrUpdate);
            });
            */
            
        }
        catch(e){
            console.log(e);
        }
        finally{
            playing = false;
        }
    };
    
    this.RecAndPlay.play.waitForServerCallsToComplete = function(msg, proceed){
        /*
        let callback = (data)=>{
            console.log("server done " + data);

            Keeler.stopListening(MessageBus.msg, msg + apputils.EventMsgTags.svrUpdate, callback);

            proceed();
        };

        Keeler.listenTo(MessageBus.msg, msg + apputils.EventMsgTags.svrUpdate, callback);
        */

        /* this doesn't work
        let callback = (data)=>{
            console.log("server done " + data);

            Keeler.stopListening(MessageBus.msg, "serverCallComplete", callback);

            proceed();
        };

        Keeler.listenTo(MessageBus.msg, "serverCallComplete", callback);
        */

        /*this works*/
        $(document).idle({
              onIdle: function(){
                //alert('You did nothing for 5 seconds');
                console.log("system idle");
                proceed();
              },
              idle: 1000,
              keepTracking: false
            });
        
    };

    this.RecAndPlay.record = /*async*/ function(){
        
        if (playing) {
            console.log("Cannot record while another session is playing");
            return;
        }

        recording = true;

        try{

            //only all input data gets recorded
            let msg = ""; //options.viewid + options.rowIndex + options.field;
            Keeler.listenTo(MessageBus.msg, msg + apputils.EventMsgTags.usrUpdate, (data)=>{
                console.log(data);
            });

            //record other things like button clicks, scroll etc
        }
        catch(e){
            console.log(e);
        }
        finally{
            recording = false;
        }
        
    }

    this.RecAndPlay.QunitStarted = false;

    this.RecAndPlay.TestSuite = function(){
        
        QUnit.config.autostart = false;
        QUnit.config.reorder = false;

        QUnit.start();
                
    }

    this.RecAndPlay.TestSuiteRevisedEst = async function () {

        QUnit.config.autostart = false;
        QUnit.config.reorder = false;

        pmRevisedEstTestSuite();
        pmRevisedEstTestSuite.all();

        QUnit.start();

        return await new Promise((resolve) => {

            let myInterval = setInterval(() => {
                console.log("check test is still running");

                if (RecAndPlay.testComplete) {
                    clearInterval(myInterval);
                    resolve("testing complete");
                }
            }, 5000);

        });
    }

    this.RecAndPlay.TestSuiteDetailLine = function () {

        QUnit.config.autostart = false;
        QUnit.config.reorder = false;

        pmRevisedEstDetailLinesTestSuite();
        pmRevisedEstDetailLinesTestSuite.all();

        QUnit.start();
    }

    this.RecAndPlay.TestSuitePostCheckNavigate = function () {

        QUnit.config.autostart = false;
        QUnit.config.reorder = false;

        pmRevisedEstPostCheckNavigateTestSuite();
        pmRevisedEstPostCheckNavigateTestSuite.all();

        QUnit.start();
    }

    this.RecAndPlay.TestSuiteOpt = function () {

        QUnit.config.autostart = false;
        QUnit.config.reorder = false;

        // run this
        pmRevisedEstOptionalFieldTestSuite();
        pmRevisedEstOptionalFieldTestSuite.saveAndEdit();

        QUnit.start();

    }

    this.RecAndPlay.TestSuiteBOMCRUD = function () {

        QUnit.config.autostart = false;
        QUnit.config.reorder = false;

        // run this
        icbomCrudTestSuite();
        icbomCrudTestSuite.all();

        QUnit.start();
    }

    this.RecAndPlay.TestSuiteBOMTree = function () {

        QUnit.config.autostart = false;
        QUnit.config.reorder = false;

        icbomTreeTestSuite();
        icbomTreeTestSuite.all();

        QUnit.start();
    }

    this.RecAndPlay.TestSuiteAssembly = async function () {

        QUnit.config.autostart = false;
        QUnit.config.reorder = false;

        icAssemblyTestSuite();
        icAssemblyTestSuite.all();

        QUnit.start();

        return await new Promise((resolve) => {

            let myInterval = setInterval(() => {
                console.log("check test is still running");

                if (RecAndPlay.testComplete) {
                    clearInterval(myInterval);
                    resolve("testing complete");
                }
            }, 5000);

        });
    }

    this.RecAndPlay.TestSuiteLotRecallAndRelease = function () {

        QUnit.config.autostart = false;
        QUnit.config.reorder = false;

        // run this
        icLotRecallReleaseTestSuite();
        icLotRecallReleaseTestSuite.all();

        QUnit.start();

    }

    this.RecAndPlay.TestSuiteSerialNumbers = function () {

        QUnit.config.autostart = false;
        QUnit.config.reorder = false;

        // run this
        icSerialNumbersTestSuite();
        icSerialNumbersTestSuite.all();

        QUnit.start();

    }

    this.RecAndPlay.TestSuiteCopyBOM = function () {

        QUnit.config.autostart = false;
        QUnit.config.reorder = false;

        icCopyBOMTestSuite();
        icCopyBOMTestSuite.all();

        QUnit.start();
    }

    this.RecAndPlay.TestSuiteEquipmentUsage = async function () {

        QUnit.config.autostart = false;
        QUnit.config.reorder = false;

        pmEquipmentUsageTestSuite();
        pmEquipmentUsageTestSuite.all();

        QUnit.start();

        return await new Promise((resolve) => {

            let myInterval = setInterval(() => {
                console.log("check test is still running");

                if (RecAndPlay.testComplete) {
                    clearInterval(myInterval);
                    resolve("testing complete");
                }
            }, 5000);

        });
    }

    this.RecAndPlay.TestSuiteEquipment = async function () {

        QUnit.config.autostart = false;
        QUnit.config.reorder = false;

        pmEquipmentTestSuite();
        pmEquipmentTestSuite.all();

        QUnit.start();

        return await new Promise((resolve) => {

            let myInterval = setInterval(() => {
                console.log("check test is still running");

                if (RecAndPlay.testComplete) {
                    clearInterval(myInterval);
                    resolve("testing complete");
                }
            }, 5000);

        });
    }

    this.RecAndPlay.asyncTestRunner = async function (tests) {

        RecAndPlay.testComplete = false;

        QUnit.config.autostart = false;
        QUnit.config.reorder = false;

        tests();

        QUnit.start();

        return await new Promise((resolve) => {

            let myInterval = setInterval(() => {
                console.log("check test is still running");

                if (RecAndPlay.testComplete) {
                    clearInterval(myInterval);
                    resolve("testing complete");
                }
            }, 5000);


        });
    }

    this.RecAndPlay.TestSuiteAdjustmentsJC = async function () {

        RecAndPlay.testComplete = false;

        QUnit.config.autostart = false;
        QUnit.config.reorder = false;

        pmAdjustmentsTestSuite();
        pmAdjustmentsTestSuite.JC();

        QUnit.start();

        return await new Promise((resolve) => {

            let myInterval = setInterval(() => {
                console.log("check test is still running");

                if (RecAndPlay.testComplete) {
                    clearInterval(myInterval);
                    resolve("testing complete");
                }
            }, 5000);
            
        });
    }

    this.RecAndPlay.TestSuiteAdjustmentsTW = async function () {

        QUnit.config.autostart = false;
        QUnit.config.reorder = false;

        pmAdjustmentsTestSuite();
        pmAdjustmentsTestSuite.TW();

        QUnit.start();

        return await new Promise((resolve) => {

            let myInterval = setInterval(() => {
                console.log("check test is still running");

                if (RecAndPlay.testComplete) {
                    clearInterval(myInterval);
                    resolve("testing complete");
                }
            }, 5000);

        });
    }

    this.RecAndPlay.TestSuiteAdjustmentsSH = async function () {

        QUnit.config.autostart = false;
        QUnit.config.reorder = false;

        pmAdjustmentsTestSuite();
        pmAdjustmentsTestSuite.SH();

        QUnit.start();

        return await new Promise((resolve) => {

            let myInterval = setInterval(() => {
                console.log("check test is still running");

                if (RecAndPlay.testComplete) {
                    clearInterval(myInterval);
                    resolve("testing complete");
                }
            }, 5000);

        });
    }

    this.RecAndPlay.TestSuiteAdjustmentsOPT = async function () {

        QUnit.config.autostart = false;
        QUnit.config.reorder = false;

        pmAdjustmentsTestSuite();
        pmAdjustmentsTestSuite.OPT();
        
        QUnit.start();

        return await new Promise((resolve) => {

            let myInterval = setInterval(() => {
                console.log("check test is still running");

                if (RecAndPlay.testComplete) {
                    clearInterval(myInterval);
                    resolve("testing complete");
                }
            }, 5000);

        });
    }

    /*this.RecAndPlay.pmDefaultTestSuite = function () {

        QUnit.config.autostart = false;
        QUnit.config.reorder = false;

        pmTimecardsTestSuite();
        pmAdjustmentsTestSuite();
        pmDefaultTestSuite();
        pmDefaultTestSuite.all();

        QUnit.start();

        return "testing..."
    }*/


        
   

    //this.RecAndPlay.pmDefaultTestSuiteVCR = async function () {

    //    return await RecAndPlay.asyncTestRunner(() => {
    //        pmTimecardsTestSuite();
            
    //        pmDefaultTestSuite();
    //        pmDefaultTestSuite.VCR();
    //    });


    //    //QUnit.config.autostart = false;
    //    //QUnit.config.reorder = false;

    //    //pmTimecardsTestSuite();
    //    //pmAdjustmentsTestSuite();
    //    //pmDefaultTestSuite();
    //    //pmDefaultTestSuite.all();

    //    //QUnit.start();
    //    //return "testing..."
    //}

    //this.RecAndPlay.pmDefaultTestSuiteStatus = async function () {

    //    return await RecAndPlay.asyncTestRunner(() => {
    //        pmTimecardsTestSuite();

    //        pmDefaultTestSuite();
    //        pmDefaultTestSuite.Status();
    //    });

    //}

    this.RecAndPlay.TestSuiteTimecardsJC = async function () {

        QUnit.config.autostart = false;
        QUnit.config.reorder = false;

        pmTimecardsTestSuite();
        pmTimecardsTestSuite.JC();

        QUnit.start();

        return await new Promise((resolve) => {

            let myInterval = setInterval(() => {
                console.log("check test is still running");

                if (RecAndPlay.testComplete) {
                    clearInterval(myInterval);
                    resolve("testing complete");
                }
            }, 5000);

        });
    }

    this.RecAndPlay.TestSuiteTimecards = async function () {

        return await RecAndPlay.asyncTestRunner(() => {
            pmTimecardsTestSuite();
            pmTimecardsTestSuite.all();
        });

        /*QUnit.config.autostart = false;
        QUnit.config.reorder = false;

        pmTimecardsTestSuite();
        pmTimecardsTestSuite.all();

        QUnit.start();

        return "testing..."*/

    }

}).call(this);