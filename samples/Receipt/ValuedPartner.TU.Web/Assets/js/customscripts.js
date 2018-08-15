// The MIT License (MIT) 
// Copyright (c) 1994-2018 The Sage Group plc or its licensors.  All rights reserved.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of 
// this software and associated documentation files (the "Software"), to deal in 
// the Software without restriction, including without limitation the rights to use, 
// copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the 
// Software, and to permit persons to whom the Software is furnished to do so, 
// subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all 
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A 
// PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE 
// OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

$(document).ready(function(){
  //  console.log("sdf");
  $("select").kendoDropDownList();
  $(".datepicker").kendoDatePicker();
  $(".tab-group").kendoTabStrip({
	    animation:  {
	        open: {
	            effects: "fadeIn"
	        }
	    }
	});

  // numeric textbox with spinner and without spinner
	$("#with-spinner, #with-spinner1, #with-spinner2 ").kendoNumericTextBox({
	    //format: "c",
	    decimals: 3,
	    spinners:true
	});
	$("#without-spinner ").kendoNumericTextBox({
	    //format: "c",
	    decimals: 3,
	    spinners:false
	});
	$("#numeric").kendoNumericTextBox();
	$("#numeric1").kendoNumericTextBox();
	$("#numeric2").kendoNumericTextBox();
	$( ".dropDown-Menu a" ).hover(
		function() {
			$( this ).find("span").removeClass("arrow-grey").addClass("arrow-white");
		  }, function() {
			$( this ).find("span").removeClass("arrow-white").addClass("arrow-grey");
		  }
	);

	$('#test-edit').click(function() {
		console.log("clicked");
          var btn = $(this);
          $('.user-preference').css({
              position: 'absolute',
              top: btn.offset().top + btn.outerHeight() ,
              left: btn.offset().left
              //top:'100px'
          }).show();
        
    });

      $('a.label-menu').hover(function(e) {
    	e.preventDefault();
    	var btn = $(this);
          $('.label-menu-popup').css({
              position: 'absolute',
              top: btn.offset().top+10 ,
              left: btn.offset().left
          }).show();
	});

    $(".label-menu-popup .icon-close").click(function(){
    	$(this).parent().hide();
    })
});