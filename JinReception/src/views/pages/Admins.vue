<script setup>
import { AdminService } from '@/service/AdminService';
import { TeamService } from '@/service/TeamService';
import { onBeforeMount, reactive, ref , onMounted} from 'vue';
 
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
 
 
let tempIdCounter = 0;
 
const loading = ref(null);
const columns = ref([
    { field: 'loginId', header: '로그인 ID' },
    { field: 'teamId', header: '소속 팀' }
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


  // 검색영역 반응형 객체
  const searchs = reactive({
    Srch: ''
    // ex) addressSearch: ''
  });


</script>
 
<template>
 




  <!-- 조회영역 -->
<form class="card flex flex-col gap-4 md:flex-row md:items-center md:justify-between" @submit.prevent="loadData">

    <div class="flex flex-col md:flex-row md:items-center gap-4 flex-1">
      <!-- 첫 번째 -->
      <div class="flex flex-col md:flex-row md:items-center gap-2 flex-1">
        <label for="name3" class="w-24 md:text-right shrink-0">검색</label>
        <InputText id="name3" type="text" v-model="searchs.Srch" placeholder="Search..." class="md:min-w-[12rem]" />
        
       
      </div>
    </div>


  <!-- 버튼 그룹 -->
  <div class="flex gap-2 w-full md:w-auto md:ml-4">
    <Button label="조회" class="flex-1 md:flex-none" @click="loadData" raised />
    <Button label="추가" class="flex-1 md:flex-none" @click="addData" raised />
    <Button label="저장" class="flex-1 md:flex-none" @click="save" raised />
    <Button label="삭제" class="flex-1 md:flex-none" @click="deleteSelected" raised />

  </div>

  </form>









  <div class="card">
    <DataTable :value="admins" dataKey="id" :loading="loading" v-model:editingRows="editingAdmin" v-model:selection="selectedAdmin" editMode="row" 
    @row-edit-save="onRowEditComplete" 
    :pt="{
        bodyRow: {
          class: 'h-[7rem] md:h-[auto]'
        },
       table: { style: '' } 
      }
    "
    >
      <template #empty> 관리자 정보가 없습니다. </template>
      <template #loading> 관리자 정보를 불러오는 중입니다. </template>

      <Column selectionMode="multiple" headerStyle="width: 3rem"></Column>

      <Column field="userName" header="사용자" style="min-width: 16rem">
        <template #body="{ data }">
          <div class="flex items-center gap-3">
            <Avatar v-if="data.photo" :image="data.photo" shape="circle" />
            <Avatar v-else :label="data.userName ? data.userName[0] : 'U'" shape="circle" />
            <div>
              <div class="font-semibold">{{ data.userName }}</div>
              <div class="text-sm text-surface-500 dark:text-surface-400">{{ data.email }}</div>
            </div>
          </div>
        </template>
        <template #editor="{ data, field }">
          <InputText v-model="data[field]" autofocus fluid />
        </template>
      </Column>

      <Column v-for="col of columns" :key="col.field" :field="col.field" :header="col.header" style="width: 25%">
        <template #body="{ data, field }">
          <template v-if="field === 'teamId'">
            <Tag severity="info" :value="teams.find((t) => t.id === data[field])?.name || '미지정'"></Tag>
          </template>
          <template v-else>
            {{ data[field] }}
          </template>
        </template>
        <template #editor="{ data, field }">
          <template v-if="field === 'teamId'">
            <Select v-model="data[field]" :options="teams" optionLabel="name" optionValue="id" placeholder="팀 선택" fluid />
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
