<script setup>
  import { CompanyService } from '@/service/CompanyService';
  import { buildQueryPayload } from '@/utils/apiUtils';
  import { reactive, ref,watch } from 'vue';
  import { useLayout } from '@/layout/composables/layout';

  import DataTable from 'primevue/datatable';
  import Column from 'primevue/column';

  let tempIdCounter = 0;

  const { loginUser } = useLayout();
  const loading = ref(null);
  const columns = ref([
    { field: 'id', header: 'Id' },
    { field: 'name', header: 'Name' }
  ]);

  // 검색영역 반응형 객체
  const searchs = reactive({
    Srch: ''
    // ex) addressSearch: ''
  });

  // 필드 구성
  const searchConfig = [
    { model: 'Srch', fields: ['name', 'addr'], operator: 'like' }
    // ex) { model: 'addressSearch', fields: ['addr'], operator: 'like' }
  ];

  // 정렬 및 페이징 설정
  const queryOptions = reactive({
    sorts: [{ field: 'id', dir: 'asc' }],
    page: 1,
    pageSize: 10
  });

  const companys = ref([]);
  const selectedCompany = ref([]);

  const onCellEditComplete = (event) => {
    let { data, newValue, field } = event;

    console.log('onCellEditComplete', data, newValue, field);
    data[field] = newValue;
    // switch (field) {
    //     default:
    //         if (newValue.trim().length > 0) data[field] = newValue;
    //         else event.preventDefault();
    //         break;
    // }
  };

  //onMounted(loadData)

  //초기화
  const initData = async () => {
    companys.value = [];
    searchs.Srch = '';
  };
  //전체
  const loadData = async () => {
    companys.value = await CompanyService.getList(loading);
  };



  //조회
  const search = async () => {
    // 검색 조건과 페이징/정렬 옵션을 합쳐 최종 페이로드를 생성합니다.
    const finalPayload = buildQueryPayload(searchs, searchConfig, queryOptions);
    companys.value = await CompanyService.search(finalPayload, loading);
  };



    watch(
    () => loginUser.value?.user_uid,
    (newUid) => {
      if (newUid) search();
    },
    { immediate: true }
  );


  

  const getItem = async () => {
    companys.value = await CompanyService.get('1', loading);
  };

  // 추가
  const addData = () => {
    tempIdCounter++;
    const tempId = `temp-${tempIdCounter}`;
    companys.value.push({ ui_id: tempId, name: '', addr: '' });
  };

  const save = async () => {
    await CompanyService.save(selectedCompany, loading);
    search();
  };

  // 삭제
  const deleteSelected = async () => {
    if (!selectedCompany.value || selectedCompany.value.length === 0) {
      return;
    }

    const isTemp = (item) => typeof item.ui_id === 'string' && item.ui_id.startsWith('temp-');

    const tempIdsToRemove = selectedCompany.value.filter(isTemp).map((c) => c.ui_id);
    if (tempIdsToRemove.length > 0) {
      companys.value = companys.value.filter((c) => !tempIdsToRemove.includes(c.ui_id));
    }

    const itemsToDeleteInDb = selectedCompany.value.filter((c) => !isTemp(c));
    if (itemsToDeleteInDb.length > 0) {
      await CompanyService.deleteSelected(itemsToDeleteInDb, loading);
    }
    search();
    selectedCompany.value = [];
  };
</script>

<template>






















<form class="card srcharea" @submit.prevent="search">

<div class="flex flex-col sm:flex-row sm:items-center" >
                   

      <IconField iconPosition="left">
        <InputText type="text" v-model="searchs.Srch" placeholder="Search..." />
        <InputIcon class="pi pi-search" />
      </IconField>
   


                    <div class="flex flex-col md:flex-row justify-between md:items-center flex-1 gap-6">
                        <div></div><div></div>
                        <div>
    <Button label="조회" class="mr-2" @click="search" />
    <Button label="추가" class="mr-2" @click="addData" />
    <Button label="저장" class="mr-2" @click="save" />
    <Button label="삭제" class="mr-2" @click="deleteSelected" />
                        </div>
                    </div>
                </div>


  </form>























































  <div class="card">
    <DataTable
      :value="companys"
      dataKey="id"
      :loading="loading"
      v-model:selection="selectedCompany"
      editMode="cell"
      @cell-edit-complete="onCellEditComplete"
      :pt="{
        table: { style: 'min-width: 50rem' },
        column: {
          bodycell: ({ state }) => ({
            class: [{ '!py-0': state['d_editing'] }]
          })
        }
      }"
    >
      <template #empty> No companys found. </template>
      <template #loading> Loading companys data. Please wait. </template>

      <Column selectionMode="multiple" headerStyle="width: 3rem"></Column>
      <Column v-for="col of columns" :key="col.field" :field="col.field" :header="col.header" style="width: 25%">
        <template #body="{ data, field }">
          {{ data[field] }}
        </template>

        <template #editor="{ data, field }">
          <template v-if="field == 'id'">
            {{ data[field] }}
          </template>
          <template v-else>
            <InputText v-model="data[field]" autofocus fluid />
          </template>
        </template>
      </Column>
    </DataTable>
  </div>
</template>
