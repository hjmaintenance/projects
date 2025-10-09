<script setup>
import AppMenu from './AppMenu.vue';
  import { useUiStore } from '@/store/ui';

  const uiStore = useUiStore();
</script>

<template>
    <div class="layout-sidebar flex flex-col">
        <div class="overflow-y-auto flex-grow">
            <app-menu></app-menu>
        </div>
        <div class="p-3 mt-auto">
            <a @click="uiStore.requestLogoutConfirmation()" class="flex items-center gap-2 cursor-pointer p-3 rounded-md text-surface-700 dark:text-surface-0/80 hover:bg-surface-100 dark:hover:bg-surface-700/80 transition-colors duration-200">
                <i class="pi pi-sign-out text-xl"></i>
                <span class="font-semibold">Log Out</span>
            </a>
        </div>
    </div>
</template>

<style lang="scss" scoped></style>
