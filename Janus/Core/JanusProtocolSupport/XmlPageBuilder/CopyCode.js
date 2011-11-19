
function copyCode(source) {
	if (!source) return;
	
	// table/tbody/tr/td
	var table = source.parentElement.parentElement.parentElement;
	
	try {
		window.clipboardData.setData("Text", table.rows[1].cells[0].innerText);
	}
	catch (e) {
	}
	
	return false;
}

function build(source) {
	if (!source)
		return;
		
	try {
		// table/tbody/tr/td
		var table = source.parentElement.parentElement.parentElement;
		var tr    = table.insertRow(0);
		var td    = tr.insertCell();
		
		td.className = "copyCode";
		td.innerHTML = "<a href='#' onclick='return copyCode(this.parentElement)' title='Нажмите, чтобы скопировать код в буфер обмена'>Скопировать</a>";
	}
	catch (e) {
	}
}

function prepare() {
	var cells   = document.getElementsByTagName("td");
	var sources = [];

	for (var i in cells) {
		if (cells[i].className == "c")
			sources.push(cells[i]);
	}

	for (var i in sources)
		build(sources[i]);
}

prepare();
