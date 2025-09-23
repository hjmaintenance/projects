<script setup>

import { RequestService } from '@/service/RequestService';
import { reactive, ref, onMounted, onBeforeUnmount } from 'vue';
import { Editor, EditorContent } from '@tiptap/vue-3';

import StarterKit from '@tiptap/starter-kit';
import { TextStyle } from '@tiptap/extension-text-style';
import { Color } from '@tiptap/extension-color';
import ListItem from '@tiptap/extension-list-item';

const editor = ref(null);






onMounted(() => {
  editor.value = new Editor({
    extensions: [
      Color.configure({ types: [TextStyle.name, ListItem.name] }),
      TextStyle.configure({ types: [ListItem.name] }),
      StarterKit,
    ],
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
    `,
  })
});


const request = reactive({
  title: '',
  description: '',
  customerId: localStorage.getItem('user.id'),
});





const save = async () => {
  const html = editor.value.getHTML();
  console.log(html);


  request.description = html;

  await RequestService.add(request);


};


onBeforeUnmount(() => {
  editor.value?.destroy()
})
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
