<script setup>
import { ref, onMounted } from 'vue';
import { ReleaseService } from '@/service/ReleaseService';
import { useToast } from 'primevue/usetoast';

const loading = ref(null);
const releaseVersion = ref('');
const buildLog = ref([]);
const toast = useToast();

const loadVersion = async () => {
  try {
    const res = await fetch('/version.json?_=' + Date.now()); // 캐시 방지
    const data = await res.json();
    releaseVersion.value = data.version;
  } catch (err) {
    console.error('버전 로드 실패:', err);
    releaseVersion.value = 'N/A';
  }
};

// 초기 1회 로드
onMounted(() => {
  loadVersion();
});

const buildNRelease = async () => {

  ReleaseService.build(loading);
  
  buildLog.value = [];
  loading.value = true;

  const steps = [
    { name: 'source get', duration: 1000 },
    { name: 'front build', duration: 3000 },
    { name: 'front publish', duration: 1500 },
    { name: 'backend build', duration: 2500 },
    { name: 'backend restart', duration: 2000 }
  ];

  try {
    // 실제 빌드 서비스 호출
    // await ReleaseService.build();

    // UI/UX 확인을 위한 빌드 과정 시뮬레이션
    for (const step of steps) {
      buildLog.value.push({ message: `[INFO] Starting: ${step.name}...`, status: 'info' });
      await new Promise((resolve) => setTimeout(resolve, step.duration));
      buildLog.value.push({ message: `[SUCCESS] Completed: ${step.name}`, status: 'success' });
    }

    toast.add({ severity: 'success', summary: '배포 성공', detail: '새 버전이 성공적으로 배포되었습니다.', life: 3000 });
    await loadVersion();
  } catch (error) {
    console.error('Build and release failed:', error);
    buildLog.value.push({ message: `[ERROR] Build failed: ${error.message}`, status: 'error' });
    toast.add({ severity: 'error', summary: '배포 실패', detail: '배포 중 오류가 발생했습니다.', life: 3000 });
  } finally {
    loading.value = false;
  }
};

const reload = async () => {
  await loadVersion();
  toast.add({ severity: 'info', summary: '버전 확인', detail: `현재 버전은 v${releaseVersion.value} 입니다.`, life: 3000 });
};
</script>

<template>
  <div class="card">
    <div class="flex flex-col md:flex-row md:justify-between md:items-center mb-6 gap-4">
      <h5 class="text-2xl font-bold m-0">릴리즈 도구</h5>
      <div class="flex items-center gap-2">
        <span class="text-lg font-medium text-surface-500 dark:text-surface-400">현재 버전:</span>
        <Tag severity="success" :value="releaseVersion ? 'v' + releaseVersion : 'Loading...'"></Tag>
        <Button icon="pi pi-refresh" rounded text @click="reload" v-tooltip.bottom="'버전 새로고침'"></Button>
      </div>
    </div>

    <div class="mb-6">
      <Button @click="buildNRelease" label="빌드 및 배포" icon="pi pi-cloud-upload" :loading="loading" class="w-full md:w-auto"></Button>
    </div>

    <div>
      <h6 class="text-xl font-semibold mb-3">진행 과정</h6>
      <div class="p-4 bg-surface-900 text-surface-0 dark:bg-surface-800 dark:text-surface-0/80 rounded-md font-mono text-sm min-h-[15rem] overflow-y-auto">
        <div v-if="buildLog.length === 0" class="text-surface-500">배포를 시작하면 로그가 표시됩니다.</div>
        <div v-for="(log, index) in buildLog" :key="index" :class="{
          'text-green-400': log.status === 'success',
          'text-red-400': log.status === 'error',
          'text-blue-400': log.status === 'info'
        }">
          {{ log.message }}
        </div>
      </div>
    </div>
  </div>
</template>
