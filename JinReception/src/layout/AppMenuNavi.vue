<script setup>
  import { useLayout } from '@/layout/composables/layout';
  import { computed, ref, watch, onMounted } from 'vue';

  import { $t, updatePreset, updateSurfacePalette } from '@primeuix/themes';

  import AppFooter from './AppFooter.vue';
  import AppSidebar from './AppSidebar.vue';
  import AppTopbar from './AppTopbar.vue';

  import Aura from '@primeuix/themes/aura';
  import Lara from '@primeuix/themes/lara';
  import Nora from '@primeuix/themes/nora';


  const home = ref({
    icon: 'pi pi-home'
});
const items = ref([
    { label: 'Electronics' },
    { label: 'Computer' },
    { label: 'Accessories' },
    { label: 'Keyboard' },
    { label: 'Wireless' }
]);
</script>

<template>
  
        <div class="flex justify-start card">
          <Breadcrumb :home="home" :model="items" />
        </div>
</template>
