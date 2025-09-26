import apiClient from './api';
import { app } from '@/main'; // main.js에서 export한 app import

/**
 * 서비스 객체를 래핑하여 모든 API 요청에 자동으로 serviceName을 추가합니다.
 * @param {string} serviceName - 서비스의 이름
 * @param {object} service - API 메소드를 포함하는 서비스 객체
 * @returns {object} - 래핑된 서비스 객체
 */
export function serviceWrapper(serviceName, service) {
  const wrappedService = {};

  // 1. 기존 서비스의 메소드들을 래핑합니다.
  for (const methodName of Object.keys(service)) {
    const originalMethod = service[methodName];

    if (typeof originalMethod !== 'function') {
      wrappedService[methodName] = originalMethod;
      continue;
    }

    wrappedService[methodName] = async (...args) => {
      // 마지막 인자가 axios config 객체인지 확인하고, 없으면 새로 만듭니다.
      const lastArg = args[args.length - 1];
      const hasConfig = typeof lastArg === 'object' && lastArg !== null && !Array.isArray(lastArg) && !('value' in lastArg); // loading ref 객체 제외
      const config = hasConfig ? lastArg : {};
      config.serviceName = serviceName;

      // originalMethod가 axios 메서드라면 마지막 인자로 config 전달
      if (hasConfig) {
        return await originalMethod(...args);
      } else {
        // config가 없는 경우, 마지막에 추가
        return await originalMethod(...args, config);
      }
    };
  }

  // 2. Proxy를 사용하여 존재하지 않는 메소드 호출을 가로챕니다.
  return new Proxy(wrappedService, {
    get(target, prop, receiver) {
      // 만약 메소드가 존재하면, 그대로 반환합니다.
      if (prop in target) {
        return Reflect.get(...arguments);
      }

      // 존재하지 않는 메소드라면, 에러를 처리하는 함수를 반환합니다.
      return () => {
        const errorMessage = `'${String(prop)}' 안만든것 같은데... in ${serviceName}.`;
        console.error(errorMessage);

        app.config.globalProperties.$toast.add({
          severity: 'error',
          summary: `Service Method Error`,
          detail: errorMessage,
          life: 50000
        });

        // 호출한 곳에서 추가적인 실행을 막기 위해 rejected Promise를 반환합니다.
        return Promise.reject(new Error(errorMessage));
      };
    }
  });
}
