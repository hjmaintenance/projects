<script setup>
  import AppMenuItem from './AppMenuItem.vue';
  import { useMenuStore } from '@/store/menu';
  import { storeToRefs } from 'pinia';

  const menuStore = useMenuStore();
  const { model } = storeToRefs(menuStore);
</script>

<template>
  <ul class="layout-menu">
    <template v-for="(item, i) in model" :key="item">
      <app-menu-item v-if="!item.separator" :item="item" :index="i"></app-menu-item>
      <li v-if="item.separator" class="menu-separator"></li>
    </template>
  </ul>
</template>

<style lang="scss" scoped></style>
