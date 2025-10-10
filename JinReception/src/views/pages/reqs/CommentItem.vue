<script setup>
import { defineProps, ref, defineEmits } from 'vue';
import { formatDate, formatDateSmart } from '@/utils/formatters';
import CommentForm from './CommentForm.vue';

const props = defineProps({
  comment: {
    type: Object,
    required: true
  }
});

const emit = defineEmits(['submit-reply']);

const showReplyForm = ref(false);

const handleReplySubmitted = (commentText) => {
  emit('submit-reply', { 
    text: commentText, 
    parentId: props.comment.id 
  });
  showReplyForm.value = false;
};
</script>

<template>
  <div class="comment-item py-3">




























<div class="flex items-start gap-3">
  <!-- 아바타 -->
 
                                <Avatar v-if=" comment?.author?.photo" :image="comment?.author?.photo" class="mr-3" size="large" shape="circle" />
                                <Avatar v-else :label="comment?.author?.userName.charAt(0).toUpperCase()"  class="mr-3" size="large" shape="circle" />



  <!-- 내용 영역 -->
  <div class="flex flex-col">
    <!-- 작성자 + 말풍선 -->
    <div class="bg-gray-100 px-4 py-2 max-w-md relative">
      <p class="text-sm text-gray-900"  v-html="comment?.commentText">
      </p>
      <!-- 풍선 꼬리 -->
      <div class="absolute -left-2 top-3 w-0 h-0 border-t-8 border-t-transparent border-r-8 border-r-gray-100 border-b-8 border-b-transparent"></div>
    </div>

    <!-- 작성 시간 -->
    <span class="text-xs text-gray-500 mt-1">{{ formatDateSmart(new Date(comment.createdAt)) }}</span>
  </div>
</div>




    <div class="comment-actions">
      <Button 
        type="button" 
        label="Reply" 
        class="p-button-sm p-button-text " 
        @click="showReplyForm = !showReplyForm"
      ></Button>
    </div>

    <div v-if="showReplyForm" class="reply-form mt-3 pl-6">
      <CommentForm @submit-comment="handleReplySubmitted" />
    </div>

    <div v-if="comment.children && comment.children.length > 0" class="nested-comments pl-6 border-l-2 border-gray-200 mt-3">
      <CommentItem
        v-for="child in comment.children"
        :key="child.id"
        :comment="child"
        @submit-reply="(payload) => emit('submit-reply', payload)"
      />
    </div>
  </div>
</template>

<style scoped>
.comment-item {
  /* Adding a bottom border for separation */
  border-bottom: 1px solid #eee;
}
.comment-item:last-child {
  border-bottom: none;
}
</style>
