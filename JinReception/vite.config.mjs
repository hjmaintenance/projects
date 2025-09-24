import { fileURLToPath, URL } from 'node:url';

import { PrimeVueResolver } from '@primevue/auto-import-resolver';
import vue from '@vitejs/plugin-vue';
import Components from 'unplugin-vue-components/vite';
import { defineConfig, loadEnv } from 'vite';

// https://vitejs.dev/config/
export default defineConfig(({ mode }) => {
  const env = loadEnv(mode, process.cwd(), '');
  const proxyUrl = env.VITE_API_URL || 'http://localhost:5223';

  return {
    optimizeDeps: {
      noDiscovery: true
    },
    plugins: [
      vue(),
      Components({
        resolvers: [PrimeVueResolver()]
      })
    ],
    resolve: {
      alias: {
        '@': fileURLToPath(new URL('./src', import.meta.url))
      }
    },
    server: {
      proxy: {
        '/api': proxyUrl
      },
      // 🔹 EMFILE 회피용 watch 설정
      watch: {
        ignored: ['**/node_modules/**', '**/dist/**', '**/.git/**', '**/logs/**'],
        usePolling: true, // fs.watch 대신 폴링 사용
        interval: 100 // 100ms마다 체크
      }
    }
  };
});
