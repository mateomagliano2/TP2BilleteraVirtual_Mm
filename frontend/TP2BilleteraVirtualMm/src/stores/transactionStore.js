import { defineStore } from 'pinia'
import api from '@/services/api'

export const useTransactionStore = defineStore('transaction', {
  state: () => ({
    movimientos: [],
    cargando: false
  }),

  actions: {
    async cargarMovimientos() {
      try {
        this.cargando = true
        const res = await api.get('/transactions')
        this.movimientos = res.data
      } catch (error) {
        console.log('Error cargando movimientos', error)
      } finally {
        this.cargando = false
      }
    },

    async eliminarMovimiento(id) {
      await api.delete(`/transactions/${id}`)
      this.movimientos = this.movimientos.filter(m => m.id !== id)
    }
  }
})
