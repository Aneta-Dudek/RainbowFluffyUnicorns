// Get the container element
var navBarContainer = document.getElementById("mytopnav");

// Get all buttons with class="btn" inside the container
var menuItems = navBarContainer.getElementsByClassName("mytopnav-item");

// Loop through the buttons and add the active class to the current/clicked button
for (var i = 0; i < menuItems.length; i++) {
    menuItems[i].addEventListener("click", function () {
        var current = document.getElementsByClassName("active");
        current[0].className = current[0].className.replace(" active", "");
        this.className += " active";
    });
}


/* Toggle between adding and removing the "responsive" class to topnav when the user clicks on the icon */
function responsiveTopBar() {
    var x = document.getElementById("mytopnav");
    if (x.className === "topnav") {
        x.className += " responsive";
    } else {
        x.className = "topnav";
    }
}