document.getElementById("ST_div").style.display = "none";
document.getElementById("ST_div1").style.display = "none";
document.getElementById("ST_div2").style.display = "none";
document.getElementById("FH_div").style.display = "none";
document.getElementById("FH_div1").style.display = "none";
document.getElementById("FH_div2").style.display = "none";
var direction = "";
var Des = "";
var v1 = "";
var v2 = "";
var v3 = "";
var v4 = "";
var v5 = "";
var v6 = "";
sessionStorage.clear();
sessionStorage.setItem("OmmisionStatus", "True");

function disclaimer() {
    document.getElementById("disclaimer").style.display = "none";
    document.getElementById("form").style.display = "";

}
function agree() {

    $("#Agree").hide();
    $("#btn_close").removeAttr("hidden");

}

function checkFH() {
    document.getElementById("FH_div").style.display = "block";
    document.getElementById("ST_div").style.display = "none";

    document.getElementById("FH_div1").style.display = "block";
    document.getElementById("ST_div1").style.display = "none";

    document.getElementById("FH_div2").style.display = "block";
    document.getElementById("ST_div2").style.display = "none";

    document.getElementById("ST_Town").value = "";
    document.getElementById("ST_Scheme").value = "";
    document.getElementById("Scheme_Number").value = "";
    document.getElementById("Scheme_Year").value = "";
    document.getElementById("ST_Unit").value = "";
    document.getElementById("ST_Right").value = "";

    direction = "FH";
    $('#FH_Town').attr('required', true);
    $('#FH_ERF').attr('required', true);
    $('#FH_Portion').attr('required', true);
    $('#FH_Re').attr('required', true);
    $("#ST_Town").removeAttr("required");
    $("#ST_Scheme").removeAttr("required");
    $("#Scheme_Number").removeAttr("required");
    $("#Scheme_Year").removeAttr("required");
    $("#ST_Unit").removeAttr("required");
}

function checkST() {
    sessionStorage.clear();
    sessionStorage.setItem("OmmisionStatus", "True");
    document.getElementById("ST_div").style.display = "block";
    document.getElementById("FH_div").style.display = "none";

    document.getElementById("ST_div1").style.display = "block";
    document.getElementById("FH_div1").style.display = "none";

    document.getElementById("ST_div2").style.display = "block";
    document.getElementById("FH_div2").style.display = "none";

    document.getElementById("FH_Town").value = "";
    document.getElementById("FH_ERF").value = "";
    document.getElementById("FH_Portion").value = "";
    document.getElementById("FH_Re").value = "";
    document.getElementById("FH_Right").value = "";
    

    direction = "ST";
    $('#ST_Town').attr('required', true);
    $('#ST_Scheme').attr('required', true);
    $('#Scheme_Number').attr('required', true);
    $('#Scheme_Year').attr('required', true);
    $('#ST_Unit').attr('required', true);
    $("#FH_Town").removeAttr("required");
    $("#FH_ERF").removeAttr("required");
    $("#FH_Portion").removeAttr("required");
    $("#FH_Re").removeAttr("required");
}

function Save() {
    if (direction == "FH") {
        sessionStorage.clear();
        sessionStorage.setItem("OmmisionStatus", "True");

        v1 = document.getElementById("FH_Re").value;
        v2 = document.getElementById("FH_Portion").value;
        v3 = document.getElementById("FH_ERF").value;
        v4 = document.getElementById("FH_Town").value;
        v5 = document.getElementById("FH_Right").value;
        sessionStorage.setItem("TownNameOmmit", v4);
        Des = "";
        if (v1=="RE") {

            Des = (v5 + v4 + " Erf " + v3 + " Portion " + v2 + " " + v1);
        }
        if (v1 == "00") {

            Des = (v5 + v4 + " Erf " + v3 + " Portion " + v2);
        }
        sessionStorage.setItem("PropertyDescOmmit", Des);
        //alert(Des);

    }

    if (direction == "ST") {
        sessionStorage.clear();
        sessionStorage.setItem("OmmisionStatus", "True");
        v1 = document.getElementById("ST_Scheme").value;
        v2 = document.getElementById("ST_Town").value;
        v3 = document.getElementById("Scheme_Number").value;
        v4 = document.getElementById("Scheme_Year").value;
        v5 = document.getElementById("ST_Unit").value;
        v6 = document.getElementById("ST_Right").value;
        Des = "";
        Des = (v6 + v1+ "  ("+ v3+"/" + v4 + "), Unit " + v5 + ", " + v2);
        sessionStorage.setItem("PropertyDescOmmit", Des);
        sessionStorage.setItem("TownNameOmmit", v2);
        //alert(Des);
    }
}

function onlyNumberKey(evt) {

    // Only ASCII character in that range allowed
    var ASCIICode = (evt.which) ? evt.which : evt.keyCode
    if (ASCIICode > 31 && (ASCIICode < 48 || ASCIICode > 57))
        return false;
    return true;
}


document.getElementById("disclaimer").style.display = "";

function disclaimer() {
    document.getElementById("disclaimer").style.display = "none";

    document.getElementById("form").style.display = "";

    direction = "FH";
    //document.getElementById("FH_div").style.display = "";
    //document.getElementById("FH_div1").style.display = "";
    //document.getElementById("FH_div2").style.display = "";

}
function agree() {

    $("#Agree").hide();
    $("#btn_close").removeAttr("hidden");

}