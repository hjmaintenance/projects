<script setup>
import { CustomerService } from '@/service/CustomerService';
import { ProductService } from '@/service/ProductService';
import { FilterMatchMode, FilterOperator } from '@primevue/core/api';
import { onBeforeMount, reactive, ref , onMounted} from 'vue';

import axios from 'axios'
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import ColumnGroup from 'primevue/columngroup';   // optional
import Row from 'primevue/row';                   // optional




//const customers1 = ref(null);
const loading1 = ref(null);

const columns = ref([
    { field: 'id', header: 'Id' },
    { field: 'userName', header: 'userName' },
    { field: 'email', header: 'email' }
]);


const customers = ref([])
const selectedCustomer = ref(null)
const dialogVisible = ref(false)
const isNew = ref(true)
const form = ref({ id: null, name: '', email: '' })


// onBeforeMount(() => {
//     search();
// });




const search = async(  ) => {
    const token = localStorage.getItem('jwt_token');
        const res = await fetch('/api/customers', {
            headers: {
                'Authorization': `Bearer ${token}`
            }
        })
        if (!res.ok) throw new Error('인증 실패 또는 사용자 조회 실패')
       const resultvalue = await res.json();
    customers.value = resultvalue;
    return resultvalue;
};



const onCellEditComplete = (event) => {
    let { data, newValue, field } = event;


    console.log('onCellEditComplete', data, newValue, field);
    
    // switch (field) {
    //     default:
    //         if (newValue.trim().length > 0) data[field] = newValue;
    //         else event.preventDefault();
    //         break;
    // }
    
    
};




// 데이터 조회 (READ)
const loadData = async () => {
  const { data } = await axios.get('/api/customers')
  customers.value = data
}
onMounted(loadData)

// 새 데이터 추가 (CREATE)
const openNew = () => {
  form.value = { id: null, name: '', email: '' }
  isNew.value = true
  dialogVisible.value = true
}
const saveCustomer = async () => {
  if (isNew.value) {
    await axios.post('/api/customers', form.value)
  } else {
    await axios.put(`/api/customers/${form.value.id}`, form.value)
  }
  dialogVisible.value = false
  loadData()
}

// 기존 데이터 수정 (UPDATE)
const editCustomer = (customer) => {
  form.value = { ...customer }
  isNew.value = false
  dialogVisible.value = true
}

// 데이터 삭제 (DELETE)
const deleteCustomer = async (customer) => {
  await axios.delete(`/api/customers/${customer.id}`)
  loadData()
}





</script>

<template>
    <div class="card">

<Button label="search" @click="search" />
<Button label="search2" @click="loadData" />


 <Button label="새 고객 추가" icon="pi pi-plus" @click="openNew" class="mb-3" />

    <DataTable :value="customers" dataKey="id" selectionMode="single" v-model:selection="selectedCustomer">
     


<!-- 

 <DataTable :value="customers1" editMode="cell" @cell-edit-complete="onCellEditComplete"
            :pt="{
                table: { style: 'min-width: 50rem' },
                column: {
                    bodycell: ({ state }) => ({
                        class: [{ '!py-0': state['d_editing'] }]
                    })
                }
            }"
        >
 -->

            <template #empty> No customers found. </template>
            <template #loading> Loading customers data. Please wait. </template>
            
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


            <Column header="Actions" :exportable="false">
        <template #body="slotProps">
          <Button icon="pi pi-pencil" class="p-button-text" @click="editCustomer(slotProps.data)" />
          <Button icon="pi pi-trash" class="p-button-text p-button-danger" @click="deleteCustomer(slotProps.data)" />
        </template>
      </Column>




        </DataTable>




    <!-- Dialog Form -->
    <Dialog v-model:visible="dialogVisible" header="고객 정보" :modal="true">
      <div class="flex flex-col gap-3">
        <div>
          <label class="block">Name</label>
          <InputText v-model="form.name" class="w-full" />
        </div>
        <div>
          <label class="block">Email</label>
          <InputText v-model="form.email" class="w-full" />
        </div>
      </div>
      <template #footer>
        <Button label="취소" icon="pi pi-times" @click="dialogVisible=false" class="p-button-text" />
        <Button label="저장" icon="pi pi-check" @click="saveCustomer" />
      </template>
    </Dialog>




    </div>



</template>
