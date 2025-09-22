import apiClient from './api';

const callWithLoading = async (loadingRef, fn) => {
    if (loadingRef && typeof loadingRef === 'object' && 'value' in loadingRef) {
        loadingRef.value = true;
    }
    try {
        return await fn();
    } finally {
        if (loadingRef && typeof loadingRef === 'object' && 'value' in loadingRef) {
            loadingRef.value = false;
        }
    }
};

const _service = {
    async getList() {
        const res = await apiClient.get('/companys');
        return res.data.data;
    },
    async get(id) {
        const res = await apiClient.get(`/companys/${id}`);
        return [res.data.data];
    },
    async add(company) {
        const res = await apiClient.post('/companys', company);
        return res.data.data;
    },
    async update(company) {
        const res = await apiClient.put(`/companys/${company.id}`, company);
        return res.data.data;
    },
    async delete(company) {
        const res = await apiClient.delete(`/companys/${company.id}`);
        return res.data.data;
    }
};

export const CompanyService = {
    getList: (loading) => callWithLoading(loading, () => _service.getList()),
    get: (id, loading) => callWithLoading(loading, () => _service.get(id)),
    add: (company, loading) => callWithLoading(loading, () => _service.add(company)),
    update: (company, loading) => callWithLoading(loading, () => _service.update(company)),
    delete: (company, loading) => callWithLoading(loading, () => _service.delete(company)),
    save(companys, loading) {
        return callWithLoading(loading, async () => {
            if (!companys.value || companys.value.length === 0) return;
    
        const savePromises = companys.value.map(company => {
            // 변경되지 않은 항목은 무시 로직 나중에 추가 하자.
            if (company.id) {
                return _service.update(company);
            } else {
                return _service.add(company);
            }
        });
    
            await Promise.all(savePromises);
        });
    },
    deleteSelected(companiesToDelete, loading) {
        return callWithLoading(loading, async () => {
            if (!companiesToDelete || companiesToDelete.length === 0) return;
    
            const deletePromises = companiesToDelete.map(company => _service.delete(company));
    
            await Promise.all(deletePromises);
        });
    }
};
