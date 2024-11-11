$(document).ready(function () {
    document.getElementById("#label_scheme").style.display = "none";
    document.getElementById("#SearchScheme").style.display = "none";

    var w = document.getElementById("label_stand");
    var x = document.getElementById("SearchStand");
    var y = document.getElementById("label_scheme");
    var z = document.getElementById("SearchScheme");


    document.getElementById("optionsRadios1").addEventListener("click", full_title);
    document.getElementById("optionsRadios2").addEventListener("click", scheme);

    function full_title() {
        w.style.display = "block";
        x.style.display = "block";
        y.style.display = "none";
        z.style.display = "none";

    }
    function scheme() {
        w.style.display = "none";
        x.style.display = "none";
        y.style.display = "block";
        z.style.display = "block";
    }
});

sessionStorage.clear();
sessionStorage.setItem("OmmisionStatus", "False");

function checkBrowser() {
    //Result
    var result;
    // Get the user-agent string
    let userAgentString =
        navigator.userAgent;

    // Detect Chrome
    let chromeAgent =
        userAgentString.indexOf("Chrome") > -1;

    // Detect Internet Explorer
    let IExplorerAgent =
        userAgentString.indexOf("MSIE") > -1 ||
        userAgentString.indexOf("rv:") > -1;

    // Detect Firefox
    let firefoxAgent =
        userAgentString.indexOf("Firefox") > -1;

    // Detect Safari
    let safariAgent =
        userAgentString.indexOf("Safari") > -1;

    // Discard Safari since it also matches Chrome
    if ((chromeAgent) && (safariAgent))
        safariAgent = false;

    // Detect Opera
    let operaAgent =
        userAgentString.indexOf("OP") > -1;

    // Discard Chrome since it also matches Opera
    if ((chromeAgent) && (operaAgent))
        chromeAgent = false;

    if (IExplorerAgent == true) {
        result = true;
        window.location.replace("unsupport");
    }

}