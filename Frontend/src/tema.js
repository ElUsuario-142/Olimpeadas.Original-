const botonTema = document.getElementById("theme-toggle");

// Revisar si el usuario ya había elegido modo oscuro
if (localStorage.getItem("modoOscuro") === "true") {
  document.body.classList.add("dark-mode");

  if (botonTema) {
    botonTema.textContent = "☀️";
  }
}

if (botonTema) {

  botonTema.addEventListener("click", () => {

    document.body.classList.toggle("dark-mode");

    const modoOscuro = document.body.classList.contains("dark-mode");

    localStorage.setItem("modoOscuro", modoOscuro);

    if (modoOscuro) {
      botonTema.textContent = "☀️";
    } else {
      botonTema.textContent = "🌙";
    }

  });

}