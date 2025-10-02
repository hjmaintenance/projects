
import { ref } from 'vue';
import { defineStore } from 'pinia';
import { useMenuStore } from './menu';

export const useBreadcrumbStore = defineStore('breadcrumb', () => {
    const items = ref([]);

    const menuStore = useMenuStore();

    const findPath = (menu, path, parentPath) => {
        for (const item of menu) {
            const currentPath = [...parentPath, item];
            if (item.to === path) {
                return currentPath;
            }
            if (item.items) {
                const result = findPath(item.items, path, currentPath);
                if (result) {
                    return result;
                }
            }
        }
        return null;
    };

    const updateBreadcrumbs = (path) => {
        const menuModel = menuStore.model;
        if (!menuModel) {
            items.value = [];
            return;
        }

        const breadcrumbPath = findPath(menuModel, path, []);
        if (breadcrumbPath) {
            items.value = breadcrumbPath.map(item => ({ label: item.label, to: item.to }));
        } else {
            items.value = [];
        }
    };

    return { items, updateBreadcrumbs };
});
