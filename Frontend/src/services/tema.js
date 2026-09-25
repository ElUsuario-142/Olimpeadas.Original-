const botonTema = document.getElementById("theme-toggle");

botonTema.addEventListener("click", () => {
  document.body.classList.toggle("dark-mode");

  if (document.body.classList.contains("dark-mode")) {
    botonTema.textContent = "☀️";
  } else {
    botonTema.textContent = "🌙";
  }
});