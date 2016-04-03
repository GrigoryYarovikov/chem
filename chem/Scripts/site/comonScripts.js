function drawScheme() {
    jQuery(function () {
        if (jQuery("body").hasClass("echem-auto-compile") || jQuery(".easyChemConfig").hasClass("auto-compile")) {
            ChemJQ.autoCompile()
        }
    });
}