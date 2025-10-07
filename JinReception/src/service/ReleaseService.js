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
  async build() {
    const res = await apiClient.post('/build/release');
    return res.data.data;
  }
};

const _service = serviceWrapper('ReleaseService', _serviceMethods);

export const ReleaseService = {
  build: (loading) => callWithLoading(loading, () => _service.build())
};
