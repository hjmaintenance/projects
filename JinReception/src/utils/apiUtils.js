/**
 * 검색 조건, 정렬, 페이징 옵션을 결합하여 API 요청을 위한 전체 쿼리 페이로드를 생성합니다.
 * @param {object} inputs - 검색 입력값이 담긴 객체. e.g., { Srch: 'someValue' }
 * @param {Array<object>} config - 검색 필드 설정이 담긴 배열.
 *   e.g., [{ model: 'Srch', fields: ['name', 'addr'], operator: 'like' }]
 * @param {object} [queryOptions] - 정렬 및 페이징 옵션. e.g., { sorts: [...], page: 1, pageSize: 10 }
 * @returns {object} API 요청에 사용될 전체 페이로드. e.g., { name_like: 'someValue', sorts: [...], ... }
 */
export function buildQueryPayload(inputs, config, queryOptions) {
  const searchConditions = {};
  config.forEach(item => {
    const value = inputs[item.model]?.trim();
    if (value) {
      item.fields.forEach(field => (searchConditions[`${field}_${item.operator}`] = value));
    }
  });

  // 검색 조건과 쿼리 옵션을 병합하여 최종 페이로드 생성
  // queryOptions가 undefined나 null일 경우를 대비하여 빈 객체로 처리
  return { ...searchConditions, ...(queryOptions || {}) };
}