<script setup>
  import { RequestService } from '@/service/RequestService';
  import { buildQueryPayload2 } from '@/utils/apiUtils';
  import { formatDate, formatDateSmart, STATUS , STATUS_ALL} from '@/utils/formatters';
  import { onMounted, reactive, ref, watch, nextTick, computed } from 'vue';
  import { useLayout } from '@/layout/composables/layout';  
  import { useRouter } from 'vue-router';
  import { useRequestStore } from '@/store/requestStore';

const { loginUser } = useLayout();
const router = useRouter();
const store = useRequestStore();
const loading = ref(null);

const companyStats = ref([]);
const adminStats = ref(null);
const allAdminStats = ref([]);

const loadAdminStats = async () => {
    adminStats.value = await RequestService.getAdminStats();
};

const loadAllAdminStats = async () => {
    allAdminStats.value = await RequestService.getAllAdminStats();
};

const adminStatsComputed = computed(() => {
    if (!adminStats.value) {
        return {
            inProgressCount: 0,
            completedCount: 0,
            rejectedCount: 0,
            acceptanceRate: '0.0',
            completionRate: '0.0',
            myTotalHandled: 0,
            totalRequests: 0
        };
    }

    const myTotalHandled = adminStats.value.inProgressCount + adminStats.value.completedCount + adminStats.value.rejectedCount;
    const acceptanceRate = adminStats.value.totalRequests > 0 ? ((myTotalHandled / adminStats.value.totalRequests) * 100).toFixed(1) : '0.0';
    const completionRate = myTotalHandled > 0 ? ((adminStats.value.completedCount / myTotalHandled) * 100).toFixed(1) : '0.0';

    return {
        ...adminStats.value,
        myTotalHandled,
        acceptanceRate,
        completionRate
    };
});


const searchConfig = computed(() => [
]);

const queryOptions = reactive({
select: 'id,title,createdAt,customer,admin,status',
remove: 'customerId,description',
sorts: [{ field: 'status', dir: 'asc' },{ field: 'createdAt', dir: 'desc' }],
page: 1,
pageSize: 5,
status_in: '0|1'
});


