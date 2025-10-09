<script setup>
import { ref, onMounted, computed } from 'vue';
import { onBeforeRouteLeave } from 'vue-router';
import { useLayout } from '@/layout/composables/layout';
import { useConfirm } from 'primevue/useconfirm';
import { useToast } from 'primevue/usetoast';
import { AdminService } from '@/service/AdminService';
import { CustomerService } from '@/service/CustomerService';
import Password from 'primevue/password';
import Divider from 'primevue/divider';

const confirm = useConfirm();
const { loginUser } = useLayout();
const toast = useToast();

const user = ref({});
const originalUser = ref({});
const loading = ref(true);
const saving = ref(false);
const passwordSaving = ref(false);

const passwordData = ref({
  currentPassword: '',
  newPassword: '',
  confirmPassword: ''
});

const sexOptions = ref([
  { name: '남성', value: 'male' },
  { name: '여성', value: 'female' }
]);

const isProfileChanged = computed(() => {
  return JSON.stringify(user.value) !== JSON.stringify(originalUser.value);
});

const isPasswordFormValid = computed(() => {
  return passwordData.value.newPassword && passwordData.value.newPassword === passwordData.value.confirmPassword && passwordData.value.currentPassword;
});

onMounted(async () => {
  try {
    const { login_type, user_uid } = loginUser.value;
    let response;

    if (login_type === 'admin') {
      response = await AdminService.get(user_uid);
    } else {
      response = await CustomerService.get(user_uid);
    }

    if (response) {
      user.value = response;
      originalUser.value = JSON.parse(JSON.stringify(response));
    } else {
      toast.add({ severity: 'error', summary: 'Error', detail: 'Failed to fetch profile', life: 3000 });
    }
  } catch (error) {
    console.error('Failed to fetch profile:', error);
    toast.add({ severity: 'error', summary: 'Error', detail: 'Failed to fetch profile', life: 3000 });
  } finally {
    loading.value = false;
  }
});

const saveProfile = async () => {
  saving.value = true;
  try {
    const { login_type } = loginUser.value;
    let response;

    if (login_type === 'admin') {
      response = await AdminService.update(user.value);
    } else {
      response = await CustomerService.update(user.value);
    }

    if (response.success === true) {
      toast.add({ severity: 'success', summary: 'Success', detail: 'Profile updated successfully', life: 3000 });
      originalUser.value = JSON.parse(JSON.stringify(user.value));
    } else {
      toast.add({ severity: 'error', summary: 'Error', detail: 'Failed to update profile', life: 3000 });
    }
  } catch (error) {
    console.error('Failed to save profile:', error);
    toast.add({ severity: 'error', summary: 'Error', detail: 'Failed to update profile', life: 3000 });
  } finally {
    saving.value = false;
  }
};

const onUpload = (event) => {
  // 실제 파일 업로드 로직을 구현해야 합니다.
  // 여기서는 임시로 Base64 인코딩된 이미지를 user.photo에 할당합니다.
  const file = event.files[0];
  const reader = new FileReader();
  reader.onload = (e) => {
    user.value.photo = e.target.result;
    toast.add({ severity: 'info', summary: 'Success', detail: 'Photo Updated. Please save your changes.', life: 3000 });
  };
  reader.readAsDataURL(file);
};

const removePhoto = () => {
  user.value.photo = null;
  toast.add({ severity: 'info', summary: '정보', detail: '사진이 제거되었습니다. 변경사항을 저장해주세요.', life: 3000 });
};

const changePassword = async () => {
  if (!isPasswordFormValid.value) {
    toast.add({ severity: 'warn', summary: '입력 확인', detail: '새 비밀번호가 일치하지 않거나, 현재 비밀번호가 입력되지 않았습니다.', life: 3000 });
    return;
  }
  passwordSaving.value = true;
  try {
    // TODO: 실제 비밀번호 변경 API를 호출해야 합니다.
    // 예: await AuthService.changePassword({ currentPassword: passwordData.value.currentPassword, newPassword: passwordData.value.newPassword });

    // API 호출 시뮬레이션
    await new Promise((resolve) => setTimeout(resolve, 1500));

    toast.add({ severity: 'success', summary: '성공', detail: '비밀번호가 성공적으로 변경되었습니다.', life: 3000 });
    // 입력 필드 초기화
    passwordData.value.currentPassword = '';
    passwordData.value.newPassword = '';
    passwordData.value.confirmPassword = '';
  } catch (error) {
    console.error('Failed to change password:', error);
    toast.add({ severity: 'error', summary: '오류', detail: '비밀번호 변경 중 오류가 발생했습니다.', life: 3000 });
  } finally {
    passwordSaving.value = false;
  }
};

