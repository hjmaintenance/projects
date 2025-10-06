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
<h2>주요 요구사항을 입력하세요.</h2>

<p>
  내용을 입력하세요.
</p>


<blockquote>해당 접수의 처리를 요청드립니다.<br />
— ${loginUser.value?.user_name} ( ${loginUser.value?.email} )
</blockquote><p/><p/><p/><p/>`
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
    router.push('/mng_request');
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
      <Button label="접수하기" type="button" class="mr-2" @click="save" />
      <Button label="접수목록보기" type="button" class="mr-2" @click="router.back()" />
                        </div>
                    </div>
                </div>


  </form>









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
