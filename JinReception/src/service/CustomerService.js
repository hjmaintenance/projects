import apiClient from './api';

export const CustomerService = {
    async getList(){
        const res = await apiClient.get('/customers');
        return res.data.data;
    },
    async get(id){
        const res = await apiClient.get(`/customers/${id}`);
        return [res.data.data];
    },
    async add(customer){
        const res = await apiClient.post('/customers', customer);
        return res.data.data;
    },
    async update(customer){
        const res = await apiClient.put(`/customers/${customer.id}`, customer);
        return res.data.data;
    },
    async delete(customer){
        const res = await apiClient.delete(`/customers/${customer.id}`);
        return res.data.data;
    },
    async save(customers){
        if (!customers.value || customers.value.length === 0) return;
        const savePromises = customers.value.map(customer => {
            // 변경되지 않은 항목은 무시 로직 나중에 추가 하자.
            if (customer.id) {
                return this.update(customer);
            } else {
                return this.add(customer);
            }
        });
        await Promise.all(savePromises);
    },
    async deleteSelected(customersToDelete){
        if (!customersToDelete || customersToDelete.length === 0) return;
        const deletePromises = customersToDelete.map(customer => this.delete(customer));
        await Promise.all(deletePromises);
    }
};
