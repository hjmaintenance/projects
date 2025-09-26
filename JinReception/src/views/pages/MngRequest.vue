<script setup>
  import { RequestService } from '@/service/RequestService';
  import { buildQueryPayload } from '@/utils/apiUtils';
  import { onMounted, reactive, ref, watch } from 'vue';
  import { useLayout } from '@/layout/composables/layout';

  import DataTable from 'primevue/datatable';
  import Column from 'primevue/column';

  const { loginUser } = useLayout();
  const loading = ref(null);
  const columns = ref([
    { field: 'id', header: 'Id' },
    { field: 'createdAt', header: 'createdAt' },
    { field: 'title', header: 'Title' }
  ]);

  const searchs = reactive({
    Srch: ''
  });

  // 필드 구성
  const searchConfig = [
    { model: 'Srch', fields: ['title'], operator: 'like' },
    { model: 'customerId', fields: ['customerId'], operator: '' }
  ];

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
    const finalPayload = buildQueryPayload(searchs, searchConfig, queryOptions);
    requests.value = await RequestService.search(finalPayload, loading);
  };

  watch(selectedRequest, async (newValue, oldValue) => {
    if (newValue) {
      visible.value = true;
      const fullRequestData = await RequestService.get(newValue.id);
      selectedRequest.value.description = fullRequestData.description;
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

  //초기화
  const initData = async () => {};

  //접수 담당자 지정
  const accept_request = async () => {
    if (!selectedRequest.value) return;

    await RequestService.accept(selectedRequest.value.id, loading);
    /*
    visible.value = false;

    // 접수 처리 후, 해당 항목의 정보만 새로 불러와 목록을 업데이트합니다.
    const updatedItem = await RequestService.get(selectedRequest.value.id, { remove: queryOptions.remove });
    const index = requests.value.findIndex((item) => item.id === updatedItem.id);
    if (index !== -1) requests.value[index] = updatedItem;
    */
  };

  const getItem = async () => {
    //requests.value = await requests.get('1', loading);
  };

  const formatDate = (value) => {
    return value.toLocaleDateString('ko-KR', {
      day: '2-digit',
      month: '2-digit',
      year: 'numeric'
    });
  };
</script>

<template>
  <form class="card srcharea" @submit.prevent="search">
    <div class="flex flex-wrap items-start gap-4">
      <div class="field">
        <IconField iconPosition="left">
          <InputText type="text" v-model="searchs.Srch" placeholder="Search..." />
          <InputIcon class="pi pi-search" />
        </IconField>
      </div>
      <Button label="조회" class="mr-2" @click="search" />
    </div>
  </form>

  <Dialog v-model:visible="visible" modal :header="selectedRequest?.title" :style="{ width: '70rem' }">
    <div class="flex justify-end gap-2">
      <Button type="button" label="이거 내가 접수 할께요" severity="danger" @click="accept_request"></Button>
    </div>
    <div class="flex items-center gap-4 mb-8">
      <div class="" v-html="selectedRequest?.description"></div>
    </div>
    <div class="flex justify-end gap-2">
      <Button type="button" :label="`이거 ${loginUser?.user_name} 접수 할께요`" severity="primary" @click="accept_request"></Button>
      <Button type="button" :label="`반려`" severity="secondary" @click="reject_request"></Button>
      <Button type="button" :label="`완료`" severity="success" @click="reject_request"></Button>
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
