
var ab = 1; var ac = 1; var ad = 1; var af = 1; var ae = 1;
var ag = 1; var ah = 1; var ai = 1; var aj = 1;
function addform2() {

    f_Exceltable = document.getElementById("Excelfieldset1").parentNode;
    var newExceltable1 = document.getElementById("Excelfieldset1").cloneNode(false);
    newExceltable1.id = "Excelfieldset1" + ab;
    f_Exceltable.appendChild(newExceltable1);

    var newExceltable = document.getElementById("Excelfieldset0").cloneNode(true);
    f_Exceltable.appendChild(newExceltable);


    var newExceltable2 = document.getElementById("Exceltr1").cloneNode(true);
    f_Exceltable.appendChild(newExceltable2);


    var newExceltable3 = document.getElementById("Excelbutton1").cloneNode(false);
    newExceltable3.id = "Excelbutton1" + ac;
    f_Exceltable.appendChild(newExceltable3);


    var newExceltable4 = document.getElementById("Excelbutton2").cloneNode(true);
    newExceltable4.id = "Excelbutton2" + ad;
    f_Exceltable.appendChild(newExceltable4);

    var newExceltable5 = document.getElementById("Excelbutton3").cloneNode(false);
    newExceltable5.id = "Excelbutton3" + ae;
    f_Exceltable.appendChild(newExceltable5);

    var newExceltable6 = document.getElementById("Excelbutton4").cloneNode(false);
    newExceltable6.id = "Excelbutton4" + af;
    f_Exceltable.appendChild(newExceltable6);

    var newExceltable7 = document.getElementById("Exceltr3").cloneNode(true);
    f_Exceltable.appendChild(newExceltable7);

    var newExceltable8 = document.getElementById("Excelbutton5").cloneNode(false);
    newExceltable8.id = "Excelbutton5" + ag;
    f_Exceltable.appendChild(newExceltable8);

    var newExceltable9 = document.getElementById("Excelbutton6").cloneNode(false);
    newExceltable9.id = "Excelbutton6" + ah;
    f_Exceltable.appendChild(newExceltable9);


    var newExceltable10 = document.getElementById("Excelbutton7").cloneNode(false);
    newExceltable10.id = "Excelbutton7" + ai;
    f_Exceltable.appendChild(newExceltable10);


    var newExceltable11 = document.getElementById("Excelbutton8").cloneNode(false);
    newExceltable11.id = "Excelbutton8" + aj;
    f_Exceltable.appendChild(newExceltable11);

    ad++; ac++; ad++; ae++;
    af++; ag++; ah++; ai++; aj++;

    
}
