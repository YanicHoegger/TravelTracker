function addCollapsedVisibility(e) {
    var elements = getElements(e);
    for(i = 0; i < elements.length; i++){
        elements[i].classList.add('visbility-collapse');
    }
}

function removeCollapsedVisibility(e) {
    var elements = getElements(e);
    for(i = 0; i < elements.length; i++){
        elements[i].classList.remove('visbility-collapse');
    }
}

function getElements(e){
    var valueElement = $(e.target)
        .prev('.panel-heading')
        .find('dd.visibility-toggled');
    var editButtons = $('a.visibility-toggled');
    return $.merge(valueElement, editButtons);
}

$('.panel-group').on('hidden.bs.collapse', removeCollapsedVisibility);
$('.panel-group').on('shown.bs.collapse', addCollapsedVisibility);

$(function() {
    if($("#NewUserNameInvalid")[0] != null) {
        $("#collapse1").collapse('show');
    }
    if($("#NewEmailInvalid")[0] != null) {
        $("#collapse2").collapse('show');
    }
    if($("#NewPasswordInvalid")[0] != null) {
        $("#collapse3").collapse('show');
    }
});