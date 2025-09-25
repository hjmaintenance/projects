<script setup>
  import { RequestService } from '@/service/RequestService';
  import { buildQueryPayload } from '@/utils/apiUtils';
  import { onMounted, reactive, ref, watch } from 'vue';

  import DataTable from 'primevue/datatable';
  import Column from 'primevue/column';

  const loading = ref(null);
  const columns = ref([
    { field: 'id', header: 'Id' },
    { field: 'createdAt', header: 'createdAt' },
    { field: 'title', header: 'Title' }
  ]);

  const searchs = reactive({
    Srch: '',
    customerId: localStorage.getItem('user.user_uid')
  });

  // 필드 구성
  const searchConfig = [{ model: 'Srch', fields: ['title'], operator: 'like' },
  { model: 'customerId', fields: ['customerId'], operator: '' }
];

  const queryOptions = reactive({
    sorts: [{ field: 'id', dir: 'desc' }],
    page: 1,
    pageSize: 100
  });

  const visible = ref(false);

  const requests = ref([]);
  const selectedRequest = ref(null);

  watch(selectedRequest, (newValue, oldValue) => {
    if (newValue) {
      visible.value = true;
    }
  });

  onMounted(() => {
    search();
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
  
    <div class="flex items-center gap-4 mb-8" >

    <div class="" v-html="selectedRequest?.description" ></div>

    </div>
    <div class="flex justify-end gap-2" >
        <Button type="button" label="close" severity="secondary" @click="visible = false"></Button>
    </div>
</Dialog>






  <div class="card">
    <DataTable :value="requests" dataKey="id" selectionMode="single" paginator :rows="10" :rowsPerPageOptions="[10, 20, 50]" :loading="loading" v-model:selection="selectedRequest">
      <template #empty> not found </template>
      
      <Column header="ID" field="id" > 
      </Column>
      <Column header="작성일" filterField="createdAt" dataType="date" style="min-width: 8rem">      
        <template #body="{ data }">
          {{ data.createdAt ? formatDate(new Date(data.createdAt)) : '' }}
        </template>      
      </Column>
      <Column header="제목" field="title"  > 
      </Column>
      <Column header="작성자" field="customerId"  style="min-width: 1rem" > 
      </Column>


    </DataTable>
  </div>
</template>
