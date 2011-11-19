
function showMedia() {
	if (this.mediaSrc) {
		var media = "<div style='margin-top:4px;'><object width='425' height='350'><param name='movie' value='@value'></param><param name='wmode' value='transparent'></param><embed src='@value' type='application/x-shockwave-flash' wmode='transparent' width='425' height='350'></embed></object><br/>@src</div>"
			.replace(/@value/g, this.mediaSrc)
			.replace(/@src/g,   this.href);
		
		this.parentNode.innerHTML = media;
		return false;
	}
	
	return true;
}

function prepareMedia() {
	var links = document.getElementsByTagName("a");
	
	for (var i in links) {
		if (links[i].mediaSrc)
			links[i].onclick = showMedia;
	}
}

prepareMedia();