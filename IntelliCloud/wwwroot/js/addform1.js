var h = 1; var j = 1; var k = 1; var l = 1; var m = 1; var n = 1; var o = 1;
var p = 1; var q = 1; var r = 1; var s = 1; var t = 1; var v = 1; var w = 1;
var aa = 1; var bb = 1; var cc = 1; var dd = 1; var ee = 1;
function addform1() {

    f_table = document.getElementById("fieldset1").parentNode;
    var newtable1 = document.getElementById("fieldset1").cloneNode(false);
    newtable1.id = "fieldset1" + h;
    f_table.appendChild(newtable1);

    var newtable2 = document.getElementById("table1").cloneNode(false);
    newtable2.id = "table1" + j;
    f_table.appendChild(newtable2);


    var newtable3 = document.getElementById("tabletr1").cloneNode(false);
    newtable3.id = "tabletr1" + k;
    f_table.appendChild(newtable3);


    var newtable4 = document.getElementById("tabletd1").cloneNode(true);
    newtable4.id = "tabletd1" + l;
    f_table.appendChild(newtable4);

    var newtable5 = document.getElementById("tabletd2").cloneNode(false);
    newtable5.id = "tabletd2" + m;
    f_table.appendChild(newtable5);

    var newtable6 = document.getElementById("wordbutton1").cloneNode(true);
    newtable6.id = "wordbutton1" + n;
    f_table.appendChild(newtable6);

    var newtable7 = document.getElementById("table2").cloneNode(true);
    newtable7.id = "table2" + o;
    f_table.appendChild(newtable7);

    var newtable8 = document.getElementById("table3").cloneNode(true);
    newtable8.id = "table3" + p;
    f_table.appendChild(newtable8);

    var newtable9 = document.getElementById("table4").cloneNode(false);
    newtable9.id = "table4" + q;
    f_table.appendChild(newtable9);


    var newtable10 = document.getElementById("form_edu").cloneNode(false);
    newtable10.id = "table_edu1" + r;
    newtable10.style.borderColor = "#0d2798";
    f_table.appendChild(newtable10);


    var newtable11 = document.getElementById("form_edu1").cloneNode(false);
    newtable11.id = "table_edu2" + s;
    f_table.appendChild(newtable11);

    var newtable12 = document.getElementById("selectword1").cloneNode(true);
    newtable12.id = "table_edu3" + t;
    f_table.appendChild(newtable12);

    var newtable13 = document.getElementById("form_edu2").cloneNode(false);
    newtable13.id = "table_edu4" + v;
    f_table.appendChild(newtable13);

    var newtable14 = document.getElementById("wordbutton3").cloneNode(false);
    newtable14.id = "table_edu5" + w;
    f_table.appendChild(newtable14);



    var newtable15 = document.getElementById("tabletr5").cloneNode(false);
    newtable15.id = "table_edu6" + aa;
    newtable15.style.borderColor = "#0d2798";
    f_table.appendChild(newtable15);


    var newtable16 = document.getElementById("tabletd15").cloneNode(false);
    newtable16.id = "table_edu7" + bb;
    f_table.appendChild(newtable16);

    var newtable17 = document.getElementById("selectword2").cloneNode(true);
    newtable17.id = "table_edu8" + cc;
    f_table.appendChild(newtable17);

    var newtable18 = document.getElementById("tabletd25").cloneNode(false);
    newtable18.id = "table_edu9" + dd;
    f_table.appendChild(newtable18);

    var newtable19 = document.getElementById("wordbutton5").cloneNode(false);
    newtable19.id = "table_edu10" + ee;
    f_table.appendChild(newtable19);

    aa++; bb++; cc++; dd++; ee++;
    h++; j++; k++; l++; m++; n++;
    o++; p++; q++; r++; s++; t++; v++; w++;
}