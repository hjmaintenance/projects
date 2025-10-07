<script setup>
import { ref, onMounted, computed, watch } from 'vue';
import { useLayout } from '@/layout/composables/layout';
import improvementRequestService from '@/service/improvementRequestService';
import Chart from 'primevue/chart';

const { loginUser } = useLayout();
const requests = ref([]);
const chartData = ref({});
const chartOptions = ref({});
const monthlyChartData = ref({});
const monthlyChartOptions = ref({});
const dailyChartData = ref({});
const dailyChartOptions = ref({});

const selectedMonth = ref(new Date().getMonth());

const statusMap = {
    0: 'Pending',
    1: 'InProgress',
    2: 'Rejected',
    3: 'Completed',
    4: 'Delete'
};

const statusCounts = computed(() => {
  const counts = {
    Pending: 0,
    InProgress: 0,
    Completed: 0,
    Rejected: 0,
    Delete: 0
  };
  requests.value.forEach(req => {
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
    requests.value.forEach(req => {
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

    requests.value.forEach(req => {
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
        stepSize: 3   // y축 간격을 10단위로
      }
    }
  }
};



};

watch(selectedMonth, () => {
    updateDailyChart();
});

onMounted(async () => {
  if (loginUser.value && loginUser.value.user_uid) {
    try {
      const response = await improvementRequestService.searchRequests({ customerId: loginUser.value.user_uid });
      requests.value = response.data.data;

      chartData.value = {
        labels: Object.keys(statusCounts.value),
        datasets: [
          {
            data: Object.values(statusCounts.value),
            backgroundColor: ['#FF6384', '#36A2EB', '#FFCE56', '#4BC0C0', '#6c757d'],
            hoverBackgroundColor: ['#FF6384', '#36A2EB', '#FFCE56', '#4BC0C0', '#6c757d']
          }
        ]
      };

      chartOptions.value = {
        plugins: {
          legend: {
            labels: {
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
  maintainAspectRatio: false,  // aspectRatio 유지하지 않음
  responsive: true, // 뷰포트에 따라 크기 조정

  scales: {
    y: {
      beginAtZero: true,
      ticks: {
        stepSize: 3   // y축 간격을 10단위로
      }
    }
  }
};

      updateDailyChart();
    } catch (error) {
      console.error('Failed to fetch improvement requests:', error);
    }
  }
});
</script>

<template>
    <div class="grid grid-cols-12 gap-8">
        <div class="col-span-12 lg:col-span-6 xl:col-span-3">
            <div class="card mb-0">
                <div class="flex justify-between mb-4">
                    <div>
                        <span class="block text-muted-color font-medium mb-4">접수대기</span>
                        <div class="text-surface-900 dark:text-surface-0 font-medium text-xl">{{ statusCounts.Pending }}</div>
                        <div class="text-sm">{{ statusPercentages.Pending }}%</div>
                    </div>
                    <div class="flex items-center justify-center bg-blue-100 dark:bg-blue-400/10 rounded-border" style="width: 2.5rem; height: 2.5rem">
                        <i class="pi pi-shopping-cart text-blue-500 !text-xl"></i>
                    </div>
                </div>
                <span class="text-primary font-medium">24 new </span>
                <span class="text-muted-color">since last visit</span>
            </div>
        </div>
        <div class="col-span-12 lg:col-span-6 xl:col-span-3">
            <div class="card mb-0">
                <div class="flex justify-between mb-4">
                    <div>
                        <span class="block text-muted-color font-medium mb-4">InProgress</span>
                        <div class="text-surface-900 dark:text-surface-0 font-medium text-xl">{{ statusCounts.InProgress }}</div>
                        <div class="text-sm">{{ statusPercentages.InProgress }}%</div>
                    </div>
                    <div class="flex items-center justify-center bg-orange-100 dark:bg-orange-400/10 rounded-border" style="width: 2.5rem; height: 2.5rem">
                        <i class="pi pi-dollar text-orange-500 !text-xl"></i>
                    </div>
                </div>
                <span class="text-primary font-medium">%52+ </span>
                <span class="text-muted-color">since last week</span>
            </div>
        </div>
        <div class="col-span-12 lg:col-span-6 xl:col-span-3">
            <div class="card mb-0">
                <div class="flex justify-between mb-4">
                    <div>
                        <span class="block text-muted-color font-medium mb-4">Completed</span>
                        <div class="text-surface-900 dark:text-surface-0 font-medium text-xl">{{ statusCounts.Completed }}</div>
                        <div class="text-sm">{{ statusPercentages.Completed }}%</div>
                    </div>
                    <div class="flex items-center justify-center bg-cyan-100 dark:bg-cyan-400/10 rounded-border" style="width: 2.5rem; height: 2.5rem">
                        <i class="pi pi-users text-cyan-500 !text-xl"></i>
                    </div>
                </div>
                <span class="text-primary font-medium">520 </span>
                <span class="text-muted-color">newly registered</span>
            </div>
        </div>
        <div class="col-span-12 lg:col-span-6 xl:col-span-3">
            <div class="card mb-0">
                <div class="flex justify-between mb-4">
                    <div>
                        <span class="block text-muted-color font-medium mb-4">Rejected</span>
                        <div class="text-surface-900 dark:text-surface-0 font-medium text-xl">{{ statusCounts.Rejected }}</div>
                        <div class="text-sm">{{ statusPercentages.Rejected }}%</div>
                    </div>
                    <div class="flex items-center justify-center bg-purple-100 dark:bg-purple-400/10 rounded-border" style="width: 2.5rem; height: 2.5rem">
                        <i class="pi pi-comment text-purple-500 !text-xl"></i>
                    </div>
                </div>
                <span class="text-primary font-medium">85 </span>
                <span class="text-muted-color">responded</span>
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
                <div class="font-semibold text-xl mb-4">상태별 비율</div>
                <Chart type="doughnut" :data="chartData" :options="chartOptions" class="w-full md:w-[30rem]"></Chart>
            </div>
        </div>

        <div class="col-span-12 xl:col-span-8">
            <div class="card">
                <div class="font-semibold text-xl mb-4">월별 접수 건수</div>
                <Chart type="bar" :data="monthlyChartData" :options="monthlyChartOptions" @select="onMonthSelect" class="w-full h-full"></Chart>
            </div>
        </div>

    </div>
</template>