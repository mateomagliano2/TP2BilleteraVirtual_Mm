<template>
  <div class="formulario">
    <form @submit.prevent="registrarCompra">


      <div class="campo">
        <label for="crypto">Criptomoneda:</label>
        <select id="crypto" v-model="crypto_code" required>
          <option value="" disabled>Seleccionar</option>
          <option value="bitcoin">Bitcoin (BTC)</option>
          <option value="ethereum">Ethereum (ETH)</option>
          <option value="usdc">USDC</option>
        </select>
      </div>


      <div class="campo">
        <label for="cantidad">Cantidad:</label>
        <input type="number" id="cantidad" v-model="crypto_amount" step="0.00001" min="0.00001"
          placeholder="Ej: 0.00070" required />
      </div>

      <div class="campo">
        <label for="fecha">Fecha y hora:</label>
        <input type="datetime-local" id="fecha" v-model="datetime" required />
      </div>

      <p v-if="error" class="mensaje-error">{{ error }}</p>
      <p v-if="exito" class="mensaje-exito">{{ exito }}</p>

      <button type="submit" :disabled="cargando">
        {{ cargando ? 'Guardando...' : 'Registrar Compra' }}
      </button>

    </form>
  </div>
</template>

<script>

const API_URL = 'http://localhost:5076/api' //acá la Api Master

export default {
  name: 'FormularioCompra',
  data() {
    return {
      crypto_code: '',
      crypto_amount: '',
      datetime: '',
      error: '',
      exito: '',
      cargando: false
    }
  },
  methods: {
    async registrarCompra() {

      this.error = ''
      this.exito = ''

      if (!this.crypto_code) {
        this.error = 'Seleccione una criptomoneda.'
        return
      }
      if (!this.crypto_amount || parseFloat(this.crypto_amount) <= 0) {
        this.error = 'La cantidad debe ser mayor a 0.'
        return
      }
      if (!this.datetime) {
        this.error = 'Ingrese la fecha y hora.'
        return
      }
      const datos = {
        crypto_code: this.crypto_code,
        action: 'purchase',
        client_id: 1,
        crypto_amount: this.crypto_amount.toString(),
        datetime: this.datetime
      }

      this.cargando = true
      console.log('Enviando datos:', datos)
      try {
        const response = await fetch(API_URL + '/transactions', {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify(datos)
        })

        if (response.ok) {
          this.exito = '¡Compra registrada correctamente!'
          this.crypto_code = ''
          this.crypto_amount = ''
          this.datetime = ''
        } else {
          this.error = 'Error al registrar la compra. Intente de nuevo.'
        }
      } catch (err) {
        console.log('Error de conexion:', err)
        this.error = 'No se pudo conectar con el servidor.'
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
  color: #333;
}

.campo select,
.campo input {
  width: 100%;
  padding: 8px;
  border: 1px solid #ccc;
  border-radius: 4px;
  font-size: 14px;
}

button {
  background-color: #3498db;
  color: white;
  padding: 10px 20px;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  font-size: 15px;
  width: 100%;
}

button:hover {
  background-color: #2980b9;
}

button:disabled {
  background-color: #95a5a6;
  cursor: not-allowed;
}

.mensaje-error {
  color: red;
  margin-bottom: 10px;
  font-size: 14px;
}

.mensaje-exito {
  color: green;
  margin-bottom: 10px;
  font-size: 14px;
}
</style>
