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
    return $(e.target)
        .prev('.panel-heading')
        .find('.visibility-toggled');
}

$('.panel-group').on('hidden.bs.collapse', removeCollapsedVisibility);
$('.panel-group').on('shown.bs.collapse', addCollapsedVisibility);