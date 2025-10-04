<script setup>
import { CustomerService } from '@/service/CustomerService';
import { CompanyService } from '@/service/CompanyService';
import { onBeforeMount, reactive, ref , onMounted, watch} from 'vue';

  import { useLayout } from '@/layout/composables/layout';



  const { loginUser } = useLayout();



 
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
 
 
let tempIdCounter = 0;
 
const loading = ref(null);
const columns = ref([
    { field: 'id', header: 'Id' },
    { field: 'loginId', header: 'loginId' },
    { field: 'userName', header: 'userName' },

    { field: 'companyId', header: 'company' }
]);
 
const customers = ref([])
const companies = ref([])
const selectedCustomer = ref([])
const editingCustomer = ref([])

const onRowEditComplete = (event) => {
  console.log('onRowEditComplete', event);
  let{newData, index} = event;
  customers.value[index] = newData;




}
// 회사정보는 한번만 불러오기 위함
onMounted(() => {
  const loadCompanies = async () => {
    companies.value = await CompanyService.getList(loading);
  }
  loadCompanies();
});

// 초기화
const initdata = () => {
  customers.value = null;
}
// 전체 조회
const loadData = async () => {
  customers.value = await CustomerService.getList(loading);
}


  watch(
    () => loginUser.value?.user_uid,
    (newUid) => {
      if (newUid) loadData();
    },
    { immediate: true }
  );



// 단일 조회
const getItem = async () => {
  customers.value = await CustomerService.get('1', loading);
}
// 추가 
const addData = () => {
  tempIdCounter++;
  const tempId = `temp-${tempIdCounter}`;
  const newCustomer = { ui_id: tempId, loginId: 'aaa', password: '12125', email: 'cccc@naver.com'};
  
  customers.value.push(newCustomer);  
}
// 저장
const save = async () => {
  await CustomerService.save(customers, loading);
  loadData();
}
// 삭제
const deleteSelected = async () => {
  console.log('deleteSelected', selectedCustomer.value);
  if (!selectedCustomer.value || selectedCustomer.value.length === 0) {
    return;
  }

  const isTemp = (item) => typeof item.ui_id === 'string' && item.ui_id.startsWith('temp-');

  const tempIdsToRemove = selectedCustomer.value.filter(isTemp).map((c) => c.ui_id);
  
  if (tempIdsToRemove.length > 0) {
    customers.value = customers.value.filter((c) => !tempIdsToRemove.includes(c.ui_id));
  }
  const itemsToDeleteInDb = selectedCustomer.value.filter((c) => !isTemp(c));
  if (itemsToDeleteInDb.length > 0) {
    await CustomerService.deleteSelected(itemsToDeleteInDb, loading);
  }
  loadData();
  selectedCustomer.value = [];
}
</script>
 
<template>
 
    <div class="card srcharea">
 
 
<Button label="조회" class="mr-2" @click="loadData" />
<Button label="추가" class="mr-2" icon="pi pi-plus" @click="addData" />
<Button label="저장" class="mr-2" @click="save" />
<Button label="삭제" class="mr-2" @click="deleteSelected" />
    </div>
    <div class="card">
    
    <!-- editMode="cell" 사용시, select컴포넌트 편집안돼서 editMode="row" 사용 -->
    <DataTable :value="customers" 
               dataKey="id" 
               :loading="loading"
               v-model:editingRows="editingCustomer"
               v-model:selection="selectedCustomer"
               editMode="row" 
               @row-edit-save="onRowEditComplete"
               :pt="{
                table: { style: 'min-width: 50rem' },
                column: {
                    bodycell: ({ state }) => ({
                        class: [{ '!py-0': state['d_editing'] }]
                    })
                }
            }"
 
               >
               <template #empty> No customers found. </template>
               <template #loading> Loading customers data. Please wait. </template>
 
            <Column selectionMode="multiple" headerStyle="width: 3rem"></Column>
<Column v-for="col of columns" :key="col.field" :field="col.field" :header="col.header" style="width: 25%">  

  <template #body="{ data, field }">
    <template v-if="field == 'companyId'">
      {{ companies && Array.isArray(companies) && companies.find(company => company.id === data[field]) ? companies.find(company => company.id === data[field]).name : '' }}
    </template>
    <template v-else>
      {{ data[field] }}
    </template>
  </template>
  <template #editor="{ data, field }">
    <template v-if="field == 'id'">
      {{ data[field] }}
    </template>
    <template v-else-if="field == 'companyId'">
      <Select v-model="data[field]" :options="companies" optionLabel="name" optionValue="id" fluid />
    </template>
    <template v-else>
      <InputText v-model="data[field]" autofocus fluid />
    </template>
  </template>
            </Column>
            <Column :rowEditor="true" style="width: 10%; min-width: 8rem" bodyStyle="text-align:center"></Column>
        </DataTable>
    </div>
 
</template>