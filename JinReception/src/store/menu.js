
import { ref, watch } from 'vue';
import { defineStore } from 'pinia';
import { useLayout } from '@/layout/composables/layout';

export const useMenuStore = defineStore('menu', () => {
    const { loginUser } = useLayout();

    const getBaseMenu = () => [
        {
            label: 'Home',
            items: [{ label: 'Dashboard', icon: 'pi pi-fw pi-home', to: '/' }]
        },
        {
            label: 'UI Components',
            items: [
                { label: 'Form Layout', icon: 'pi pi-fw pi-id-card', to: '/uikit/formlayout' },
                { label: 'Input', icon: 'pi pi-fw pi-check-square', to: '/uikit/input' },
                { label: 'Button', icon: 'pi pi-fw pi-mobile', to: '/uikit/button', class: 'rotated-icon' },
                { label: 'Table', icon: 'pi pi-fw pi-table', to: '/uikit/table' },
                { label: 'List', icon: 'pi pi-fw pi-list', to: '/uikit/list' },
                { label: 'Tree', icon: 'pi pi-fw pi-share-alt', to: '/uikit/tree' },
                { label: 'Panel', icon: 'pi pi-fw pi-tablet', to: '/uikit/panel' },
                { label: 'Overlay', icon: 'pi pi-fw pi-clone', to: '/uikit/overlay' },
                { label: 'Media', icon: 'pi pi-fw pi-image', to: '/uikit/media' },
                { label: 'Menu', icon: 'pi pi-fw pi-bars', to: '/uikit/menu' },
                { label: 'Message', icon: 'pi pi-fw pi-comment', to: '/uikit/message' },
                { label: 'File', icon: 'pi pi-fw pi-file', to: '/uikit/file' },
                { label: 'Chart', icon: 'pi pi-fw pi-chart-bar', to: '/uikit/charts' },
                { label: 'Timeline', icon: 'pi pi-fw pi-calendar', to: '/uikit/timeline' },
                { label: 'Misc', icon: 'pi pi-fw pi-circle', to: '/uikit/misc' }
            ]
        },
        {
            label: 'Pages',
            icon: 'pi pi-fw pi-briefcase',
            to: '/pages',
            items: [
                {
                    label: 'Landing',
                    icon: 'pi pi-fw pi-globe',
                    to: '/landing'
                },
                {
                    label: 'Auth',
                    icon: 'pi pi-fw pi-user',
                    items: [
                        {
                            label: 'Login',
                            icon: 'pi pi-fw pi-sign-in',
                            to: '/auth/login'
                        },
                        {
                            label: 'Error',
                            icon: 'pi pi-fw pi-times-circle',
                            to: '/auth/error'
                        },
                        {
                            label: 'Access Denied',
                            icon: 'pi pi-fw pi-lock',
                            to: '/auth/access'
                        }
                    ]
                },
                {
                    label: 'Crud',
                    icon: 'pi pi-fw pi-pencil',
                    to: '/pages/crud'
                },
                {
                    label: 'Not Found',
                    icon: 'pi pi-fw pi-exclamation-circle',
                    to: '/pages/notfound'
                },
                {
                    label: 'Empty',
                    icon: 'pi pi-fw pi-circle-off',
                    to: '/pages/empty'
                }
            ]
        },
        {
            label: 'Hierarchy',
            items: [
                {
                    label: 'Submenu 1',
                    icon: 'pi pi-fw pi-bookmark',
                    items: [
                        {
                            label: 'Submenu 1.1',
                            icon: 'pi pi-fw pi-bookmark',
                            items: [
                                { label: 'Submenu 1.1.1', icon: 'pi pi-fw pi-bookmark' },
                                { label: 'Submenu 1.1.2', icon: 'pi pi-fw pi-bookmark' },
                                { label: 'Submenu 1.1.3', icon: 'pi pi-fw pi-bookmark' }
                            ]
                        },
                        {
                            label: 'Submenu 1.2',
                            icon: 'pi pi-fw pi-bookmark',
                            items: [{ label: 'Submenu 1.2.1', icon: 'pi pi-fw pi-bookmark' }]
                        }
                    ]
                },
                {
                    label: 'Submenu 2',
                    icon: 'pi pi-fw pi-bookmark',
                    items: [
                        {
                            label: 'Submenu 2.1',
                            icon: 'pi pi-fw pi-bookmark',
                            items: [
                                { label: 'Submenu 2.1.1', icon: 'pi pi-fw pi-bookmark' },
                                { label: 'Submenu 2.1.2', icon: 'pi pi-fw pi-bookmark' }
                            ]
                        },
                        {
                            label: 'Submenu 2.2',
                            icon: 'pi pi-fw pi-bookmark',
                            items: [{ label: 'Submenu 2.2.1', icon: 'pi pi-fw pi-bookmark' }]
                        }
                    ]
                }
            ]
        },
        {
            label: 'Get Started',
            items: [
                {
                    label: 'Documentation',
                    icon: 'pi pi-fw pi-book',
                    to: '/documentation'
                },
                {
                    label: 'ui kit',
                    icon: 'pi pi-fw pi-github',
                    url: 'https://primevue.org/datatable/',
                    target: '_blank'
                }
            ]
        }
    ];

    const model = ref(getBaseMenu());

    const menuMake = () => {
        model.value = getBaseMenu();

        var customer = {
            label: loginUser.value?.user_name || '고객',
            items: [
                { label: '공지', icon: 'pi pi-fw pi-id-card', to: '/uikit/formlayout' },
                { label: '접수목록', icon: 'pi pi-fw pi-check-square', to: '/user_request' },
                { label: '접수', icon: 'pi pi-fw pi-mobile', to: '/request', class: 'rotated-icon' },
                { label: '비밀번호변경', icon: 'pi pi-fw pi-key', to: '/auth/change-password' }
            ]
        };

        var admin = {
            label: loginUser.value?.user_name || '관리자',
            items: [
                { label: '회사관리', icon: 'pi pi-fw pi-id-card', to: '/company' },
                { label: '고객관리', icon: 'pi pi-fw pi-id-card', to: '/customer' },
                { label: '팀관리', icon: 'pi pi-fw pi-check-square', to: '/teams' },
                { label: '관리자관리', icon: 'pi pi-fw pi-mobile', to: '/admins', class: 'rotated-icon' },
                { label: '접수관리', icon: 'pi pi-fw pi-table', to: '/mng_request' },
                { label: '공지관리', icon: 'pi pi-fw pi-id-card', to: '/notice' },
                { label: '모니터링', icon: 'pi pi-fw pi-list', to: '/uikit/list' },
                { label: '비밀번호변경', icon: 'pi pi-fw pi-key', to: '/auth/change-password' }
            ]
        };

        const loginType = loginUser.value?.login_type;
        if (loginType === 'admin') {
            model.value.unshift(admin);
        } else if (loginType === 'customer') {
            model.value.unshift(customer);
        }
    };

    watch(
        loginUser,
        () => {
            menuMake();
        },
        { immediate: true, deep: true }
    );

    return { model };
});
