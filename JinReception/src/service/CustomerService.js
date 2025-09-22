import apiClient from './api';

export const CustomerService = {
    async getList(){
        const res = await apiClient.get('/customers');
        return res.data.data;
    }
};
