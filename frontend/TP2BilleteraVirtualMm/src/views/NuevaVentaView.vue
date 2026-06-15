<template>
  <div>
    <h2>Nueva Venta de Criptomoneda</h2>
    <FormularioVenta />
  </div>
</template>

<script>
import FormularioVenta from '../components/FormularioVenta.vue'

export default {
  name: 'NuevaVentaView',
  components: {
    FormularioVenta
  }
}
</script>

<style scoped>
h2 {
  margin-bottom: 20px;
  color: #2c3e50;
  margin: 0 auto;
  flex-direction: column;
  align-items: center;
  display: flex;
}
</style>
