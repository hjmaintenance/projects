// 서비스 워커 파일 (sw.js)

const CACHE_NAME = 'jin-reception-cache-v1';
const urlsToCache = [
  '/',
  '/index.html',
  // 여기에 빌드 후 생성되는 주요 JS, CSS 파일 경로를 추가할 수 있습니다.
  // 하지만 Vite 같은 번들러는 파일명에 해시를 붙이므로 수동 관리가 어렵습니다.
  // 우선은 기본적인 파일만 캐싱합니다.
];

// 서비스 워커 설치 시
self.addEventListener('install', (event) => {
  // 캐시할 파일들을 미리 저장합니다.
  event.waitUntil(caches.open(CACHE_NAME).then((cache) => cache.addAll(urlsToCache)));
});

// 네트워크 요청이 있을 때 (가장 중요한 부분)
self.addEventListener('fetch', (event) => {
  // 이 핸들러가 존재해야 PWA 설치가 가능해집니다.
  // 가장 기본적인 전략: 네트워크 우선, 실패 시 캐시에서 찾기
  event.respondWith(fetch(event.request).catch(() => caches.match(event.request)));
});