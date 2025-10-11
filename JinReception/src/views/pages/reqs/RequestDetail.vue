<script setup>
  import { RequestService } from '@/service/RequestService';
  import { buildQueryPayload2 } from '@/utils/apiUtils';
  import { formatDate, STATUS, formatDateSmart } from '@/utils/formatters';
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


    if (statusName === 'COMPLETED' || statusName === 'REJECTED' || statusName === 'Reset') {
      if( loginUser.value?.user_uid !== String(selectedRequest.value.adminId)){
        alert('접수 담당자가 아닙니다.');
        return;
      }
    }


    if (statusName === 'Reset') {

      await RequestService.reset(selectedRequest.value.id);
      await getDetail();
    }
    else{
      
      const statusObject = STATUS.find((s) => s.name === statusName);
      if (!statusObject) {
        console.error(`'${statusName}'에 해당하는 상태를 찾을 수 없습니다.`);
        return;
      }

      //selectedRequest.value.status = statusObject.code;
      //selectedRequest.value.adminId = loginUser.value?.user_uid;
      await RequestService.accept(selectedRequest.value.id, statusObject.code);
      if (statusName === 'DELETE') {
        // 삭제 후 이전 페이지로 이동
        router.back();
      } else {
        // 상태 변경 후 상세 정보를 다시 불러와 화면을 갱신합니다.
        await getDetail();
      }
    }
  };

  const prev_vue = async () => {
    // 이 화면으로 넘긴 화면으로 돌아가기.
    router.back();
  };

  const getIconForFile = (fileName) => {
    if (!fileName) return 'pi-file';
    const extension = fileName.split('.').pop().toLowerCase();
    switch (extension) {
      case 'pdf':
        return 'pi-file-pdf';
      case 'png':
      case 'jpg':
      case 'jpeg':
      case 'gif':
        return 'pi-image';
      case 'doc':
      case 'docx':
        return 'pi-file-word';
      case 'xls':
      case 'xlsx':
        return 'pi-file-excel';
      case 'zip':
      case 'rar':
        return 'pi-file-zip';
      default:
        return 'pi-file';
    }
  };
</script>

<template>


  <!-- 조회영역 전체 -->
<form class="card flex flex-col gap-4 md:flex-row md:items-center md:justify-between" @submit.prevent="search">

    <!-- label + input 묶음 -->
    <div class="flex flex-col md:flex-row md:items-center gap-4 flex-1">

    </div>


  <!-- 버튼 그룹 -->
  <div class="flex gap-2 w-full md:w-auto md:ml-4">
    <!-- 버튼이 여러 개라면 flex-1 로 균등분할 -->
    <Button type="button" label="Reload" severity="primary" class="flex-1 md:flex-none" @click="getDetail"></Button>


    <Button @click="router.push('/request/edit/'+route.query.id)" >수정</Button>

    <!-- Show buttons based on current status -->
    <template v-if="selectedRequest && loginUser && loginUser?.login_type === 'admin'">
      <!-- Status 0 (Pending): Can move to In Progress (1) or Delete (4) -->
      <Button v-if="selectedRequest.status === 0" type="button" :label="`${loginUser?.user_name} 접수`" class="flex-1 md:flex-none" severity="primary" @click="updateRequestStatus('IN_PROGRESS')"></Button>
      <!-- <Button v-if="selectedRequest.status === 0" type="button" label="삭제" severity="danger" @click="updateRequestStatus('DELETE')"></Button> -->

      <!-- Status 1 (In Progress): Can move to Completed (2) or Rejected (3) -->
      <Button v-if="selectedRequest.status === 1" type="button" label="완료" severity="success" class="flex-1 md:flex-none" @click="updateRequestStatus('COMPLETED')"></Button>
      <Button v-if="selectedRequest.status === 1" type="button" label="반려" severity="danger" class="flex-1 md:flex-none" @click="updateRequestStatus('REJECTED')"></Button>
      <Button v-if="selectedRequest.status === 2 || selectedRequest.status === 3" type="button" label="접수초기화" severity="success" class="flex-1 md:flex-none" @click="updateRequestStatus('Reset')"></Button>
    </template>

    <template v-if="selectedRequest && loginUser && selectedRequest?.customer?.id === loginUser?.user_uid && loginUser?.login_type != 'admin'">
      <Button v-if="selectedRequest.status === 0" type="button" label="삭제" severity="danger" class="flex-1 md:flex-none" @click="updateRequestStatus('DELETE')"></Button>
    </template>

    <Button type="button" label="닫기" severity="secondary" class="flex-1 md:flex-none" @click="prev_vue"></Button>





  </div>

  </form>














































  <div class="card">
    <h3>{{ selectedRequest?.title }}</h3>

    <div class="comment-form flex flex-row items-center gap-2">

          <span  class="flex-1 resize-none" >
        {{ selectedRequest?.createdAt ? formatDateSmart(new Date(selectedRequest.createdAt)) : '' }} - {{ selectedRequest?.customer?.userName }}
        </span>




        <div 
          class="shrink-0"
        >


          <span class="inline-flex items-center px-2 py-0.5 rounded-full text-xs font-medium bg-blue-100 text-blue-800">
          {{ selectedRequest?.statusName }}
          </span>
          <span class="inline-flex items-center px-2 py-0.5 rounded-full text-xs font-medium bg-blue-100 text-blue-800">
          {{ selectedRequest?.admin?.userName }}
          </span>


      </div>
    </div>

<Divider />
    <div v-if="selectedRequest?.attachments?.length > 0" class="mt-4">

      <ul>
        <li v-for="attachment in selectedRequest.attachments" :key="attachment.id" class="flex items-center space-x-2">
          <i :class="['pi', getIconForFile(attachment.originalFileName)]"></i>
          <a :href="`/api/attachments/download/${attachment.id}`" target="_blank" download>{{ attachment.originalFileName }}</a>
        </li>
      </ul>
<Divider />
    </div>

    <div class="gap-4 mt-8 mb-8 min-h-[25rem] " style="align-items: flex-start" v-html="selectedRequest?.description"></div>


  </div>

  <!-- Comments Section -->
  <CommentList :request-id="selectedRequest.id" v-if="selectedRequest" />
</template>
