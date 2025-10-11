<script setup>
  import { ref, watch, computed } from 'vue';
  import { useToast } from 'primevue/usetoast';
  import { useRouter } from 'vue-router';
  import { useLayout } from '@/layout/composables/layout';
  import { useConfirm } from 'primevue/useconfirm';
  import { useUiStore } from '@/store/ui';

  const confirm = useConfirm();
  const toast = useToast();
  const router = useRouter();
  const { loginUser, clearLoginUser } = useLayout();
  const uiStore = useUiStore();

  const menu = ref();

  const userInitials = computed(() => {
    if (!loginUser.value?.user_name) return '';
    return loginUser.value.user_name.charAt(0).toUpperCase();
  });

  const menuItems = ref([
    {
      label: '내 계정',
      items: [
        {
          label: '프로필',
          icon: 'pi pi-user',
          command: () => router.push('/profile')
        }
      ]
    },
    { separator: true },
    { label: '로그아웃', icon: 'pi pi-sign-out', command: () => requireConfirmation() }
  ]);

  const logout = async () => {
    try {
      clearLoginUser();
      router.push('/auth/login');
    } catch (err) {
      console.error('Logout failed:', err);
    }
  };

  const requireConfirmation = () => {
    confirm.require({
      group: 'headless',
      header: 'Are you sure?',
      message: 'want to quit?',
      accept: () => {
        //toast.add({ severity: 'info', summary: 'Confirmed', detail: 'You have accepted', life: 3000 });
        logout();
      },
      reject: () => {
        //toast.add({ severity: 'error', summary: 'Rejected', detail: 'You have rejected', life: 3000 });
      }
    });
  };

  watch(() => uiStore.showLogoutConfirmation, (newValue) => {
    if (newValue) {
      requireConfirmation();
      uiStore.clearLogoutConfirmation();
    }
  });

  const toggle = (event) => {
    menu.value.toggle(event);
  };
</script>

<template>
  <div class="cursor-pointer flex items-center gap-3" @click="toggle" aria-haspopup="true" aria-controls="overlay_menu">
    <Avatar v-if="loginUser?.photo" :image="loginUser.photo" shape="circle" />
    <Avatar v-else :label="userInitials" shape="circle" />
    <div class="hidden md:block">

      <div class="font-semibold text-sm">{{ loginUser?.user_name }}</div>
      <div class="text-xs text-surface-500 dark:text-surface-400">{{ loginUser?.affiliation }}</div>
    </div>
  </div>

  <Menu ref="menu" id="overlay_menu" :model="menuItems" :popup="true">
    <template #start>
      <div class="p-3">

      
        <div>
          <Avatar v-if="loginUser?.photo" :image="loginUser.photo" shape="circle" />
          <div  class="font-bold">{{ loginUser?.user_name }}</div>
        </div>
        <div class="text-sm text-surface-500 dark:text-surface-400">{{ loginUser?.email }}</div>
      </div>
    </template>
  </Menu>
</template>
