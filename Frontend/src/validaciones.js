// Constantes y expresiones regulares
const EMAIL_REGEX = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
const TELEFONO_REGEX = /^[0-9\s\+\-\(\)]{7,15}$/;

// Simulación de verificación de calles oficial de Morón
export async function validarCalleMoron(ubicacion) {
  if (!ubicacion.trim()) return "";
  
  // Simula latencia de respuesta de red
  await new Promise((resolve) => setTimeout(resolve, 400));

  const texto = ubicacion.toLowerCase();
  // Validamos que mencione alguna calle o referencia válida básica
  if (texto.length < 5) {
    return "Ingresá una dirección o referencia más precisa (mínimo 5 caracteres).";
  }

  return ""; // Sin error
}

// Función para validar un campo individual
export function validarCampo(nombre, valores) {
  const valor = valores[nombre];

  switch (nombre) {
    case "tipo":
      if (!valor) return "Seleccioná un tipo de incidente.";
      break;

    case "ubicacion":
      if (!valor || !valor.trim()) return "Ingresá la ubicación del problema.";
      break;

    case "descripcion":
      if (!valor || !valor.trim()) return "Ingresá una descripción de la situación.";
      if (valor.trim().length < 10) return "La descripción debe tener al menos 10 caracteres.";
      break;

    case "contacto":
      if (!valor || !valor.trim()) return "Ingresá un correo electrónico o teléfono de contacto.";
      const esEmail = EMAIL_REGEX.test(valor.trim());
      const esTelefono = TELEFONO_REGEX.test(valor.trim());
      if (!esEmail && !esTelefono) {
        return "Ingresá un email válido (ej: nombre@correo.com) o un teléfono.";
      }
      break;

    case "acepto":
      if (!valor) return "Debes aceptar los términos para enviar el reporte.";
      break;

    case "archivos":
      if (valor && valor.length > 0) {
        const pesoMaximo = 15 * 1024 * 1024; // 15 MB
        for (let i = 0; i < valor.length; i++) {
          if (valor[i].size > pesoMaximo) {
            return `El archivo "${valor[i].name}" supera el límite máximo de 15 MB.`;
          }
        }
      }
      break;

    default:
      break;
  }

  return "";
}

