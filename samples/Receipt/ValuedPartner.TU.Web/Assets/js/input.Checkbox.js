$(document).ready(function() {
  $( '.ctrl-group-inline input[type=radio]').parent().wrap( "<div><label class='radio-container'></label></div>" );
  $( '.ctrl-group-inline input[type=checkbox]').parent().wrap( "<div><label class='checkbox-container'></label></div>" );
  $( '.ctrl-group-inline input[type=radio]').parent().append( "<span class='checkmark'></span>" );
  $( '.ctrl-group-inline input[type=checkbox]').parent().append( "<span class='checkmark'></span>" );
  $( '.ctrl-group:not(.ctrl-group-inline) input[type=radio]').parent().wrap( "<label class='radio-container'></label>" );
  $( '.ctrl-group:not(.ctrl-group-inline) input[type=checkbox]').parent().wrap( "<label class='checkbox-container'></label>" );
  $( '.ctrl-group:not(.ctrl-group-inline) input[type=radio]').parent().append( "<span class='checkmark'></span>" );
  $( '.ctrl-group:not(.ctrl-group-inline) input[type=checkbox]').parent().append( "<span class='checkmark'></span>" );
});