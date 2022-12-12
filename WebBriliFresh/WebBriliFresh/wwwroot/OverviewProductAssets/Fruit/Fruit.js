$(document).ready(function() {
   
    const hours = $(".hours-countdown .countdown-num");
    const minutes = $(".minute-countdown .countdown-num");
    const seconds = $(".second-countdown .countdown-num");
    
    const targetDate = new Date('Nov 1, 2022 23:24:00');
    
    function convertMillis(milliseconds, format) {
    var days, hours, minutes, seconds, total_hours, total_minutes, total_seconds;
    
    total_seconds = parseInt(Math.floor(milliseconds / 1000));
    total_minutes = parseInt(Math.floor(total_seconds / 60));
    total_hours = parseInt(Math.floor(total_minutes / 60));
    days = parseInt(Math.floor(total_hours / 24));
  
    seconds = parseInt(total_seconds % 60);
    minutes = parseInt(total_minutes % 60);
    hours = parseInt(total_hours % 24);
    
    switch(format) {
      case 's':
          return total_seconds;
      case 'm':
          return total_minutes;
      case 'h':
          return total_hours;
      default:
          return {h: hours, m: minutes, s: seconds };
      }
    };
    
    window.setInterval( function()
    {
      // Where we check if 'now' is greater than the target date
      var date = Date.now();
      if (date > targetDate)
      {
        // Where we break
        console.log("Expired");
        clearInterval();
      } else
      {
        // Where we set values
        var millis = targetDate - date;
        var millisObject = convertMillis(millis);
        
        // Display values in HTML
     
    
        if(millisObject.h>10){hours.text(millisObject.h)}
        else{hours.text("0"+millisObject.h)}

        if(millisObject.m>10){minutes.text(millisObject.m)}
        else{minutes.text("0"+millisObject.m)}
        seconds.text(millisObject.s);
      };
    }, 1000);
  });
  