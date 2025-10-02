<script setup>
import { ref, onMounted, computed, defineProps, watch } from 'vue';
import { RequestService } from '@/service/RequestService'; // Assuming this is the correct path
import { useLayout } from '@/layout/composables/layout';
import CommentItem from './CommentItem.vue';
import CommentForm from './CommentForm.vue';

const props = defineProps({
  requestId: {
    type: [Number, String],
    required: true
  }
});

const { loginUser } = useLayout();
const comments = ref([]);
const loading = ref(false);

const fetchComments = async () => {
  if (!props.requestId) return;
  loading.value = true;
  try {
    // This service method needs to be created.
    const flatComments = await RequestService.getComments(props.requestId);
    comments.value = flatComments;
  } catch (error) {
    console.error('Error fetching comments:', error);
    comments.value = []; // Clear on error
  } finally {
    loading.value = false;
  }
};

// Turn the flat list of comments into a nested tree
const commentTree = computed(() => {
  const map = {};
  const roots = [];
  
  comments.value.forEach(comment => {
    map[comment.id] = { ...comment, children: [] };
  });

  comments.value.forEach(comment => {
    if (comment.parentCommentId) {
      if (map[comment.parentCommentId]) {
        map[comment.parentCommentId].children.push(map[comment.id]);
      }
    } else {
      roots.push(map[comment.id]);
    }
  });

  // Sort comments by creation date
  const sortByDate = (a, b) => new Date(a.createdAt) - new Date(b.createdAt);
  roots.sort(sortByDate);
  Object.values(map).forEach(comment => {
      comment.children.sort(sortByDate);
  });

  return roots;
});

const handleCommentSubmitted = async (text) => {
  await submitComment({ text, parentId: null });
};

const handleReplySubmitted = async ({ text, parentId }) => {
  await submitComment({ text, parentId });
};

const submitComment = async ({ text, parentId }) => {
  if (!loginUser.value || !props.requestId) {
    console.error('User not logged in or requestId not provided');
    return;
  }

  const commentData = {
    requestId: props.requestId,
    commentText: text,
    parentCommentId: parentId,
    // Backend should handle AuthorId, AuthorType, CreatedBy from the authenticated user context
    // createdBy: loginUser.value.user_name, 
    // authorId: loginUser.value.user_uid,
  };

  try {
    // This service method needs to be created.
    await RequestService.addComment(commentData);
    // Refresh the comments list to show the new comment
    await fetchComments();
  } catch (error) {
    console.error('Error submitting comment:', error);
  }
};

watch(() => props.requestId, (newId) => {
  if (newId) {
    fetchComments();
  }
}, { immediate: true });

</script>

<template>
  <div class="comments-section card mt-4">


      <div class="new-comment-form">
        <h5 class="mb-3">Add a new comment</h5>
        <CommentForm @submit-comment="handleCommentSubmitted" />
      </div>

      
    <h4 class="mb-4">Comments ({{ comments.length }})</h4>
    
    <div v-if="loading">Loading comments...</div>

    <div v-else>
      <div v-if="commentTree.length > 0" class="comment-list">
        <CommentItem 
          v-for="comment in commentTree" 
          :key="comment.id" 
          :comment="comment"
          @submit-reply="handleReplySubmitted"
        />
      </div>
      <div v-else class="text-center text-gray-500 py-4">
        No comments yet. Be the first to comment!
      </div>

    </div>
  </div>
</template>

<style scoped>
.comments-section {
  padding: 1.5rem;
}
</style>
