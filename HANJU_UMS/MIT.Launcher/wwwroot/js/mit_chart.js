
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


//import Chart from 'chart.js/auto';
//import zoomPlugin from 'chartjs-plugin-zoom';
//const zoomPlugin = window['chartjs-plugin-zoom'];

// 등록
//Chart.register(zoomPlugin);

//import * as zoomPlugin from '/js/chartjs-plugin-zoom.min.js';
//Chart.register(zoomPlugin);

//import * as zoomPlugin from '/js/chartjs-plugin-zoom.min.js';
//import zoomPlugin from '/js/chartjs-plugin-zoom.esm.js';
//Chart.register(zoomPlugin);


Chart.register(window.ChartZoom);

const verticalLinePlugin = {
  id: 'customVerticalLine',
  afterDraw(chart) {
    if (chart.tooltip?._active?.length) {
      const ctx = chart.ctx;
      const x = chart.tooltip._active[0].element.x;
      const topY = chart.scales.y.top;
      const bottomY = chart.scales.y.bottom;

      ctx.save();
      ctx.beginPath();
      ctx.moveTo(x, topY);
      ctx.lineTo(x, bottomY);
      ctx.lineWidth = 1;
      ctx.strokeStyle = 'rgba(220, 53, 69, 1)'; //#dc3545
      ctx.stroke();
      ctx.restore();
    }
  },
  title: {
    display: false,
    text: 'xxxxx'
  },
  legend: {
    display: false
  },
  tooltip: {
    mode: 'index',
    intersect: false
  }
};

Chart.register(verticalLinePlugin); // 반드시 등록

const horizontalLinePlugin = {
  id: 'customHorizontalLine',
  afterDraw(chart) {
    if (chart.tooltip?._active?.length) {
      const ctx = chart.ctx;
      const y = chart.tooltip._active[0].element.y;
      const leftX = chart.scales.x.left;
      const rightX = chart.scales.x.right;

      ctx.save();
      ctx.beginPath();
      ctx.moveTo(leftX, y);
      ctx.lineTo(rightX, y);
      ctx.lineWidth = 1;
      ctx.strokeStyle = 'rgba(220, 53, 69, 1)'; //#dc3545
      ctx.stroke();
      ctx.restore();
    }
  },
  title: {
    display: false
  },
  legend: {
    display: false
  },
  tooltip: {
    mode: 'index',
    intersect: false
  }
};

//Chart.register(horizontalLinePlugin); // 반드시 등록




let mouseY = null;

const horizontalLineOnMousePlugin = {
  id: 'horizontalLineOnMouse',
  afterEvent(chart, args) {
    const event = args.event;
    if (event.type === 'mousemove') {
      mouseY = event.y;
    } else if (event.type === 'mouseout') {
      mouseY = null;
    }
  },
  afterDraw(chart) {
    if (mouseY !== null) {
      const ctx = chart.ctx;
      const leftX = chart.scales.x.left;
      const rightX = chart.scales.x.right;

      ctx.save();
      ctx.beginPath();
      ctx.moveTo(leftX, mouseY);
      ctx.lineTo(rightX, mouseY);
      ctx.lineWidth = 1;
      ctx.strokeStyle = 'rgba(95, 54, 141, 1)'; //#5f368d
      ctx.stroke();
      ctx.restore();
    }
  }
};



Chart.register(horizontalLineOnMousePlugin); // 반드시 등록














  var bgColor = ['Red', 'Orange', 'Yellow', 'Green', 'Blue', 'Purple', 'Grey'];
  var CHART_COLORS = [
  '#5f368d',
  '#ff0000', // red
  //'rgb(255, 99, 132)', // red
  'rgb(255, 159, 64)', // orange
  'rgb(255, 205, 86)', // yellow
  'rgb(75, 192, 192)', // green
  'rgb(54, 162, 235)', // blue
  'rgb(153, 102, 255)', // purple
  'rgb(201, 203, 207)' //grey
  ];

  var ctx = null; // document.getElementById('myChart').getContext('2d');

  var data = {
    labels: [],
  datasets: []
  };

  function transparentize(value, opacity) {
    var alpha = opacity === undefined ? 0.5 : 1 - opacity;
  return colorLib(value).alpha(alpha).rgbString();
  }



  var _tinfos = null;

  var _chart = null;
var config = null;


window.chartDeleyLoad = function (tinfos) {
  setTimeout(() => {
    chartSetData(tinfos);
  }, 500);
}

