<script setup>
  import { RequestService } from '@/service/RequestService';
  import { buildQueryPayload2 } from '@/utils/apiUtils';
  import { formatDate, formatDateSmart, STATUS , STATUS_ALL} from '@/utils/formatters';
  import { onMounted, reactive, ref, watch, nextTick, computed } from 'vue';
  import { useLayout } from '@/layout/composables/layout';
  import { useRouter } from 'vue-router';

  import apiClient from '@/service/api';


import router from '@/router';

const { loginUser } = useLayout();
const loading = ref(null);

const companyStats = ref([]);

const searchConfig = computed(() => [
]);

const queryOptions = reactive({
select: 'id,title,createdAt,customer,admin,status',
remove: 'customerId,description',
sorts: [{ field: 'status', dir: 'asc' },{ field: 'createdAt', dir: 'desc' }],
page: 1,
pageSize: 10,
status_in: '0|1'
});


const requests = ref([]);

  //조회
  const search = async () => {
    const finalPayload = buildQueryPayload2(searchConfig.value, queryOptions);
    requests.value = await RequestService.search(queryOptions, loading);

    loadCompanyStats();
  };

  const loadCompanyStats = async () => {
    companyStats.value = await RequestService.getCompanyStats();
  };


  const updateRequestStatus = async (id, statusName) => {

    const statusObject = STATUS.find((s) => s.name === statusName);
    if (!statusObject) {
      console.error(`'${statusName}'에 해당하는 상태를 찾을 수 없습니다.`);
      return;
    }

    await RequestService.accept(id, statusObject.code);
    
    await search();
  };


onMounted(() => {
    search();
});

const getStatusColor = (status) => {
      switch (status) {
        case '접수대기':
          return 'blue';
        case '처리중':
          return 'orange';
        case '완료':
          return 'green';
        case '반려':
          return 'red';
        default:
          return 'grey';
      }
    };

</script>

<template>

  <!-- 조회영역 전체 -->
<form class="card flex flex-col gap-4 md:flex-row md:items-center md:justify-between" @submit.prevent="search">


  <!-- 버튼 그룹 -->
  <div class="flex gap-2 w-full md:w-auto md:ml-4">
    <!-- 버튼이 여러 개라면 flex-1 로 균등분할 -->
    <Button label="조회" class="flex-1 md:flex-none" @click="search" raised />
  </div>

  </form>

    <!-- 회사별 접수 진행사항-->
    <div class="card">
        <h5 class="text-xl font-semibold mb-4">회사별 요청 현황</h5>
        <div class="grid grid-cols-1 md:grid-cols-2 xl:grid-cols-3 gap-4">
            <div v-for="stat in companyStats" :key="stat.companyName">
                <div class="card h-full">
                    <div class="flex justify-between items-start mb-3">
                        <div>
                            <span class="block text-lg font-medium">{{ stat.companyName }}</span>
                            <span v-if="stat.lastPendingDate" class="text-sm text-muted-color">마지막 접수: {{ formatDate(stat.lastPendingDate) }}</span>
                            <span v-else class="text-sm text-muted-color">접수 대기 없음</span>
                        </div>
                        <div class="flex items-center justify-center bg-blue-100 dark:bg-blue-400/10 rounded-full p-2">
                            <i class="pi pi-building text-blue-500 !text-xl"></i>
                        </div>
                    </div>

                    <div>
                        <div class="flex justify-between items-center mb-1">
                            <span class="text-sm font-medium">완료율</span>
                            <span class="text-sm font-bold">{{ stat.completionRate }}%</span>
                        </div>
                        <div class="w-full bg-gray-200 rounded-full h-2.5 dark:bg-gray-700">
                            <div class="bg-green-500 h-2.5 rounded-full" :style="{ width: stat.completionRate + '%' }"></div>
                        </div>
                    </div>

                    <div class="mt-4 grid grid-cols-2 gap-x-4 gap-y-2">
                        <div class="flex items-center">
                            <i class="pi pi-inbox text-blue-500 mr-2"></i>
                            <span class="font-medium">접수대기:</span>
                            <span class="ml-auto font-bold">{{ stat.pendingCount }}</span>
                        </div>
                        <div class="flex items-center">
                            <i class="pi pi-spin pi-spinner text-orange-500 mr-2"></i>
                            <span class="font-medium">진행중:</span>
                            <span class="ml-auto font-bold">{{ stat.inProgressCount }}</span>
                        </div>
                        <div class="flex items-center">
                            <i class="pi pi-check-circle text-green-500 mr-2"></i>
                            <span class="font-medium">완료:</span>
                            <span class="ml-auto font-bold">{{ stat.completedCount }}</span>
                        </div>
                        <div class="flex items-center">
                            <i class="pi pi-times-circle text-red-500 mr-2"></i>
                            <span class="font-medium">반려:</span>
                            <span class="ml-auto font-bold">{{ stat.rejectedCount }}</span>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>


        <div class="card">
        <h5 class="text-xl font-semibold mb-4">요청 접수 목록Top10</h5>
            <DataView :value="requests" layout="list">

                <template #list="slotProps">
                    <div class="flex flex-col">
                        <div v-for="(item, index) in slotProps.items" :key="index">
                            <div class="flex flex-col sm:flex-row sm:items-center p-6 gap-4" :class="{ 'border-t border-surface': index !== 0 }">
                                <div class="md:w-40 relative">
                                    <img class="block xl:block mx-auto rounded w-full" :src="`https://primefaces.org/cdn/primevue/images/product/bamboo-watch.jpg`" :alt="item.title"
                                    @click="router.push(`/request_detail?id=${item.id}`)"
                                    />
                                </div>
                                <div class="flex flex-col md:flex-row justify-between md:items-center flex-1 gap-6">
                                    <div class="flex flex-row md:flex-col justify-between items-start gap-2">
                                        <div>
                                            <div class="text-lg font-medium mt-2 cursor-pointer" @click="router.push(`/request_detail?id=${item.id}`)">{{ item.title }}</div>
                                            <span class="font-medium text-surface-500 dark:text-surface-400 text-sm">{{ item.customer?.userName }} ({{item.customer?.company?.name}})</span>
                                            <span class="font-medium text-surface-500 dark:text-surface-400 text-sm ml-2">{{ formatDateSmart(item.createdAt) }}</span>
                                        </div>
                                        <div class="flex items-center">
                                            <Chip :label="STATUS.find(s=>s.code === item.status)?.ttl || '알수없음'" :class="`bg-${getStatusColor(STATUS.find(s=>s.code === item.status)?.ttl)}-100 text-${getStatusColor(STATUS.find(s=>s.code === item.status)?.ttl)}-800`"></Chip>
                                        </div>
                                    </div>
                                    <div class="flex flex-col md:items-end gap-8">
                                        <div class="flex flex-row-reverse md:flex-row gap-2">
                                            <Button icon="pi pi-check" label="접수하기" :disabled="item.status !== 0" class="flex-auto md:flex-initial whitespace-nowrap"
                                            @click="updateRequestStatus(item.id, 'IN_PROGRESS')"
                                            ></Button>
                                            <Button icon="pi pi-eye" label="보기" class="flex-auto md:flex-initial whitespace-nowrap" @click="router.push(`/request_detail?id=${item.id}`)"></Button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </template>

            </DataView>
        </div>
    
</template>