const requests = ref([]);

  //조회
  const search = async () => {
    const finalPayload = buildQueryPayload2(searchConfig.value, queryOptions);
    requests.value = await RequestService.search(queryOptions, loading);

    loadCompanyStats();
    loadAdminStats();
    loadAllAdminStats();
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

const goToListByStatus = (statusName, admin = null) => {
  // 상태 이름으로 상태 객체 찾기
  const status = STATUS_ALL.find(s => s.name === statusName);
  if (status) {
    store.dropdownItem = status;
  }

  if (admin) {
    store.adminItem = { id: admin.adminId, userName: admin.adminName };
  } else {
    store.adminItem = { id: loginUser.value.user_uid, userName: loginUser.value.user_name };
  }
  // 목록 페이지로 이동
  router.push('/mng_request');
}
</script>

<template>

  <!-- 조회영역 전체 -->
<form class="card hidden md:block flex flex-col gap-4 md:flex-row md:items-center md:justify-between" @submit.prevent="search">


  <!-- 버튼 그룹 -->
  <div class="flex gap-2 w-full md:w-auto md:ml-4">
    <!-- 버튼이 여러 개라면 flex-1 로 균등분할 -->
    <Button label="조회" class="flex-1 md:flex-none" @click="search" raised />
  </div>

  </form>



    <!-- 나의 접수 진행사항-->
    <div class="card">
        <h5 class="text-xl font-semibold mb-4">나의 요청 처리 현황</h5>
        <div v-if="adminStatsComputed" class="grid grid-cols-1 lg:grid-cols-3 gap-4">
            <!-- First Column: Stats Cards - flex-row on mobile, flex-col on lg screens -->
            <div class="lg:col-span-1 flex flex-row lg:flex-col gap-4">
                <Card class="flex-1 text-center cursor-pointer hover:bg-surface-100 dark:hover:bg-surface-700/80" @click="goToListByStatus('IN_PROGRESS')">
                    <template #title>
                        <div class="flex items-center justify-between">
                            <span>진행중</span>
                            <i class="pi pi-spin pi-spinner text-orange-500"></i>
                        </div>
                    </template>
                    <template #content>
                        <p class="text-2xl font-bold">{{ adminStatsComputed.inProgressCount }}건</p>
                    </template>
                </Card>
                <Card class="flex-1 text-center cursor-pointer hover:bg-surface-100 dark:hover:bg-surface-700/80" @click="goToListByStatus('COMPLETED')">
                    <template #title>
                        <div class="flex items-center justify-between">
                            <span>완료</span>
                            <i class="pi pi-check-circle text-green-500"></i>
                        </div>
                    </template>
                    <template #content>
                        <p class="text-2xl font-bold">{{ adminStatsComputed.completedCount }}건</p>
                    </template>
                </Card>
                <Card class="flex-1 text-center cursor-pointer hover:bg-surface-100 dark:hover:bg-surface-700/80" @click="goToListByStatus('REJECTED')">
                    <template #title>
                        <div class="flex items-center justify-between">
                            <span>반려</span>
                            <i class="pi pi-times-circle text-red-500"></i>
                        </div>
                    </template>
                    <template #content>
                        <p class="text-2xl font-bold">{{ adminStatsComputed.rejectedCount }}건</p>
                    </template>
                </Card>
            </div>

            <!-- Second and Third Column: Charts -->
            <div class="lg:col-span-2 grid grid-cols-2 gap-4">
                <Card class="flex flex-col items-center justify-center">
                    <template #title>
                        <div class="text-center">나의 접수율</div>
                    </template>
                    <template #content>
                        <div class="relative w-48 h-48 mx-auto">
                            <Knob v-model="adminStatsComputed.acceptanceRate" readonly :strokeWidth="10" :size="180" valueTemplate="{value}%" />
                        </div>
                        <div class="text-center mt-2 text-sm text-muted-color">
                            전체 {{ adminStatsComputed.totalRequests }}건 중 {{ adminStatsComputed.myTotalHandled }}건 처리
                        </div>
                    </template>
                </Card>
                <Card class="flex flex-col items-center justify-center">
                    <template #title>
                        <div class="text-center">나의 완료율</div>
                    </template>
                    <template #content>
                        <div class="relative w-48 h-48 mx-auto">
                            <Knob v-model="adminStatsComputed.completionRate" readonly :strokeWidth="10" :size="180" valueTemplate="{value}%" class="completion-knob" />
                        </div>
                        <div class="text-center mt-2 text-sm text-muted-color">
                            처리한 {{ adminStatsComputed.myTotalHandled }}건 중 {{ adminStatsComputed.completedCount }}건 완료
                        </div>
                    </template>
                </Card>
            </div>
        </div>
    </div>

    <!-- 모든 관리자 처리 현황 -->
    <div class="card">
        <h5 class="text-xl font-semibold mb-4">모든 관리자 처리 현황</h5>
        <DataView :value="allAdminStats" layout="grid" :paginator="true" :rows="6">
            <template #grid="slotProps">
                <div class="grid grid-cols-12 gap-4">
                    <div v-for="(item, index) in slotProps.items" :key="index" class="col-span-12 md:col-span-6 lg:col-span-4">
                        <div class="card h-full">
                            <div class="flex items-center mb-4">
                                <Avatar v-if=" item.adminPhoto " :image="item.adminPhoto" class="mr-3" size="large" shape="circle" />
                                <Avatar v-else :label="item.adminName.charAt(0).toUpperCase()"  class="mr-3" size="large" shape="circle" />
                                <div>
                                    <div class="font-bold">{{ item.adminName }}</div>
                                    <div class="text-sm text-muted-color">총 처리: {{ item.totalHandled }}건</div>
                                </div>
                            </div>

                            <div class="grid grid-cols-3 gap-2 text-center mb-4">
                                <div class="cursor-pointer p-2 rounded-md hover:bg-surface-100 dark:hover:bg-surface-700/80" @click="goToListByStatus('IN_PROGRESS', item)">
                                    <div class="text-orange-500 font-semibold">{{ item.inProgressCount }}</div>
                                    <div class="text-xs">진행중</div>
                                </div>
                                <div class="cursor-pointer p-2 rounded-md hover:bg-surface-100 dark:hover:bg-surface-700/80" @click="goToListByStatus('COMPLETED', item)">
                                    <div class="text-green-500 font-semibold">{{ item.completedCount }}</div>
                                    <div class="text-xs">완료</div>
                                </div>
                                <div class="cursor-pointer p-2 rounded-md hover:bg-surface-100 dark:hover:bg-surface-700/80" @click="goToListByStatus('REJECTED', item)">
                                    <div class="text-red-500 font-semibold">{{ item.rejectedCount }}</div>
                                    <div class="text-xs">반려</div>
                                </div>
                            </div>

                            <div>
                                <div class="mb-2">
                                    <div class="flex justify-between mb-1">
                                        <span class="text-sm">접수율</span>
                                        <span class="text-sm font-bold">{{ item.acceptanceRate }}%</span>
                                    </div>
                                    <ProgressBar :value="item.acceptanceRate" :showValue="false" style="height: 6px"></ProgressBar>
                                </div>
                                <div>
                                    <div class="flex justify-between mb-1">
                                        <span class="text-sm">완료율</span>
                                        <span class="text-sm font-bold">{{ item.completionRate }}%</span>
                                    </div>
                                    <ProgressBar :value="item.completionRate" :showValue="false" style="height: 6px" class="custom-progress-bar"></ProgressBar>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </template>
        </DataView>
    </div>







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
        <h5 class="text-xl font-semibold mb-4">요청 접수 목록Top5</h5>
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

<style scoped>
:deep(.completion-knob .p-knob-value) {
    stroke: theme('colors.green.500');
}
.custom-progress-bar :deep(.p-progressbar-value) {
    background-color: theme('colors.green.500');
}
</style>