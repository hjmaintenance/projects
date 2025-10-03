<script setup>
import { ref, onMounted } from 'vue';
import { useLayout } from '@/layout/composables/layout';
import { useToast } from 'primevue/usetoast';



import { AdminService } from '@/service/AdminService';


import { CustomerService } from '@/service/CustomerService';

const { loginUser } = useLayout();
const toast = useToast();

const user = ref({});

onMounted(async () => {
  try {
    const { login_type, user_uid} = loginUser.value;
    let response;


    if (login_type === 'admin') {
      response = await AdminService.get(user_uid);
    } else {
      response = await CustomerService.get(user_uid);
    }


    if (response) {
      user.value = await response;

      console.log('user: ', user);

    } else {
      toast.add({ severity: 'error', summary: 'Error', detail: 'Failed to fetch profile', life: 3000 });
    }

  } catch (error) {
    console.error('Failed to fetch profile:', error);
    toast.add({ severity: 'error', summary: 'Error', detail: 'Failed to fetch profile', life: 3000 });
  }
});

const saveProfile = async () => {
  try {
    const { login_type, user_uid} = loginUser.value;

    let response;

    if (login_type === 'admin') {
      response = await AdminService.update(user.value);
    } else {
      response = await CustomerService.update(user.value);
    }
console.log('response: ', response);
    if (response.success === true) {
      toast.add({ severity: 'success', summary: 'Success', detail: 'Profile updated successfully', life: 3000 });
    } else {
      toast.add({ severity: 'error', summary: 'Error', detail: 'Failed to update profile', life: 3000 });
    }
  } catch (error) {
    console.error('Failed to save profile:', error);
    toast.add({ severity: 'error', summary: 'Error', detail: 'Failed to update profile', life: 3000 });
  }
};
</script>

<template>
  <div class="card">
    <h5>Profile</h5>
    <div class="p-fluid formgrid grid gap-y-10">
      <FloatLabel>
          <InputText id="username" v-model="user.userName" />
          <label for="username">User Name</label>
      </FloatLabel>



      <FloatLabel>
          <InputText id="email" v-model="user.email" />
          <label for="email">Email</label>
      </FloatLabel>


      <FloatLabel>
          <InputText id="photo" v-model="user.photo" />
          <label for="photo">Photo</label>
      </FloatLabel>



      

      <!-- Admin specific fields -->
      <template v-if="loginUser.login_type === 'admin'">



      </template>

      <!-- Customer specific fields -->
      <template v-else>


      <FloatLabel>
          <InputText id="sex" v-model="user.sex" />
          <label for="sex">Sex</label>
      </FloatLabel>





      </template>

      <div class="field col-12">
        <Button label="Save" @click="saveProfile"></Button>
      </div>
    </div>
  </div>
</template>
