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

const statusMap = {
    0: 'Pending',
    1: 'InProgress',
    2: 'Completed',
    3: 'Rejected',
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
    const counts = Array(12).fill(0);
    requests.value.forEach(req => {
        const month = new Date(req.requestedAt).getMonth();
        counts[month]++;
    });
    return counts;
});


const aaaaaaaaaaa = async()=>{

  if (loginUser.value && loginUser.value.user_uid) {
    try {
      const response = await improvementRequestService.searchRequests({ customerId: loginUser.value.user_uid });

requests.value = response.data.data;

console.log('requests.value', requests.value);

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



    } catch (error) {
      console.error('Failed to fetch improvement requests:', error);
    }
  }

}

onMounted(async () => {



      const response = await improvementRequestService.searchRequests({ customerId: loginUser.value.user_uid });
 
requests.value = response.data.data;

      console.log('response.data', response.data);

//requests.value = response.data.data;

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




});

</script>






<template>







<form class="card srcharea" @submit.prevent="search">

<div class="flex flex-col sm:flex-row sm:items-center" >
                   




                    <div class="flex flex-col md:flex-row justify-between md:items-center flex-1 gap-6">
                        <div></div><div></div>
                        <div>


      <Button  size="small" label="Reload" class="ml-2 mr-2" @click="aaaaaaaaaaa" raised />

                        </div>
                    </div>
                </div>


  </form>





















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
    
    


    <div class="col-span-12 xl:col-span-4">
          
      <div class="card">
          <div class="font-semibold text-xl mb-4">상태별 비율</div>

          <Chart type="doughnut" :data="chartData" :options="chartOptions" class="w-full md:w-[30rem]"></Chart>

      </div>

    </div>



    <div class="col-span-12 xl:col-span-8">
          
      <div class="card">
          <div class="font-semibold text-xl mb-4">월별 접수 건수</div>

                <Chart type="bar" :data="monthlyChartData" :options="monthlyChartOptions"></Chart>

      </div>

    </div>









    </div>


</template>