onBeforeRouteLeave(() => {
  if (!isProfileChanged.value) {
    return true;
  }

  return new Promise((resolve) => {
    confirm.require({
      header: '변경사항 확인',
      message: '저장되지 않은 변경사항이 있습니다. 정말로 페이지를 떠나시겠습니까?',
      icon: 'pi pi-exclamation-triangle',
      acceptLabel: '나가기',
      rejectLabel: '머무르기',
      accept: () => resolve(true),
      reject: () => resolve(false)
    });
  });
});
</script>

<template>
  <ConfirmDialog />
  <div class="card">
    <div class="flex justify-between items-center mb-6">
      <h5 class="text-2xl font-bold m-0">프로필 설정</h5>
      <Button label="저장" icon="pi pi-check" @click="saveProfile" :loading="saving" :disabled="!isProfileChanged" />
    </div>

    <Skeleton v-if="loading" height="20rem"></Skeleton>

    <div v-else class="grid grid-cols-1 md:grid-cols-12 gap-6">
      <div class="md:col-span-4 lg:col-span-3 flex flex-col items-center text-center">
        <Avatar v-if="user.photo" :image="user.photo" class="w-64 h-64 text-6xl mb-4" shape="circle" style="width: 16rem;height: 16rem;" />
        <Avatar v-else icon="pi pi-user" class="w-64 h-64 text-6xl mb-4" shape="circle" />

        <div class="flex items-center gap-2">
          <FileUpload mode="basic" name="photo[]" url="/api/upload" accept="image/*" :maxFileSize="1000000" @select="onUpload" :auto="true" customUpload chooseLabel="사진 변경" class="p-button-sm" />
          <Button v-if="user.photo" icon="pi pi-trash" severity="danger" outlined @click="removePhoto" class="p-button-sm" v-tooltip.bottom="'사진 제거'" />
        </div>
        <h6 class="text-xl font-semibold mt-4 mb-1">{{ user.userName }}</h6>
        <span class="text-surface-500 dark:text-surface-400">{{ user.email }}</span>
      </div>

      <div class="md:col-span-8 lg:col-span-9">
        <div class="p-fluid grid grid-cols-1 lg:grid-cols-2 gap-x-4 gap-y-6">
          <div class="field grid grid-cols-[6rem,1fr] items-center">
            <label for="username" class="pr-4 text-right">사용자 이름</label>
            <InputText id="username" v-model="user.userName" />
          </div>
          <div class="field grid grid-cols-[6rem,1fr] items-center">
            <label for="email" class="pr-4 text-right">이메일</label>
            <InputText id="email" v-model="user.email" type="email" />
          </div>

          <!-- Customer specific fields -->
          <template v-if="loginUser.login_type !== 'admin'">
            <div class="field grid grid-cols-[6rem,1fr] items-center">
              <label for="sex" class="pr-4 text-right">성별</label>
              <Dropdown id="sex" v-model="user.sex" :options="sexOptions" optionLabel="name" optionValue="value" placeholder="성별 선택"></Dropdown>
            </div>
          </template>

          <!-- Admin specific fields -->
          <template v-if="loginUser.login_type === 'admin'">
            <!-- 관리자 전용 필드를 여기에 추가하세요. -->
          </template>
        </div>
      </div>
    </div>

    <Divider type="solid" class="my-8">
      <b>비밀번호 변경</b>
    </Divider>

    <div class="grid grid-cols-1 md:grid-cols-12 gap-6">
      <div class="md:col-span-4 lg:col-span-3">
        <h6 class="font-semibold text-lg">보안 설정</h6>
        <p class="mt-1 text-sm text-surface-500 dark:text-surface-400">안전한 계정 사용을 위해 주기적으로 비밀번호를 변경해주세요.</p>
      </div>
      <div class="md:col-span-8 lg:col-span-9">
        <div class="p-fluid grid grid-cols-1 gap-y-6">
          <div class="field grid grid-cols-[8rem,1fr] items-center">
            <label for="currentPassword" class="pr-4 text-right">현재 비밀번호</label>
            <Password id="currentPassword" v-model="passwordData.currentPassword" :feedback="false" toggleMask></Password>
          </div>
          <div class="field grid grid-cols-[8rem,1fr] items-center">
            <label for="newPassword" class="pr-4 text-right">새 비밀번호</label>
            <Password id="newPassword" v-model="passwordData.newPassword" toggleMask></Password>
          </div>
          <div class="field grid grid-cols-[8rem,1fr] items-center">
            <label for="confirmPassword" class="pr-4 text-right">새 비밀번호 확인</label>
            <Password id="confirmPassword" v-model="passwordData.confirmPassword" :feedback="false" toggleMask></Password>
          </div>
        </div>
        <Button label="비밀번호 변경" icon="pi pi-key" class="mt-4" @click="changePassword" :loading="passwordSaving" :disabled="!isPasswordFormValid"></Button>
      </div>
    </div>
  </div>

</template>
