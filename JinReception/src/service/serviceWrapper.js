import apiClient from './api';

/**
 * 서비스 객체를 래핑하여 모든 API 요청에 자동으로 serviceName을 추가합니다.
 * @param {string} serviceName - 서비스의 이름
 * @param {object} service - API 메소드를 포함하는 서비스 객체
 * @returns {object} - 래핑된 서비스 객체
 */
export function serviceWrapper(serviceName, service) {
  const wrappedService = {};

  for (const methodName in service) {
    if (Object.prototype.hasOwnProperty.call(service, methodName)) {
      const originalMethod = service[methodName];

      wrappedService[methodName] = (...args) => {
        // 마지막 인자가 axios config 객체인지 확인하고, 없으면 새로 만듭니다.
        const lastArg = args[args.length - 1];
        const hasConfig = typeof lastArg === 'object' && lastArg !== null && !Array.isArray(lastArg);
        const config = hasConfig ? lastArg : {};
        config.serviceName = serviceName;

        console.log(`Calling ${serviceName}.${methodName} with config:`, config);

        //return originalMethod(...args);

        // originalMethod가 axios 메서드라면 마지막 인자로 config 전달
        if (args.length === 1) {
          // data만 있는 경우
          return originalMethod(args[0], config);
        } else if (args.length === 2 && hasConfig) {
          // 이미 config를 넘긴 경우
          return originalMethod(args[0], args[1]);
        } else {
          return originalMethod(...args);
        }
      };
    }
  }
  return wrappedService;
}
