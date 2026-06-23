<template>
  <div class="contenedor">
   <div class="formulario">
    <form @submit.prevent="registrarCompra">


      <div class="campo">
        <label for="cliente">Cliente:</label>
        <select id="cliente" v-model="client_id" required>
          <option disabled :value="null">Seleccionar cliente</option>
          <option v-for="cliente in clientes" :key="cliente.id" :value="cliente.id">
            {{ cliente.name }} / {{ cliente.email }}
          </option>
        </select>
      </div>


      <div class="campo">
        <label for="crypto">Criptomoneda:</label>
        <select id="crypto" v-model="crypto_code" required>
          <option value="" disabled>Seleccionar cripto</option>
          <option value="bitcoin">Bitcoin</option>
          <option value="ethereum">Ethereum</option>
          <option value="usdc">USDC</option>
        </select>
      </div>

      <div class="campo">
        <label for="cantidad">Cantidad:</label>
        <input
          type="number"
          id="cantidad"
          v-model="crypto_amount"
          step="0.00001"
          min="0.00001"
          placeholder="Ej: 0.00070"
          required
        />
      </div>

      <div class="campo">
        <label for="fecha">Fecha y hora:</label>
        <input
          id="fecha"
          v-model="datetime"
          type="datetime-local"
          required
        />
      </div>

      <p v-if="error" class="mensaje-error">{{ error }}</p>
      <p v-if="exito" class="mensaje-exito">{{ exito }}</p>

      <button type="submit" :disabled="cargando">
        {{ cargando ? 'Guardando...' : 'Registrar Compra' }}
      </button>

    </form>
  </div>
  </div>
</template>

<script>import api from '../services/api'

export default {
  name: 'FormularioCompra',
  data() {
    return {
      clientes: [],
      client_id: null,
      crypto_code: '',
      crypto_amount: '',
      datetime: '',
      action: 'purchase',
      error: '',
      exito: '',
      cargando: false
    }
  },

  async mounted() {
    await this.cargarClientes()
    this.datetime = new Date().toISOString().slice(0, 16)
  },

  methods: {
    async cargarClientes() {
      try {
        const res = await api.get('/clientesbv')
        this.clientes = res.data
      } catch (err) {
        console.log(err)
        this.error = 'Error al conectar con la API de clientes'
      }
    },
    validarFormulario() {
      if (!this.client_id) return 'Debe seleccionar un cliente'

      if (!this.crypto_code) return 'Debe seleccionar una criptomoneda'

      if (!this.crypto_amount || parseFloat(this.crypto_amount) <= 0) {
        return 'La cantidad debe ser mayor a 0'
      }

      if (!this.datetime) return 'Debe ingresar fecha y hora'

      return null
    },

    async registrarCompra() {
      this.error = ''
      this.exito = ''

      const errorValidacion = this.validarFormulario()

      if (errorValidacion) {
        this.error = errorValidacion
        return
      }

      const datos = {
        crypto_code: this.crypto_code,
        action: 'purchase',
        client_id: this.client_id,
        crypto_amount: this.crypto_amount.toString(),
        datetime: this.datetime
      }

      try {
        this.cargando = true

        await api.post('/transactions', datos)

        this.exito = 'Compra registrada correctamente'

        this.client_id = null
        this.crypto_code = ''
        this.crypto_amount = ''
        this.datetime = new Date().toISOString().slice(0, 16)

      } catch (err) {
        console.log(err)

        if (err.response) {
          this.error = err.response.data || 'Error al guardar la compra'
        } else {
          this.error = 'No se pudo conectar con el backend'
        }
      } finally {
        this.cargando = false
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

.campo {
  margin-bottom: 15px;
}

.campo label {
  display: block;
  margin-bottom: 5px;
  font-weight: bold;
}

.campo select,
.campo input {
  width: 100%;
  padding: 8px;
  border: 1px solid #ccc;
  border-radius: 4px;
}

button {
  background-color: #3498db;
  color: white;
  padding: 10px 20px;
  border: none;
  border-radius: 4px;
  width: 100%;
  cursor: pointer;
}

button:disabled {
  background-color: #95a5a6;
}

.mensaje-error {
  color: red;
  margin-top: 10px;
}

.mensaje-exito {
  color: green;
  margin-top: 10px;
}
.contenedor {
  display: flex;
  justify-content: center;
  margin-top: 30px;
}
</style>
