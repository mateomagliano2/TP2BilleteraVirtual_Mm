import { defineStore } from 'pinia'
import api from '../services/api'

export const useClientStore = defineStore('client', {
  state: () => ({
    clientes: [],
    clienteSeleccionado: null
  }),

  actions: {
    async cargarClientes() {
      try {
        const res = await api.get('/clientesbv')
        this.clientes = res.data
      } catch (error) {
        console.error('Error cargando clientes', error)
      }
    },

    seleccionarCliente(cliente) {
      this.clienteSeleccionado = cliente
    },

      limpiarClienteSeleccionado() {
        this.clienteSeleccionado = null
    }

  }
})
