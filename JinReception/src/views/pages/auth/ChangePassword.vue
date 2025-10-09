<script setup>
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import { AuthService } from '@/service/AuthService';
import { useLayout } from '@/layout/composables/layout';
import { useToast } from 'primevue/usetoast';

const oldPassword = ref('');
const newPassword = ref('');
const confirmPassword = ref('');
const router = useRouter();
const { setLoginUser } = useLayout();
const toast = useToast();

const submit = async () => {
  if (newPassword.value !== confirmPassword.value) {
    toast.add({ severity: 'warn', summary: '입력 오류', detail: '새 비밀번호가 일치하지 않습니다.', life: 3000 });
    return;
  }

  if (oldPassword.value === newPassword.value) {
    toast.add({ severity: 'warn', summary: '입력 오류', detail: '새 비밀번호는 이전 비밀번호와 같을 수 없습니다.', life: 3000 });
    return;
  }

  try {
    await AuthService.changePassword({ 
      OldPassword: oldPassword.value, 
      NewPassword: newPassword.value 
    });

    toast.add({ severity: 'success', summary: '성공', detail: '비밀번호가 변경되었습니다. 다시 로그인해주세요.', life: 3000 });

    // Log out user
    localStorage.removeItem('jwt_token');
    setLoginUser(null);

    setTimeout(() => {
      router.push('/auth/login');
    }, 2000);

  } catch (err) {
    const detail = err.response?.data || '비밀번호 변경 중 오류가 발생했습니다.';
    toast.add({ severity: 'error', summary: '변경 실패', detail: detail, life: 3000 });
  }
};
</script>

<template>
  <div class="flex items-center justify-center min-h-screen">
    <div class="w-full max-w-md">
      <div class="bg-white shadow-md rounded px-8 pt-6 pb-8 mb-4">
        <div class="mb-4">
          <h1 class="text-2xl font-bold text-center">Change Password</h1>
        </div>
        <div class="mb-4">
          <label class="block text-gray-700 text-sm font-bold mb-2" for="oldPassword">
            Old Password
          </label>
          <Password id="oldPassword" v-model="oldPassword" :feedback="false" toggleMask fluid />
        </div>
        <div class="mb-4">
          <label class="block text-gray-700 text-sm font-bold mb-2" for="newPassword">
            New Password
          </label>
          <Password id="newPassword" v-model="newPassword" :feedback="false" toggleMask fluid />
        </div>
        <div class="mb-6">
          <label class="block text-gray-700 text-sm font-bold mb-2" for="confirmPassword">
            Confirm New Password
          </label>
          <Password id="confirmPassword" v-model="confirmPassword" :feedback="false" toggleMask fluid />
        </div>
        <div class="flex items-center justify-between">
          <Button label="Change Password" @click="submit" class="w-full"  severity="danger"></Button>
        </div>
        <div class="flex items-center justify-between  mt-4">
          <Button label="취소" @click="router.push('/')" class="w-full" severity="secondary"></Button>
        </div>
      </div>
    </div>
  </div>
  <Toast></Toast>
</template>
