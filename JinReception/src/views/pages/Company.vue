<script setup>
import { CompanyService } from '@/service/CompanyService';
import { onBeforeMount, reactive, ref , onMounted} from 'vue';

import DataTable from 'primevue/datatable';
import Column from 'primevue/column';


let tempIdCounter = 0;

const loading = ref(null);
const columns = ref([
    { field: 'id', header: 'Id' },
    { field: 'name', header: 'Name' }
]);

const companys = ref([])
const selectedCompany = ref(null)

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
const initdata = async () => {
  companys.value = null;
}
//조회
const loadData = async () => {
  companys.value = await CompanyService.getList(loading);
}
const getItem = async () => {
  companys.value = await CompanyService.get('1', loading);
}

// 추가 
const addData = () => {
  tempIdCounter++;
  const tempId = `temp-${tempIdCounter}`;
  companys.value.push({ ui_id: tempId, name: '', addr: '' });
}

const save = async () => {
  await CompanyService.save(companys, loading);
  loadData();
}

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
  loadData();
  selectedCompany.value = [];
}


</script>

<template>

    <div class="card srcharea">


<Button label="초기화" class="mr-2" @click="initdata" />
<Button label="전체" class="mr-2" @click="loadData" />
<Button label="단건" class="mr-2" @click="getItem" />
<Button label="저장" class="mr-2" @click="save" />
<Button label="추가" class="mr-2" icon="pi pi-plus"  @click="addData" />
<Button label="삭제" class="mr-2" @click="deleteSelected" />

    </div>

    <div class="card">


    <DataTable :value="companys" 
               dataKey="id" 
               :loading="loading"
               selectionMode="single" 
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
