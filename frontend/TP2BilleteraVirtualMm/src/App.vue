<template>
  <div id="app">
    <nav class="navbar">
      <div class="nav-container">
        <h2 class="brand">🪙 Billetera Crypto</h2>
        <div class="nav-links">
          <router-link to="/nueva-compra">Nueva Compra</router-link>
          <router-link to="/historial">Historial</router-link>
        </div>
      </div>
    </nav>
    <main class="contenido">
      <div class="contenido-container">
        <router-view />
      </div>
    </main>
  </div>
</template>

<script>
export default {
  name: 'App'
}
</script>

<style>
/* Reset general y tipografía moderna */
* {
  margin: 0;
  padding: 0;
  box-sizing: border-box;
}

body {
  font-family: 'Segoe UI', Roboto, Helvetica, Arial, sans-serif;
  background-color: #f8fafc;
  /* Un gris/azul mucho más limpio y tecnológico */
  color: #1e293b;
  /* Texto principal oscuro pero suave */
  -webkit-font-smoothing: antialiased;
}

/* Contenedor principal que ocupa toda la pantalla */
#app {
  min-height: 100vh;
  display: flex;
  flex-direction: column;
}

/* Navbar que se expande al 100% de la pantalla de tu PC */
.navbar {
  background-color: #0f172a;
  /* Fondo Slate oscuro (muy usado en apps Crypto) */
  box-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.1), 0 2px 4px -2px rgba(0, 0, 0, 0.1);
  position: sticky;
  top: 0;
  z-index: 100;
}

/* Contenedor interno del Navbar para que el contenido no se pegue a las puntas en monitores anchos */
.nav-container {
  max-width: 1200px;
  /* Ancho ideal para pantallas de PC */
  margin: 0 auto;
  padding: 16px 24px;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.brand {
  font-size: 22px;
  font-weight: 700;
  color: #f8fafc;
  letter-spacing: -0.5px;
}

/* Enlaces de navegación estilo botones modernos */
.nav-links a {
  color: #94a3b8;
  /* Color apagado por defecto */
  text-decoration: none;
  font-weight: 500;
  margin-left: 16px;
  padding: 8px 16px;
  border-radius: 8px;
  transition: all 0.25s ease;
  /* Transición suave para el hover */
}

/* Efecto Hover */
.nav-links a:hover {
  color: #ffffff;
  background-color: #1e293b;
}

/* Vista activa (donde estás parado) - Azul Crypto eléctrico */
.nav-links a.router-link-active {
  color: #ffffff;
  background-color: #3b82f6;
  box-shadow: 0 4px 12px rgba(59, 130, 246, 0.3);
  /* Sutil brillo */
}

/* Sección de contenido */
.contenido {
  flex: 1;
  padding: 40px 24px;
}

/* Contenedor del contenido centrado y alineado con el Navbar */
.contenido-container {
  max-width: 1200px;
  margin: 0 auto;
  width: 100%;
}
</style>
