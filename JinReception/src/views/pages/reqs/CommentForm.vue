<script setup>
import { ref, defineEmits , onMounted, onBeforeUnmount } from 'vue';
import { Editor, EditorContent, useEditor } from '@tiptap/vue-3';


import StarterKit from '@tiptap/starter-kit';
import { TextStyle } from '@tiptap/extension-text-style';
import { Color } from '@tiptap/extension-color';
import ListItem from '@tiptap/extension-list-item';
import Image from '@tiptap/extension-image';



const emit = defineEmits(['submit-comment']);
const commentText = ref('');

const handleSubmit = () => {



    const html = editor.value.getHTML();

  //editor.content = html;
    commentText.value = html;

  //if (!commentText.value.trim()) return;
  emit('submit-comment', commentText.value);

   editor.value.commands.setContent("<p> </p>");


};


  const editor = useEditor({
    extensions: [Color.configure({ types: [TextStyle.name, ListItem.name] }), TextStyle.configure({ types: [ListItem.name] }), StarterKit, Image],
    content: ` `
  });



  onMounted(() => {
    document.addEventListener('paste', handlePaste);
  });

  onBeforeUnmount(() => {
    document.removeEventListener('paste', handlePaste);
    editor.value?.destroy();
  });


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




<div class="comment-form flex flex-col gap-2">

<Fieldset legend="Add Comment ...">
    <editor-content :editor="editor" />
</Fieldset>



  <div class="flex justify-end">
    <Button 
      label="덧글달기" 
      @click="handleSubmit"
    ></Button>
  </div>
</div>









</template>
