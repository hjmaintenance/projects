<script setup>
import { ref, defineEmits } from 'vue';

const emit = defineEmits(['submit-comment']);
const commentText = ref('');

const handleSubmit = () => {
  if (!commentText.value.trim()) return;
  emit('submit-comment', commentText.value);
  commentText.value = '';
};
</script>

<template>
  <div class="comment-form flex flex-col gap-2">
    <Textarea v-model="commentText" rows="3" placeholder="Add a comment..." class="w-full"></Textarea>
    <div class="flex justify-end">
      <Button label="Submit" @click="handleSubmit"></Button>
    </div>
  </div>
</template>
