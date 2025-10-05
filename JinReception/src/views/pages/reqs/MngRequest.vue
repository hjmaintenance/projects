<script setup>
  import { RequestService } from '@/service/RequestService';
  import { buildQueryPayload2 } from '@/utils/apiUtils';
  import { formatDate, STATUS , STATUS_ALL} from '@/utils/formatters';
  import { onMounted, reactive, ref, watch, nextTick, computed } from 'vue';
  import { useLayout } from '@/layout/composables/layout';
  import { useRouter } from 'vue-router';

  import DataTable from 'primevue/datatable';
  import Column from 'primevue/column';

  const router = useRouter();


  const { loginUser } = useLayout();
  const loading = ref(null);



  const searchs = reactive({
    Srch: '',
    customerId: loginUser.value?.user_uid,
    dropdownItem: ref(null)
  });


// const dropdownItem = ref(null);

  // 필드 구성을 computed 속성으로 변경하여 항상 최신 값을 유지합니다.
  const searchConfig = computed(() => [
    { model: searchs.Srch, fields: ['title'], operator: 'like' },
    ...(loginUser.value?.login_type !== 'admin'
      ? [{ model: searchs.customerId, fields: ['customerId'] }]
      : []),
    //{ model: 'Srch', fields: ['id'], operator: '' },
    { model: searchs.dropdownItem?.code, fields: ['status'], operator: '' }
  ]);

  const queryOptions = reactive({
    select: 'id,title,createdAt,customer,admin,status',
    remove: 'customerId,description',
    sorts: [{ field: 'id', dir: 'desc' }],
    page: 1,
    pageSize: 100
  });

  const visible = ref(false);

  const requests = ref([]);
  const selectedRequest = ref(null);

  //조회
  const search = async () => {
    // 검색 조건과 페이징/정렬 옵션을 합쳐 최종 페이로드를 생성합니다.
    const finalPayload = buildQueryPayload2(searchConfig.value, queryOptions);

    //var aaa={"title_like":searchs.Srch,"status":searchs.dropdownItem?.code,remove: 'customerId,description'};

    requests.value = await RequestService.search(finalPayload, loading);
    //requests.value = await RequestService.search(aaa, loading);
  };

  watch(selectedRequest, async (newValue, oldValue) => {
    if (newValue) {
      router.push(`/request_detail?id=${newValue.id}`);
      //visible.value = true;
      //getDetail();
    }
  });

  // loginUser.value.user_uid 값이 변경되거나 초기값이 있을 때 search()를 호출합니다.
  watch(
    () => loginUser.value?.user_uid,
    (newUid) => {
      if (newUid) search();
    },
    { immediate: true }
  );



  //
  const getDetail = async () => {
      const fullRequestData = await RequestService.get(selectedRequest.value.id);
      selectedRequest.value.description = fullRequestData.description;
  };





  //접수 담당자 지정
  const accept_request = async () => {
    if (!selectedRequest.value) return;
        visible.value = false;
await nextTick();

    // 매직 넘버 대신 명확한 상수를 사용합니다.
    selectedRequest.value.status = STATUS.IN_PROGRESS;
    selectedRequest.value.adminId = loginUser.value?.user_uid;
    await RequestService.accept(selectedRequest.value);
    
    actionRun();
    
    //search(); // 목록 새로고침
  };

  //
  const actionRun = async () => {

    const updatedItem = await RequestService.get(selectedRequest.value.id, {
      remove: queryOptions.remove
    });
    const index = requests.value.findIndex((item) => item.id === updatedItem.id);
    if (index !== -1) requests.value[index] = updatedItem;



  };



  //반려
  const reject_request = async () => {
    if (!selectedRequest.value) return;
        visible.value = false;
await nextTick();

    selectedRequest.value.status = STATUS.REJECTED; // '대기' 상태로 변경 (또는 별도의 '반려' 상태가 있다면 해당 값 사용)
    selectedRequest.value.adminId = loginUser.value?.user_uid;
    await RequestService.accept(selectedRequest.value); // 'accept'는 PUT 요청이므로 재사용 가능

    actionRun();
  };

  //완료
  const complete_request = async () => {
    if (!selectedRequest.value) return;
        visible.value = false;
    await nextTick();

    selectedRequest.value.status = STATUS.COMPLETED; // '완료' 상태로 변경
    selectedRequest.value.adminId = loginUser.value?.user_uid;
    await RequestService.accept(selectedRequest.value);

    actionRun();
  };



