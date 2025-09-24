<script setup>
  import { RequestService } from '@/service/RequestService';
  import { buildQueryPayload } from '@/utils/apiUtils';
  import { reactive, ref, watch } from 'vue';

  import DataTable from 'primevue/datatable';
  import Column from 'primevue/column';

  const loading = ref(null);
  const columns = ref([
    { field: 'id', header: 'Id' },
    { field: 'title', header: 'Title' }
  ]);

  const searchs = reactive({
    Srch: ''
  });

  // 필드 구성
  const searchConfig = [{ model: 'Srch', fields: ['title'], operator: 'like' }];

  const queryOptions = reactive({
    sorts: [{ field: 'id', dir: 'desc' }],
    page: 1,
    pageSize: 100
  });

  const requests = ref([]);
  const selectedRequest = ref(null);

  watch(selectedRequest, (newValue, oldValue) => {
    //alert(`변경됨: ${oldValue} → ${newValue}`)
  });

  //초기화
  const initData = async () => {};

  //조회
  const search = async () => {
    // 검색 조건과 페이징/정렬 옵션을 합쳐 최종 페이로드를 생성합니다.
    const finalPayload = buildQueryPayload(searchs, searchConfig, queryOptions);
    requests.value = await RequestService.search(finalPayload, loading);
  };

  const getItem = async () => {
    //requests.value = await requests.get('1', loading);
  };
</script>

<template>
  <form class="card srcharea" @submit.prevent="search">
    <div class="flex flex-col md:flex-row gap-8">
      <IconField iconPosition="left">
        <InputText type="text" v-model="searchs.Srch" placeholder="Search..." />
        <InputIcon class="pi pi-search" />
      </IconField>
    </div>
  </form>

  <div class="srchbtnarea mt-2">
    <Button label="조회" class="mr-2" @click="search" />
  </div>

  <div class="card">
    <h3>{{ selectedRequest?.title }}</h3>
    <div v-html="selectedRequest?.description"></div>
  </div>

  <div class="card">
    <DataTable :value="requests" dataKey="id" selectionMode="single" :loading="loading" v-model:selection="selectedRequest">
      <template #empty> xxxxxxxxxxxxxxx </template>
      <Column v-for="col of columns" :key="col.field" :field="col.field" :header="col.header" style="width: 25%"> </Column>
    </DataTable>
  </div>
</template>
