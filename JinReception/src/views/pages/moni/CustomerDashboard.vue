<script setup>
import { ref, onMounted, computed, watch,nextTick } from 'vue';
import { useLayout } from '@/layout/composables/layout';
import { useRouter } from 'vue-router';
import Chart from 'primevue/chart';
import { RequestService } from '@/service/RequestService';
import { useRequestStore } from '@/store/requestStore';
import { STATUS_ALL } from '@/utils/formatters';

const { loginUser } = useLayout();
const requests = ref([]);
const chartData = ref({});
const loading = ref(false);
const chartOptions = ref({});
const monthlyChartData = ref({});
const monthlyChartOptions = ref({});
const dailyChartData = ref({});
const dailyChartOptions = ref({});
let documentStyle;
const router = useRouter();
const store = useRequestStore();
let textColor;
let textColorSecondary;
let surfaceBorder;

const companyStats = ref(null);

const loadCompanyStats = async () => {
    try {
        companyStats.value = await RequestService.getMyCompanyStats();
    } catch (error) {
        console.error('Failed to fetch company stats:', error);
    }
};

const companyChartData = computed(() => {
    if (!companyStats.value) {
        return {};
    }
    const stats = companyStats.value;
    return {
        labels: ['접수대기', '진행중', '완료', '반려'],
        datasets: [
            {
                data: [stats.pendingCount, stats.inProgressCount, stats.completedCount, stats.rejectedCount],
                backgroundColor: ['#FF6384', '#36A2EB', '#FFCE56', '#4BC0C0'],
                hoverBackgroundColor: ['#FF6384', '#36A2EB', '#FFCE56', '#4BC0C0']
            }
        ]
    };
});

const selectedMonth = ref(new Date().getMonth());

const statusMap = Object.freeze({
    0: 'Pending',
    1: 'InProgress',
    2: 'Rejected',
    3: 'Completed',
    4: 'Delete'
});

const statusCounts = computed(() => {
  const counts = {
    Pending: 0,
    InProgress: 0,
    Completed: 0,
    Rejected: 0,
    Delete: 0
  };
  requests.value?.forEach(req => {
    const statusString = statusMap[req.status];
    if (statusString) {
      counts[statusString]++;
    }
  });
  return counts;
});

const totalRequests = computed(() => requests.value.length);

const statusPercentages = computed(() => {
  const percentages = {};
  for (const status in statusCounts.value) {
    percentages[status] = totalRequests.value > 0 ? ((statusCounts.value[status] / totalRequests.value) * 100).toFixed(2) : 0;
  }
  return percentages;
});

const monthlyCounts = computed(() => {
    const total = Array(12).fill(0);
    const completed = Array(12).fill(0);
    requests.value?.forEach(req => {
        const month = new Date(req.requestedAt).getMonth();
        total[month]++;
        if (req.status === 3) { // 3: Completed
            completed[month]++;
        }
    });
    return { total, completed };
});

const dailyCounts = computed(() => {
    const today = new Date();
    const year = today.getFullYear();
    const month = selectedMonth.value;
    const daysInMonth = new Date(year, month + 1, 0).getDate();
    
    const total = Array(daysInMonth).fill(0);
    const inProgress = Array(daysInMonth).fill(0);
    const completed = Array(daysInMonth).fill(0);
    const rejected = Array(daysInMonth).fill(0);

    requests.value?.forEach(req => {
        const reqDate = new Date(req.requestedAt);
        if (reqDate.getFullYear() === year && reqDate.getMonth() === month) {
            const day = reqDate.getDate() - 1;
            total[day]++;
            if (req.status === 1) { // InProgress
                inProgress[day]++;
            } else if (req.status === 3) { // Completed
                completed[day]++;
            } else if (req.status === 2) { // Rejected
                rejected[day]++;
            }
        }
    });
    return { total, inProgress, completed, rejected };
});

const dailyChartTitle = computed(() => {
    const monthNames = ["1월", "2월", "3월", "4월", "5월", "6월", "7월", "8월", "9월", "10월", "11월", "12월"];
    return `일별 접수 건수 (${monthNames[selectedMonth.value]})`;
});

