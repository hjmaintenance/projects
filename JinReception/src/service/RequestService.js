import apiClient from './api';
import { serviceWrapper } from './serviceWrapper';



  import { useLayout } from '@/layout/composables/layout';
  
  const { loginUser } = useLayout();




const _RequestService = {
  async search(srchobj) {
    const res = await apiClient.post('/requests/srch', srchobj);
    return res.data.data;
  },
  async add(request) {
    const res = await apiClient.post('/requests', request);
    return res.data.data;
  },
  async addWithAttachments(formData) {
    const res = await apiClient.post('/requests', formData, {
      headers: {
        'Content-Type': 'multipart/form-data'
      }
    });
    return res.data.data;
  },
  async get(id) {
    //const res = await apiClient.get(`/requests/${id}`);

    const res = await apiClient.post('/requests/srch', {id:id+'',remove	:"customerId"});

    return res.data.data[0];
  },
  async accept(id, status) {

    const payload = {
        status: status,
        adminId: loginUser.value?.user_uid,
      };

    const res = await apiClient.put(`/requests/accept/${id}`, payload);
    return res.data.data;
  },
    async reset(id) {

    const payload = {
        adminId: loginUser.value?.user_uid,
      };
    const res = await apiClient.put(`/requests/reset/${id}`, payload);
    return res.data.data;
  },
  async update(request) {
    const res = await apiClient.put(`/requests/${request.id}`, request);
    return res.data.data;
  },

  async updateWithAttachments(id, formData) {
    const res = await apiClient.put(`/requests/${id}`, formData, {
      headers: {
        'Content-Type': 'multipart/form-data'
      }
    });
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
  async getCompanyStats() {
    const res = await apiClient.get('/dashboard/company-stats');
    return res.data.data;
  },
  async getAdminStats() {
    const res = await apiClient.get('/dashboard/admin-stats');
    return res.data.data;
  },
  async getAllAdminStats() {
    const res = await apiClient.get('/dashboard/all-admin-stats');
    return res.data.data;
  },
  async getMyCompanyStats() {
    const res = await apiClient.get('/dashboard/my-company-stats');
    return res.data.data;
  },
};

export const RequestService = serviceWrapper('RequestService', _RequestService);
