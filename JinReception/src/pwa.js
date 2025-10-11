import { ref } from 'vue';

const deferredInstallPrompt = ref(null);
const showInstallButton = ref(false);

const saveBeforeInstallPromptEvent = (evt) => {
  if (window.matchMedia('(display-mode: standalone)').matches) {
    showInstallButton.value = false;
    return;
  }
  evt.preventDefault();
  deferredInstallPrompt.value = evt;
  showInstallButton.value = true;
};

const installPWA = async () => {
  if (!deferredInstallPrompt.value) {
    return;
  }
  deferredInstallPrompt.value.prompt();
  const { outcome } = await deferredInstallPrompt.value.userChoice;

  if (outcome === 'accepted') {
    showInstallButton.value = false;
  }
  deferredInstallPrompt.value = null;
};

if ('serviceWorker' in navigator) {
  window.addEventListener('load', () => {
    navigator.serviceWorker
      .register('/sw.js')
      .then((registration) => {
        console.log('Service Worker registered: ', registration);
      })
      .catch((registrationError) => {
        console.log('Service Worker registration failed: ', registrationError);
      });
  });
}

window.addEventListener('beforeinstallprompt', saveBeforeInstallPromptEvent);

export const pwa = {
  showInstallButton,
  installPWA,
};