const monthlyChartTitle = computed(() => {
    const monthNames = ["1월", "2월", "3월", "4월", "5월", "6월", "7월", "8월", "9월", "10월", "11월", "12월"];
    return `월별 접수 건수 (${monthNames[selectedMonth.value]} 선택됨)`;
});



// Chart.js plugin to display text in the center of the doughnut chart
const DoughnutCenterText = {
  id: 'doughnutCenterText',
  beforeDraw: (chart, args, options) => {
    const { ctx, data } = chart;

    if( data.datasets.length <= 0) return;

    // Calculate the total number of requests
    const total = data.datasets[0].data.reduce((a, b) => a + b, 0);

    // Calculate the completed percentage
    const completed = data.datasets[0].data[2]; // Assuming '완료' is the third status
    const percentage = total > 0 ? ((completed / total) * 100).toFixed(0) : 0;

    // Ensure percentage is not NaN
    const text = `${percentage}%`;

    const style = getComputedStyle(document.documentElement);
    // 완료율에 따라 텍스트 색상 변경
    if (percentage >= 50) {
      ctx.fillStyle = '#26C6DA';
    } else {
      ctx.fillStyle = '#FFA726';
    }

    ctx.save();

    const x = chart.getDatasetMeta(0).data[0].x;
    const y = chart.getDatasetMeta(0).data[0].y;
    ctx.textAlign = 'center';
    ctx.textBaseline = 'middle';
    // 폰트 스타일 명시적으로 설정
    ctx.font = 'bold 3rem Arial';

    ctx.fillText(text, x, y);

    ctx.restore();
  }
};

const goToListByStatus = (statusName) => {
  // 상태 이름으로 상태 객체 찾기
  const status = STATUS_ALL.find(s => s.name === statusName);
  if (status) {
    store.dropdownItem = status;
  }

  // 다른 필터 초기화
  store.adminItem = null;
  store.companyItem = null;
  store.Srch = '';

  router.push('/mng_request');
};





const onMonthSelect = (event) => {
    if (event.element) {
        selectedMonth.value = event.element.index;
    }
};

const updateDailyChart = () => {
    const year = new Date().getFullYear();
    const month = selectedMonth.value;
    const daysInMonth = new Date(year, month + 1, 0).getDate();

    dailyChartData.value = {
        labels: Array.from({ length: daysInMonth }, (_, i) => i + 1),
        datasets: [
            {
                label: '접수',
                data: dailyCounts.value.total,
                backgroundColor: '#42A5F5', // Blue
                borderColor: '#1E88E5',
                borderWidth: 1
            },
            {
                label: '진행',
                data: dailyCounts.value.inProgress,
                backgroundColor: '#FFA726', // Orange
                borderColor: '#FB8C00',
                borderWidth: 1
            },
            {
                label: '완료',
                data: dailyCounts.value.completed,
                backgroundColor: '#26C6DA', // Cyan
                borderColor: '#00ACC1',
                borderWidth: 1
            },
            {
                label: '반려',
                data: dailyCounts.value.rejected,
                backgroundColor: '#AB47BC', // Purple
                borderColor: '#8E24AA',
                borderWidth: 1
            }
        ]
    };

    
      dailyChartOptions.value = {
  maintainAspectRatio: false,  // aspectRatio 유지하지 않음
  responsive: true, // 뷰포트에 따라 크기 조정

  scales: {
    y: {
        beginAtZero: true,
        ticks: {
            color: textColorSecondary,
            stepSize: 3
        },
        grid: {
            color: surfaceBorder
        }
    },
    x: {
        ticks: {
            color: textColorSecondary
        },
        grid: {
            color: surfaceBorder
        }
    }
}
};



};

watch(selectedMonth, () => {
    updateDailyChart();
});

