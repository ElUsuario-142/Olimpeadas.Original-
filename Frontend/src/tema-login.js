const botonTema = document.getElementById("theme-toggle");

if (botonTema) {

  if (localStorage.getItem("modoOscuro") === "true") {
    document.body.classList.add("dark-mode");
    botonTema.textContent = "☀️";
  }

  botonTema.addEventListener("click", function () {

    document.body.classList.toggle("dark-mode");

    const modoOscuro =
      document.body.classList.contains("dark-mode");

    localStorage.setItem("modoOscuro", modoOscuro);

    if (modoOscuro) {
      botonTema.textContent = "☀️";
    } else {
      botonTema.textContent = "🌙";
    }

  });

}