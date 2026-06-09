import { createRouter, createWebHistory } from 'vue-router'
import NuevaCompraView from '../views/NuevaCompraView.vue'
import HistorialView from '../views/HistorialView.vue'

const routes = [
  {
    path: '/',
    redirect: '/nueva-compra',
  },
  {
    path: '/nueva-compra',
    name: 'NuevaCompra',
    component: NuevaCompraView,
  },
  {
    path: '/historial',
    name: 'Historial',
    component: HistorialView,
  },
]

const router = createRouter({
  history: createWebHistory(),
  routes,
})

export default router
