$("#LoginForm").submit(function(event) {

    event.preventDefault();

    var posting =  $.post("login/login", $("#LoginForm").serialize());
        
    posting.done(function(data){

        if(data.startsWith("Redirect")) {
            window.location.href = data.substring(9, data.length);
        } else {
            $("#LoginStatus").html(data);
        }                    
    });
}); 
