$(function () {
	$('#back').click(function () {
		parent.history.back();
		return false;
	});
});