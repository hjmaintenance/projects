import apiClient from './api';
import { serviceWrapper } from './serviceWrapper';

const callWithLoading = async (loadingRef, fn) => {
  if (loadingRef?.value !== undefined) loadingRef.value = true;
  try {
    return await fn();
  } finally {
    if (loadingRef?.value !== undefined) loadingRef.value = false;
  }
};

const _serviceMethods = {
  getList() {
    return apiClient.get('/notices').then(res => res.data.data);
  },
  get(id) {
    return apiClient.get(`/notices/${id}`).then(res => res.data.data);
  },
  add(notice) {
    return apiClient.post('/notices', notice).then(res => res.data.data);
  },
  update(notice) {
    return apiClient.put(`/notices/${notice.id}`, notice).then(res => res.data.data);
  },
  delete(notice) {
    return apiClient.delete(`/notices/${notice.id}`).then(res => res.data.data);
  }
};

const _service = serviceWrapper('NoticeService', _serviceMethods);

export const NoticeService = {
  getList: (loading) => callWithLoading(loading, () => _service.getList()),
  get: (id, loading) => callWithLoading(loading, () => _service.get(id)),
  add: (notice, loading) => callWithLoading(loading, () => _service.add(notice)),
  update: (notice, loading) => callWithLoading(loading, () => _service.update(notice)),
  delete: (notice, loading) => callWithLoading(loading, () => _service.delete(notice)),

  saveNotice: (notices, loading) =>
    callWithLoading(loading, async () => {
      if (!notices?.value?.length) return;
      const promises = notices.value.map(n =>
        n.id ? _service.update(n) : _service.add(n)
      );
      await Promise.all(promises);
    }),

  deleteNotices: (notices, loading) =>
    callWithLoading(loading, async () => {
      if (!notices?.length) return;
      await Promise.all(notices.map(n => _service.delete(n)));
    })
};
