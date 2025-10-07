<script setup>
import { ref } from 'vue';
import { ReleaseService } from '@/service/ReleaseService';
const loading = ref(null);
const releaseVersion = ref('')

const loadVersion = async () => {
  try {
    const res = await fetch('/version.json?_=' + Date.now()) // 캐시 방지
    const data = await res.json()
    releaseVersion.value = data.version
  } catch (err) {
    console.error('버전 로드 실패:', err)
  }
}

// 초기 1회 로드
loadVersion()

const buildNRelease = async () => {
  ReleaseService.build(loading);
}

const reload = async () => {
  await loadVersion()
}
</script>

<template>
  <div class="card">
    <Button @click="buildNRelease">빌드 및 배포</Button>
    <Button @click="reload">버전 확인</Button>
  </div>
  <div class="card">
    <span>Release v{{ releaseVersion }}</span>
  </div>
</template>
