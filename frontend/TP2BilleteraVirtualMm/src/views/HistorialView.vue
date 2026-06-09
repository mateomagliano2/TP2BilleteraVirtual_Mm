<template>
  <div>
    <h2>Historial de Movimientos</h2>
    <TablaMovimientos />
  </div>
</template>

<script>
import TablaMovimientos from '../components/TablaMovimientos.vue'

export default {
  name: 'HistorialView',
  components: {
    TablaMovimientos
  }
}
</script>

<style scoped>
h2 {
  margin-bottom: 20px;
  color: #2c3e50;
}
</style>
