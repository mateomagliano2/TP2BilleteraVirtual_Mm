<template>
  <div class="formulario">
    <h2>Alta de Cliente</h2>

    <form @submit.prevent="guardarCliente">

      <div class="campo">
        <label for="name">Nombre:</label>
        <input
          id="name"
          v-model="name"
          type="text"
          placeholder="Ej: Mateo Magliano"
        />
      </div>

      <div class="campo">
        <label for="email">Email:</label>
        <input
          id="email"
          v-model="email"
          type="email"
          placeholder="Ej: mateo@gmail.com"
        />
      </div>

      <p v-if="error" class="mensaje-error">{{ error }}</p>
      <p v-if="ok" class="mensaje-exito">Cliente creado ✅</p>

      <button type="submit" :disabled="cargando">
        {{ cargando ? 'Guardando...' : 'Guardar Cliente' }}
      </button>

    </form>
  </div>
</template>

<script>
import api from '../services/api.js'

export default {
  name: 'NuevoClienteView',
  data() {
    return {
      name: '',
      email: '',
      error: '',
      ok: false,
      cargando: false
    }
  },
  methods: {
    async guardarCliente() {
      this.error = ''
      this.ok = false

      if (!this.name) {
        this.error = 'El nombre es obligatorio'
        return
      }

      if (!this.email || !this.email.includes('@')) {
        this.error = 'Email inválido'
        return
      }

      try {
        this.cargando = true

        const res = await api.post('/clientesbv', {
          name: this.name,
          email: this.email
        })

        if (res.ok) {
          this.ok = true
          this.name = ''
          this.email = ''
        } else {
          this.error = 'Error al guardar'
        }
      } catch (e) {
        console.log(e)
        this.error = 'No conecta con backend'
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

.campo input {
  width: 100%;
  padding: 9px;
  border: 1px solid #ccc;
  border-radius: 4px;
  font-size: 14px;
}

button {
  background-color: #2ecc71;
  color: white;
  padding: 10px 18px;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  font-size: 15px;
  width: 100%;
}

button:hover {
  background-color: #27ae60;
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
</style>
