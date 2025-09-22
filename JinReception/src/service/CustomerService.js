import apiClient from './api';

export const CustomerService = {
    getList(){
        const res = await apiClient.get('/customers');
        return res.data.data;
    }
};
