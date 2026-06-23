<template>
  <div class="contenedor">
    <div class="card" v-if="transaccion">
      <h2>Detalle de Transacción</h2>

      <p><strong>ID:</strong> {{ transaccion.id }}</p>
      <p><strong>Cliente:</strong> {{ transaccion.client_name || transaccion.client_id }}</p>
      <p><strong>Criptomoneda:</strong> {{ transaccion.crypto_code }}</p>
      <p><strong>Acción:</strong> {{ transaccion.action === 'purchase' ? 'Compra' : 'Venta' }}</p>
      <p><strong>Cantidad:</strong> {{ transaccion.crypto_amount }}</p>
      <p><strong>Monto:</strong> $ {{ transaccion.money }}</p>
      <p><strong>Fecha:</strong> {{ formatearFecha(transaccion.datetime) }}</p>

      <button @click="$router.push('/historial')">Volver</button>
    </div>

    <p v-else>Cargando...</p>
  </div>
</template>

<script>
import api from '../services/api.js'

export default {
  name: 'VerTransaccionView',
  data() {
    return {
      transaccion: null
    }
  },
  async mounted() {
    const id = this.$route.params.id

    try {
      const response = await api.get('/transactions/' + id)
      this.transaccion = response.data
    } catch (error) {
      console.log(error)
    }
  },
  methods: {
    formatearFecha(fecha) {
      return new Date(fecha).toLocaleString('es-AR')
    }
  }
}
</script>

<style scoped>
.contenedor {
  display: flex;
  justify-content: center;
  margin-top: 20px;
}

.card {
  background: white;
  padding: 25px;
  border-radius: 8px;
  box-shadow: 0 2px 6px rgba(0,0,0,0.1);
  width: 450px;
}

h2 {
  margin-bottom: 20px;
  color: #2c3e50;
}

p {
  margin-bottom: 10px;
}

button {
  margin-top: 15px;
  background-color: #3498db;
  color: white;
  padding: 10px 16px;
  border: none;
  border-radius: 4px;
  cursor: pointer;
}
</style>
