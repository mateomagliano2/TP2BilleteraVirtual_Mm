<template>
  <div class="contenedor">
  <div class="formulario">
    <form @submit.prevent="registrarVenta">


      <div class="campo">
        <label for="cliente">Cliente:</label>
        <select id="cliente" v-model.number="client_id" required>
          <option disabled :value="null"> Seleccionar cliente </option>
          <option
            v-for="cliente in clientes"
            :key="cliente.id"
            :value="cliente.id"
          >
            {{ cliente.name }} - {{ cliente.email }}
          </option>
        </select>
      </div>

      <div class="campo">
        <label for="crypto">Criptomoneda:</label>
        <select id="crypto" v-model="crypto_code" required>
          <option disabled value=""> Seleccionar cripto </option>
          <option value="bitcoin">Bitcoin</option>
          <option value="ethereum">Ethereum</option>
          <option value="usdc">USDC</option>
        </select>
      </div>


      <div class="campo">
        <label for="cantidad">Cantidad:</label>
        <input
          id="cantidad"
          v-model="crypto_amount"
          type="number"
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
        {{ cargando ? 'Guardando...' : 'Registrar Venta' }}
      </button>
    </form>
  </div>
 </div>
</template>

<script>
const API_URL = 'http://localhost:5076/api'

export default {
  name: 'FormularioVenta',
  data() {
    return {
      clientes: [],
      client_id: null,
      crypto_code: '',
      crypto_amount: '',
      datetime: '',
      error: '',
      exito: '',
      cargando: false
    }
  },
  mounted() {
    this.cargarClientes()
    this.datetime = new Date().toISOString().slice(0, 16)
  },
  methods: {
    async cargarClientes() {
      try {
        const response = await fetch(API_URL + '/clientesbv')

        if (response.ok) {
          this.clientes = await response.json()
        } else {
          this.error = 'No se pudieron cargar los clientes'
        }
      } catch (err) {
        console.log(err)
        this.error = 'Error al conectar con la API de clientes'
      }
    },

    async registrarVenta() {
      this.error = ''
      this.exito = ''

      if (!this.client_id) {
        this.error = 'Debe seleccionar un cliente'
        return
      }

      if (!this.crypto_code) {
        this.error = 'Debe seleccionar una criptomoneda'
        return
      }

      if (!this.crypto_amount || parseFloat(this.crypto_amount) <= 0) {
        this.error = 'La cantidad debe ser mayor a 0'
        return
      }

      if (!this.datetime) {
        this.error = 'Debe ingresar fecha y hora'
        return
      }

      const datos = {
        crypto_code: this.crypto_code,
        action: 'sale',
        client_id: this.client_id,
        crypto_amount: this.crypto_amount.toString(),
        datetime: this.datetime
      }

      try {
        this.cargando = true

        const response = await fetch(API_URL + '/transactions', {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json'
          },
          body: JSON.stringify(datos)
        })

        if (response.ok) {
          this.exito = 'Venta registrada correctamente'
          this.client_id = null
          this.crypto_code = ''
          this.crypto_amount = ''
          this.datetime = new Date().toISOString().slice(0, 16)
        } else {
          const mensaje = await response.text()
          this.error = mensaje || 'Error al guardar la venta'
        }
      } catch (err) {
        console.log(err)
        this.error = 'No se pudo conectar con el backend'
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
  max-width: 550px;
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

.campo select,
.campo input {
  width: 100%;
  padding: 9px;
  border: 1px solid #ccc;
  border-radius: 4px;
  font-size: 14px;
}

button {
  background-color: #e67e22;
  color: white;
  padding: 10px 18px;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  font-size: 15px;
  width: 100%;
}

button:hover {
  background-color: #d35400;
}

button:disabled {
  background-color: #95a5a6;
  cursor: not-allowed;
}

.mensaje-error {
  color: #e74c3c;
  margin-top: 10px;
  font-size: 14px;
}

.mensaje-exito {
  color: #27ae60;
  margin-top: 10px;
  font-size: 14px;
}
.contenedor {
  display: flex;
  justify-content: center;
  margin-top: 30px;
}
</style>
