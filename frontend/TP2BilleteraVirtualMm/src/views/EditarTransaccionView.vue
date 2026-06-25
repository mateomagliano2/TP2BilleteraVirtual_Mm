<template>
  <div class="formulario" v-if="transaccion">
    <h2>Editar Transacción</h2>

    <form @submit.prevent="guardarCambios">
      <div class="campo">
        <label>Criptomoneda:</label>
        <select v-model="transaccion.crypto_code">
          <option value="bitcoin">Bitcoin</option>
          <option value="ethereum">Ethereum</option>
          <option value="usdc">USDC</option>
        </select>
      </div>

      <div class="campo">
        <label>Acción:</label>
        <select v-model="transaccion.action">
          <option value="purchase">Compra</option>
          <option value="sale">Venta</option>
        </select>
      </div>

      <div class="campo">
        <label>Cantidad:</label>
        <p v-if="transaccion.action === 'sale'" style="font-size: 12px; color: #888">No debes modificar la cantidad de una venta</p>
        <input v-model="transaccion.crypto_amount" type="text" />
      </div>

      <div class="campo">
        <label>Monto:</label>

        <p v-if="transaccion.action === 'sale'" style="font-size: 12px; color: #888"> El monto no se actualizará al editar la cantidad</p>
        <input v-model.number="transaccion.money" type="number" step="0.01" />
      </div>

        <div class="campo">
          <label>Cliente:</label>
          <select v-model.number="transaccion.client_id">
            <option
              v-for="c in clientes"
              :key="c.id"
              :value="c.id"
            >
              {{ c.name }}
            </option>
          </select>
        </div>

      <div class="campo">
        <label>Fecha y hora:</label>
        <input v-model="transaccion.datetimeLocal" type="datetime-local" />
      </div>

      <p v-if="error" class="mensaje-error">{{ error }}</p>
      <p v-if="ok" class="mensaje-exito">Transacción actualizada ✅</p>

      <button type="submit">Guardar Cambios</button>
    </form>
  </div>
</template>

<script>
import api from '../services/api'

export default {
  name: 'EditarTransaccionView',
  data() {
    return {
      transaccion: null,
      error: '',
      ok: false,
      clientes: []
    }
  },

  async mounted() {
    const id = this.$route.params.id

    try {

      const res = await api.get(`/transactions/${id}`)
      const data = res.data

      data.datetimeLocal = new Date(data.datetime).toISOString().slice(0, 16)
      this.transaccion = data


      const resClientes = await api.get('/clientesbv')
      this.clientes = resClientes.data

    } catch (error) {
      console.log(error)
      this.error = 'Error al cargar la transacción'
    }
  },

  methods: {
    async guardarCambios() {
      this.error = ''
      this.ok = false

      try {
        await api.patch(`/transactions/${this.transaccion.id}`, {
          crypto_code: this.transaccion.crypto_code,
          action: this.transaccion.action,
          crypto_amount: this.transaccion.crypto_amount,
          client_id: this.transaccion.client_id,
          money: this.transaccion.money,
          datetime: this.transaccion.datetimeLocal
        })


        this.ok = true

      } catch (error) {
        console.log(error)

        if (error.response) {
          this.error = error.response.data || 'Error al actualizar'
        } else {
          this.error = 'No se pudo conectar con el backend'
        }
      }
    }
  }
}
</script>

<style scoped>
.formulario {
  background-color: white;
  padding: 25px;
  border-radius: 8px;
  box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
  max-width: 500px;
}

h2 {
  margin-bottom: 20px;
  color: #2c3e50;
}

.campo {
  margin-bottom: 15px;
}

.campo label {
  display: block;
  margin-bottom: 6px;
  font-weight: bold;
  color: #333;
}

.campo input,
.campo select {
  width: 100%;
  padding: 8px;
  border: 1px solid #ccc;
  border-radius: 4px;
}

button {
  background-color: #f39c12;
  color: white;
  padding: 10px 18px;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  width: 100%;
}

.mensaje-error {
  color: #e74c3c;
  margin-top: 10px;
}

.mensaje-exito {
  color: #27ae60;
  margin-top: 10px;
}
</style>
