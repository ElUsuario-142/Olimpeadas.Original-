document.addEventListener("DOMContentLoaded", () => {
  const loginForm = document.getElementById("loginForm");
  const emailInput = document.getElementById("email");
  const passwordInput = document.getElementById("password");

  if (!loginForm) return;

  // Regex para validar formato de email
  const EMAIL_REGEX = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

  // Helper para mostrar/limpiar errores visuales
  const mostrarError = (inputEl, mensaje) => {
    let errorEl = inputEl.nextElementSibling;

    if (!errorEl || !errorEl.classList.contains("field__error")) {
      errorEl = document.createElement("p");
      errorEl.className = "field__error";
      errorEl.id = `${inputEl.id}-error`;
      errorEl.setAttribute("role", "alert");
      inputEl.parentNode.appendChild(errorEl);
    }

    errorEl.textContent = mensaje;
    inputEl.setAttribute("aria-invalid", "true");
    inputEl.setAttribute("aria-describedby", errorEl.id);
  };

  const limpiarError = (inputEl) => {
    const errorEl = inputEl.parentNode.querySelector(".field__error");
    if (errorEl) {
      errorEl.remove();
    }
    inputEl.removeAttribute("aria-invalid");
    inputEl.removeAttribute("aria-describedby");
  };

  // Validaciones individuales
  const validarEmail = () => {
    const valor = emailInput.value.trim();
    if (!valor) {
      mostrarError(emailInput, "Ingresá tu correo electrónico.");
      return false;
    }
    if (!EMAIL_REGEX.test(valor)) {
      mostrarError(emailInput, "Ingresá un correo electrónico válido.");
      return false;
    }
    limpiarError(emailInput);
    return true;
  };

  const validarPassword = () => {
    const valor = passwordInput.value;
    if (!valor) {
      mostrarError(passwordInput, "Ingresá tu contraseña.");
      return false;
    }
    if (valor.length < 6) {
      mostrarError(passwordInput, "La contraseña debe tener al menos 6 caracteres.");
      return false;
    }
    limpiarError(passwordInput);
    return true;
  };

  // Eventos al perder el foco (onBlur) y al escribir (onInput)
  emailInput.addEventListener("blur", validarEmail);
  passwordInput.addEventListener("blur", validarPassword);

  emailInput.addEventListener("input", () => {
    if (emailInput.getAttribute("aria-invalid") === "true") validarEmail();
  });

  passwordInput.addEventListener("input", () => {
    if (passwordInput.getAttribute("aria-invalid") === "true") validarPassword();
  });

  // Evento Submit
  loginForm.addEventListener("submit", async (e) => {
    e.preventDefault();

    const isEmailValido = validarEmail();
    const isPasswordValida = validarPassword();

    if (!isEmailValido || !isPasswordValida) {
      // Hace foco en el primer campo inválido
      if (!isEmailValido) emailInput.focus();
      else if (!isPasswordValida) passwordInput.focus();
      return;
    }

    const submitBtn = loginForm.querySelector("button[type='submit']");
    const textoOriginal = submitBtn.textContent;

    try {
      submitBtn.disabled = true;
      submitBtn.textContent = "Ingresando...";

      // Datos listos para enviar al backend
      const datosLogin = {
        email: emailInput.value.trim(),
        password: passwordInput.value,
        recordar: document.getElementById("remember")?.checked || false,
      };

      console.log("Datos de login listos para backend:", datosLogin);

      // TODO: Aquí va tu llamada fetch/axios a la API
      // await apiLogin(datosLogin);

    } catch (error) {
      console.error("Error en el inicio de sesión:", error);
    } finally {
      submitBtn.disabled = false;
      submitBtn.textContent = textoOriginal;
    }
  });
});