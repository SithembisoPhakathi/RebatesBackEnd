document.getElementById("pass_input").disabled = true;
document.getElementById("pass_input_L").disabled = false;
function enable_ID() {
    document.getElementById("pass_input_L").disabled = true;
    document.getElementById("id_input_L").disabled = false;
    document.getElementById("pass_input").disabled = true;
    document.getElementById("id_input").disabled = false;
}
function disable_ID() {
    document.getElementById("pass_input_L").disabled = false;
    document.getElementById("id_input_L").disabled = true;
    document.getElementById("pass_input").disabled = false;
    document.getElementById("id_input").disabled = true;
}




function callback() {

    var submitButton = document.getElementById("registerSubmit");

    submitButton.removeAttribute("hidden");
    submitButton.disabled = false;

}