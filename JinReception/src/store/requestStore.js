// stores/requestStore.js
import { defineStore } from 'pinia'
import { ref } from 'vue'

export const useRequestStore = defineStore('request', () => {
  const Srch = ref('')
  const dropdownItem = ref(null)
  return { Srch, dropdownItem }
})
