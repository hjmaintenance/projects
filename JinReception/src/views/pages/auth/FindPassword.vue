<script setup>
  import { ref } from 'vue';
  import { AdminService } from '@/service/AdminService';

  const loginId = ref('');
  const email = ref('');
  const newPassword = ref('');
  const errorMsg = ref('');
  const successMsg = ref('');

  const findPassword = async () => {
    errorMsg.value = '';
    successMsg.value = '';
    newPassword.value = '';

    try {
      const response = await AdminService.findPassword({ 
        loginId: loginId.value, 
        email: email.value 
      });
        
      if (response.data.data.tempPassword) {
        newPassword.value = response.data.data.tempPassword;
        successMsg.value = '새로운 임시 비밀번호가 발급되었습니다. 로그인 후 비밀번호를 변경해주세요.';
      } else {
        errorMsg.value = '일치하는 사용자가 없습니다. 아이디와 이메일을 확인해주세요.';
      }
    } catch (err) {
      errorMsg.value = '존재하지 않는 아이디입니다. 확인후 다시 시도해주세요.';
    }
  };
</script>

<template>
  <div class="bg-surface-50 dark:bg-surface-950 flex items-center justify-center min-h-screen">
    <div class="flex flex-col items-center justify-center">
      <div style="border-radius: 56px; padding: 0.3rem; background: linear-gradient(180deg, var(--primary-color) 10%, rgba(33, 150, 243, 0) 30%)">
        <div class="w-full bg-surface-0 dark:bg-surface-900 py-20 px-8 sm:px-20" style="border-radius: 53px">
          <div class="text-center mb-8">
            <div class="text-surface-900 dark:text-surface-0 text-3xl font-medium mb-4">비밀번호 찾기</div>
            <span class="text-muted-color font-medium">아이디와 이메일을 입력하세요</span>
          </div>

          <form @submit.prevent="findPassword">
            <label for="loginId" class="block text-surface-900 dark:text-surface-0 text-xl font-medium mb-2">아이디</label>
            <InputText id="loginId" type="text" placeholder="Login ID" class="w-full md:w-[30rem] mb-8" v-model="loginId" />

            <label for="email" class="block text-surface-900 dark:text-surface-0 font-medium text-xl mb-2">이메일</label>
            <InputText id="email" type="email" placeholder="Email" class="w-full md:w-[30rem] mb-4" v-model="email" />

            <Button type="submit" label="찾기" class="w-full mt-4"></Button>
          </form>

          <div v-if="successMsg" class="mt-4 p-4 bg-green-100 border border-green-400 text-green-700 rounded">
            <p>{{ successMsg }}</p>
            <p class="font-bold text-lg">임시 비밀번호: {{ newPassword }}</p>
          </div>

          <div v-if="errorMsg" class="mt-4 p-4 bg-red-100 border border-red-400 text-red-700 rounded">
            {{ errorMsg }}
          </div>

          <div class="mt-8 text-center">
            <router-link to="/auth/login" class="font-medium no-underline cursor-pointer text-primary">로그인으로 돌아가기</router-link>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
