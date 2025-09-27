
/*
import Chart from 'chart.js/auto';
import zoomPlugin from 'chartjs-plugin-zoom';

Chart.register(zoomPlugin);

// 전역에 chart 생성 함수 정의
window.createZoomChart = function (canvasId, chartConfig) {
	const ctx = document.getElementById(canvasId).getContext('2d');
	return new Chart(ctx, chartConfig);
};
*/

//console.log("app.js loaded");

$(document).mousedown(function (e) {
	return;
	if ($(".expand").is(":visible")) {
		$(".expand").each(function () {
			var l_position = $(this).offset();
			l_position.right = parseInt(l_position.left) + ($(this).width());
			l_position.bottom = parseInt(l_position.top) + parseInt($(this).height());

			if ((l_position.left <= e.pageX && e.pageX <= l_position.right)
				&& (l_position.top <= e.pageY && e.pageY <= l_position.bottom)) {
			} else {
				//$(this).hide();
				$(this).toggleClass('expand');
				$(this).toggleClass('collapse');
			}
		});
	}
});





window.dynamicScriptLoader = {
	load: function (url) {
		return new Promise((resolve, reject) => {
			if (document.querySelector(`script[src="${url}"]`)) {
				resolve(); // 이미 로드됨
				return;
			}

			const script = document.createElement("script");
			script.src = url;
			script.type = "text/javascript";
			script.onload = resolve;
			script.onerror = () => reject(`Failed to load ${url}`);
			document.head.appendChild(script);
		});
	}
};


window.dynamicStyleLoader = {
	load: function (url) {
		return new Promise((resolve, reject) => {
			// 이미 로드된 경우 중복 방지
			if (document.querySelector(`link[href="${url}"]`)) {
				resolve();
				return;
			}

			const link = document.createElement("link");
			link.rel = "stylesheet";
			link.type = "text/css";
			link.href = url;

			link.onload = () => resolve();
			link.onerror = () => reject(`Failed to load CSS: ${url}`);

			document.head.appendChild(link);
		});
	}
};

let zoomTimeout = null;

window.registerBlazorCallback = function (dotNetHelper) {


	window.blazorZoomCallback = function (start, end) {
		if (zoomTimeout) clearTimeout(zoomTimeout);

		zoomTimeout = setTimeout(() => {
			dotNetHelper.invokeMethodAsync("OnChartZoomed", start + '', end + '');
		}, 1000); // 마지막 휠 이벤트 이후 1초 후 호출

	};




	//window.blazorZoomCallback = function (start, end) {
	//	dotNetHelper.invokeMethodAsync("OnChartZoomed", start+'', end+'');
	//};
}


