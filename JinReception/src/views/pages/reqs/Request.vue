<script setup>
  import { useLayout } from '@/layout/composables/layout';
  import { RequestService } from '@/service/RequestService';
  import { reactive, ref, onMounted, onBeforeUnmount } from 'vue';
  import { Editor, EditorContent, useEditor } from '@tiptap/vue-3';
  import { useRouter } from 'vue-router';

  import StarterKit from '@tiptap/starter-kit';
  import { TextStyle } from '@tiptap/extension-text-style';
  import { Color } from '@tiptap/extension-color';
  import ListItem from '@tiptap/extension-list-item';
  import Image from '@tiptap/extension-image';

  const { loginUser } = useLayout();
  const router = useRouter();

  const editor = useEditor({
    extensions: [Color.configure({ types: [TextStyle.name, ListItem.name] }), TextStyle.configure({ types: [ListItem.name] }), StarterKit, Image],
    content: `
<h2>요점 내용</h2><p/><p/><p/>
<p>상세한 설명을 여기에 작성하세요.</p><p/>
<p>이미지를 붙여넣기 해보세요.</p><p/>
<blockquote>programming is fun!<br />
— quristyle</blockquote><p/><p/><p/><p/><p/><p/><p/>`
  });

  const filesToUpload = ref([]);

  const onFileSelect = (event) => {
    filesToUpload.value = event.files;
  };

  onMounted(() => {
    document.addEventListener('paste', handlePaste);
  });

  onBeforeUnmount(() => {
    document.removeEventListener('paste', handlePaste);
    editor.value?.destroy();
  });

  const request = reactive({
    title: '',
    description: ''
  });

  const save = async () => {
    if (!loginUser.value || !loginUser.value.user_uid) {
      alert('사용자 정보가 없습니다. 다시 로그인해주세요.');
      return;
    }

    const html = editor.value.getHTML();
    request.description = html;

    const formData = new FormData();
    formData.append('title', request.title);
    formData.append('description', request.description);
    formData.append('customerId', loginUser.value.user_uid);

    filesToUpload.value.forEach((file) => {
      formData.append('files', file);
    });

    await RequestService.addWithAttachments(formData);
    router.push('/user_request');
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
  <div class="card srcharea">
    <div class="flex flex-wrap items-start gap-4">
      <div class="field">
        <InputText type="text" v-model="request.title" placeholder="Title..." />
      </div>
      <Button label="요청" type="button" class="mr-2" @click="save" />
    </div>
  </div>

  <div class="card">
    <!-- 본문 작성 editor -->
    <editor-content :editor="editor" />

    <!-- 첨부파일 업로드 -->
    <FileUpload name="files[]" @select="onFileSelect" :multiple="true" :maxFileSize="10000000" :showUploadButton="false" :showCancelButton="false">
      <template #empty>
        <p>이곳을 클릭하거나 파일을 드래그하여 첨부하세요.</p>
      </template>
    </FileUpload>
  </div>
</template>
