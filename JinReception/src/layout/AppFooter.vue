<script setup>
import { ref, onMounted } from 'vue';


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




onMounted(async () => {
  await loadVersion()
  //const res = await fetch('/api/version')
  //const data = await res.json()
  //console.log("Backend reported version:", data.version)
})

</script>

<template>
    <div class="layout-footer">
        Jin114 helper by
        <a href="//jin114.co.kr" target="_blank" rel="noopener noreferrer" class="text-primary font-bold hover:underline">Jsini</a>
        <span>Release v{{ releaseVersion }}</span>
    </div>
</template>
