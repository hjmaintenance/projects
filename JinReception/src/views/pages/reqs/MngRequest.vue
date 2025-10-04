<script setup>
  import { RequestService } from '@/service/RequestService';
  import { buildQueryPayload2 } from '@/utils/apiUtils';
  import { formatDate, STATUS } from '@/utils/formatters';
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
    dropdownItem: ref(null)
  });


// const dropdownItem = ref(null);

  // 필드 구성을 computed 속성으로 변경하여 항상 최신 값을 유지합니다.
  const searchConfig = computed(() => [
    { model: searchs.Srch, fields: ['title'], operator: 'like' },
    //{ model: 'Srch', fields: ['id'], operator: '' },
    { model: searchs.dropdownItem?.code, fields: ['status'], operator: '' }
  ]);

  const queryOptions = reactive({
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


  /*
  const dropdownItems = ref([
      { name: 'PENDING', code: '0' },
      { name: 'IN_PROGRESS', code: '1' },
      { name: 'REJECTED', code: '2' },
      { name: 'COMPLETED', code: '3' },
  ]);
*/

</script>

<template>










<form class="card srcharea" @submit.prevent="search">

<div class="flex flex-col sm:flex-row sm:items-center" >
                   


        <IconField iconPosition="left">
          <InputText type="text" v-model="searchs.Srch" placeholder="Search..."  size="small"/>
          <InputIcon class="pi pi-search" />
        </IconField>

        <Select  size="small" id="state" v-model="searchs.dropdownItem" :options="STATUS" optionLabel="name" placeholder="Select One" class="ml-2"></Select>



   


                    <div class="flex flex-col md:flex-row justify-between md:items-center flex-1 gap-6">
                        <div></div><div></div>
                        <div>


      <Button  size="small" label="조회" class="ml-2 mr-2" @click="search" raised />

                        </div>
                    </div>
                </div>


  </form>



























  <Dialog v-model:visible="visible" modal :header="selectedRequest?.title" :style="{ width: '70rem', height: '90vh' }">
    <div class="flex justify-end gap-2">
      <Button type="button" label="이거 내가 접수 할께요" severity="danger" @click="accept_request"></Button>
    </div>
    <div class="flex items-center gap-4 mb-8" style="overflow-y: auto;
  height: calc(81vh - 100px);
  align-items: flex-start;">
      <div class="" v-html="selectedRequest?.description"></div>
    </div>
    <div class="flex justify-end gap-2">
      <Button type="button" label="reload" severity="primary" @click="getDetail"></Button>
      <Button type="button" :label="`이거 ${loginUser?.user_name} 접수 할께요`" severity="primary" @click="accept_request"></Button>
      <Button type="button" label="반려" severity="secondary" @click="reject_request"></Button>
      <Button type="button" label="완료" severity="success" @click="complete_request"></Button>
      <Button type="button" label="Close" severity="secondary" @click="visible = false"></Button>
    </div>
  </Dialog>

  <div class="card">
    <DataTable :value="requests" dataKey="id" selectionMode="single" paginator :rows="10" :rowsPerPageOptions="[10, 20, 50]" :loading="loading" v-model:selection="selectedRequest">
      <template #empty> not found </template>

      <Column header="ID" field="id"> </Column>
      <Column header="작성일" filterField="createdAt" dataType="date" style="min-width: 8rem">
        <template #body="{ data }">
          {{ data.createdAt ? formatDate(new Date(data.createdAt)) : '' }}
        </template>
      </Column>
      <Column header="제목" field="title"> </Column>
      <Column header="상태" field="statusName" style="min-width: 1rem"> </Column>
      <Column header="작성자" field="customer.userName" style="min-width: 1rem"> </Column>
      <Column header="접수자" field="admin.userName" style="min-width: 1rem"> </Column>
    </DataTable>
  </div>
</template>
