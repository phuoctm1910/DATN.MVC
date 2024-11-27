
Chart.defaults.font.family = '-apple-system, system-ui, BlinkMacSystemFont, "Segoe UI", Roboto, "Helvetica Neue", Arial, sans-serif';
Chart.defaults.color = '#292b2c';

var ctx = document.getElementById("myAreaChart").getContext('2d');
var myLineChart = new Chart(ctx, {
    type: 'line',
    data: {
        labels: [
            "2023-03-01", "2023-03-02", "2023-03-03", "2023-03-04",
            "2023-03-05", "2023-03-06", "2023-03-07", "2023-03-08",
            "2023-03-09", "2023-03-10", "2023-03-11", "2023-03-12",
            "2023-03-13"
        ],
        datasets: [{
            label: "Sessions",
            tension: 0.3,
            backgroundColor: "rgba(2,117,216,0.2)",
            borderColor: "rgba(2,117,216,1)",
            pointRadius: 5,
            pointBackgroundColor: "rgba(2,117,216,1)",
            pointBorderColor: "rgba(255,255,255,0.8)",
            pointHoverRadius: 5,
            pointHoverBackgroundColor: "rgba(2,117,216,1)",
            pointHitRadius: 50,
            pointBorderWidth: 2,
            data: [10000, 30162, 26263, 18394, 18287, 28682, 31274, 33259, 25849, 24159, 32651, 31984, 38451],
            fill: true
        }],
    },
    options: {
        responsive: true,
        scales: {
            x: {
                type: 'time',
                time: {
                    unit: 'day'
                },
                grid: {
                    display: false
                },
                ticks: {
                    maxTicksLimit: 7
                }
            },
            y: {
                beginAtZero: true,
                max: 40000,
                ticks: {
                    maxTicksLimit: 5
                },
                grid: {
                    color: "rgba(0, 0, 0, .125)"
                }
            },
        },
        plugins: {
            legend: {
                display: false
            }
        }
    }
});
