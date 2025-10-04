<script setup>
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import { AuthService } from '@/service/AuthService';
import { useLayout } from '@/layout/composables/layout';

const oldPassword = ref('');
const newPassword = ref('');
const confirmPassword = ref('');
const errorMsg = ref('');
const successMsg = ref('');

const router = useRouter();
const { setLoginUser } = useLayout();

const submit = async () => {
  errorMsg.value = '';
  successMsg.value = '';

  if (newPassword.value !== confirmPassword.value) {
    errorMsg.value = 'New passwords do not match.';
    return;
  }

  try {
    await AuthService.changePassword({ 
      OldPassword: oldPassword.value, 
      NewPassword: newPassword.value 
    });

    successMsg.value = 'Password changed successfully. Please log in again.';
    router.push('/');
/*
    // Log out user
    localStorage.removeItem('jwt_token');
    setLoginUser(null);

    setTimeout(() => {
      router.push('/auth/login');
    }, 2000);
    */

  } catch (err) {
    errorMsg.value = err.response?.data || err.message;
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
        <div v-if="errorMsg" class="text-red-500 text-center mt-4">{{ errorMsg }}</div>
        <div v-if="successMsg" class="text-green-500 text-center mt-4">{{ successMsg }}</div>
      </div>
    </div>
  </div>
</template>
