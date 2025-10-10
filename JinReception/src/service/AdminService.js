import api from '@/service/api';

export const AdminService = {
  findPassword(findPasswordDto) {
    return api.post('/admins/find-password', findPasswordDto);
  }
};