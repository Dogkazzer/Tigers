// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    console.log("hey tiger fans");
    var theForm = $("#theForm"); //jQuery syntax using the dollar alias instead of jquery
    theForm.hide(); //this calls an api to hide the form that has been identified
    var button = $("#buyButton");
    button.on("click", function () {
        console.log("Buying item");  //on a click call this function
    });
    var productInfo = $(".product-props");
    productInfo.on("click", function () {
        console.log("You clicked on " + $(this).text());
    });
    var $loginToggle = $("#loginToggle");
    var $popupForm = $(".popup-form");

    $loginToggle.on("click", function (){
        $popupForm.toggle(1000);
    });

});