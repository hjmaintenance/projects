<script setup>
  import { ref, watch } from 'vue';
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

  const op = ref();

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
        toast.add({ severity: 'info', summary: 'Confirmed', detail: 'You have accepted', life: 3000 });
        logout();
      },
      reject: () => {
        toast.add({ severity: 'error', summary: 'Rejected', detail: 'You have rejected', life: 3000 });
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
    op.value.toggle(event);
  };
</script>

<template>
  <div class="cursor-pointer" @click="toggle">
    <button type="button" class="layout-topbar-action" :title="loginUser.name" aria-haspopup="true" aria-controls="overlay_tmenu">
      <i class="pi pi-user"></i>
    </button>
    <span>{{ loginUser?.affiliation }} - {{ loginUser?.user_name }}</span>
  </div>

  <Popover ref="op">
    <div class="flex flex-col gap-4 w-[25rem]">
      <div>
        <h6>{{ loginUser?.user_name }}</h6>
      </div>
      <div>
        <Button @click="router.push('/auth/change-password');">비번변경</Button>
      </div>
      <div>
        <Button @click="router.push('/profile');">프로필</Button>
      </div>
      <div>
        <button @click="requireConfirmation">logout</button>
      </div>
    </div>
  </Popover>
</template>
