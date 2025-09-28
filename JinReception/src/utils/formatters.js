/**
 * 날짜를 'YYYY. MM. DD.' 형식의 문자열로 변환합니다.
 * @param {string | Date} value - 변환할 날짜 (문자열 또는 Date 객체)
 * @returns {string} 포맷팅된 날짜 문자열. 유효하지 않은 값이면 빈 문자열을 반환합니다.
 */
export function formatDate(value) {
    if (!value) return '';
    try {
        const date = new Date(value);
        // isNaN(date)는 날짜가 유효하지 않은지 확인합니다.
        if (isNaN(date)) return '';

        return date.toLocaleDateString('ko-KR', {
            year: 'numeric',
            month: '2-digit',
            day: '2-digit'
        });
    } catch (error) {
        console.error('날짜 포맷팅 오류:', value, error);
        return '';
    }
}

/*
export const STATUS = Object.freeze({
    PENDING: 0,
    IN_PROGRESS: 1,
    COMPLETED: 2,
    REJECTED: 3,
    DELETE: 4
});
*/


export const STATUS = Object.freeze([
     { name: 'PENDING', code: 0 },
    { name: 'IN_PROGRESS', code: 1 },
     { name: 'REJECTED', code: 2 },
     { name: 'COMPLETED', code: 3 },
     { name: 'DELETE', code: 4 },
]);