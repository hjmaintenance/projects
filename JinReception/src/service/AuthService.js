import apiClient from './api';

export const AuthService = {
  async login(credentials) {
    const res = await apiClient.post('/users/login', credentials);
    return res.data.data;
  },

  async changePassword(passwords) {
    return await apiClient.post('/admins/change-password', passwords);
  }
};
