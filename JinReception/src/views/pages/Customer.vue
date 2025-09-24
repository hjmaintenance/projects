<script setup>
import { CustomerService } from '@/service/CustomerService';
import { onBeforeMount, reactive, ref , onMounted} from 'vue';
 
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
 
 
let tempIdCounter = 0;
 
const loading = ref(null);
const columns = ref([
    { field: 'id', header: 'Id' },
    { field: 'userName', header: 'userName' }
]);
 
const customers = ref([])
const selectedCustomer = ref(null)
 
const onCellEditComplete = (event) => {
    let { data, newValue, field } = event;
 
 
    console.log('onCellEditComplete', data, newValue, field);
    data[field] = newValue;
 
    
};
 
// 초기화
const initdata = () => {
  customers.value = null;
}
// 전체 조회
const loadData = async () => {
  customers.value = await CustomerService.getList();
}
// 단일 조회
const getItem = async () => {
  customers.value = await CustomerService.get('1');
}
// 추가 
const addData = () => {
  tempIdCounter++;
  const tempId = `temp-${tempIdCounter}`;
  customers.value.push({ ui_id: tempId, loginId: 'aaa', password: '12125', email: 'cccc@naver.com',companyId: 1});
}
// 저장
const save = async () => {
  await CustomerService.save(customers);
  loadData();
}
// 삭제
const deleteSelected = async () => {
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
    await CustomerService.deleteSelected(itemsToDeleteInDb);
  }
  loadData();
  selectedCustomer.value = [];
}
</script>
 
<template>
 
    <div class="card srcharea">
 
 
<Button label="초기화" class="mr-2" @click="initdata" />
<Button label="전체" class="mr-2" @click="loadData" />
<Button label="단일" class="mr-2" @click="getItem" />
<Button label="추가" class="mr-2" icon="pi pi-plus" @click="addData" />
<Button label="저장" class="mr-2" @click="save" />
<Button label="삭제" class="mr-2" @click="deleteSelected" />
 
    </div>
 
    <div class="card">
 
 
    <DataTable :value="customers" 
               dataKey="id" 
               :loading="loading"
               selectionMode="multiple" 
               v-model:selection="selectedCustomer"
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