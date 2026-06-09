<template>
  <div>
    <h2>Nueva Compra de Criptomoneda</h2>
    <FormularioCompra />
  </div>
</template>

<script>
import FormularioCompra from '../components/FormularioCompra.vue'

export default {
  name: 'NuevaCompraView',
  components: {
    FormularioCompra
  }
}
</script>

<style scoped>
h2 {
  margin-bottom: 20px;
  color: #2c3e50;
}
</style>
