$(document).ready(function(){
  //  console.log("sdf");
  $("select.single-select").kendoDropDownList();
  $("select.multi-select").kendoMultiSelect(
  	{
        autoClose: false
    });
  $(".datepicker").kendoDatePicker();
  $(".tab-group").kendoTabStrip({
	    animation:  {
	        open: {
	            effects: "fadeIn"
	        }
	    }
	});

  $(".timepicker").kendoTimePicker({
        format: "hh:mm:ss tt",
        interval: 15
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