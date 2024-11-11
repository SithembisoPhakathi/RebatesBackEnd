
function done() {
    history.pushState('', '', '/Home/Index');
    history.pushState('', '', '/Home/Index');
    history.pushState('', '', '/Home/Index');
    history.pushState('', '', '/Home/Index');
    history.pushState('', '', '/Home/Index');
    history.pushState('', '', '/Home/Index');
    history.pushState('', '', '/Home/Index');
    history.pushState('', '', '/Home/Index');
    history.pushState('', '', '/Home/Index');
    history.pushState('', '', '/Home/Index');
    history.pushState('', '', '/Home/Index');
    history.pushState('', '', '/Home/Index');
    history.pushState('', '', '/Home/Index');
    history.pushState('', '', '/Home/Index');
    history.pushState('', '', '/Home/Index');
    history.pushState('', '', '/Home/Index');
    history.pushState('', '', '/Home/Index');
    history.pushState('', '', '/Home/Index');
    history.pushState('', '', '/Home/Index');
    history.pushState('', '', '/Home/Index');

    //printJS('acknowledge', 'html');
    close();

    function close() {

        document.getElementById("close").removeAttribute("hidden");
    }

}

var AppealStatus = sessionStorage.getItem('AppealStatus');
if (AppealStatus == "True") {
    document.getElementById("line1").innerHTML = "appeal";
    document.getElementById("line2").innerHTML = "APPEAL";
    document.getElementById("line3").innerHTML = "Thank you for Lodging your Appeal";
    document.getElementById("line4").innerHTML = "PROPERTY DETAILS AS LISTED ON Municipal Valuers' Decision (MVD)";
    document.getElementById("line5").innerHTML = "PROPERTY DETAILS AS OBJECTED ON Municipal Valuers' Decision (MVD)"; 
    document.getElementById("obj_ref").innerHTML = "OBJECTION NUMBER: ";
}
document.getElementById("descr").innerHTML =
    sessionStorage.getItem("desc");
document.getElementById("ownner").innerHTML =
    sessionStorage.getItem("owner");

    
document.getElementById("m_value").innerHTML =
    sessionStorage.getItem("Market_Value");
document.getElementById("category").innerHTML =
    sessionStorage.getItem("cat");

document.getElementById("rand").innerHTML =
    sessionStorage.getItem("pin");
