<script setup>
  import { RequestService } from '@/service/RequestService';
  import { AdminService } from '@/service/AdminService';
  import { CompanyService } from '@/service/CompanyService';
  import { buildQueryPayload2 } from '@/utils/apiUtils';
  import { formatDate, STATUS , STATUS_ALL} from '@/utils/formatters';
  import { onMounted, reactive, ref, watch, nextTick, computed } from 'vue';
  import { useLayout } from '@/layout/composables/layout';
  import { useRouter } from 'vue-router';
  

  import { useRequestStore } from '@/store/requestStore';




  import DataTable from 'primevue/datatable';
  import Column from 'primevue/column';

  const router = useRouter();

const store = useRequestStore()

  const { loginUser } = useLayout();
  const loading = ref(null);
  const admins = ref([]);
  const companies = ref([]);



  const searchs = reactive({
    customerId: loginUser.value?.user_uid
  });


// const dropdownItem = ref(null);

  // 필드 구성을 computed 속성으로 변경하여 항상 최신 값을 유지합니다.
  const searchConfig = computed(() => [
    { model: store.Srch, fields: ['title'], operator: 'like' },
    ...(loginUser.value?.login_type !== 'admin'
      ? [{ model: searchs.customerId, fields: ['customerId'] }]
      : []),
    ...(loginUser.value?.login_type === 'admin' ? [
      { model: store.adminItem?.id, fields: ['adminId'], operator: '' },
      { model: store.companyItem?.id, fields: ['customer.companyId'], operator: '' }
    ] : []),
    { model: store.dropdownItem?.code, fields: ['status'], operator: '' }
  ]);

  const queryOptions = reactive({
    select: 'id,title,createdAt,customer,admin,status',
    remove: 'customerId,description',
    sorts: [{ field: 'id', dir: 'desc' }],
    page: 1,
    pageSize: 100
  });


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

  onMounted(async () => {
    const adminList = await AdminService.getList(loading);
    admins.value = [{ userName: '전체', id: null }, ...adminList];

    const companyList = await CompanyService.getList(loading);
    companies.value = [{ name: '전체', id: null }, ...companyList];

    // 다른 페이지에서 관리자 필터를 설정하고 넘어온 경우,
    // admins 목록에서 완전한 객체를 찾아 다시 할당해줍니다.

    console.log('(store.adminItem && store.adminItem.id)', (store.adminItem && store.adminItem.id) );
    if (store.adminItem && store.adminItem.id) {

    console.log('다른 페이지에서 관리자 필터를 설정하고 넘어온 경우 이다.');

      const foundAdmin = admins.value.find(a => a.id == store.adminItem.id);
      if (foundAdmin) {

    console.log('admin 찾았다.');

        store.adminItem = foundAdmin;
      }
    }

  });

  // 상태 선택이 변경되면 search()를 호출합니다.
  watch(
    () => store.dropdownItem,
    (newValue, oldValue) => {
      if (newValue !== oldValue) search();
    }
  );

  // 관리자 선택이 변경되면 search()를 호출합니다.
  watch(
    () => store.adminItem,
    (newValue, oldValue) => {
      if (newValue !== oldValue) search();
    }
  );

  // 회사 선택이 변경되면 search()를 호출합니다.
  watch(
    () => store.companyItem,
    (newValue, oldValue) => {
      if (newValue !== oldValue) search();
    }
  );




</script>

<template>


  <!-- 조회영역 전체 -->
<form class="card flex flex-col gap-4 md:flex-row md:items-center md:justify-between" @submit.prevent="search">

    <!-- label + input 묶음 -->
    <div class="flex flex-col md:flex-row md:items-center gap-4 flex-1">
      <!-- 첫 번째 -->
      <div class="flex flex-col md:flex-row md:items-center gap-2 flex-1">
        <label for="name3" class="w-24 md:text-right shrink-0">검색</label>
        <InputText id="name3" type="text" v-model="store.Srch" placeholder="Search..." class="md:min-w-[12rem]" />
        
        <label for="state" class="w-24 md:text-right shrink-0">상태</label>
        <Select id="state" v-model="store.dropdownItem" :options="STATUS_ALL" optionLabel="ttl" placeholder="Select One" class="md:min-w-[12rem]"></Select>
     
        <template v-if="loginUser?.login_type === 'admin'">
          <label for="admin" class="w-24 md:text-right shrink-0">관리자</label>
          <Select id="admin" v-model="store.adminItem" :options="admins" optionLabel="userName" placeholder="Select One" class="md:min-w-[12rem]"></Select>

          <label for="company" class="w-24 md:text-right shrink-0">회사</label>
          <Select id="company" v-model="store.companyItem" :options="companies" optionLabel="name" placeholder="Select One" class="md:min-w-[12rem]"></Select>
        </template>

     
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
  <div class="flex flex-col text-right text-sm flex-shrink-0 w-32">
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
