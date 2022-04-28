var num = 1;        //全局变量 默认为一组控件
        function add() {                    /* 增加人员行 */ 
            num++;
            var str = String.fromCharCode(64 + num);        
            var $tr = $("<tr class=\"addPerson\"><td width=\"30 %\" height=\"20 % \" align=\"center\"><select style=\"border - radius: 5px; width: 80 %; height: 40px\" name=\"selectti1\" id=\"selectword1\"><option>请选择分类</option><option>任意字符</option><option>数字</option></select></td><td width=\"30 % \" height=\"20 % \" align=\"center\"><input style=\"border: 1px solid #a1a1a1; border - radius: 5px; width: 80 %; height: 40px\" type=\"text\" placeholder=\"请填写长度\" id=\"wordbutton3\" name=\"wordbutton3\" /></td>< td width =\"30%\" height =\"20%\" align = \"center\" ><input class=\"wordbutton2\" id=\"wordbutton4\" type=\"button\" onclick=\"add()\" value=\"追加\" /></td> </tr>");
var $parent;
if (num == 1) {
    $parent = $("table tr:.addPersonTh");   //num默认为1 如果当前没元素就在标题后添加
} else {
    $parent = $("table tr:.addPerson:eq(" + (num - 2) + ")");   //num默认为1 进入add事件首先将num+1，所以此处要获取在哪里添加元素需-2
}
$parent.after($tr);
init();
        }