import apiClient from './api';
import { serviceWrapper } from './serviceWrapper';

const _RequestService = {
  async search(srchobj) {
    const res = await apiClient.post('/requests/srch', srchobj);
    return res.data.data;
  },
  async add(request) {
    const res = await apiClient.post('/requests', request);
    return res.data.data;
  }
};

export const RequestService = serviceWrapper('RequestService', _RequestService);
