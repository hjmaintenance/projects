import axios from 'axios';
import router from '@/router'; // router import
// axios 인스턴스
const apiClient = axios.create({
  baseURL: '/api'
});

// 요청 인터셉터
apiClient.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('jwt_token');
    if (token) {
      config.headers['Authorization'] = `Bearer ${token}`;
    }

    console.log(`get config check:`, config);

    config.headers['X-Service-Name'] = config.url;

    // 현재 라우트 가져오기
    const currentRoute = router.currentRoute.value;
    //config.headers['X-Menu-Name'] = config.menuName || 'unknown-menu'; // menuName은 그대로 둡니다.
    //config.headers['X-Menu-Name'] = currentRoute.meta.menuName || 'unknown-menu';
    config.headers['X-Menu-Name'] = currentRoute.name || 'unknown-menu';
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

export default apiClient;
