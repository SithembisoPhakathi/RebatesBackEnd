var property_type = "";
var objector_type = "";

var x, y, z, x1, x2, x3, w1;
var direction = "Normal";

$("#form_key").hide();
var newD = sessionStorage.getItem("PropertyDescOmmit");
var OS = sessionStorage.getItem("OmmisionStatus");
var AppealStatus = sessionStorage.getItem("AppealStatus");
        
if (OS == "True") {
    //alert(newD + OS);
}

if (AppealStatus == "True") {
    document.getElementById("index_head").innerHTML = "<br /><br /><strong>Disclaimer | Online Appeal</strong><br />";
    document.getElementById("obj_head2").innerHTML = "Appellant";

}

//Residential Function
$("#btncheck1").click(function () {
    document.getElementById("obj_type").value = "Res";
    property_type = document.getElementById("obj_type").value;
    sessionStorage.setItem('property_choice', property_type);
    document.getElementById("p_type").innerHTML = "Residential (Full Title and Sectional Title used for Residential Purposes)";
    x = document.getElementById("btn_check1").checked;
    y = document.getElementById("btn_check2").checked;
    z = document.getElementById("btn_check3").checked;
    w1 = document.getElementById("btncheck4").checked;
    if (w1 == true) {
        direction = "Multi";
        sessionStorage.setItem('direction', direction);
    } else {
        direction = "Normal";
        sessionStorage.setItem('direction', direction);
    }
    if (x == true || y == true || z == true) {

        $("#form_key").show();
        callback();
    } else {
        $("#form_key").hide();
    }
});
//Agricultural Function
$("#btncheck2").click(function () {

    document.getElementById("obj_type").value = "Agric";
    property_type = document.getElementById("obj_type").value;
    sessionStorage.setItem('property_choice', property_type);
    document.getElementById("p_type").innerHTML = "Agricultural Holdings or Farms";
    x = document.getElementById("btn_check1").checked;
    y = document.getElementById("btn_check2").checked;
    z = document.getElementById("btn_check3").checked;
    w1 = document.getElementById("btncheck4").checked;

    if (w1 == true) {
        direction = "Multi";
        sessionStorage.setItem('direction', direction);
    } else {
        direction = "Normal";
        sessionStorage.setItem('direction', direction);
    }

    if (x == true || y == true || z == true) {
        $("#form_key").show();
        callback();
    } else {
        $("#form_key").hide();
    }
});
//Business Function 
$("#btncheck3").click(function () {

    document.getElementById("obj_type").value = "Bus";
    property_type = document.getElementById("obj_type").value;
    sessionStorage.setItem('property_choice', property_type);
    document.getElementById("p_type").innerHTML = "Properties other than Residential or Agricultural (e.g. Business, Factories, Offices, Schools)";
    x = document.getElementById("btn_check1").checked;
    y = document.getElementById("btn_check2").checked;
    z = document.getElementById("btn_check3").checked;
    w1 = document.getElementById("btncheck4").checked;

    if (w1 == true) {
        direction = "Multi";
        sessionStorage.setItem('direction', direction);
    } else {
        direction = "Normal";
        sessionStorage.setItem('direction', direction);
    }

    if (x == true || y == true || z == true) {

        $("#form_key").show();
        callback();
    } else {
        $("#form_key").hide();
    }
});
//Multiple Purpose Function 
$("#btncheck4").click(function () {

    document.getElementById("obj_type").value = "Multi";
    property_type = document.getElementById("obj_type").value;
    sessionStorage.setItem('property_choice', property_type);
    document.getElementById("p_type").innerHTML = "Multiple Purpose (The use of a property for more than one purpose)";
    x = document.getElementById("btn_check1").checked;
    y = document.getElementById("btn_check2").checked;
    z = document.getElementById("btn_check3").checked;
    w1 = document.getElementById("btncheck4").checked;

    if (w1 == true) {
        direction = "Multi";
        sessionStorage.setItem('direction', direction);
    } else {
        direction = "Normal";
        sessionStorage.setItem('direction', direction);
    }

    if (x == true || y == true || z == true) {

        $("#form_key").show();
        callback();
    } else {
        $("#form_key").hide();
    }

});

