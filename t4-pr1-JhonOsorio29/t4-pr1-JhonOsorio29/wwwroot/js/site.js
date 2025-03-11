// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

document.addEventListener("DOMContentLoaded", function () {
    document.getElementById("mostrar_formulario").addEventListener("click", function () {
        document.getElementById("formulario").style.display = "block";
    });

    //Para cambiar el nombre de parametro a el tipo que sea
    document.getElementById("Tipe").addEventListener("change", function () {
        var parameterLabel = document.getElementById("parameter-label");
        switch (this.value) {
            case "Solar":
                parameterLabel.textContent = "Horas de sol";
                break;
            case "Eolic":
                parameterLabel.textContent = "Velocidad del viento";
                break;
            case "Hidroeléctrico":
                parameterLabel.textContent = "Cabal de agua";
                break;
            default:
                parameterLabel.textContent = "Parámetro";
        }
    });

    if (window.location.search.includes("handler=Post")) {
        document.getElementById("formulario").style.display = "none";
    }
})


