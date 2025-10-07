<script setup>
  import { ref, computed } from 'vue';
  import { storeToRefs } from 'pinia';
  import { useBreadcrumbStore } from '@/store/breadcrumb';

  const home = ref({
    icon: 'pi pi-home',
    to: '/'
  });

  const breadcrumbStore = useBreadcrumbStore();
  const { items } = storeToRefs(breadcrumbStore);

  const lastBreadcrumbLabel = computed(() => {
    if (items.value && items.value.length > 0) {
      return items.value[items.value.length - 1].label;
    }
    return 'Reception'; // Default value
  });
</script>

<template>


<router-link to="/" class="layout-topbar-logo">
        <span>{{ lastBreadcrumbLabel }}</span>




        <span style="font-size: 0.8rem; width: 77rem; padding-top: 0.3rem" class="hidden md:block" >
        

  <div class="flex justify-start ">
    <Breadcrumb :home="home" :model="items" />
  </div>


  </span>

</router-link>



</template>