//Owner Function
$("#btn_check1").click(function () {
    document.getElementById("objector_type").value = "Owner";
    objector_type = document.getElementById("objector_type").value;
    sessionStorage.setItem('objector_choice', objector_type);
    //document.getElementById("o_type").innerHTML = "by Owner";
    document.getElementById("o_type").innerHTML = "Objector is the Owner";
    x1 = document.getElementById("btncheck1").checked;
    y1 = document.getElementById("btncheck2").checked;
    z1 = document.getElementById("btncheck3").checked;
    w1 = document.getElementById("btncheck4").checked;

    if (w1 == true) {
        direction = "Multi";
        sessionStorage.setItem('direction', direction);
    } else {
        direction = "Normal";
        sessionStorage.setItem('direction', direction);
    }

    if (x1 == true || y1 == true || z1 == true || w1 == true) {

        $("#form_key").show();
        callback();
    } else {
        $("#form_key").hide();
    }
});
//Objector Function
$("#btn_check2").click(function () {

    document.getElementById("objector_type").value = "Third_Party";
    objector_type = document.getElementById("objector_type").value;
    sessionStorage.setItem('objector_choice', objector_type);
    //document.getElementById("o_type").innerHTML = "by Objector";
    document.getElementById("o_type").innerHTML = "Objector is not the Owner or Municipality is the Objector";
    x1 = document.getElementById("btncheck1").checked;
    y1 = document.getElementById("btncheck2").checked;
    z1 = document.getElementById("btncheck3").checked;
    w1 = document.getElementById("btncheck4").checked;

    if (w1 == true) {
        direction = "Multi";
        sessionStorage.setItem('direction', direction);
    } else {
        direction = "Normal";
        sessionStorage.setItem('direction', direction);
    }

    if (x1 == true || y1 == true || z1 == true || w1 == true) {

        $("#form_key").show();
        callback();
    } else {
        $("#form_key").hide();
    }
});
//Representative Function 
$("#btn_check3").click(function () {

    document.getElementById("objector_type").value = "Representative";
    objector_type = document.getElementById("objector_type").value;
    sessionStorage.setItem('objector_choice', objector_type);
    //document.getElementById("o_type").innerHTML = "by Representative";
    document.getElementById("o_type").innerHTML = "Authorised Representative of the Objector";
    x1 = document.getElementById("btncheck1").checked;
    y1 = document.getElementById("btncheck2").checked;
    z1 = document.getElementById("btncheck3").checked;
    w1 = document.getElementById("btncheck4").checked;

    if (w1 == true) {
        direction = "Multi";
        sessionStorage.setItem('direction', direction);
    } else {
        direction = "Normal";
        sessionStorage.setItem('direction', direction);
    }

    if (x1 == true || y1 == true || z1 == true || w1 == true) {

        $("#form_key").show();
        callback();
    } else {
        $("#form_key").hide();
    }
});


//document.getElementById("btn_choice").disabled = true;
document.getElementById("disclaimer").style.display = "";

function disclaimer() {
    document.getElementById("disclaimer").style.display = "none";
    document.getElementById("main_index").style.display = "";
    document.getElementById("main_index2").style.display = "";

}
function agree() {

    $("#Agree").hide();
    $("#btn_close").removeAttr("hidden");

}

var drive = ""
function callback() {

    if (sessionStorage.getItem('direction') == "Multi") {

        if (sessionStorage.getItem('OmmisionStatus') == "True") {
            const submitButton = document.getElementById("btn_choice3");
            //submitButton.removeAttribute("disabled");
            submitButton.removeAttribute("hidden");
            document.getElementById("btn_choice").hidden = true;
            document.getElementById("btn_choice1").hidden = true;
            document.getElementById("btn_choice2").hidden = true;
        } else {
            const submitButton = document.getElementById("btn_choice1");
            //submitButton.removeAttribute("disabled");
            submitButton.removeAttribute("hidden");
            document.getElementById("btn_choice").hidden = true;
            document.getElementById("btn_choice2").hidden = true;
            document.getElementById("btn_choice3").hidden = true;

        }
    }
    if (sessionStorage.getItem('direction') == "Normal") {
        if (sessionStorage.getItem('OmmisionStatus') == "True") {
            const submitButton = document.getElementById("btn_choice2");
            document.getElementById("btn_choice").hidden = true;
            document.getElementById("btn_choice1").hidden = true;
            document.getElementById("btn_choice3").hidden = true;
            //submitButton.removeAttribute("disabled");
            submitButton.removeAttribute("hidden");
        } else {
            const submitButton = document.getElementById("btn_choice");
            //submitButton.removeAttribute("disabled");
            submitButton.removeAttribute("hidden");
            document.getElementById("btn_choice1").hidden = true;
            document.getElementById("btn_choice2").hidden = true;
            document.getElementById("btn_choice3").hidden = true;
        }
    }

}
