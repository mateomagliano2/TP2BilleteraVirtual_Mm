<template>
  <div class="formulario" v-if="cliente">
    <h2>Editar Cliente</h2>

    <form @submit.prevent="guardar">

      <div class="campo">
        <label>Nombre:</label>
        <input
          v-model="cliente.name"
          type="text"
          placeholder="Nombre completo"
        />
      </div>

      <div class="campo">
        <label>Email:</label>
        <input
          v-model="cliente.email"
          type="email"
          placeholder="cliente@mail.com"
        />
      </div>

      <p v-if="error" class="mensaje-error">{{ error }}</p>
      <p v-if="ok" class="mensaje-exito">Cliente actualizado ✅</p>

      <button type="submit" :disabled="cargando">
        {{ cargando ? 'Guardando...' : 'Guardar Cambios' }}
      </button>

    </form>
  </div>

  <p v-else>Cargando...</p>
</template>

<script>
import api from '@/services/api'

export default {
  name: 'EditarClienteView',
  data() {
    return {
      cliente: null,
      ok: false,
      error: '',
      cargando: false
    }
  },

  async mounted() {
    const id = this.$route.params.id

    try {
      const res = await api.get('/clientesbv')
      const lista = res.data

      this.cliente = lista.find(c => c.id == id)

    } catch (err) {
      console.log(err)
      this.error = 'Error al cargar cliente'
    }
  },

  methods: {
    async guardar() {
      this.ok = false
      this.error = ''

      if (!this.cliente.name) {
        this.error = 'El nombre es obligatorio'
        return
      }

      if (!this.cliente.email || !this.cliente.email.includes('@')) {
        this.error = 'Email inválido'
        return
      }

      try {
        this.cargando = true

        await api.put(`/clientesbv/${this.cliente.id}`, this.cliente)

        // ✅ SI LLEGA ACÁ → OK
        this.ok = true

      } catch (err) {
        console.log(err)

        if (err.response) {
          this.error = err.response.data || 'Error al guardar cambios'
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
  background-color: #f39c12;
  color: white;
  padding: 10px 18px;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  font-size: 15px;
  width: 100%;
}

button:hover {
  background-color: #e67e22;
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
