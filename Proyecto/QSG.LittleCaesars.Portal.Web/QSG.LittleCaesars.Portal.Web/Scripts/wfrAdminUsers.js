function check (event) {
	var _ul=$(event.target).parent().parent();
	var cs=$(event.target).attr('class');
	var cs1="";
	var csNew="_unchecked";

	var cl=$(event.target).parent().attr('class');

	//var _oldSU=FindSUcheck(event);
/*
	_ul.parent().removeClass();
	_ul.parent().addClass('RowNew');
	_ul.find('li.column_sttReg').children('img').removeClass();
	_ul.find('li.column_sttReg').children('img').addClass('_editing');
	_ul.removeClass();
	_ul.addClass('editRow');

	*/
	if(cs=="_unchecked")
		csNew="_checked";

	//console.log(cl);
/*
	if(cl=="column_Check"){
		AllChecked(event,csNew);
	}
*/
	$(event.target).removeClass();
	$(event.target).addClass(csNew);

/*
	if(!AllVer(event)){
		_ul.find('li.column_sttReg').children('img').removeClass();
		_ul.find('li.column_sttReg').children('img').addClass('_lock');
		//_ul.parent().removeClass();
		//_ul.parent().addClass('RowDelete');
		_ul.removeClass();
		_ul.addClass('removeRow');
		//_ul.find('li.column_eject').children('input').val(_oldSU);

		}*/
	
}
