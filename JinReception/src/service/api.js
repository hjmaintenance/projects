import axios from 'axios';
import router from '@/router'; // router import
import { app } from '@/main'; // main.js에서 export한 app import
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

// 응답 인터셉터
apiClient.interceptors.response.use(
  (response) => {
    // 2xx 범위의 상태 코드
    const message = response.data?.message;
    const serviceName = response.config.serviceName;

    // message가 있고, GET 요청이 아닐 때 성공 토스트를 보여줍니다.
    //if (message && response.config.method !== 'get') {

    // 성공일때 code 값을 보고 실패이면 에러 메시지 보여줘라...

    /*
    app.config.globalProperties.$toast.add({
      severity: 'success',
      summary: `${serviceName || 'API'} Success`,
      detail: message,
      life: 3000
    });
*/

    //}

    return response;
  },
  (error) => {
    // 2xx 범위를 벗어나는 상태 코드
    if (error.response) {
      const { status, data } = error.response;
      const serviceName = error.config.serviceName;

      app.config.globalProperties.$toast.add({
        severity: 'error',
        summary: `${serviceName || 'API'} Error (Status: ${status})`,
        detail: data.message || 'An unexpected error occurred.'
        //life: 3000
      });

      // 401 Unauthorized 에러 시 로그인 페이지로 리다이렉트
      if (status === 401) {
        //router.push('/login');
      }
    }

    return Promise.reject(error);
  }
);

export default apiClient;