const loadDashboardData = async () => {
    loading.value = true;
    if (loginUser.value && loginUser.value.user_uid) {
        try {
            const response = await RequestService.search({ customerId: loginUser.value.user_uid });
            requests.value = response;//.data.data;
            const currentStatusCounts = statusCounts.value;

            chartData.value = {
                labels: ['접수대기', '진행중', '완료', '반려'],
                datasets: [
                    {
                        data: [currentStatusCounts.Pending, currentStatusCounts.InProgress, currentStatusCounts.Completed, currentStatusCounts.Rejected],
                       
            backgroundColor: ['#FF6384', '#36A2EB', '#FFCE56', '#4BC0C0', '#6c757d'],
            hoverBackgroundColor: ['#FF6384', '#36A2EB', '#FFCE56', '#4BC0C0', '#6c757d']
                    }
                ]
            };

            chartOptions.value = {
                plugins: {
                    legend: {
                        labels: { 
                            color: textColor,
                            usePointStyle: true
                        }
                    }
                }
            };

            monthlyChartData.value = {
                labels: ['1월', '2월', '3월', '4월', '5월', '6월', '7월', '8월', '9월', '10월', '11월', '12월'],
                
        datasets: [
          {
            label: '접수건수',
            data: monthlyCounts.value.total,
            backgroundColor: '#42A5F5',
            borderColor: '#1E88E5',
            borderWidth: 1
          },
          {
            label: '완료건수',
            data: monthlyCounts.value.completed,
            backgroundColor: '#66BB6A',
            borderColor: '#4CAF50',
            borderWidth: 1
          }
        ]
            };

            monthlyChartOptions.value = {
                maintainAspectRatio: false,
                responsive: true,
                scales: {
                    y: {
                        beginAtZero: true,
                        ticks: {
                            color: textColorSecondary,
                            stepSize: 3
                        },
                        grid: {
                            color: surfaceBorder
                        }
                    },
                    x: {
                        ticks: {
                            color: textColorSecondary
                        },
                        grid: {
                            color: surfaceBorder
                        }
                    }
                },
                plugins: {
                    legend: { labels: { color: textColor } }
                },
            }
            updateDailyChart();
        } catch (error) {
            console.error('Failed to fetch improvement requests:', error);
        } finally {
            loading.value = false;
        }
    } else {
        loading.value = false;
    }
};

onMounted(async () => {
    documentStyle = getComputedStyle(document.documentElement);
    textColor = documentStyle.getPropertyValue('--text-color');
    textColorSecondary = documentStyle.getPropertyValue('--text-color-secondary');
    await loadDashboardData();
    await loadCompanyStats();
});

const reloadData = async () => {
    await loadDashboardData();
    await loadCompanyStats();
};
</script>

