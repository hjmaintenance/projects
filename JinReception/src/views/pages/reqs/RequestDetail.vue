<script setup>
  import { RequestService } from '@/service/RequestService';
  import { buildQueryPayload2 } from '@/utils/apiUtils';
  import { formatDate, STATUS } from '@/utils/formatters';
  import { onMounted, reactive, ref, watch, nextTick, computed } from 'vue';
  import { useRoute, useRouter } from 'vue-router';
  import { useLayout } from '@/layout/composables/layout';

  import DataTable from 'primevue/datatable';
  import Column from 'primevue/column';
  import CommentList from './CommentList.vue';

  const route = useRoute();
  const router = useRouter();
  const { loginUser } = useLayout();
  const loading = ref(null);



  const searchs = reactive({
    Srch: '',
    dropdownItem: ref(null)
  });


// const dropdownItem = ref(null);

  // 필드 구성을 computed 속성으로 변경하여 항상 최신 값을 유지합니다.
  const searchConfig = computed(() => [
    { model: searchs.Srch, fields: ['title'], operator: 'like' },
    //{ model: 'Srch', fields: ['id'], operator: '' },
    { model: searchs.dropdownItem?.code, fields: ['status'], operator: '' }
  ]);

  const queryOptions = reactive({
    remove: 'customerId,description',
    sorts: [{ field: 'id', dir: 'desc' }],
    page: 1,
    pageSize: 100
  });

  const visible = ref(false);

  const requests = ref([]);
  const selectedRequest = ref(null);

  //
  const getDetail = async () => {

    const requestId = route.query.id;
    if (!requestId) return;

    const fullRequestData = await RequestService.get(requestId);
    selectedRequest.value = fullRequestData;
  };


  // loginUser.value.user_uid 값이 변경되거나 초기값이 있을 때 search()를 호출합니다.
  watch(
    () => loginUser.value?.user_uid,
    (newUid) => {
      if (newUid) {
        getDetail();
      }
    },
    { immediate: true }
  );





  const updateRequestStatus = async (statusName) => {
    if (!selectedRequest.value) return;

    const statusObject = STATUS.find(s => s.name === statusName);
    if (!statusObject) {
      console.error(`'${statusName}'에 해당하는 상태를 찾을 수 없습니다.`);
      return;
    }

    selectedRequest.value.status = statusObject.code;
    selectedRequest.value.adminId = loginUser.value?.user_uid;
    await RequestService.accept(selectedRequest.value);
    if (statusName === 'DELETE') {
      // 삭제 후 이전 페이지로 이동
      router.back();
    }
    else{
    // 상태 변경 후 상세 정보를 다시 불러와 화면을 갱신합니다.
    await getDetail(); 
    }
  };


  

    const prev_vue = async () => {
     // 이 화면으로 넘긴 화면으로 돌아가기.
    router.back();
  };



</script>

<template>

    <div class="flex justify-end gap-2">
      <Button type="button" label="reload" severity="primary" @click="getDetail"></Button>
      
      <!-- Show buttons based on current status -->
      <template v-if="selectedRequest">
        <!-- Status 0 (Pending): Can move to In Progress (1) or Delete (4) -->
        <Button v-if="selectedRequest.status === 0" type="button" :label="`이거 ${loginUser?.user_name} 접수 할께요`" severity="primary" @click="updateRequestStatus('IN_PROGRESS')"></Button>
        <Button v-if="selectedRequest.status === 0" type="button" label="삭제" severity="danger" @click="updateRequestStatus('DELETE')"></Button>

        <!-- Status 1 (In Progress): Can move to Completed (2) or Rejected (3) -->
        <Button v-if="selectedRequest.status === 1" type="button" label="완료" severity="success" @click="updateRequestStatus('COMPLETED')"></Button>
        <Button v-if="selectedRequest.status === 1" type="button" label="반려" severity="secondary" @click="updateRequestStatus('REJECTED')"></Button>
      </template>

      <Button type="button" label="Close" severity="secondary" @click="prev_vue"></Button>
    </div>

    <div class="card">

      <h3 >{{selectedRequest?.title}}</h3>

      <ul>
        <li><span>{{ selectedRequest?.createdAt ? formatDate(new Date(selectedRequest.createdAt)) : ''    }}</span></li>
        <li><span>{{ selectedRequest?.statusName  }}</span></li>
        <li><span>{{ selectedRequest?.admin?.userName    }}</span></li>
        <li><span>{{ selectedRequest?.customer?.userName  }}</span></li>
      </ul>


      <hr class=" border-gray-400"/>


      <div class="gap-4 mt-8 mb-8" style="align-items: flex-start;"
       v-html="selectedRequest?.description"
      >
      </div>  

    </div>

    <!-- Comments Section -->
    <CommentList :request-id="selectedRequest.id" v-if="selectedRequest" />

    

  
</template>
