<template>
  <div class="historial">
    <div class="controles">
      <button @click="cargarMovimientos" :disabled="cargando">
        {{ cargando ? 'Cargando...' : 'Cargar Movimientos' }}
      </button>
    </div>
    <p v-if="error" class="mensaje-error">{{ error }}</p>

    <div v-if="movimientos.length > 0" class="tabla-container">
      <table>
        <thead>
          <tr>
            <th>ID</th>
            <th>Cliente</th>
            <th>Cripto</th>
            <th>Acción</th>
            <th>Cantidad</th>
            <th>Monto (ARS)</th>
            <th>Fecha</th>
            <th>Acciones</th>
          </tr>
        </thead>

        <tbody>
          <tr v-for="mov in movimientos" :key="mov.id">
            <td>{{ mov.id }}</td>
            <td>{{ mov.client_name || mov.client_id }}</td>
            <td>{{ mov.crypto_code.toUpperCase() }}</td>
            <td>{{ mov.action === 'purchase' ? 'Compra' : 'Venta' }}</td>
            <td>{{ mov.crypto_amount }}</td>
            <td>$ {{ formatearMonto(mov.money) }}</td>
            <td>{{ formatearFecha(mov.datetime) }}</td>
            <td class="acciones">
              <button class="btn-ver" @click="verDetalle(mov)">Ver</button>
              <button class="btn-editar" @click="editarMovimiento(mov)">Editar</button>
              <button class="btn-eliminar" @click="confirmarEliminar(mov.id)">Eliminar</button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>

    <p v-else-if="!cargando && consultado" class="sin-datos">
      No se encontraron movimientos.
    </p>
    <div v-if="mostrarModalEliminar" class="modal-fondo" @click="cerrarModalEliminar">
      <div class="modal-contenido" @click.stop>
        <h3>Confirmar eliminación</h3>
        <p>¿Seguro que quieres eliminar esta transacción?</p>

        <div class="botones-modal">
          <button class="btn-cancelar" @click="cerrarModalEliminar">Cancelar</button>
          <button class="btn-confirmar" @click="eliminarMovimientoConfirmado">Eliminar</button>
        </div>
      </div>
    </div>

  </div>
</template>

<script>
const API_URL = 'http://localhost:5076/api'

export default {
  name: 'TablaMovimientos',
  data() {
    return {
      movimientos: [],
      cargando: false,
      error: '',
      consultado: false,
      mostrarModalEliminar: false,
      idAEliminar: null
    }
  },
  mounted() {
    this.cargarMovimientos()
  },
  methods: {
    async cargarMovimientos() {
      this.cargando = true
      this.error = ''
      this.consultado = false

      try {
        const response = await fetch(API_URL + '/transactions')

        if (response.ok) {
          this.movimientos = await response.json()
        } else {
          this.error = 'Error al obtener los movimientos.'
        }
      } catch (err) {
        console.log('Error:', err)
        this.error = 'No se pudo conectar con el servidor.'
      } finally {
        this.cargando = false
        this.consultado = true
      }
    },

    formatearFecha(fecha) {
      if (!fecha) return '-'
      const d = new Date(fecha)
      return d.toLocaleString('es-AR')
    },

    formatearMonto(valor) {
      if (valor === null || valor === undefined || valor === '') return '-'
      return Number(valor).toLocaleString('es-AR', {
        minimumFractionDigits: 2,
        maximumFractionDigits: 2
      })
    },

    verDetalle(mov) {
      this.$router.push('/transaccion/' + mov.id)
    },

    editarMovimiento(mov) {
      this.$router.push('/transaccion/editar/' + mov.id)
    },

    confirmarEliminar(id) {
      this.idAEliminar = id
      this.mostrarModalEliminar = true
    },

    cerrarModalEliminar() {
      this.mostrarModalEliminar = false
      this.idAEliminar = null
    },

    async eliminarMovimientoConfirmado() {
      try {
        const response = await fetch(API_URL + '/transactions/' + this.idAEliminar, {
          method: 'DELETE'
        })

        if (response.ok) {
          this.movimientos = this.movimientos.filter(m => m.id !== this.idAEliminar)
          this.cerrarModalEliminar()
        } else {
          alert('Error al eliminar.')
        }
      } catch (err) {
        console.log('Error:', err)
        alert('No se pudo conectar con el servidor.')
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
  box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
}

.controles {
  margin-bottom: 20px;
}

.controles button {
  background-color: #3498db;
  color: white;
  padding: 8px 18px;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  font-size: 14px;
}

.controles button:hover {
  background-color: #2980b9;
}

.controles button:disabled {
  background-color: #95a5a6;
  cursor: not-allowed;
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

th,
td {
  padding: 10px 8px;
  text-align: left;
  border-bottom: 1px solid #ddd;
}

tr:hover {
  background-color: #f9f9f9;
}

.acciones {
  display: flex;
  gap: 5px;
  flex-wrap: wrap;
}

.btn-ver {
  background-color: #2ecc71;
  color: white;
  border: none;
  padding: 5px 10px;
  border-radius: 3px;
  cursor: pointer;
  font-size: 12px;
}

.btn-editar {
  background-color: #f39c12;
  color: white;
  border: none;
  padding: 5px 10px;
  border-radius: 3px;
  cursor: pointer;
  font-size: 12px;
}

.btn-eliminar {
  background-color: #e74c3c;
  color: white;
  border: none;
  padding: 5px 10px;
  border-radius: 3px;
  cursor: pointer;
  font-size: 12px;
}

.btn-ver:hover {
  background-color: #27ae60;
}

.btn-editar:hover {
  background-color: #e67e22;
}

.btn-eliminar:hover {
  background-color: #c0392b;
}

.mensaje-error {
  color: red;
  margin-bottom: 10px;
}

.sin-datos {
  color: #999;
  font-style: italic;
  margin-top: 10px;
}

/* Modal */
.modal-fondo {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.5);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 100;
}

.modal-contenido {
  background-color: white;
  padding: 25px;
  border-radius: 8px;
  min-width: 350px;
  box-shadow: 0 4px 10px rgba(0, 0, 0, 0.3);
}

.modal-contenido h3 {
  margin-bottom: 15px;
  color: #2c3e50;
}

.modal-contenido p {
  margin-bottom: 8px;
}

.botones-modal {
  margin-top: 15px;
  display: flex;
  justify-content: flex-end;
  gap: 10px;
}

.btn-cancelar {
  background-color: #95a5a6;
  color: white;
  padding: 8px 14px;
  border: none;
  border-radius: 4px;
  cursor: pointer;
}

.btn-confirmar {
  background-color: #e74c3c;
  color: white;
  padding: 8px 14px;
  border: none;
  border-radius: 4px;
  cursor: pointer;
}

.btn-cancelar:hover {
  background-color: #7f8c8d;
}

.btn-confirmar:hover {
  background-color: #c0392b;
}
</style>
