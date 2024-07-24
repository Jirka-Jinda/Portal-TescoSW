// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.






function onAppLayoutLoad() {
    var cookieValue = document.cookie
        .split(';')
        .map(cookie => cookie.trim())
        .find(cookie => cookie.startsWith('MPSopt='))
        .split('=')[1];

    var optSwitch = document.getElementById("OptionsSwitch");

    if (optSwitch != null) {
        if (cookieValue == "false") {
            optSwitch.setAttribute("value", false);
            optSwitch.checked = false;
        }
        else {
            optSwitch.setAttribute("value", true);
            optSwitch.checked = true;
        }
    }

    optSwitch.addEventListener("change", function () {
        var optSwitch = document.getElementById("OptionsSwitch");
        var cookieValue = document.cookie
            .split(';')
            .map(cookie => cookie.trim())
            .find(cookie => cookie.startsWith('MPSopt='))
            .split('=')[1];

        if (cookieValue != null) {
            if (optSwitch.value == "false") {
                document.cookie = "MPSopt=true";
                optSwitch.setAttribute("value", true);

            }
            else if (optSwitch.value == "true") {
                document.cookie = "MPSopt=false";
                optSwitch.setAttribute("value", false);          
            }
        }

        location.reload();
    });
}