</script>

<template>


  <!-- 조회영역 전체 -->
<form class="card flex flex-col gap-4 md:flex-row md:items-center md:justify-between" @submit.prevent="search">

    <!-- label + input 묶음 -->
    <div class="flex flex-col md:flex-row md:items-center gap-4 flex-1">
      <!-- 첫 번째 -->
      <div class="flex flex-col md:flex-row md:items-center gap-2 flex-1">
        <label for="name3" class="w-24 md:text-right shrink-0">검색</label>
        <InputText id="name3" type="text" v-model="searchs.Srch" placeholder="Search..." />
        
        <label for="state" class="w-24 md:text-right shrink-0">상태</label>
        <Select id="state" v-model="searchs.dropdownItem" :options="STATUS_ALL" optionLabel="ttl" placeholder="Select One" class=""></Select>
      </div>
    </div>


  <!-- 버튼 그룹 -->
  <div class="flex gap-2 w-full md:w-auto md:ml-4">
    <!-- 버튼이 여러 개라면 flex-1 로 균등분할 -->
    <Button label="조회" class="flex-1 md:flex-none" @click="search" raised />
  </div>

  </form>













  <div class="card">
   
<!-- 데스크탑 전용 -->
<div class="hidden md:block">
  <DataTable
    :value="requests"
    dataKey="id"
    selectionMode="single"
    paginator
    :rows="10"
    :rowsPerPageOptions="[10, 20, 50]"
    :loading="loading"
    v-model:selection="selectedRequest"
  >
    <template #empty> not found </template>

    <Column header="ID" field="id"></Column>
    <Column header="제목" field="title"></Column>

    <Column
      header="작성일"
      filterField="createdAt"
      dataType="date"
      style="min-width: 8rem"
    >
      <template #body="{ data }">
        {{ data.createdAt ? formatDate(new Date(data.createdAt)) : '' }}
      </template>
    </Column>

    <Column header="상태" field="statusName"></Column>

    <Column
      header="작성자"
      field="customer.userName"
      style="min-width: 1rem"
    ></Column>

    <Column
      header="접수자"
      field="admin.userName"
      style="min-width: 1rem"
    ></Column>

    <Column header="" style="width: 3rem">
      <template #body="{ data }">
        <i v-if="data.attachmentCount > 0" class="pi pi-file"></i>
      </template>
    </Column>
  </DataTable>
</div>

<!-- 모바일 전용 -->
<div class="block md:hidden space-y-2">
  <template v-if="requests.length > 0">
    <div
      v-for="req in requests"
      :key="req.id"
      @click="selectedRequest = req"
    >



<div class="flex justify-between items-start">
  <!-- 왼쪽: 제목 -->
  <div class="flex-1 min-w-0">
    <span class="font-medium ">
        <span class="font-medium">{{ req.title }}</span>
    </span>
  </div>

  <!-- 오른쪽: 작성일 / 상태 -->
  <div class="flex flex-col text-right text-sm flex-shrink-0 w-30">
        <span>{{ req.createdAt ? formatDate(new Date(req.createdAt)) : '' }}</span>
        <span>{{ req.statusName }}</span>
  </div>
</div>



    <Divider />

    </div>
  </template>
  <div v-else class="text-center py-4">
    not found
  </div>
</div>

  </div>
</template>
