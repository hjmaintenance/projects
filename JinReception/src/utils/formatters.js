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
};

export function formatDateSmart(value) {
    if (!value) return '';
    try {
        const date = new Date(value);
        if (isNaN(date)) return '';

        const now = new Date();
        const diffMs = now - date;
        const diffSec = Math.floor(diffMs / 1000);
        const diffMin = Math.floor(diffSec / 60);
        const diffHour = Math.floor(diffMin / 60);

        if (diffHour < 1) {
            // 1시간 미만 → 시:분:초 (24시간제)
            return date.toLocaleTimeString('ko-KR', {
                hour: '2-digit',
                minute: '2-digit',
                second: '2-digit',
                hour12: false,
            });
        } else if (diffHour < 24) {
            // 1시간 이상 ~ 24시간 미만 → 년.월.일 시:분
            return date.toLocaleString('ko-KR', {
                year: 'numeric',
                month: '2-digit',
                day: '2-digit',
                hour: '2-digit',
                minute: '2-digit',
                hour12: false,
            });
        } else {
            // 24시간 이상 → 년.월.일 시 (분은 제외)
            return date.toLocaleString('ko-KR', {
                year: 'numeric',
                month: '2-digit',
                day: '2-digit',
                hour: '2-digit',
                hour12: false,
            });
        }
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

export const STATUS_ALL = Object.freeze([
     { name: 'ALL', code: null },
     { name: 'PENDING', code: 0 },
    { name: 'IN_PROGRESS', code: 1 },
     { name: 'REJECTED', code: 2 },
     { name: 'COMPLETED', code: 3 },
     { name: 'DELETE', code: 4 },
]);
