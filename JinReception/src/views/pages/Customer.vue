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
 
 
//조회
const loadData = async () => {
  customers.value = await CustomerService.getList();
}
</script>
 
<template>
 
    <div class="card srcharea">
 
 
<Button label="전체" class="mr-2" @click="loadData" />
 
    </div>
 
    <div class="card">
 
 
    <DataTable :value="customers" 
               dataKey="id" 
               :loading="loading"
               selectionMode="single" 
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