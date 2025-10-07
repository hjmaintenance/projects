<script setup>
  import { ref } from 'vue';
  import { useToast } from 'primevue/usetoast';
  import { useRouter } from 'vue-router';
  import { useLayout } from '@/layout/composables/layout';
  import AppConfigurator from './AppConfigurator.vue';
  import UserConfigurator from './UserConfigurator.vue';  
  import AppMenuNavi from './AppMenuNavi.vue';

  import { useConfirm } from 'primevue/useconfirm';

  const confirm = useConfirm();

  const toast = useToast();
  const { toggleMenu, toggleDarkMode, isDarkTheme, loginUser, clearLoginUser, initLoginUser } = useLayout();
  const router = useRouter();
  const menu = ref();

  const logout = async () => {
    try {
      clearLoginUser();
      router.push('/auth/login');
    } catch (err) {
      console.error('Logout failed:', err);
    }
  };

  const toggle = (event) => {
    menu.value.toggle(event);
  };
</script>

<template>
  <div class="layout-topbar">
    <div class="layout-topbar-logo-container">
      <button class="layout-menu-button layout-topbar-action" @click="toggleMenu">
        <i class="pi pi-bars"></i>
      </button>
     <app-menu-navi></app-menu-navi>
    </div>

    <div class="layout-topbar-actions">
      <div class="layout-config-menu">

        <button type="button" class="layout-topbar-action" @click="toggleDarkMode">
          <i :class="['pi', { 'pi-moon': isDarkTheme, 'pi-sun': !isDarkTheme }]"></i>
        </button>

        <div class="relative">
          <button
            v-styleclass="{ selector: '@next', enterFromClass: 'hidden', enterActiveClass: 'animate-scalein', leaveToClass: 'hidden', leaveActiveClass: 'animate-fadeout', hideOnOutsideClick: true }"
            type="button"
            class="layout-topbar-action layout-topbar-action-highlight"
          >
            <i class="pi pi-palette"></i>
          </button>
          <AppConfigurator />
        </div>

      </div>

      <button
        class="layout-topbar-menu-button layout-topbar-action"
        v-styleclass="{ selector: '@next', enterFromClass: 'hidden', enterActiveClass: 'animate-scalein', leaveToClass: 'hidden', leaveActiveClass: 'animate-fadeout', hideOnOutsideClick: true }"
      >
        <i class="pi pi-ellipsis-v"></i>
      </button>

      <div class="layout-topbar-menu hidden lg:block">
        <div class="layout-topbar-menu-content">

          <button type="button" class="layout-topbar-action">
            <i class="pi pi-calendar"></i>
            <span>Calendar</span>
          </button>

          <button type="button" class="layout-topbar-action">
            <i class="pi pi-inbox"></i>
            <span>Messages</span>
          </button>

        </div>
      </div>



      <div v-if="loginUser" class="layout-config-menu ">
        <div class="relative">
          <ConfirmDialog group="headless">
            <template #container="{ message, acceptCallback, rejectCallback }">
              <div class="flex flex-col items-center p-8 bg-surface-0 dark:bg-surface-900 rounded">
                <div class="rounded-full bg-primary text-primary-contrast inline-flex justify-center items-center h-24 w-24 -mt-20">
                  <i class="pi pi-question !text-4xl"></i>
                </div>
                <span class="font-bold text-2xl block mb-2 mt-6">{{ message.header }}</span>
                <p class="mb-0">{{ message.message }}</p>
                <div class="flex items-center gap-2 mt-6">
                  <Button label="Quit" @click="acceptCallback"></Button>
                  <Button label="Cancel" variant="outlined" @click="rejectCallback"></Button>
                </div>
              </div>
            </template>
          </ConfirmDialog>

          <UserConfigurator />
        </div>
      </div>




    </div>
  </div>
</template>
