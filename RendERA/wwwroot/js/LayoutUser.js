
$('#button').on('click', function () {
    $('#file-input').trigger('click');
});
$('#file-input').change(function () {
    $('#target').submit();
});

function Details(id) {
    if (id == '0' || id == 0) {
        var value = $("#DocumentList").val();
        id = parseInt(value);
    }
    if (id == '0' || id == 0) {

    }
    else {
        $.get(document.location.origin +'/Parameters/SetParameter?id=' + id,
            function (data) {
                $('.bodydata').html(data);
            });
        jQuery.noConflict();
        $("#myModal").modal("show");
    }
}

$(document).ready(function () {

    $('.output').click(function () {
        $('.forhide').show();
    });
    $('.output').click(function () {
        $('.icondiv').hide();
    });
    $('.input').click(function () {
        $('.icondiv').show();
    });
    $('.input').click(function () {
        $('.forhide').hide();
    });


    var acc = document.getElementsByClassName("accordion");
    var i;

    for (i = 0; i < acc.length; i++) {
        acc[i].addEventListener("click", function () {
            /* Toggle between adding and removing the "active" class,
            to highlight the button that controls the panel */
            this.classList.toggle("active");

            /* Toggle between hiding and showing the active panel */
            var panel = this.nextElementSibling;
            if (panel.style.display === "block") {
                $('.uploadform').show();
                panel.style.display = "none";
            } else {
                $('.uploadform').hide();
                panel.style.display = "block";
            }
        });
    }

});

//$(document).ready(function(){

//  $(".assets").click(function(){
//        $("#assets").fadeIn(1000);
//    $('#content-section').hide()
//            $('#analsys').hide();
//        $('#state').hide();
//         $('#balanc').hide();
//           $('#profile').hide();
//           $('#render').hide();

//  });
//})

//$(document).ready(function(){
//  $(".new-job").click(function(){
//    $('#content-section').hide();
//    $("#assets").hide();
//      $('#profile').hide();
//      $('#render').hide();
//            $('#state').hide();
//         $('#balanc').hide();

//  });
//});


$(document).ready(function () {
    $(".accordion").click(function () {
        $('.uploadpng').hide()
        $('#profile').hide()


    });

    // show analytics section
    //  $(".fa-line-chart").click(function(){
    //  $('#analsys').fadeIn()
    //  $("#assets").hide(1000);
    //     $('#content-section').hide();
    //         $('#state').hide();
    //           $('#profile').hide();
    //           $('#balance').hide();
    //            $('#render').hide();
    //            $('#render').hide();


    //});
    // show render section
    //      $(".fa-tachometer").click(function(){
    //  $('#render').fadeIn();
    //  $("#assets").hide();
    //  $('#analsys').hide();
    //     $('#content-section').hide();
    //         $('#state').hide();
    //           $('#profile').hide();
    //           $('#balance').hide();


    //});

    // show analytics section
    //  $(".fa-bar-chart").click(function(){
    //  $('#state').fadeIn()
    //  $("#assets").hide(1000);
    //     $('#content-section').hide();
    //      $('#analsys').hide();
    //      $('#balanc').hide();
    //        $('#profile').hide();
    //         $('#render').hide();


    //});

    $(".fa-credit-card").click(function () {
        $('#balanc').fadeIn()
        $("#assets").hide(1000);
        $('#content-section').hide();
        $('#analsys').hide();
        $('#state').hide();
        $('#profile').hide();
        $('#render').hide();

    });


    $(".fa-user").click(function () {
        $('#profile').fadeIn()
        $("#assets").hide(1000);
        $('#content-section').hide();
        $('#analsys').hide();
        $('#state').hide();
        $('#balance').hide();
        $('#render').hide();

    });
});


$('#DocumentParameterbutton').click(function () {
    if ($("#DocumentList").val() == '0') {
        $('#Parameter').hide();
        $('#documentmsg').text("Please select document");
    }
    else if ($("#parameter").val() == '0') {
        $('#parametermsg').text("Please select parameter");
    }
    else {
        // $('#DocumentParameter').submit();
    }
});
$(document).ready(function () {
    if ($("#DocumentList").val() == '0') {
        $('#Parameter').hide();
    }
});

$("#DocumentList").on("change", function () {
    if ($(this).val() === '0') {
        $('#Parameter').hide();
    } else {
        $("#SetDocuemntId").val($(this).val());
        $('#Parameter').show();
    }
});


$("#parameter").on("change", function () {
    var value = 0;
    var selectedValues = $('#parameter').val();

    for (i = 0; i < selectedValues.length; i++) {
        value = value + parseInt(selectedValues[i]);
    }
    $("#price").text(value);
});

$(function () {
    $("input[id*='txtQty']").keydown(function (event) {


        if (event.shiftKey == true) {
            event.preventDefault();
        }

        if ((event.keyCode >= 48 && event.keyCode <= 57) || (event.keyCode >= 96 && event.keyCode <= 105) || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 39 || event.keyCode == 46 || event.keyCode == 190) {

        } else {
            event.preventDefault();
        }

        if ($(this).val().indexOf('.') !== -1 && event.keyCode == 190)
            event.preventDefault();

    });
});

$(document).on('keydown', 'input[pattern]', function (e) {
    var input = $(this);
    var oldVal = input.val();
    var regex = new RegExp(input.attr('pattern'), 'g');

    setTimeout(function () {
        var newVal = input.val();
        if (!regex.test(newVal)) {
            input.val(oldVal);
        }
    }, 0);
});

$.fn.sort_select_box = function () {
    // Get options from select box
    var my_options = $(this).children('option');
    // sort alphabetically
    my_options.sort(function (a, b) {
        if (a.text > b.text) return 1;
        else if (a.text < b.text) return -1;
        else return 0
    })
    //replace with sorted my_options;
    $(this).empty().append(my_options);

    // clearing any selections
    $("#" + this.attr('id') + " option").attr('selected', false);
}





$(document).on("click", "#submitfrombutton", function (e) {

    e.preventDefault();
    $.ajax({
        url: document.location.origin +'/Parameters/AdditionalParameter',
        type: 'POST',
        data: $('#submitfrom').serialize(),
        success: function (data) {
            var id = $("#DocumentIdoFadditionalPara").val();
            if (data.status == "Success") {
                $.get(document.location.origin +'/Parameters/SetParameter?id=' + id,
                    function (data) {
                        $('.bodydata').html(data);
                    });
                jQuery.noConflict();
                $("#myModal").modal("show");
            }
            else {
                alert("Something went wrong");
            }


        }
    });
});
