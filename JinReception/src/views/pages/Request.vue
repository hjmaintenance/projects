<script setup>
  import { RequestService } from '@/service/RequestService';
  import { reactive, ref, onMounted, onBeforeUnmount } from 'vue';
  import { Editor, EditorContent, useEditor } from '@tiptap/vue-3';

  import StarterKit from '@tiptap/starter-kit';
  import { TextStyle } from '@tiptap/extension-text-style';
  import { Color } from '@tiptap/extension-color';
  import ListItem from '@tiptap/extension-list-item';

  // import StarterKit from '@tiptap/starter-kit';
  import Image from '@tiptap/extension-image';

  const editor = useEditor({
    extensions: [Color.configure({ types: [TextStyle.name, ListItem.name] }), TextStyle.configure({ types: [ListItem.name] }), StarterKit, Image],
    content: `
      <h2>
        Hi there,
      </h2>
      <p>이놈이 지금은 1등이야. 그래서 이것으로.
      </p>
      <blockquote>
        programming is fun!
        <br />
        — quristyle
      </blockquote>
    `
  });

  // 라이프사이클
  onMounted(() => {
    document.addEventListener('paste', handlePaste);
  });

  onBeforeUnmount(() => {
    document.removeEventListener('paste', handlePaste);
    editor.value?.destroy();
  });

  // onMounted(() => {
  //   document.addEventListener('paste', handlePaste)
  // })

  // onBeforeUnmount(() => {
  //   document.removeEventListener('paste', handlePaste)
  // })

  const request = reactive({
    title: '',
    description: '',
    customerId: localStorage.getItem('user.id')
  });

  const save = async () => {
    const html = editor.value.getHTML();
    console.log(html);

    request.description = html;

    await RequestService.add(request);
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
          //  editor.chain().focus().setImage({ src: base64 }).run();

          if (editor && editor.value) {
            editor.value.chain().focus().setImage({ src: base64 }).run();
          }
        };

        reader.readAsDataURL(file);
        event.preventDefault(); // 기본 붙여넣기 방지
      }
    }
  };
</script>

<template>
  <div>
    <InputText type="text" v-model="request.title" placeholder="Title..." />
    <Button label="요청" type="button" class="mr-2" @click="save" />
  </div>
  <div class="card">
    <editor-content :editor="editor" />
  </div>
</template>
