//$(window).on("load", function () {
//    var ctx = $('#mainChart');

//    // Chart Options
//    var chartOptions = {
//        responsive: true,
//        maintainAspectRatio: false,
//        plugins: {
//            legend: {
//                position: 'bottom',
//                labels: {
//                    boxWidth: 12,
//                    padding: 15
//                }
//            },
//            tooltip: {
//                mode: 'index',
//                intersect: false
//            },
//            title: {
//                display: true,
//                text: 'Overview'
//            }
//        },
//        interaction: {
//            mode: 'nearest',
//            intersect: false
//        },
//        scales: {
//            x: {
//                grid: {
//                    color: "#f3f3f3",
//                    drawTicks: false
//                },
//                title: {
//                    display: true,
//                    text: 'Year'
//                }
//            },
//            y: {
//                grid: {
//                    color: "#f3f3f3",
//                    drawTicks: false
//                },
//                title: {
//                    display: true,
//                    text: 'Total'
//                },
//                beginAtZero: true
//            }
//        }
//    };

//    // Chart Data
//    var chartData = {
//        labels: ["2020", "2021", "2022", "2023", "2024", "2025", "2026"],
//        datasets: [
//            {
//                label: "Page Views",
//                data: [0, 18, 10, 45, 17, 35, 20],
//                backgroundColor: "rgba(38,198,218,0.5)",
//                borderColor: "transparent",
//                pointBorderColor: "#26C6DA",
//                pointBackgroundColor: "#fff",
//                pointBorderWidth: 2,
//                pointHoverBorderWidth: 2,
//                pointRadius: 3,
//                fill: true,
//                tension: 0.4
//            },
//            {
//                label: "Total Revenue",
//                data: [0, 5, 28, 15, 18, 20, 10],
//                backgroundColor: "rgba(255,159,64,0.5)",
//                borderColor: "transparent",
//                pointBorderColor: "#FF9F40",
//                pointBackgroundColor: "#fff",
//                pointBorderWidth: 2,
//                pointHoverBorderWidth: 2,
//                pointRadius: 3,
//                fill: true,
//                tension: 0.4
//            },
//            {
//                label: "Total Bookings",
//                data: [0, 3, 22, 17, 28, 25, 5],
//                backgroundColor: "rgba(255,99,132,0.5)",
//                borderColor: "transparent",
//                pointBorderColor: "#FF6384",
//                pointBackgroundColor: "#fff",
//                pointBorderWidth: 2,
//                pointHoverBorderWidth: 2,
//                pointRadius: 3,
//                fill: true,
//                tension: 0.4
//            }
//        ]
//    };

//    var config = {
//        type: 'line',
//        options: chartOptions,
//        data: chartData
//    };

//    // Create the chart
//    new Chart(ctx, config);
//});


//$(window).on("load", function () {
//    const ctx = $("#sessionChart");

//    const chartOptions = {
//        responsive: true,
//        maintainAspectRatio: false,
//        elements: {
//            line: {
//                tension: 0.4 // 👈 Bo tròn đường cong
//            },
//            point: {
//                radius: 0 // 👈 Ẩn các chấm tròn
//            }
//        },
//        plugins: {
//            legend: {
//                display: false
//            },
//            tooltip: {
//                mode: 'index',
//                intersect: false
//            }
//        },
//        scales: {
//            x: {
//                grid: {
//                    color: "#f3f3f3",
//                    drawTicks: false
//                },
//                ticks: {
//                    color: "#999"
//                }
//            },
//            y: {
//                grid: {
//                    color: "#f3f3f3",
//                    drawTicks: false
//                },
//                ticks: {
//                    color: "#999"
//                }
//            }
//        }
//    };

//    const chartData = {
//        labels: ["2016-12-01", "2016-12-02", "2016-12-03", "2016-12-04", "2016-12-05", "2016-12-08"],
//        datasets: [
//            {
//                label: "Pages/Session",
//                data: [0, 150, 140, 105, 190, 230],
//                backgroundColor: "rgba(209,212,219,.4)",
//                fill: true
//            },
//            {
//                label: "Avg. Session Duration",
//                data: [0, 90, 120, 240, 140, 250],
//                backgroundColor: "rgba(81,117,224,.7)",
//                fill: true
//            }
//        ]
//    };

//    new Chart(ctx, {
//        type: "line",
//        data: chartData,
//        options: chartOptions
//    });
//});


// (function(window, document, $) {
//     'use strict';

//     $(document).ready(function() {

//         // Debug: Kiểm tra các thư viện đã load chưa
//         console.log('jQuery loaded:', typeof $ !== 'undefined');
//         console.log('jVectorMap loaded:', typeof $.fn.vectorMap !== 'undefined');
//         console.log('Available maps:', $.fn.vectorMap && $.fn.vectorMap.maps ? Object.keys($.fn.vectorMap.maps) : 'No maps found');

//         // Kiểm tra world_mill map có sẵn không
//         if (!$.fn.vectorMap || !$.fn.vectorMap.maps || !$.fn.vectorMap.maps.world_mill) {
//             console.error('world_mill map data chưa được load! Kiểm tra file jquery-jvectormap-world-mill.js');
//             return;
//         }

