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
  },
  async get(id) {
    //const res = await apiClient.get(`/requests/${id}`);

    const res = await apiClient.post('/requests/srch', {id:id+'',remove	:"customerId"});

    return res.data.data[0];
  },
  async accept(request) {
    const res = await apiClient.put(`/requests/${request.id}`, request);
    return res.data.data;
  },

  async getComments(requestId) {
    const res = await apiClient.get(`/requests/${requestId}/comments`);
    return res.data.data;
  },

  async addComment(comment) {
    const res = await apiClient.post('/comments', comment);
    return res.data.data;
  },
};

export const RequestService = serviceWrapper('RequestService', _RequestService);
