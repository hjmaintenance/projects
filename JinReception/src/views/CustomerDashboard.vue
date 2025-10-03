<script setup>
import { ref, onMounted, computed } from 'vue';
import { useLayout } from '@/layout/composables/layout';
import improvementRequestService from '@/service/improvementRequestService';
import Chart from 'primevue/chart';

const { loginUser } = useLayout();
const requests = ref([]);
const chartData = ref({});
const chartOptions = ref({});
const monthlyChartData = ref({});
const monthlyChartOptions = ref({});

const statusCounts = computed(() => {
  const counts = {
    Pending: 0,
    InProgress: 0,
    Completed: 0,
    Rejected: 0
  };
  requests.value.forEach(req => {
    if (req.status in counts) {
      counts[req.status]++;
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
    const counts = Array(12).fill(0);
    requests.value.forEach(req => {
        const month = new Date(req.requestedAt).getMonth();
        counts[month]++;
    });
    return counts;
});

onMounted(async () => {
  if (loginUser.value && loginUser.value.id) {
    try {
      const response = await improvementRequestService.searchRequests({ customerId: loginUser.value.id });
      requests.value = response.data;

      chartData.value = {
        labels: Object.keys(statusCounts.value),
        datasets: [
          {
            data: Object.values(statusCounts.value),
            backgroundColor: ['#FF6384', '#36A2EB', '#FFCE56', '#4BC0C0'],
            hoverBackgroundColor: ['#FF6384', '#36A2EB', '#FFCE56', '#4BC0C0']
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
        labels: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
        datasets: [
          {
            label: '월별 접수 건수',
            data: monthlyCounts.value,
            backgroundColor: '#42A5F5',
            borderColor: '#1E88E5',
            borderWidth: 1
          }
        ]
      };

      monthlyChartOptions.value = {
        scales: {
          y: {
            beginAtZero: true
          }
        }
      };

    } catch (error) {
      console.error('Failed to fetch improvement requests:', error);
    }
  }
});

</script>

<template>
    <div class="grid">
        <div class="col-12 md:col-6 lg:col-3">
            <div class="card">
                <h5>접수대기</h5>
                <div class="text-xl font-bold">{{ statusCounts.Pending }} 건</div>
                <div class="text-sm">{{ statusPercentages.Pending }}%</div>
            </div>
        </div>
        <div class="col-12 md:col-6 lg:col-3">
            <div class="card">
                <h5>처리중</h5>
                <div class="text-xl font-bold">{{ statusCounts.InProgress }} 건</div>
                <div class="text-sm">{{ statusPercentages.InProgress }}%</div>
            </div>
        </div>
        <div class="col-12 md:col-6 lg:col-3">
            <div class="card">
                <h5>처리완료</h5>
                <div class="text-xl font-bold">{{ statusCounts.Completed }} 건</div>
                <div class="text-sm">{{ statusPercentages.Completed }}%</div>
            </div>
        </div>
        <div class="col-12 md:col-6 lg:col-3">
            <div class="card">
                <h5>반려</h5>
                <div class="text-xl font-bold">{{ statusCounts.Rejected }} 건</div>
                <div class="text-sm">{{ statusPercentages.Rejected }}%</div>
            </div>
        </div>

        <div class="col-12 xl:col-6">
            <div class="card flex flex-col items-center">
                <h5 class="text-left w-full">상태별 비율</h5>
                <Chart type="doughnut" :data="chartData" :options="chartOptions" class="w-full md:w-[30rem]"></Chart>
            </div>
        </div>

        <div class="col-12 xl:col-6">
            <div class="card">
                <h5>월별 접수 건수</h5>
                <Chart type="bar" :data="monthlyChartData" :options="monthlyChartOptions"></Chart>
            </div>
        </div>
    </div>
</template>