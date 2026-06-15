import { createRouter, createWebHistory } from 'vue-router'
import NuevaCompraView from '../views/NuevaCompraView.vue'
import HistorialView from '../views/HistorialView.vue'
import NuevoClienteView from '@/views/NuevoClienteView.vue'
import NuevaVentaView from '@/views/NuevaVentaView.vue'
import VerTransaccionView from '@/views/VerTransaccionView.vue'
import EditarTransaccionView from '@/views/EditarTransaccionView.vue'
import TablaClientes from '@/components/TablaClientes.vue'
import EditarClienteView from '@/views/EditarClienteView.vue'

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
  {
    path: '/cliente',
    component: NuevoClienteView
  },
  {
    path: '/nueva-venta',
    name: 'NuevaVenta',
    component: NuevaVentaView
  },
  {
    path: '/transaccion/:id',
    name: 'VerTransaccion',
    component: VerTransaccionView
  },
  {
    path: '/transaccion/editar/:id',
    name: 'EditarTransaccion',
    component: EditarTransaccionView
  },
  {
    path: '/clientes',
    name: 'Clientes',
    component: TablaClientes
  },
  {
    path: '/cliente/editar/:id',
    name: 'EditarCliente',
    component: EditarClienteView
  }
]

const router = createRouter({
  history: createWebHistory(),
  routes,
})

export default router