//         /********************************************
//         *               Vector Maps                 *
//         ********************************************/
//         try {
//             $('#world-map-markers').vectorMap({
//                 backgroundColor: '#1DE9B6',
//                 zoomOnScroll: false,
//                 map: 'world_mill',
//                 scaleColors: ['#C8EEFF', '#0071A4'],
//                 normalizeFunction: 'polynomial',
//                 hoverOpacity: 0.7,
//                 hoverColor: false,
//                 markerStyle: {
//                     initial: {
//                         fill: '#FF9E80',
//                         stroke: '#FF6E40',
//                         'stroke-width': 2
//                     },
//                     hover: {
//                         fill: '#FF6E40',
//                         stroke: '#FF6E40',
//                     },
//                     selected: {
//                         fill: '#FF3D00',
//                         stroke: '#FF6E40',
//                     },
//                     selectedHover: {
//                         fill: '#DD2C00',
//                         stroke: '#FF6E40',
//                     }
//                 },
//                 regionStyle: {
//                     initial: {
//                         fill: '#A7FFEB',
//                     },
//                     hover: {
//                         fill: '#64FFDA'
//                     },
//                     selected: {
//                         fill: '#1DE9B6'
//                     },
//                     selectedHover: {
//                         fill: '#00BFA5'
//                     }
//                 },
//                 markers: [
//                     {latLng: [51.52, -0.14], name: 'London'},
//                     {latLng: [48.87, 2.34], name: 'Paris'},
//                     {latLng: [47.36, 8.53], name: 'Switzerland'},
//                     {latLng: [40.54, -3.77], name: 'Spain'},
//                     {latLng: [39.59, -105.19], name: 'USA'},
//                     {latLng: [19.67, -99.12], name: 'Mexico'},
//                     {latLng: [-8.37, -56.06], name: 'Brazil'},
//                     {latLng: [-31.17, -64.14], name: 'Argentina'},
//                     {latLng: [-26.20, 28.02], name: 'Johanesburg'},
//                     {latLng: [19.05, 72.87], name: 'Mumbai'},
//                     {latLng: [40.03, 116.46], name: 'Beijing'},
//                     {latLng: [31.27, 121.47], name: 'Shanghai'},
//                     {latLng: [35.739, 139.75], name: 'Japan'},
//                     {latLng: [-34.83, 138.60], name: 'Adelaide'},
//                     {latLng: [-37.77, 144.97], name: 'Melbourne'},
//                     {latLng: [1.3, 103.8], name: 'Singapore'},
//                     {latLng: [26.02, 50.55], name: 'Bahrain'},
//                 ],
//                 onMarkerClick: function(event, index) {
//                     console.log('Clicked marker:', index);
//                 }
//             });

//             console.log('World map initialized successfully!');

//         } catch (error) {
//             console.error('Error initializing world map:', error);
//         }

//         /*******************************************
//         *               Total Orders               *
//         *******************************************/
//         try {
//             Morris.Line({
//                 element: 'map-total-orders',
//                 data: [{y: '1', a: 14, }, {y: '2', a: 12 }, {y: '3', a: 4 }, {y: '4', a: 13 }, {y: '5', a: 9 }, {y: '6', a: 14 }, {y: '7', a: 12 }, {y: '8', a: 20 }],
//                 xkey: 'y',
//                 ykeys: ['a'],
//                 labels: ['Likes'],
//                 axes: false,
//                 grid: false,
//                 behaveLikeLine: true,
//                 ymax: 20,
//                 resize: true,
//                 pointSize: 4,
//                 pointFillColors: ['#FFF'],
//                 pointStrokeColors: ['#FFA87D'],
//                 smooth: false,
//                 numLines: 6,
//                 lineWidth: 2,
//                 lineColors: ['#FFA87D'],
//                 hideHover: true,
//                 hoverCallback: function (index, options, content, row) {
//                     return "";
//                 }
//             });
//         } catch (error) {
//             console.error('Error initializing Total Orders chart:', error);
//         }

//         /*******************************************
//         *               Total Profit               *
//         *******************************************/
//         try {
//             Morris.Line({
//                 element: 'map-total-profit',
//                 data: [{y: '1', a: 14, }, {y: '2', a: 12 }, {y: '3', a: 4 }, {y: '4', a: 13 }, {y: '5', a: 7 }, {y: '6', a: 14 }, {y: '7', a: 8 }, {y: '8', a: 20 }],
//                 xkey: 'y',
//                 ykeys: ['a'],
//                 labels: ['Likes'],
//                 axes: false,
//                 grid: false,
//                 behaveLikeLine: true,
//                 ymax: 20,
//                 resize: true,
//                 pointSize: 4,
//                 pointFillColors: ['#FFF'],
//                 pointStrokeColors: ['#16D39A'],
//                 smooth: false,
//                 numLines: 6,
//                 lineWidth: 2,
//                 lineColors: ['#16D39A'],
//                 hideHover: true,
//                 hoverCallback: function (index, options, content, row) {
//                     return "";
//                 }
//             });
//         } catch (error) {
//             console.error('Error initializing Total Profit chart:', error);
//         }

//     }); // End document.ready

// })(window, document, jQuery);

