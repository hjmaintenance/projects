
import { ref, watch } from 'vue';
import { defineStore } from 'pinia';
import { useLayout } from '@/layout/composables/layout';
import { useUiStore } from '@/store/ui';

export const useMenuStore = defineStore('menu', () => {
    const { loginUser } = useLayout();
    const uiStore = useUiStore();
    //const getBaseMenu = () => [];

 

    //const model = ref(getBaseMenu());
const model = ref(null);

    const menuMake = () => {
        //model.value = getBaseMenu();

        const customer = [
                { label: '접수', icon: 'pi pi-fw pi-inbox',
                  items: [
                          { label: '나의 접수 목록', icon: 'pi pi-fw pi-list', to: '/mng_request' }
                         ]
                },
                { label: '설정', icon: 'pi pi-fw pi-cog',
                  items: [
                          { label: '프로필', icon: 'pi pi-fw pi-user', to: '/profile' }
                         ]
                
        }];
        




        const admin = [
                { label: '모니터링', icon: 'pi pi-fw pi-desktop',
                  items: [
                            { label: '접수모니터링', icon: 'pi pi-fw pi-list', to: '/request_monitor' },

                        ]
                },
                { label: '고객관리', icon: 'pi pi-fw pi-users',
                  items: [
                            { label: '회사관리', icon: 'pi pi-fw pi-building', to: '/company' },
                            { label: '고객관리', icon: 'pi pi-fw pi-id-card', to: '/customer' },
                            { label: '접수관리', icon: 'pi pi-fw pi-table', to: '/mng_request' },
                        ]
                },
                { label: 'Act', icon: 'pi pi-fw pi-bolt',
                  items: [
                            { label: '팀관리', icon: 'pi pi-fw pi-sitemap', to: '/teams' },
                            { label: '관리자관리', icon: 'pi pi-fw pi-user-edit', to: '/admins' },
                            { label: '공지관리', icon: 'pi pi-fw pi-megaphone', to: '/notice' },
                         ]
                },
                { label: '시스템', icon: 'pi pi-fw pi-server',
                  items: [
                            { label: '배포관리', icon: 'pi pi-fw pi-cloud-upload', to: '/buildRelease' },
                         ]
                },
                { label: '설정', icon: 'pi pi-fw pi-cog',
                  items: [
                          { label: '프로필 변경', icon: 'pi pi-fw pi-user', to: '/profile' }
            ]
        }];
        
        
        
        
        
        

        const loginType = loginUser.value?.login_type;
        if (loginType === 'admin') {
            //model.value.unshift(admin);
            model.value = admin;
        } else if (loginType === 'customer') {
            //model.value.unshift(customer);
            model.value = customer;
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
