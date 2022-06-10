import { createRouter, createWebHistory } from 'vue-router'
import App from '../App.vue'

const routes = [
  {
    path: '/',
    name: 'app',
    component: App,
    redirect: '/dashboard',
    children: [
      {
        path: '/dashboard',
        name: 'dashboard',
        component: () => import('../components/Dashboard.vue')
      },
      {
        path: '/testhome',
        name: 'testhome',
        component: () => import('../views/TestHome.vue')        
      },
      {
        path: '/bug',
        name: 'bug',
        component: () => import('../views/Bug.vue')
      },
      {
        path: '/testcase',
        name: 'TestCase',
        component: () => import('../views/TestCase.vue')
      },
      {
        path: '/testtask',
        name: 'TestTask',
        component: () => import('../views/TestTask.vue')
      },
      {
        path: '/testreport',
        name: 'TestReport',
        component: () => import('../views/TestReport.vue')
      }
    ]
  },
]

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes
})

export default router