<template>
    <div class="card  hidden md:block ">
        <Button label="데이터 새로고침" icon="pi pi-refresh" :loading="loading" @click="reloadData" raised />
    </div>
    <div class="grid grid-cols-12 gap-8">
        <div class="col-span-12 lg:col-span-6 xl:col-span-3 cursor-pointer" @click="goToListByStatus('PENDING')">
            <div class="card mb-0 border-l-4 border-blue-500 hover:bg-surface-100 dark:hover:bg-surface-700/80">
                <div class="flex items-start justify-between mb-4">
                    <div>
                        <div class="text-muted-color font-medium mb-2">접수대기</div>
                        <div class="text-2xl font-bold text-surface-900 dark:text-surface-0">{{ statusCounts.Pending }}건</div>
                    </div>
                    <div class="flex items-center justify-center bg-blue-100 dark:bg-blue-400/10 rounded-border" style="width: 2.5rem; height: 2.5rem">
                        <i class="pi pi-inbox text-blue-500 !text-xl"></i>
                    </div>
                </div>
                <ProgressBar :value="statusPercentages.Pending" :showValue="false" style="height: 6px"></ProgressBar>
            </div>
        </div>
        <div class="col-span-12 lg:col-span-6 xl:col-span-3 cursor-pointer" @click="goToListByStatus('IN_PROGRESS')">
            <div class="card mb-0 border-l-4 border-orange-500 hover:bg-surface-100 dark:hover:bg-surface-700/80">
                <div class="flex items-start justify-between mb-4">
                    <div>
                        <div class="text-muted-color font-medium mb-2">진행중</div>
                        <div class="text-2xl font-bold text-surface-900 dark:text-surface-0">{{ statusCounts.InProgress }}건</div>
                    </div>
                    <div class="flex items-center justify-center bg-orange-100 dark:bg-orange-400/10 rounded-border" style="width: 2.5rem; height: 2.5rem">
                        <i class="pi pi-spin pi-spinner text-orange-500 !text-xl"></i>
                    </div>
                </div>
                <ProgressBar :value="statusPercentages.InProgress" :showValue="false" style="height: 6px" class="text-orange-500"></ProgressBar>
            </div>
        </div>
        <div class="col-span-12 lg:col-span-6 xl:col-span-3 cursor-pointer" @click="goToListByStatus('COMPLETED')">
            <div class="card mb-0 border-l-4 border-green-500 hover:bg-surface-100 dark:hover:bg-surface-700/80">
                <div class="flex items-start justify-between mb-4">
                    <div>
                        <div class="text-muted-color font-medium mb-2">완료</div>
                        <div class="text-2xl font-bold text-surface-900 dark:text-surface-0">{{ statusCounts.Completed }}건</div>
                    </div>
                    <div class="flex items-center justify-center bg-green-100 dark:bg-green-400/10 rounded-border" style="width: 2.5rem; height: 2.5rem">
                        <i class="pi pi-check-circle text-green-500 !text-xl"></i>
                    </div>
                </div>
                <ProgressBar :value="statusPercentages.Completed" :showValue="false" style="height: 6px"></ProgressBar>
            </div>
        </div>
        <div class="col-span-12 lg:col-span-6 xl:col-span-3 cursor-pointer" @click="goToListByStatus('REJECTED')">
            <div class="card mb-0 border-l-4 border-red-500 hover:bg-surface-100 dark:hover:bg-surface-700/80">
                <div class="flex items-start justify-between mb-4">
                    <div>
                        <div class="text-muted-color font-medium mb-2">반려</div>
                        <div class="text-2xl font-bold text-surface-900 dark:text-surface-0">{{ statusCounts.Rejected }}건</div>
                    </div>
                    <div class="flex items-center justify-center bg-red-100 dark:bg-red-400/10 rounded-border" style="width: 2.5rem; height: 2.5rem">
                        <i class="pi pi-times-circle text-red-500 !text-xl"></i>
                    </div>
                </div>
                <ProgressBar :value="statusPercentages.Rejected" :showValue="false" style="height: 6px"></ProgressBar>
            </div>
        </div>



        <div class="col-span-12 xl:col-span-12" >
            <div class="card ">
                <div class="font-semibold text-xl mb-4">{{ dailyChartTitle }}</div>
                <Chart type="bar" :data="dailyChartData" :options="dailyChartOptions" ></Chart>
            </div>
        </div>

        <div class="col-span-12 xl:col-span-4">
            <div class="card">
                <div class="font-semibold text-xl mb-4">나의 접수 상태별 비율</div>
                <Chart type="doughnut" :data="chartData" :options="chartOptions"  :plugins="[DoughnutCenterText]" class="w-full"></Chart>
            </div>
        </div>



        <div class="col-span-12 xl:col-span-4">
            <div class="card">
                <div class="font-semibold text-xl mb-4">나의 소속 회사의 접수 상태별 비율</div>
                <Chart type="doughnut" :data="companyChartData" :options="chartOptions" :plugins="[DoughnutCenterText]" class="w-full"></Chart>
            </div>
        </div>



        <div class="col-span-12 xl:col-span-8">
            <div class="card">
                <div class="font-semibold text-xl mb-4">{{ monthlyChartTitle }}</div>
                <Chart type="bar" :data="monthlyChartData" :options="monthlyChartOptions" @select="onMonthSelect" class="w-full h-full"></Chart>
            </div>
        </div>

    </div>
    
</template>