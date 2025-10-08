<!-- MobileWriteFab.vue -->
<template>
  <!-- Teleport로 body에 렌더해서 다른 레이아웃에 가려지는 문제 방지 -->
  <teleport to="body">
    <!-- md 이상에서는 숨김: 모바일에서만 보임 -->
    <button
      @click="onClick"
      @keydown.enter.prevent="onClick"
      @keydown.space.prevent="onClick"
      type="button"
      :title="title"
      aria-label="글 작성"
      class="fixed right-4 bottom-[calc(env(safe-area-inset-bottom,1rem)+1rem)]  z-50 w-14 h-14 rounded-full flex items-center justify-center shadow-lg
             bg-primary-600 hover:bg-primary-700 active:scale-95 transform-gpu transition-all
             focus:outline-none focus:ring-4 focus:ring-primary-300
             text-white"
    >
      <!-- PrimeIcons 사용 -->
      <i class="pi pi-pencil text-lg"></i>
      <!-- 알림 뱃지 예시 (선택) -->
      <span v-if="badge" class="absolute -top-1 -right-1 bg-red-600 text-xs w-5 h-5 rounded-full flex items-center justify-center shadow-sm">
        {{ badge }}
      </span>
    </button>
  </teleport>
</template>

<script setup>
import { ref } from 'vue';
import { useRouter } from 'vue-router';

// props-like (원하면 부모에서 badge 전달 가능)
const badge = ref(null); // 예: "3" 등

const router = useRouter();
const title = '글 작성';

// 클릭 시 처리 — 예: 모달 오픈 또는 라우터 이동
function onClick() {
  // 예1: 라우터로 글쓰기 페이지로 이동
  // router.push({ name: 'Write' });

  // 예2: 전역 이벤트 / 상태로 모달 열기
  // emit('openCompose') 등 (부모/Pinia로 구현)

  // 여기서는 예시로 라우터 사용:
  router.push('/request');
}
</script>

<style scoped>
/* Tailwind로 대부분 스타일 처리. 필요시 추가 */
</style>
