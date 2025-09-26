import { computed, reactive, ref } from 'vue';

const loginUser = ref(null);

function setLoginUser(userData) {
  loginUser.value = userData;
  if (userData) {
    localStorage.setItem('loginUser', JSON.stringify(userData));
  } else {
    localStorage.removeItem('loginUser');
  }
}

function clearLoginUser() {
  loginUser.value = null;
  localStorage.removeItem('jwt_token');
  localStorage.removeItem('loginUser');
}

function initLoginUser() {
  const userJSON = localStorage.getItem('loginUser');
  if (userJSON) {
    try {
      loginUser.value = JSON.parse(userJSON);
    } catch (e) {
      console.error('Failed to parse loginUser from localStorage', e);
      clearLoginUser();
    }
  }
}

const layoutConfig = reactive({
  preset: localStorage.getItem('theme.preset') || 'Aura',
  primary: localStorage.getItem('theme.primary') || 'emerald',
  surface: localStorage.getItem('theme.surface') || null,
  darkTheme: localStorage.getItem('theme.darkTheme') ? localStorage.getItem('user.theme.darkTheme') === 'true' : true,
  menuMode: localStorage.getItem('theme.menuMode') || 'static'
});

const layoutState = reactive({
  staticMenuDesktopInactive: false,
  overlayMenuActive: false,
  profileSidebarVisible: false,
  configSidebarVisible: false,
  staticMenuMobileActive: false,
  menuHoverActive: false,
  activeMenuItem: null
});

export function useLayout() {
  const initializeTheme = () => {
    if (layoutConfig.darkTheme) {
      initLoginUser();

      document.documentElement.classList.add('app-dark');
    } else {
      document.documentElement.classList.remove('app-dark');
    }
  };

  const setActiveMenuItem = (item) => {
    layoutState.activeMenuItem = item.value || item;
  };

  const toggleDarkMode = () => {
    if (!document.startViewTransition) {
      executeDarkModeToggle();

      return;
    }

    document.startViewTransition(() => executeDarkModeToggle(event));
  };

  const executeDarkModeToggle = () => {
    layoutConfig.darkTheme = !layoutConfig.darkTheme;

    localStorage.setItem('theme.darkTheme', layoutConfig.darkTheme);

    document.documentElement.classList.toggle('app-dark');
  };

  const toggleMenu = () => {
    if (layoutConfig.menuMode === 'overlay') {
      layoutState.overlayMenuActive = !layoutState.overlayMenuActive;
    }

    if (window.innerWidth > 991) {
      layoutState.staticMenuDesktopInactive = !layoutState.staticMenuDesktopInactive;
    } else {
      layoutState.staticMenuMobileActive = !layoutState.staticMenuMobileActive;
    }
  };

  const isSidebarActive = computed(() => layoutState.overlayMenuActive || layoutState.staticMenuMobileActive);

  const isDarkTheme = computed(() => layoutConfig.darkTheme);

  const getPrimary = computed(() => layoutConfig.primary);

  const getSurface = computed(() => layoutConfig.surface);

  return {
    loginUser,
    layoutConfig,
    layoutState,
    toggleMenu,
    isSidebarActive,
    isDarkTheme,
    getPrimary,
    getSurface,
    setActiveMenuItem,
    setLoginUser,
    clearLoginUser,
    initLoginUser,
    toggleDarkMode,
    initializeTheme
  };
}
