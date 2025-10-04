import { defineStore } from 'pinia';
import { ref } from 'vue';

export const useUiStore = defineStore('ui', () => {
    const showLogoutConfirmation = ref(false);

    function requestLogoutConfirmation() {
        showLogoutConfirmation.value = true;
    }

    function clearLogoutConfirmation() {
        showLogoutConfirmation.value = false;
    }

    return { showLogoutConfirmation, requestLogoutConfirmation, clearLogoutConfirmation };
});
