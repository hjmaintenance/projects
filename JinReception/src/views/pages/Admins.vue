<script setup>
import { AdminService } from '@/service/AdminService';
import { TeamService } from '@/service/TeamService';
import { onBeforeMount, reactive, ref , onMounted} from 'vue';
 
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
 
 
let tempIdCounter = 0;
 
const loading = ref(null);
const columns = ref([
    { field: 'id', header: 'Id' },
    { field: 'loginId', header: 'Login Id' },
    { field: 'userName', header: 'User Name' },
    { field: 'email', header: 'Email' },
    { field: 'teamId', header: 'Team' }
]);
 
const admins = ref([])
const teams = ref([])
const selectedAdmin = ref([])
const editingAdmin = ref([])

const onRowEditComplete = (event) => {
  console.log('onRowEditComplete', event);
  let{newData, index} = event;
  admins.value[index] = newData;
}

onMounted(() => {
  const loadTeams = async () => {
    teams.value = await TeamService.getList(loading);
  }
  loadTeams();
  loadData();
});

// 초기화
const initdata = () => {
  admins.value = null;
}
// 전체 조회
const loadData = async () => {
  admins.value = await AdminService.getList(loading);
}
// 단일 조회
const getItem = async () => {
  admins.value = await AdminService.get('1', loading);
}
// 추가 
const addData = () => {
  tempIdCounter++;
  const tempId = `temp-${tempIdCounter}`;
  const newAdmin = { ui_id: tempId, loginId: '', userName: '', email: ''};
  
  admins.value.push(newAdmin);  
}
// 저장
const save = async () => {
  const results = await AdminService.save(admins, loading);
  if (results) {
    const newAdminResults = results.filter(r => r && r.tempPassword);
    if (newAdminResults.length > 0) {
      alert(`New admin(s) created. Please save the temporary password(s): ${newAdminResults.map(r => `${r.admin.userName}: ${r.tempPassword}`).join(', ')}`);
    }
  }
  loadData();
}
// 삭제
const deleteSelected = async () => {
  if (!selectedAdmin.value || selectedAdmin.value.length === 0) {
    return;
  }

  const isTemp = (item) => typeof item.ui_id === 'string' && item.ui_id.startsWith('temp-');

  const tempIdsToRemove = selectedAdmin.value.filter(isTemp).map((c) => c.ui_id);
  
  if (tempIdsToRemove.length > 0) {
    admins.value = admins.value.filter((c) => !tempIdsToRemove.includes(c.ui_id));
  }
  const itemsToDeleteInDb = selectedAdmin.value.filter((c) => !isTemp(c));
  if (itemsToDeleteInDb.length > 0) {
    await AdminService.deleteSelected(itemsToDeleteInDb, loading);
  }
  loadData();
  selectedAdmin.value = [];
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
    
    <DataTable :value="admins" 
               dataKey="id" 
               :loading="loading"
               v-model:editingRows="editingAdmin"
               v-model:selection="selectedAdmin"
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
               <template #empty> No admins found. </template>
               <template #loading> Loading admins data. Please wait. </template>
 
            <Column selectionMode="multiple" headerStyle="width: 3rem"></Column>
<Column v-for="col of columns" :key="col.field" :field="col.field" :header="col.header" style="width: 25%">  

  <template #body="{ data, field }">
    <template v-if="field == 'teamId'">
      {{ teams && Array.isArray(teams) && teams.find(team => team.id === data[field]) ? teams.find(team => team.id === data[field]).name : '' }}
    </template>
    <template v-else>
      {{ data[field] }}
    </template>
  </template>
  <template #editor="{ data, field }">
    <template v-if="field == 'id'">
      {{ data[field] }}
    </template>
    <template v-else-if="field == 'teamId'">
      <Select v-model="data[field]" :options="teams" optionLabel="name" optionValue="id" fluid />
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
