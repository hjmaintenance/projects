<script setup>
  import { ref } from 'vue';
  import { storeToRefs } from 'pinia';
  import { useBreadcrumbStore } from '@/store/breadcrumb';

  const home = ref({
    icon: 'pi pi-home',
    to: '/'
  });

  const breadcrumbStore = useBreadcrumbStore();
  const { items } = storeToRefs(breadcrumbStore);
</script>

<template>
  <div class="flex justify-start card">
    <Breadcrumb :home="home" :model="items" />
  </div>
</template>
