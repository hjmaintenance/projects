import AppLayout from '@/layout/AppLayout.vue';
import { createRouter, createWebHistory } from 'vue-router';
import { jwtDecode } from 'jwt-decode';

const router = createRouter({
  history: createWebHistory(),
  routes: [
    {
      path: '/',
      component: AppLayout,
      children: [
        {
          path: '/request_detail',
          name: 'request_detail',
          component: () => import('@/views/pages/reqs/RequestDetail.vue')
        },
    {
      path: '/profile',
      name: 'profile',
      component: () => import('@/views/pages/Profile.vue')
    },
        {
          path: '/mng_request',
          name: 'mng_request',
          component: () => import('@/views/pages/reqs/MngRequest.vue'),
          meta: { keepAlive: true }
        },
        {
          path: '/buildRelease',
          name: 'buildRelease',
          component: () => import('@/views/pages/sys/ReleseTool.vue'),
          meta: { keepAlive: true }
        },
        {
          path: '/request',
          name: 'request',
          component: () => import('@/views/pages/reqs/Request.vue'),
          meta: { hideFab: true }
        },
        {
          path: '/company',
          name: 'company',
          meta: { menuName: 'Company Management' },
          component: () => import('@/views/pages/companys/Company.vue')
        },
        {
          path: '/teams',
          name: 'teams',
          meta: { menuName: 'Team Management' },
          component: () => import('@/views/pages/Teams.vue')
        },
        {
          path: '/admins',
          name: 'admins',
          meta: { menuName: 'Admin Management' },
          component: () => import('@/views/pages/Admins.vue')
        },
        {
          path: '/customer',
          name: 'customer',
          component: () => import('@/views/pages/companys/Customer.vue')
        },
        {
          path: '/notice',
          name: 'list_notice',
          component: () => import('@/views/pages/noti/NoticeList.vue')
        },
        {
          path: '/notice/form/:id',
          name: 'form_notice',
          component: () => import('@/views/pages/noti/NoticeForm.vue')
        },
        {
          path: '/notive/view/:id',
          name: 'view_notice',
          component: () => import('@/views/pages/noti/NoticeView.vue')
        },
        {
          path: '/dashboard',
          name: 'dashboard',
          component: () => import('@/views/pages/moni/Dashboard.vue')
        },
        {
          path: '/custom_dashboard',
          name: 'custom_dashboard',
          component: () => import('@/views/pages/moni/CustomerDashboard.vue')
        },
        {
          path: '/',
          name: 'home',
          component: () => import('@/views/pages/moni/Dashboard.vue')
        },

        {
          path: '/request_monitor',
          name: 'request_monitor',
          component: () => import('@/views/pages/moni/RequestMonitor.vue')
        },

        

        {
          path: '/pages/empty',
          name: 'empty',
          component: () => import('@/views/pages/Empty.vue')
        },
        {
          path: '/pages/crud',
          name: 'crud',
          component: () => import('@/views/pages/Crud.vue')
        },
        {
          path: '/documentation',
          name: 'documentation',
          component: () => import('@/views/pages/Documentation.vue')
        }
      ]
    },
    {
      path: '/pages/notfound',
      name: 'notfound',
      component: () => import('@/views/pages/NotFound.vue')
    },

    {
      path: '/auth/login',
      name: 'login',
      component: () => import('@/views/pages/auth/Login.vue')
    },
    {
      path: '/auth/register',
      name: 'register',
      component: () => import('@/views/pages/auth/Register.vue')
    },
    {
      path: '/auth/change-password',
      name: 'change-password',
      component: () => import('@/views/pages/auth/ChangePassword.vue')
    }
  ]
});

router.beforeEach((to, from, next) => {
  // 'login' 페이지와 같이 인증이 필요 없는 페이지 목록
  const publicPages = ['login', 'register', 'landing', 'notfound', 'accessDenied', 'error'];
  const authRequired = !publicPages.includes(to.name);
  const loggedIn = localStorage.getItem('jwt_token');

  // 인증이 필요한 페이지에 접근하는데, 토큰이 없는 경우
  if (authRequired && !loggedIn) {
    return next({ name: 'login' });
  }

  // 인증이 필요한 페이지에 접근하는데, 토큰이 있는 경우 만료에 대한 처리
  if (loggedIn) {
    try {
      //jwt-decode와 같은 라이브러리로 토큰의 만료(exp) 여부를 확인할 수 있습니다.
      const decoded = jwtDecode(loggedIn);
      if (decoded.exp < Date.now() / 1000) {
        localStorage.removeItem('jwt_token');
        localStorage.removeItem('loginUser');
        if (authRequired) return next({ name: 'login' });
      }
    } catch (e) {
      localStorage.removeItem('jwt_token');
      localStorage.removeItem('loginUser');
      if (authRequired) return next({ name: 'login' });
    }
  }

  next();
});

export default router;
