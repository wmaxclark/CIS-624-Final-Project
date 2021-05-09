//$(document).ready(function () {
//    $("#GerminationDate").datepicker();
//    $("#PlantDate").datepicker();
//    $("#TransplantDate").datepicker();
//    $("#HarvestDate").datepicker();

//    var GerminationDate = $("#GerminationDate").datepicker("getDate");
//    var PlantDate = $("#PlantDate").datepicker("getDate");
//    var TransplantDate = $("#TransplantDate").datepicker("getDate");
//    var HarvestDate = $("#HarvestDate").datepicker("getDate");
//    var daysAfterToPlant = parseInt((PlantDate - GerminationDate) / 84000000);
//    var daysAfterToTransplant = parseInt((TransplantDate - GerminationDate) / 84000000);
//    var daysAfterToHarvest = parseInt((HarvestDate - GerminationDate) / 84000000);

    
    
//    console.log("Something Happened.");
//    $("#GerminationDateBox").on("mouseout", function () {
//        $("#GerminationDate").attr("value", $("#GerminationDate").datepicker("getDate")).trigger("change");

//        console.log(daysAfterToPlant);
//        var other = addDays($("#GerminationDate").datepicker("getDate"), daysAfterToPlant).toISOString().substring(0, 10);

//        console.log(other);

//        // Handle plant date at same interval from original
//        var date = $.datepicker.parseDate("mm/dd/yy",$("#GerminationDate").val());

//        console.log(date);

//        // Handle transplant date at same interval from original

//        // Handle harvest date at same interbal from original
//    });
    
//});

//function addDays(date, days) {
//    var result = new Date(date);
//    result.setDate(result.getDate() + days);
//    return result;
//}

