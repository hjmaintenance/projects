<script setup>


import { ref, onMounted } from 'vue'

import LayoutAuthenticated from '@/layouts/LayoutAuthenticated.vue'
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import ColumnGroup from 'primevue/columngroup';   // optional
import Row from 'primevue/row';                   // optional



const company = ref([])
const errorMsg = ref('')

onMounted(async () => {
    errorMsg.value = ''
    try {
        const token = localStorage.getItem('jwt_token')
        const res = await fetch('/companys', {
            headers: {
                'Authorization': `Bearer ${token}`
            }
        })
        if (!res.ok) throw new Error('인증 실패 또는 사용자 조회 실패')
        company.value = await res.json()
    } catch (err) {
        errorMsg.value = err.message
    }
})



</script>
<template>
  <LayoutAuthenticated>
<DataTable :value="company" tableStyle="min-width: 50rem">
    <Column field="id" header="Code"></Column>
    <Column field="name" header="Name"></Column>
    <Column field="companyName" header="CompanyName"></Column>
    <Column field="ceoName" header="CeoName"></Column>
</DataTable>


                                        <div v-if="errorMsg" style="color:red">{{ errorMsg }}</div>

  <Button label="클릭" icon="pi pi-check" />
</LayoutAuthenticated>
</template>