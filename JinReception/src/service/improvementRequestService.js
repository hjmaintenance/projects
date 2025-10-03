import apiClient from './api';
import { serviceWrapper } from './serviceWrapper';

const improvementRequestService = {
  searchRequests(params) {
    return apiClient.post('/api/requests/srch', params);
  }
};

export default serviceWrapper('improvementRequestService', improvementRequestService);
