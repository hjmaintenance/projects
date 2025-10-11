<script setup>
  import { useLayout } from '@/layout/composables/layout';
  import { RequestService } from '@/service/RequestService';
  import { reactive, ref, onMounted, onBeforeUnmount } from 'vue';
  import { Editor, EditorContent, useEditor } from '@tiptap/vue-3';
  import { useRouter, useRoute } from 'vue-router';

  import StarterKit from '@tiptap/starter-kit';
  import { TextStyle } from '@tiptap/extension-text-style';
  import { Color } from '@tiptap/extension-color';
  import ListItem from '@tiptap/extension-list-item';
  import Image from '@tiptap/extension-image';

  const { loginUser } = useLayout();
  const router = useRouter();
  const route = useRoute();

  const editor = useEditor({
    extensions: [Color.configure({ types: [TextStyle.name, ListItem.name] }), TextStyle.configure({ types: [ListItem.name] }), StarterKit, Image],
    content: ''
  });

  const filesToUpload = ref([]);
  const existingFiles = ref([]);
  const deletedFiles = ref([]);

  const onFileSelect = (event) => {
    filesToUpload.value = event.files;
  };

  const deleteFile = (fileId) => {
    deletedFiles.value.push(fileId);
    existingFiles.value = existingFiles.value.filter(f => f.id !== fileId);
  }

  onMounted(async () => {
    document.addEventListener('paste', handlePaste);
    const requestId = route.params.id;
    if (requestId) {
      const requestData = await RequestService.get(requestId);

      if (requestData.customerId !== loginUser.value.user_uid) {
        alert('작성자만 수정할 수 있습니다.');
        router.back();
        return;
      }

      request.title = requestData.title;
      editor.value.commands.setContent(requestData.description);
      existingFiles.value = requestData.attachments;
    }
  });

  onBeforeUnmount(() => {
    document.removeEventListener('paste', handlePaste);
    editor.value?.destroy();
  });

  const request = reactive({
    title: '',
    description: ''
  });

  const update = async () => {
    if (!loginUser.value || !loginUser.value.user_uid) {
      alert('사용자 정보가 없습니다. 다시 로그인해주세요.');
      return;
    }

    const html = editor.value.getHTML();
    request.description = html;

    const formData = new FormData();
    formData.append('title', request.title);
    formData.append('description', request.description);
    
    filesToUpload.value.forEach((file) => {
      formData.append('files', file);
    });

    if (deletedFiles.value.length > 0) {
      formData.append('deletedFiles', JSON.stringify(deletedFiles.value));
    }

    const requestId = route.params.id;
    await RequestService.updateWithAttachments(requestId, formData);
    //router.push('/mng_request');
    router.back();
  };

  const handlePaste = (event) => {
    const clipboardItems = event.clipboardData?.items;
    if (!clipboardItems) return;

    for (let i = 0; i < clipboardItems.length; i++) {
      const item = clipboardItems[i];
      if (item.type.startsWith('image/')) {
        const file = item.getAsFile();
        const reader = new FileReader();

        reader.onload = (readerEvent) => {
          const base64 = readerEvent.target.result;
          if (editor && editor.value) {
            editor.value.chain().focus().setImage({ src: base64 }).run();
          }
        };

        reader.readAsDataURL(file);
        event.preventDefault();
      }
    }
  };
</script>

<template>
  <form class="card srcharea" @submit.prevent="search">
    <div class="flex flex-col sm:flex-row sm:items-center" >
      <label>제목</label>
      <InputText type="text" v-model="request.title" placeholder="Title..." class="ml-2" />
      <div class="flex flex-col md:flex-row justify-between md:items-center flex-1 gap-6">
        <div class="flex flex-row md:flex-col justify-between items-end gap-2">
        </div>
        <div></div>
        <div>                          
          <Button label="수정하기" type="button" class="mr-2" @click="update" />
          <Button label="취소" type="button" class="mr-2" @click="router.back()" />
        </div>
      </div>
    </div>
  </form>

  <div class="card">
    <!-- 본문 작성 editor -->
    <editor-content :editor="editor" />

    <!-- Existing Files -->
    <div v-if="existingFiles.length > 0">
        <h5>기존 첨부파일</h5>
        <ul>
            <li v-for="file in existingFiles" :key="file.id">
              {{ file.fileName }}
              <Button icon="pi pi-times" class="p-button-rounded p-button-danger p-button-text" @click="deleteFile(file.id)"></Button>
            </li>
        </ul>
    </div>

    <!-- 첨부파일 업로드 -->
    <FileUpload name="files[]" @select="onFileSelect" :multiple="true" :maxFileSize="10000000" :showUploadButton="false" :showCancelButton="false">
      <template #empty>
        <p>이곳을 클릭하거나 파일을 드래그하여 첨부하세요.</p>
      </template>
    </FileUpload>
  </div>
</template>
