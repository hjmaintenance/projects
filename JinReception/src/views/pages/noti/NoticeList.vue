<script setup>
import { NoticeService } from '@/service/NoticeService';
import { ref, onMounted } from 'vue';
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import Button from 'primevue/button';
import InputText from 'primevue/inputtext';

const loading = ref(false);
const notices = ref([]);
const selectedNotices = ref([]);
const editingNotice = ref([]);

let tempIdCounter = 0;

const columns = ref([
  { field: 'id', header: 'ID' },
  { field: 'title', header: 'Title' },
  { field: 'content', header: 'Content' }
]);

// 초기 데이터 로딩
onMounted(async () => {
  await loadNotices();
});

// 전체 조회
const loadNotices = async () => {
  loading.value = true;
  notices.value = await NoticeService.getList();
  loading.value = false;
};

// 초기화
const initData = () => {
  notices.value = [];
};

// 추가
const addNotice = () => {
  tempIdCounter++;
  notices.value.push({ ui_id: `temp-${tempIdCounter}`, title: '', content: '' });
};

// 저장
const saveNotices = async () => {
  await NoticeService.save(notices.value);
  await loadNotices();
};

// 삭제
const deleteSelected = async () => {
  const toDelete = selectedNotices.value.filter(n => !n.ui_id.startsWith('temp-'));
  if (toDelete.length > 0) await NoticeService.deleteSelected(toDelete);

  notices.value = notices.value.filter(n => !selectedNotices.value.includes(n));
  selectedNotices.value = [];
};

// 행 편집 완료
const onRowEditComplete = (event) => {
  const { newData, index } = event;
  notices.value[index] = newData;
};
</script>

<template>
  <div class="card srcharea">
    <Button label="초기화" class="mr-2" @click="initData" />
    <Button label="조회" class="mr-2" @click="loadNotices" />
    <Button label="추가" class="mr-2" icon="pi pi-plus" @click="addNotice" />
    <Button label="저장" class="mr-2" @click="saveNotices" />
    <Button label="삭제" class="mr-2" @click="deleteSelected" />
  </div>

  <div class="card">
    <DataTable
      :value="notices"
      dataKey="id"
      v-model:selection="selectedNotices"
      v-model:editingRows="editingNotice"
      editMode="row"
      :loading="loading"
      @row-edit-save="onRowEditComplete"
    >
      <Column selectionMode="multiple"></Column>

      <Column v-for="col in columns" :key="col.field" :field="col.field" :header="col.header">
        <template #body="{ data, field }">
          {{ data[field] }}
        </template>
        <template #editor="{ data, field }">
          <InputText v-model="data[field]" />
        </template>
      </Column>

      <Column :rowEditor="true" style="width: 10%; min-width: 8rem"></Column>
    </DataTable>
  </div>
</template>
