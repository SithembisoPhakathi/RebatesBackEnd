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
		window.location.replace("Home/Support");
	}

}


window.onload = function () {
		OpenBootstrapPopup();
	};
	function OpenBootstrapPopup() {
		$("#PopUp").modal('show');
		var display = sessionStorage.getItem["StartController"];
		if(display == null)
		{
			$("#PopUp").modal('show');
			sessionStorage.setItem("StartController", "True");
			
		}
		else
		{
			alert(sessionStorage.getItem["StartController"]);
		}
	}