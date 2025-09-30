1<script setup>
  import { RequestService } from '@/service/RequestService';
  import { buildQueryPayload } from '@/utils/apiUtils';
  import { formatDate } from '@/utils/formatters';
  import { onMounted, reactive, ref, watch, computed } from 'vue';
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
    Srch: '',
    customerId: computed(() => loginUser.value?.user_uid)
  });

  // 필드 구성
  const searchConfig = [
    { model: 'Srch', fields: ['title'], operator: 'like' },
    { model: 'customerId', fields: ['customerId'], operator: '' }
  ];

  const queryOptions = reactive({
    select: 'id,title,createdAt,customerId,Customer.UserName as CustomerName',
    sorts: [{ field: 'id', dir: 'desc' }],
    page: 1,
    pageSize: 100
  });

  const visible = ref(false);

  const requests = ref([]);
  const selectedRequest = ref(null);

  const search = async () => {
    console.log('loginUser', loginUser);
    // customerId를 검색 조건에 동적으로 추가합니다.
    const dynamicSearchConfig = [...searchConfig];
    //if (searchs.customerId) dynamicSearchConfig.push({ model: 'customerId', fields: ['customerId'], operator: '' });

    // 검색 조건과 페이징/정렬 옵션을 합쳐 최종 페이로드를 생성합니다.
    const finalPayload = buildQueryPayload(searchs, dynamicSearchConfig, queryOptions);
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

  //조회

  const getItem = async () => {
    //requests.value = await requests.get('1', loading);
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
    <div class="flex items-center gap-4 mb-8">
      <div class="" v-html="selectedRequest?.description"></div>
    </div>
    <div class="flex justify-end gap-2">
      <Button type="button" label="close" severity="secondary" @click="visible = false"></Button>
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
      <Column header="작성자" field="customerId" style="min-width: 1rem"> </Column>
      <Column header="작성자" field="customerName" style="min-width: 1rem"> </Column>
    </DataTable>
  </div>
</template>
