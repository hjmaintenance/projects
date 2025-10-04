<script setup>
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import { CustomerService } from '@/service/CustomerService';
import RadioButton from 'primevue/radiobutton';
import Textarea from 'primevue/textarea';

const router = useRouter();

const loginId = ref('');
const userName = ref('');
const email = ref('');
const password = ref('');
const passwordConfirm = ref('');
const sex = ref('M');
const remake = ref('');

const errorMsg = ref('');
const successMsg = ref('');

const submit = async () => {
  errorMsg.value = '';
  successMsg.value = '';

  if (password.value !== passwordConfirm.value) {
    errorMsg.value = '비밀번호가 일치하지 않습니다.';
    return;
  }

  try {
    const customerData = {
      loginId: loginId.value,
      userName: userName.value,
      email: email.value,
      password: password.value,
      sex: sex.value,
      companyId: 2, // 회사 ID 2번으로 고정
      remake: remake.value
    };

    await CustomerService.register(customerData);
    
    successMsg.value = '회원가입 요청이 완료되었습니다. 관리자 승인 후 로그인이 가능합니다.';
    setTimeout(() => {
        router.push('/auth/login');
    }, 3000);

  } catch (err) {
    errorMsg.value = (err.response && err.response.data && err.response.data.message) || '회원가입 중 오류가 발생했습니다.';
  }
};
</script>

<template>
  <div class="bg-surface-50 dark:bg-surface-950 flex items-center justify-center min-h-screen min-w-[100vw] overflow-hidden">
    <div class="flex flex-col items-center justify-center">
      <div style="border-radius: 56px; padding: 0.3rem; background: linear-gradient(180deg, var(--primary-color) 10%, rgba(33, 150, 243, 0) 30%)">
        <div class="w-full bg-surface-0 dark:bg-surface-900 py-12 px-8 sm:px-20" style="border-radius: 53px">
          <div class="text-center mb-6">
            <div class="text-surface-900 dark:text-surface-0 text-3xl font-medium mb-4">회원가입</div>
            <span class="text-muted-color font-medium">Create your account</span>
          </div>

          <form @submit.prevent="submit">
            <div class="space-y-6">
              <div>
                <label for="loginId" class="block text-surface-900 dark:text-surface-0 text-xl font-medium mb-2">아이디</label>
                <InputText id="loginId" type="text" placeholder="Login ID" class="w-full md:w-[30rem]" v-model="loginId" required />
              </div>

              <div>
                <label for="userName" class="block text-surface-900 dark:text-surface-0 text-xl font-medium mb-2">이름</label>
                <InputText id="userName" type="text" placeholder="User Name" class="w-full md:w-[30rem]" v-model="userName" required />
              </div>

              <div>
                <label for="email" class="block text-surface-900 dark:text-surface-0 text-xl font-medium mb-2">이메일</label>
                <InputText id="email" type="email" placeholder="Email" class="w-full md:w-[30rem]" v-model="email" required />
              </div>

              <div>
                <label for="password" class="block text-surface-900 dark:text-surface-0 font-medium text-xl mb-2">비밀번호</label>
                <Password id="password" v-model="password" placeholder="Password" :toggleMask="true" class="w-full" fluid :feedback="false" required></Password>
              </div>

              <div>
                <label for="passwordConfirm" class="block text-surface-900 dark:text-surface-0 font-medium text-xl mb-2">비밀번호 확인</label>
                <Password id="passwordConfirm" v-model="passwordConfirm" placeholder="Confirm Password" :toggleMask="true" class="w-full" fluid :feedback="false" required></Password>
              </div>

              <div>
                  <label class="block text-surface-900 dark:text-surface-0 font-medium text-xl mb-2">성별</label>
                  <div class="flex items-center">
                      <div class="flex items-center mr-4">
                          <RadioButton id="sexM" name="sex" value="M" v-model="sex" />
                          <label for="sexM" class="ml-2">남자</label>
                      </div>
                      <div class="flex items-center">
                          <RadioButton id="sexF" name="sex" value="F" v-model="sex" />
                          <label for="sexF" class="ml-2">여자</label>
                      </div>
                  </div>
              </div>

              <div>
                <label for="remake" class="block text-surface-900 dark:text-surface-0 text-xl font-medium mb-2">비고</label>
                <Textarea id="remake" placeholder="비고" :autoResize="true" rows="3" class="w-full md:w-[30rem]" v-model="remake" />
              </div>
            </div>

            <div v-if="errorMsg" class="mt-4 text-red-500">{{ errorMsg }}</div>
            <div v-if="successMsg" class="mt-4 text-green-500">{{ successMsg }}</div>

            <Button label="회원가입" class="w-full mt-8" type="submit"></Button>

            <div class="text-center mt-4">
                <router-link to="/auth/login" class="font-medium no-underline cursor-pointer text-primary">이미 계정이 있으신가요? 로그인</router-link>
            </div>
          </form>
        </div>
      </div>
    </div>
  </div>
</template>