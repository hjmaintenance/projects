<script setup>
import { defineProps, ref, defineEmits } from 'vue';
import { formatDate } from '@/utils/formatters';
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
    <div class="comment-header flex justify-between items-center mb-2">
      <span class="font-bold text-sm">{{ comment.createdBy }}</span>
      <span class="text-xs text-gray-500">{{ formatDate(new Date(comment.createdAt)) }}</span>
    </div>
    <div class="comment-body mb-2">
      <p class="text-base">{{ comment.commentText }}</p>
    </div>
    <div class="comment-actions">
      <Button 
        type="button" 
        label="Reply" 
        class="p-button-sm p-button-text" 
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
