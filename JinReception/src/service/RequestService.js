import apiClient from './api';

export const RequestService = {

    async add(request) {
        const res = await apiClient.post('/requests', request);
        return res.data.data;
    }



};