// Manejo del DOM cuando carga la página
document.addEventListener("DOMContentLoaded", () => {
  const form = document.getElementById("form-reporte");
  if (!form) return;

  const campos = {
    tipo: document.getElementById("tipo"),
    ubicacion: document.getElementById("ubicacion"),
    descripcion: document.getElementById("descripcion"),
    nombre: document.getElementById("nombre"),
    contacto: document.getElementById("contacto"),
    acepto: document.getElementById("acepto"),
    archivos: document.getElementById("archivos"),
  };

  let verificacionCalleId = 0;

  // Helper para mostrar error debajo del campo
  function mostrarError(elemento, mensaje) {
    if (!elemento) return;
    
    // Si es el checkbox o el input de archivos, buscamos el contenedor padre
    const contenedor = elemento.closest(".field") || elemento.closest(".upload")?.parentNode;
    if (!contenedor) return;

    limpiarError(elemento);

    if (mensaje) {
      const errorP = document.createElement("p");
      errorP.className = "field__error";
      errorP.id = `${elemento.id}-error`;
      errorP.setAttribute("role", "alert");
      errorP.textContent = mensaje;

      contenedor.appendChild(errorP);
      elemento.setAttribute("aria-invalid", "true");
      elemento.setAttribute("aria-describedby", errorP.id);
    }
  }

  // Helper para remover el error
  function limpiarError(elemento) {
    if (!elemento) return;
    const contenedor = elemento.closest(".field") || elemento.closest(".upload")?.parentNode;
    if (!contenedor) return;

    const errorPrevio = contenedor.querySelector(".field__error");
    if (errorPrevio) errorPrevio.remove();

    elemento.removeAttribute("aria-invalid");
    elemento.removeAttribute("aria-describedby");
  }

  // Obtener todos los valores del formulario
  function obtenerValores() {
    return {
      tipo: campos.tipo?.value || "",
      ubicacion: campos.ubicacion?.value || "",
      descripcion: campos.descripcion?.value || "",
      nombre: campos.nombre?.value || "",
      contacto: campos.contacto?.value || "",
      acepto: campos.acepto?.checked || false,
      archivos: campos.archivos?.files || [],
    };
  }

  // Eventos `blur` (al salir del campo)
  Object.entries(campos).forEach(([nombre, elemento]) => {
    if (!elemento) return;

    elemento.addEventListener("blur", async () => {
      const valores = obtenerValores();
      const error = validarCampo(nombre, valores);
      mostrarError(elemento, error);

      // Verificación asíncrona adicional para la calle en Morón
      if (nombre === "ubicacion" && !error) {
        const currentId = ++verificacionCalleId;
        const errorCalle = await validarCalleMoron(valores.ubicacion);
        if (currentId === verificacionCalleId) {
          mostrarError(campos.ubicacion, errorCalle);
        }
      }
    });

    // Revalidad al cambiar/escribir si ya hay un error visible
    elemento.addEventListener("input", () => {
      if (elemento.getAttribute("aria-invalid") === "true") {
        const error = validarCampo(nombre, obtenerValores());
        mostrarError(elemento, error);
      }
    });
  });

  // Mostrar el nombre de archivos seleccionados
  if (campos.archivos) {
    campos.archivos.addEventListener("change", () => {
      const label = campos.archivos.closest(".upload");
      let infoFiles = label.querySelector(".upload__files");

      if (campos.archivos.files.length > 0) {
        if (!infoFiles) {
          infoFiles = document.createElement("small");
          infoFiles.className = "upload__files";
          label.appendChild(infoFiles);
        }
        const nombres = Array.from(campos.archivos.files).map(f => f.name).join(", ");
        infoFiles.textContent = `Seleccionados: ${nombres}`;
      } else if (infoFiles) {
        infoFiles.remove();
      }

      const error = validarCampo("archivos", obtenerValores());
      mostrarError(campos.archivos, error);
    });
  }

  // Envío del formulario
  form.addEventListener("submit", async (e) => {
    e.preventDefault();

    const valores = obtenerValores();
    const nombresCampos = ["tipo", "ubicacion", "descripcion", "contacto", "acepto", "archivos"];
    let hayErrores = false;
    let primerCampoError = null;

    // Validar todos los campos obligatorios
    for (const nombre of nombresCampos) {
      const error = validarCampo(nombre, valores);
      if (error) {
        mostrarError(campos[nombre], error);
        hayErrores = true;
        if (!primerCampoError) primerCampoError = campos[nombre];
      }
    }

    // Validar la ubicación con la API si no tenía error previo
    if (!hayErrores) {
      const errorCalle = await validarCalleMoron(valores.ubicacion);
      if (errorCalle) {
        mostrarError(campos.ubicacion, errorCalle);
        hayErrores = true;
        primerCampoError = campos.ubicacion;
      }
    }

    if (hayErrores) {
      primerCampoError?.focus();
      return;
    }

    // Si todo está correcto
    const submitBtn = form.querySelector("button[type='submit']");
    const textoOriginal = submitBtn.textContent;

    try {
      submitBtn.disabled = true;
      submitBtn.textContent = "Enviando reporte...";

      console.log("Reporte enviado con éxito:", valores);

      // Muestra mensaje de confirmación
      let msgOk = form.querySelector(".form__ok");
      if (!msgOk) {
        msgOk = document.createElement("p");
        msgOk.className = "form__ok";
        msgOk.setAttribute("role", "status");
        form.appendChild(msgOk);
      }
      msgOk.textContent = "¡Listo! Recibimos tu reporte. Te vamos a avisar las novedades.";

      form.reset();
      const labelFiles = form.querySelector(".upload__files");
      if (labelFiles) labelFiles.remove();

    } finally {
      submitBtn.disabled = false;
      submitBtn.textContent = textoOriginal;
    }
  });
});