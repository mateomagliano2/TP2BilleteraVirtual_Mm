<template>
  <div class="historial">
    <div class="header">
       <h2>Listado de Clientes</h2>
   </div>
   <div class="controles">
    <button @click="cargarClientes" :disabled="cargando"> {{ cargando ? 'Cargando...' : 'Cargar Clientes' }}  </button>
    <button @click="$router.push('/cliente')" class="btn-nuevo">➕ Nuevo Cliente </button>
   </div>


    <p v-if="error" class="mensaje-error">{{ error }}</p>

    <div v-if="clientes.length > 0" class="tabla-container">
      <table>
        <thead>
          <tr>
            <th>ID</th>
            <th>Nombre</th>
            <th>Email</th>
            <th>Acciones</th>
          </tr>
        </thead>

        <tbody>
          <tr v-for="c in clientes" :key="c.id">
            <td>{{ c.id }}</td>
            <td>{{ c.name }}</td>
            <td>{{ c.email }}</td>
            <td class="acciones">
              <button class="btn-editar" @click="editarCliente(c)">Editar</button>
              <button class="btn-eliminar" @click="confirmarEliminar(c.id)">Eliminar</button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>

    <p v-else-if="!cargando" class="sin-datos">No hay clientes.</p>

    <div v-if="mostrarModal" class="modal-fondo" @click="cerrarModal">
      <div class="modal-contenido" @click.stop>
        <h3>Confirmar eliminación</h3>
        <p>¿Seguro que querés eliminar este cliente?</p>

        <div class="botones-modal">
          <button class="btn-cancelar" @click="cerrarModal">Cancelar</button>
          <button class="btn-confirmar" @click="eliminarCliente">Eliminar</button>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
const API_URL = 'http://localhost:5076/api'

export default {
  name: 'TablaClientes',
  data() {
    return {
      clientes: [],
      cargando: false,
      error: '',
      mostrarModal: false,
      idAEliminar: null
    }
  },
  mounted() {
    this.cargarClientes()
  },
  methods: {
    async cargarClientes() {
      this.cargando = true
      this.error = ''

      try {
        const res = await fetch(API_URL + '/clientesbv')
        this.clientes = await res.json()
      } catch (err) {
        console.log(err)
        this.error = 'Error al cargar clientes'
      } finally {
        this.cargando = false
      }
    },

    editarCliente(c) {
      this.$router.push('/cliente/editar/' + c.id)
    },

    confirmarEliminar(id) {
      this.idAEliminar = id
      this.mostrarModal = true
    },

    cerrarModal() {
      this.mostrarModal = false
      this.idAEliminar = null
    },

    async eliminarCliente() {
      try {
        await fetch(API_URL + '/clientesbv/' + this.idAEliminar, {
          method: 'DELETE'
        })

        this.clientes = this.clientes.filter(c => c.id !== this.idAEliminar)
        this.cerrarModal()
      } catch (err) {
        console.log(err)
        alert('No se pudo eliminar')
      }
    }
  }
}
</script>

<style scoped>
.historial {
  background-color: white;
  padding: 25px;
  border-radius: 8px;
  box-shadow: 0 2px 5px rgba(0,0,0,0.1);
}

h2 {
  margin-bottom: 20px;
  color: #2c3e50;
}

button {
  font-size: 14px;
  background-color: #3498db;
  color: white;
  padding: 8px 16px;
  border: none;
  border-radius: 4px;
  cursor: pointer;
}

button:hover {
  background-color: #2980b9;
}

button:disabled {
  background-color: #95a5a6;
}

.tabla-container {
  overflow-x: auto;
}

table {
  width: 100%;
  border-collapse: collapse;
}

thead {
  background-color: #2c3e50;
  color: white;
}

th, td {
  padding: 10px;
  border-bottom: 1px solid #ddd;
  text-align: left;
}

tr:hover {
  background-color: #f9f9f9;
}

.acciones {
  display: flex;
  gap: 6px;
}

.btn-editar {
  background-color: #f39c12;
}

.btn-eliminar {
  background-color: #e74c3c;
}

.btn-editar:hover {
  background-color: #e67e22;
}

.btn-eliminar:hover {
  background-color: #c0392b;
}

.mensaje-error {
  color: red;
}

.sin-datos {
  color: #999;
  margin-top: 10px;
}


.modal-fondo {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0,0,0,0.45);
  display: flex;
  justify-content: center;
  align-items: center;
}

.modal-contenido {
  background: white;
  padding: 25px;
  border-radius: 8px;
  min-width: 300px;
}

.botones-modal {
  display: flex;
  justify-content: flex-end;
  gap: 10px;
  margin-top: 20px;
}

.btn-cancelar {
  background-color: #95a5a6;
}

.btn-confirmar {
  background-color: #e74c3c;
}
.controles-top {
  margin-bottom: 15px;
  display: flex;
  justify-content: flex-end;
}

.btn-nuevo {
  background-color: #2ecc71;
  color: white;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  font-size: 14px;
}

.btn-nuevo:hover {
  background-color: #27ae60;
}

.header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 20px;
}
.controles {
  display: flex;
  margin-bottom: 15px;
  align-items: center;
  gap: 12px;
}
</style>