window.chartSetData = function ( tinfos ){

  if (_chart) {
    _chart.destroy();
    _chart = null;
  }

  var w__h = $(window).height();
  var scd = $('.split-chat-div');//.height();
  scd.height(w__h - 300);

  var mchart = $('#myChart');
  var _w = mchart.width();
  var _h = mchart.height();
  mchart.attr('width', _w);
  mchart.attr('height', _h);

  if (_chart == null) {
    ctx = document.getElementById('myChart').getContext('2d');

    config = {
      type: 'line',
      data: data,
      options: {
        responsive: false,
        interaction: {
          mode: 'index',
          intersect: false,
        },
        spanGaps: true,
        stacked: false,
        plugins: {   //verticalLinePlugin
          zoom: {
            pan: {
              enabled: true,
              mode: 'x', // or 'xy', 'y'
            },
            zoom: {
              wheel: {
                enabled: true, // 마우스 휠로 줌 가능
              },
              pinch: {
                enabled: true, // 터치 핀치 줌
              },
              mode: 'x', // x축 기준 줌 (또는 'xy', 'y')
              onZoom({ chart }) {


                const xAxis = chart.scales.x;
                const start = xAxis.min;
                const end = xAxis.max;

                if (window.blazorZoomCallback) {
                  window.blazorZoomCallback(start, end);
                }

                // 예: 줌 범위에 따라 서버 요청


              },
            }
          },
          legend: {
            display: false,
          },
        }

  ,
  scales: {
    y: {
    type: 'linear',
  display: false,
  position: 'left',
  //min: 0,  // 최소값 설정
  //max: 50, // 최대값 설정
  ticks: {
    //stepSize: 500 // 간격 지정 (선택 사항)
  }
               },
  x: {
    ticks: {
    minRotation: 0, // 최소 회전 각도
  maxRotation: 0, // 최대 회전 각도
  callback: function(value, index, values) {
                              // 너무 길면 잘라서 표현
                              var label = this.getLabelForValue(value);
  label = label.substring(2,10);
  var aaa = label.split(' ');
  //return label.length > 10 ? label.slice(0, 10) + '…' : label;
  return aaa;
                              //return [aaa[0].substring(2,4), aaa[1].substring(0,4)];
                            }
                  }
                }
          }
        },
      };


  _chart =   new Chart(ctx, config);

      }


  var legend = false;
  var monoton = 'monotone';


  config.data = {
    labels: [],
  datasets: []
  };


  //config.data.datasets = [];
  config.options.plugins.legend.display = legend;


  _tinfos = tinfos;

  var isFrist = true;
  for (var i = 0; i < tinfos.length; i++) {

                
        var ti = _tinfos[i];



  if(isFrist){
    config.data.labels = ti.labels;
  isFrist = false;
                }
  var dset = { };
  dset.label = ti.title;
  dset.data = ti.qtys;
  dset.yAxisID = ti.id;
  dset.pointStyle = false;
  dset.cubicInterpolationMode = monoton; //'monotone';
  //dset.backgroundColor = '#ff0000';//CHART_COLORS[i];
    dset.backgroundColor = ti.rQtyColor;
    dset.borderColor = ti.rQtyColor;
  config.data.datasets[i] = dset;



              }

  JsSetLoad2(tinfos );



    }



window.chartAddSetData = function (tinfos) {

  _tinfos = tinfos;

  var isFrist = true;
  for (var i = 0; i < tinfos.length; i++) {

    var ti = _tinfos[i];
    if (isFrist) {
      config.data.labels = ti.labels;
      isFrist = false;
    }


    //config.data.datasets[i].label = ti.title;
    config.data.datasets[i].data = ti.qtys;

  }


  // Zoom에 의해 제한된 x축 범위 초기화
  //chart.options.scales.x.min = undefined;
  //chart.options.scales.x.max = undefined;

  _chart.update();


}


window.JsSetLoad2 = function ( tis ) {

  config.options.scales = {
    y: {
      type: 'linear',
      display: false,
      position: 'left',
      //min: 0,  // 최소값 설정
      //max: 50, // 최대값 설정
      ticks: {
        //stepSize: 500 // 간격 지정 (선택 사항)
      },
      grid: {
        display: false // ← y축의 선 숨김
      }
    },
    x: {
      ticks: {
        minRotation: 0, // 최소 회전 각도
        maxRotation: 0, // 최대 회전 각도
        callback: function (value, index, values) {
          // 너무 길면 잘라서 표현
          const label = this.getLabelForValue(value);
          const aaa = label.split(' ');
          //return label.length > 10 ? label.slice(0, 10) + '…' : label;
          return aaa;
        }
      },
      grid: {
        display: true // ← x축의 세로선 숨김
      }
    }
  }
    ;


  for (var i = 0; i < tis.length; i++) {
    var ti = tis[i];



    config.data.datasets[i].hidden = !ti.isLineView;
    config.options.scales[ti.id] = {

      type: 'linear',
      display: ti.isYView,
      position: 'left',
      ticks: {
        maxRotation: 0,
        minRotation: 0,
        autoSkip: true,
        color: ti.rQtyColor,
      },
      grid: {
        display: false // ← y축의 선 숨김
      }
      , min: ti.moveMin
      , max: ti.moveMax


    };


  }


  _chart.update();




  document.querySelectorAll('input[type="range"]').forEach(input => {
    input.value = 0;
    //input.dispatchEvent(new Event('input')); // input 이벤트 트리거
  });


}




