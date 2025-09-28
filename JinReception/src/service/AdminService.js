import apiClient from './api';
import { serviceWrapper } from './serviceWrapper';

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

const _serviceMethods = {
  async getList() {
    const res = await apiClient.get('/admins');
    return res.data.data;
  },
  async search(srchobj) {
    const res = await apiClient.post('/admins/srch', srchobj);
    return res.data.data;
  },
  async get(id) {
    const res = await apiClient.get(`/admins/${id}`);
    return [res.data.data];
  },
  async add(admin) {
    const res = await apiClient.post('/admins', admin);
    return res.data.data; // Returns { admin, tempPassword }
  },
  async update(admin) {
    const res = await apiClient.put(`/admins/${admin.id}`, admin);
    return res.data.data;
  },
  async delete(admin) {
    const res = await apiClient.delete(`/admins/${admin.id}`);
    return res.data.data;
  }
};

const _service = serviceWrapper('AdminService', _serviceMethods);

export const AdminService = {
  getList: (loading) => callWithLoading(loading, () => _service.getList()),
  search: (srchobj, loading) => callWithLoading(loading, () => _service.search(srchobj)),
  get: (id, loading) => callWithLoading(loading, () => _service.get(id)),
  add: (admin, loading) => callWithLoading(loading, () => _service.add(admin)),
  update: (admin, loading) => callWithLoading(loading, () => _service.update(admin)),
  delete: (admin, loading) => callWithLoading(loading, () => _service.delete(admin, { serviceName: 'AdminService' })),
  save(admins, loading) {
    return callWithLoading(loading, async () => {
      if (!admins.value || admins.value.length === 0) return;

      const savePromises = admins.value.map((admin) => {
        if (admin.id) {
          return _service.update(admin);
        } else {
          return _service.add(admin);
        }
      });

      return Promise.all(savePromises);
    });
  },
  deleteSelected(adminsToDelete, loading) {
    return callWithLoading(loading, async () => {
      if (!adminsToDelete || adminsToDelete.length === 0) return;

      const deletePromises = adminsToDelete.map((admin) => _service.delete(admin));

      await Promise.all(deletePromises);
    });
  }
};
