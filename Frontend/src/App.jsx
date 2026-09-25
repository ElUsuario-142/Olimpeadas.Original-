import React, { createContext, useContext, useEffect, useMemo, useState } from "react";
import { BrowserRouter, Routes, Route, Link, NavLink, Navigate, useLocation, useNavigate, useParams } from "react-router-dom";

/*
  OLIMPEADAS - FRONTEND COMPLETO EN UN SOLO ARCHIVO

  Este archivo reemplaza src/App.jsx.

  Dependencias utilizadas por tu proyecto actual:
    - react
    - react-dom
    - react-router-dom

  API .NET de tu repositorio:
    HTTPS: https://localhost:58432
    HTTP:  http://localhost:58433

  Si tu API corre en otro puerto, podés definir:
    VITE_API_URL=https://localhost:58432

  Importante:
    Tu backend exige JWT para crear/listar reportes y para los endpoints
    protegidos. Por eso el formulario de reportes pide iniciar sesión.
*/

/* =========================================================
   CONFIGURACIÓN
========================================================= */

const API_URL = import.meta.env.VITE_API_URL || "http://localhost:58433";

/* =========================================================
   CSS
   El CSS está dentro del mismo archivo para que no necesites
   crear archivos CSS adicionales.
========================================================= */

const styles = `
:root {
  --bg: #f5f7fa;
  --surface: #ffffff;
  --surface-2: #eef2f6;
  --text: #17202a;
  --muted: #64748b;
  --border: #dbe2ea;
  --primary: #d9381e;
  --primary-dark: #b92c17;
  --primary-soft: #fff0ed;
  --success: #198754;
  --warning: #d98b00;
  --danger: #c62828;
  --shadow: 0 12px 35px rgba(15, 23, 42, .08);
  --radius: 16px;
  --max: 1180px;
  font-family: Inter, system-ui, -apple-system, BlinkMacSystemFont, "Segoe UI", sans-serif;
}

* { box-sizing: border-box; }

html { scroll-behavior: smooth; }

body {
  margin: 0;
  background: var(--bg);
  color: var(--text);
}

button, input, textarea, select {
  font: inherit;
}

button, a {
  -webkit-tap-highlight-color: transparent;
}

a {
  color: inherit;
  text-decoration: none;
}

.container {
  width: min(calc(100% - 32px), var(--max));
  margin: 0 auto;
}

.header {
  position: sticky;
  top: 0;
  z-index: 50;
  background: rgba(255,255,255,.94);
  backdrop-filter: blur(14px);
  border-bottom: 1px solid var(--border);
}

.header__inner {
  min-height: 74px;
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 24px;
}

.brand {
  display: flex;
  align-items: center;
  gap: 12px;
  font-size: 14px;
}

.brand__logo {
  width: 42px;
  height: 42px;
  border-radius: 12px;
  display: grid;
  place-items: center;
  color: white;
  background: var(--primary);
  font-weight: 800;
  font-size: 21px;
}

.brand__text {
  display: flex;
  flex-direction: column;
  line-height: 1.15;
}

.brand__text small {
  color: var(--muted);
  margin-top: 4px;
}

.nav {
  display: flex;
  align-items: center;
  gap: 8px;
  flex-wrap: wrap;
  justify-content: flex-end;
}

.nav a:not(.btn), .nav button {
  padding: 9px 11px;
  border-radius: 9px;
  color: #475569;
  border: 0;
  background: transparent;
  cursor: pointer;
}

.nav a:not(.btn):hover, .nav button:hover {
  background: var(--surface-2);
  color: var(--text);
}

.btn {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  gap: 8px;
  min-height: 44px;
  padding: 10px 17px;
  border: 1px solid transparent;
  border-radius: 10px;
  cursor: pointer;
  font-weight: 700;
  transition: .18s ease;
}

.btn:disabled {
  opacity: .55;
  cursor: not-allowed;
}

.btn--primary {
  color: #fff;
  background: var(--primary);
}

.btn--primary:hover:not(:disabled) {
  background: var(--primary-dark);
  transform: translateY(-1px);
}

.btn--secondary {
  color: var(--text);
  background: white;
  border-color: var(--border);
}

.btn--secondary:hover:not(:disabled) {
  background: var(--surface-2);
}

.btn--danger {
  color: white;
  background: var(--danger);
}

.btn--success {
  color: white;
  background: var(--success);
}

.btn--sm {
  min-height: 36px;
  padding: 7px 12px;
  font-size: 14px;
}

.hero {
  padding: 92px 0 72px;
  background:
    radial-gradient(circle at 80% 20%, rgba(217,56,30,.13), transparent 35%),
    linear-gradient(180deg, #fff 0%, #f5f7fa 100%);
}

.hero h1 {
  max-width: 780px;
  margin: 0 0 18px;
  font-size: clamp(40px, 7vw, 72px);
  line-height: .98;
  letter-spacing: -2.5px;
}

.hero__lead {
  max-width: 700px;
  color: #526070;
  font-size: 19px;
  line-height: 1.65;
  margin: 0 0 26px;
}

.hero__note {
  color: var(--muted);
  font-size: 14px;
  margin-top: 14px;
}

.section {
  padding: 76px 0;
}

.section--white {
  background: white;
}

.section h2 {
  margin: 0 0 10px;
  font-size: clamp(28px, 4vw, 42px);
  letter-spacing: -1px;
}

.section__lead {
  margin: 0 0 30px;
  color: var(--muted);
  line-height: 1.6;
}

.eyebrow {
  display: inline-block;
  color: var(--primary);
  text-transform: uppercase;
  letter-spacing: 1.3px;
  font-size: 12px;
  font-weight: 800;
  margin-bottom: 8px;
}

.form {
  max-width: 780px;
  background: var(--surface);
  padding: 28px;
  border: 1px solid var(--border);
  border-radius: var(--radius);
  box-shadow: var(--shadow);
}

.field {
  display: flex;
  flex-direction: column;
  gap: 7px;
  margin-bottom: 18px;
}

.field label {
  font-weight: 700;
  font-size: 14px;
}

.field input,
.field textarea,
.field select {
  width: 100%;
  border: 1px solid #cfd8e3;
  background: #fff;
  color: var(--text);
  border-radius: 10px;
  padding: 12px 13px;
  outline: none;
}

.field textarea {
  min-height: 120px;
  resize: vertical;
}

.field input:focus,
.field textarea:focus,
.field select:focus {
  border-color: var(--primary);
  box-shadow: 0 0 0 3px rgba(217,56,30,.10);
}

.field__error {
  margin: 0;
  color: var(--danger);
  font-size: 13px;
}

.hint {
  color: var(--muted);
  font-size: 13px;
  margin: -6px 0 18px;
}

.form__row {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 16px;
}

.check {
  display: flex;
  align-items: flex-start;
  gap: 9px;
  color: #4b5563;
  font-size: 13px;
  line-height: 1.5;
  margin: 14px 0 20px;
}

.check input {
  margin-top: 3px;
}

.upload {
  border: 1.5px dashed #c6d0db;
  border-radius: 12px;
  padding: 20px;
  display: flex;
  flex-direction: column;
  align-items: center;
  text-align: center;
  gap: 6px;
  margin: 12px 0 20px;
  cursor: pointer;
}

.upload:hover {
  border-color: var(--primary);
  background: var(--primary-soft);
}

.upload small {
  color: var(--muted);
}

.form__ok {
  margin: 15px 0 0;
  padding: 12px 14px;
  border-radius: 10px;
  background: #e9f8ef;
  color: #166534;
}

.form__error {
  margin: 15px 0 0;
  padding: 12px 14px;
  border-radius: 10px;
  background: #fff0f0;
  color: #991b1b;
}

.overview {
  padding: 76px 0;
  background: #eef2f6;
}

.overview__inner {
  display: grid;
  grid-template-columns: .8fr 1.2fr;
  gap: 32px;
  align-items: center;
}

.stats {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 12px;
  margin: 24px 0 0;
}

.stat {
  padding: 18px;
  background: white;
  border: 1px solid var(--border);
  border-radius: 13px;
}

.stat strong {
  display: block;
  font-size: 30px;
  margin-bottom: 5px;
}

.stat span {
  color: var(--muted);
  font-size: 13px;
}

.map {
  height: 420px;
  overflow: hidden;
  border-radius: var(--radius);
  border: 1px solid var(--border);
  background: #dfe8ef;
  box-shadow: var(--shadow);
}

.map iframe {
  width: 100%;
  height: 100%;
  border: 0;
}

.news__grid {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 18px;
}

.card {
  background: white;
  border: 1px solid var(--border);
  border-radius: var(--radius);
  padding: 20px;
  box-shadow: 0 8px 22px rgba(15,23,42,.045);
}

.card h3 {
  margin: 0 0 9px;
  font-size: 18px;
}

.card p {
  color: #596779;
  line-height: 1.55;
  margin: 0 0 14px;
}

.card__header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  gap: 12px;
  margin-bottom: 13px;
}

.card__header small {
  color: var(--muted);
}

.badge {
  display: inline-flex;
  padding: 5px 9px;
  border-radius: 999px;
  font-size: 11px;
  font-weight: 800;
}

.badge--pending { background: #fff4d6; color: #9a6700; }
.badge--revision { background: #e9f0ff; color: #315da8; }
.badge--resolved { background: #e9f8ef; color: #166534; }
.badge--rejected { background: #ffe9e9; color: #991b1b; }
.badge--sent { background: #e9f0ff; color: #315da8; }
.badge--attended { background: #e9f8ef; color: #166534; }
.badge--cancelled { background: #ffe9e9; color: #991b1b; }

.cta {
  padding: 62px 0;
  background: #18212c;
  color: white;
}

.cta__inner {
  display: flex;
  justify-content: space-between;
  align-items: center;
  gap: 25px;
}

.cta h2 {
  margin: 0 0 8px;
}

.cta p {
  margin: 0;
  color: #c5ced8;
}

.emerg {
  background: #121a22;
  color: white;
}

.topbar {
  padding: 14px 0;
}

.topbar__inner {
  display: flex;
  justify-content: space-between;
  align-items: center;
  gap: 15px;
}

.topbar p {
  margin: 0;
}

.topbar a {
  color: #ffad9f;
  font-weight: 800;
}

.footer {
  background: #0d141b;
  color: white;
  padding: 45px 0;
}

.footer__inner {
  display: grid;
  grid-template-columns: 1.5fr repeat(3, 1fr);
  gap: 28px;
}

.footer h3, .footer h4 {
  margin-top: 0;
}

.footer p, .footer li {
  color: #aeb8c2;
  line-height: 1.6;
}

.footer ul {
  list-style: none;
  padding: 0;
  margin: 0;
}

.footer li {
  margin: 7px 0;
}

.auth-page {
  min-height: 100vh;
  display: grid;
  place-items: center;
  padding: 35px 16px;
  background:
    radial-gradient(circle at 10% 10%, rgba(217,56,30,.12), transparent 30%),
    var(--bg);
}

.auth-card {
  width: min(100%, 520px);
  background: white;
  border: 1px solid var(--border);
  border-radius: 20px;
  box-shadow: var(--shadow);
  padding: 30px;
}

.auth-card h1 {
  margin: 0 0 8px;
}

.auth-card > p {
  color: var(--muted);
  margin-top: 0;
  line-height: 1.5;
}

.auth-actions {
  display: flex;
  justify-content: space-between;
  gap: 12px;
  margin-top: 20px;
  align-items: center;
}

.auth-actions a {
  color: var(--primary);
  font-weight: 700;
  font-size: 14px;
}

.dashboard {
  padding: 45px 0 80px;
}

.dashboard-header {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  gap: 20px;
  margin-bottom: 28px;
}

.dashboard-header h1 {
  margin: 0 0 8px;
}

.dashboard-header p {
  margin: 0;
  color: var(--muted);
}

.dashboard-grid {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 18px;
}

.dashboard-card {
  background: white;
  border: 1px solid var(--border);
  border-radius: var(--radius);
  padding: 20px;
}

.dashboard-card strong {
  font-size: 30px;
  display: block;
}

.dashboard-card span {
  color: var(--muted);
  font-size: 13px;
}

.toolbar {
  display: flex;
  flex-wrap: wrap;
  gap: 10px;
  margin-bottom: 18px;
}

.toolbar select,
.toolbar input {
  padding: 10px 12px;
  border: 1px solid var(--border);
  border-radius: 9px;
  background: white;
}

.table-wrap {
  overflow-x: auto;
  border: 1px solid var(--border);
  border-radius: 14px;
  background: white;
}

.table {
  width: 100%;
  border-collapse: collapse;
  min-width: 760px;
}

.table th,
.table td {
  text-align: left;
  padding: 13px 15px;
  border-bottom: 1px solid #edf0f3;
  vertical-align: top;
}

.table th {
  font-size: 12px;
  color: var(--muted);
  text-transform: uppercase;
  letter-spacing: .6px;
}

.table tr:last-child td {
  border-bottom: 0;
}

.actions {
  display: flex;
  flex-wrap: wrap;
  gap: 7px;
}

.detail-grid {
  display: grid;
  grid-template-columns: 1.2fr .8fr;
  gap: 20px;
}

.info-list {
  display: grid;
  gap: 10px;
}

.info-item {
  padding: 13px;
  border-radius: 10px;
  background: var(--surface-2);
}

.info-item small {
  display: block;
  color: var(--muted);
  margin-bottom: 4px;
}

.timeline {
  display: grid;
  gap: 12px;
}

.timeline-item {
  border-left: 3px solid var(--primary);
  padding: 4px 0 4px 14px;
}

.timeline-item small {
  color: var(--muted);
}

.image-grid {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 10px;
}

.image-grid img {
  width: 100%;
  aspect-ratio: 1.5;
  object-fit: cover;
  border-radius: 10px;
  border: 1px solid var(--border);
}

.alert {
  padding: 12px 14px;
  border-radius: 10px;
  margin: 0 0 18px;
}

.alert--error {
  background: #fff0f0;
  color: #991b1b;
}

.alert--success {
  background: #e9f8ef;
  color: #166534;
}

.empty {
  padding: 35px 20px;
  text-align: center;
  color: var(--muted);
  background: white;
  border: 1px dashed var(--border);
  border-radius: 14px;
}

.loading {
  padding: 45px 20px;
  text-align: center;
  color: var(--muted);
}

.user-badge {
  padding: 7px 10px;
  background: var(--surface-2);
  border-radius: 9px;
  font-size: 13px;
  color: #475569;
}

@media (max-width: 900px) {
  .overview__inner,
  .detail-grid {
    grid-template-columns: 1fr;
  }

  .news__grid,
  .dashboard-grid {
    grid-template-columns: 1fr 1fr;
  }

  .footer__inner {
    grid-template-columns: 1fr 1fr;
  }
}

@media (max-width: 700px) {
  .header__inner {
    align-items: flex-start;
    padding: 12px 0;
    flex-direction: column;
  }

  .nav {
    justify-content: flex-start;
  }

  .hero {
    padding: 65px 0 55px;
  }

  .form__row,
  .news__grid,
  .dashboard-grid,
  .footer__inner,
  .stats {
    grid-template-columns: 1fr;
  }

  .cta__inner,
  .topbar__inner,
  .dashboard-header {
    flex-direction: column;
    align-items: flex-start;
  }

  .map {
    height: 330px;
  }
}
`;

/* =========================================================
   HELPERS API
========================================================= */

async function apiRequest(path, options = {}) {
  const token = localStorage.getItem("ol_token");

  const headers = {
    ...(options.body instanceof FormData ? {} : { "Content-Type": "application/json" }),
    ...(options.headers || {}),
  };

  if (token) {
    headers.Authorization = `Bearer ${token}`;
  }

  const response = await fetch(`${API_URL}${path}`, {
    ...options,
    headers,
  });

  if (response.status === 204) return null;

  const text = await response.text();
  let data = null;

  try {
    data = text ? JSON.parse(text) : null;
  } catch {
    data = text;
  }

  if (!response.ok) {
    let message = "Ocurrió un error en la solicitud.";

    if (data?.detail) message = data.detail;
    else if (data?.title) message = data.title;
    else if (data?.errors) {
      message = Object.values(data.errors).flat().join(" ");
    } else if (typeof data === "string" && data) {
      message = data;
    }

    const error = new Error(message);
    error.status = response.status;
    error.data = data;
    throw error;
  }

  return data;
}

const api = {
  login: (body) =>
    apiRequest("/api/Auth/login", {
      method: "POST",
      body: JSON.stringify(body),
    }),

  register: (body) =>
    apiRequest("/api/Auth/register", {
      method: "POST",
      body: JSON.stringify(body),
    }),

  profile: () => apiRequest("/api/Auth/perfil"),

  categorias: () => apiRequest("/api/Categorias"),
  municipios: () => apiRequest("/api/Municipios"),
  contactos: (municipioId) =>
    apiRequest(`/api/Contactos?municipioId=${encodeURIComponent(municipioId)}`),

  noticias: (municipioId) =>
    apiRequest(municipioId ? `/api/Noticias?municipioId=${municipioId}` : "/api/Noticias"),

  noticia: (id) => apiRequest(`/api/Noticias/${id}`),

  crearNoticia: (body) =>
    apiRequest("/api/Noticias", {
      method: "POST",
      body: JSON.stringify(body),
    }),

  vincularReporte: (id, body) =>
    apiRequest(`/api/Noticias/${id}/reportes`, {
      method: "POST",
      body: JSON.stringify(body),
    }),

  desvincularReporte: (id, reporteId) =>
    apiRequest(`/api/Noticias/${id}/reportes/${reporteId}`, {
      method: "DELETE",
    }),

  reportes: (query = "") => apiRequest(`/api/Reportes${query}`),
  misReportes: () => apiRequest("/api/Reportes/mios"),
  reportesAsignados: () => apiRequest("/api/Reportes/asignados"),
  reporte: (id) => apiRequest(`/api/Reportes/${id}`),
  historial: (id) => apiRequest(`/api/Reportes/${id}/historial`),

  crearReporte: (body) =>
    apiRequest("/api/Reportes", {
      method: "POST",
      body: JSON.stringify(body),
    }),

  asignarReporte: (id, empleadoId) =>
    apiRequest(`/api/Reportes/${id}/asignacion`, {
      method: "PUT",
      body: JSON.stringify({ empleadoId }),
    }),

  cambiarEstadoReporte: (id, nuevoEstado, comentario) =>
    apiRequest(`/api/Reportes/${id}/estado`, {
      method: "PATCH",
      body: JSON.stringify({ nuevoEstado, comentario }),
    }),

  agregarImagenReporte: (id, url) =>
    apiRequest(`/api/Reportes/${id}/imagenes`, {
      method: "POST",
      body: JSON.stringify({ url }),
    }),

  usuarios: (query = "") => apiRequest(`/api/Usuarios${query}`),
  usuario: (id) => apiRequest(`/api/Usuarios/${id}`),

  actualizarPerfil: (body) =>
    apiRequest("/api/Usuarios/me", {
      method: "PUT",
      body: JSON.stringify(body),
    }),

  actualizarUsuario: (id, body) =>
    apiRequest(`/api/Usuarios/${id}`, {
      method: "PUT",
      body: JSON.stringify(body),
    }),

  cambiarActivo: (id, activo) =>
    apiRequest(`/api/Usuarios/${id}/activo`, {
      method: "PATCH",
      body: JSON.stringify({ activo }),
    }),

  crearUsuario: (body) =>
    apiRequest("/api/Usuarios", {
      method: "POST",
      body: JSON.stringify(body),
    }),

  crearCategoria: (nombre) =>
    apiRequest("/api/Categorias", {
      method: "POST",
      body: JSON.stringify({ nombre }),
    }),

  alertasMias: () => apiRequest("/api/Alertas/mias"),
  alertas: (query = "") => apiRequest(`/api/Alertas${query}`),

  crearAlerta: (body) =>
    apiRequest("/api/Alertas", {
      method: "POST",
      body: JSON.stringify(body),
    }),

  cambiarEstadoAlerta: (id, estado) =>
    apiRequest(`/api/Alertas/${id}/estado`, {
      method: "PATCH",
      body: JSON.stringify({ estado }),
    }),
};

/* =========================================================
   AUTH CONTEXT
========================================================= */

const AuthContext = createContext(null);

function AuthProvider({ children }) {
  const [user, setUser] = useState(null);
  const [loading, setLoading] = useState(true);

  async function loadProfile() {
    const token = localStorage.getItem("ol_token");

    if (!token) {
      setUser(null);
      setLoading(false);
      return;
    }

    try {
      const profile = await api.profile();
      setUser(normalizeUser(profile));
    } catch {
      localStorage.removeItem("ol_token");
      setUser(null);
    } finally {
      setLoading(false);
    }
  }

  useEffect(() => {
    loadProfile();
  }, []);

  async function login(email, contraseña) {
    const response = await api.login({ email, contraseña });
    const token = response.token || response.Token || response.accessToken;

    if (!token) {
      throw new Error("La API no devolvió un token JWT.");
    }

    localStorage.setItem("ol_token", token);

    const profile = response.usuario || response.user || await api.profile();
    setUser(normalizeUser(profile));
    return profile;
  }

  async function register(data) {
    const response = await api.register(data);
    return response;
  }

  function logout() {
    localStorage.removeItem("ol_token");
    setUser(null);
  }

  const value = useMemo(
    () => ({ user, loading, login, register, logout, refresh: loadProfile }),
    [user, loading]
  );

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}

function useAuth() {
  return useContext(AuthContext);
}

function normalizeUser(user) {
  if (!user) return null;

  return {
    id: Number(user.id ?? user.Id),
    nombre: user.nombre ?? user.Nombre ?? "",
    apellido: user.apellido ?? user.Apellido ?? "",
    email: user.email ?? user.Email ?? "",
    dni: user.dni ?? user.DNI ?? "",
    rol: normalizeRole(user.rol ?? user.Rol),
    municipioId: Number(user.municipioId ?? user.MunicipioId ?? 0),
    telefono: user.telefono ?? user.Telefono ?? "",
    activo: user.activo ?? user.Activo ?? true,
  };
}

function normalizeRole(role) {
  if (typeof role === "number") {
    return ["Ciudadano", "Empleado", "Admin"][role] ?? "Ciudadano";
  }

  const value = String(role || "").toLowerCase();

  if (value.includes("admin")) return "Admin";
  if (value.includes("empleado")) return "Empleado";
  return "Ciudadano";
}

/* =========================================================
   ROUTES
========================================================= */

function ProtectedRoute({ children, roles }) {
  const { user, loading } = useAuth();

  if (loading) return <div className="loading">Cargando sesión...</div>;

  if (!user) return <Navigate to="/login" replace />;

  if (roles && !roles.includes(user.rol)) {
    return <Navigate to="/" replace />;
  }

  return children;
}

/* =========================================================
   LAYOUT
========================================================= */

function Header() {
  const { user, logout } = useAuth();

  return (
    <header className="header">
      <div className="container header__inner">
        <Link className="brand" to="/">
          <span className="brand__logo">M</span>
          <span className="brand__text">
            <strong>Municipio de Morón</strong>
            <small>Atención Ciudadana</small>
          </span>
        </Link>

        <nav className="nav">
          <Link to="/#noticias">Noticias</Link>
          <Link to="/#panorama">Panorama</Link>
          <Link to="/#emergencias">Emergencias</Link>

          {user ? (
            <>
              {user.rol === "Ciudadano" && (
                <NavLink to="/mis-reportes">Mis reportes</NavLink>
              )}

              {(user.rol === "Empleado" || user.rol === "Admin") && (
                <NavLink to="/panel">Panel</NavLink>
              )}

              <span className="user-badge">{user.nombre} · {user.rol}</span>

              <button className="btn btn--secondary btn--sm" onClick={logout}>
                Salir
              </button>
            </>
          ) : (
            <Link className="btn btn--primary btn--sm" to="/login">
              Iniciar sesión
            </Link>
          )}
        </nav>
      </div>
    </header>
  );
}

function Footer() {
  return (
    <footer className="footer">
      <div className="container footer__inner">
        <div>
          <h3>Municipio de Morón</h3>
          <p>Cerca de cada vecino y vecina. Atención ciudadana.</p>
        </div>

        <div>
          <h4>Atención</h4>
          <ul>
            <li><Link to="/#reporte">Hacer un reporte</Link></li>
            <li><Link to="/#emergencias">Emergencias</Link></li>
          </ul>
        </div>

        <div>
          <h4>Municipio</h4>
          <ul>
            <li><Link to="/#noticias">Noticias</Link></li>
            <li><Link to="/#panorama">Panorama</Link></li>
          </ul>
        </div>

        <div>
          <h4>Contacto</h4>
          <ul>
            <li>0800-666-6766</li>
            <li>atencion@moron.gob.ar</li>
            <li>Lun a vie · 8 a 18 hs</li>
          </ul>
        </div>
      </div>
    </footer>
  );
}

/* =========================================================
   HOME
========================================================= */

function Home() {
  const { user } = useAuth();
  const [categorias, setCategorias] = useState([]);
  const [noticias, setNoticias] = useState([]);
  const [reportes, setReportes] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    Promise.all([
      api.categorias(),
      api.noticias(),
      user ? api.reportes() : Promise.resolve([]),
    ])
      .then(([cats, news, reports]) => {
        setCategorias(cats || []);
        setNoticias(news || []);
        setReportes(reports || []);
      })
      .catch(() => {})
      .finally(() => setLoading(false));
  }, [user]);

  const resolved = reportes.filter((r) => String(r.estado).toLowerCase() === "resuelto").length;

  return (
    <>
      <Header />

      <main>
        <section className="hero">
          <div className="container">
            <h1>Tu municipio, más cerca.</h1>
            <p className="hero__lead">
              Informá problemas en la vía pública de forma simple. El equipo
              municipal recibirá tu solicitud y podrás seguir su avance.
            </p>

            {user ? (
              <a href="#reporte" className="btn btn--primary">
                Reportar un incidente
              </a>
            ) : (
              <Link to="/login" className="btn btn--primary">
                Iniciar sesión para reportar
              </Link>
            )}

            <p className="hero__note">
              Los reportes requieren una cuenta porque la API asocia cada reporte
              al usuario autenticado.
            </p>
          </div>
        </section>

        <section className="section" id="reporte">
          <div className="container">
            <span className="eyebrow">Atención ciudadana</span>
            <h2>Contanos qué está pasando</h2>
            <p className="section__lead">
              Completá los datos del incidente. Las categorías se cargan desde
              la API de .NET.
            </p>

            {user ? (
              <ReportForm categorias={categorias} />
            ) : (
              <div className="card">
                <h3>Necesitás iniciar sesión</h3>
                <p>
                  El backend de Olimpeadas exige autenticación JWT para crear
                  reportes.
                </p>
                <Link className="btn btn--primary" to="/login">
                  Iniciar sesión
                </Link>
              </div>
            )}
          </div>
        </section>

        <section className="overview" id="panorama">
          <div className="container overview__inner">
            <div>
              <span className="eyebrow">Datos del sistema</span>
              <h2>Panorama de incidencias</h2>
              <p className="section__lead">
                Cuando hay sesión iniciada, estos datos se obtienen de
                <code>GET /api/Reportes</code>.
              </p>

              <div className="stats">
                <div className="stat">
                  <strong>{loading ? "—" : reportes.length}</strong>
                  <span>Reportes cargados</span>
                </div>
                <div className="stat">
                  <strong>{loading ? "—" : resolved}</strong>
                  <span>Reportes resueltos</span>
                </div>
              </div>

              {!user && (
                <p className="hero__note">
                  Iniciá sesión para consultar los reportes protegidos por la API.
                </p>
              )}
            </div>

            <div className="map" aria-label="Mapa de Morón">
              <iframe
                title="Mapa de Morón"
                src="https://www.openstreetmap.org/export/embed.html?bbox=-58.68%2C-34.69%2C-58.52%2C-34.58&layer=mapnik"
              />
            </div>
          </div>
        </section>

        <section className="section" id="noticias">
          <div className="container">
            <span className="eyebrow">Información al vecino</span>
            <h2>Noticias y novedades locales</h2>
            <p className="section__lead">
              Las noticias se cargan desde <code>GET /api/Noticias</code>.
            </p>

            {noticias.length === 0 ? (
              <div className="empty">No hay noticias para mostrar.</div>
            ) : (
              <div className="news__grid">
                {noticias.slice(0, 6).map((noticia) => (
                  <article className="card" key={noticia.id}>
                    <div className="card__header">
                      <span className="badge badge--revision">
                        Información
                      </span>
                      <small>
                        {formatDate(noticia.fecha)}
                      </small>
                    </div>
                    <h3>{noticia.titulo}</h3>
                    <p>{noticia.contenido || "Sin contenido."}</p>
                    <Link className="btn btn--secondary btn--sm" to={`/noticias/${noticia.id}`}>
                      Ver noticia
                    </Link>
                  </article>
                ))}
              </div>
            )}
          </div>
        </section>

        <section className="cta">
          <div className="container cta__inner">
            <div>
              <h2>Construyendo juntos el futuro de Morón</h2>
              <p>Tu aviso puede mejorar la vida cotidiana de todo el barrio.</p>
            </div>

            {user ? (
              <a href="#reporte" className="btn btn--primary">
                Hacer un reporte
              </a>
            ) : (
              <Link to="/login" className="btn btn--primary">
                Iniciar sesión
              </Link>
            )}
          </div>
        </section>
      </main>

      <EmergencyBar />
      <Footer />
    </>
  );
}

/* =========================================================
   REPORT FORM
========================================================= */

function ReportForm({ categorias }) {
  const [form, setForm] = useState({
    categoriaId: "",
    calle: "",
    latitud: "-34.6534",
    longitud: "-58.6198",
    titulo: "",
    descripcion: "",
    imagenesUrls: "",
  });

  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");
  const [saving, setSaving] = useState(false);

  function change(e) {
    setForm((prev) => ({ ...prev, [e.target.name]: e.target.value }));
  }

  async function submit(e) {
    e.preventDefault();
    setError("");
    setSuccess("");

    if (!form.categoriaId) return setError("Seleccioná una categoría.");
    if (form.titulo.trim().length < 3) return setError("El título debe tener entre 3 y 50 caracteres.");
    if (form.descripcion.trim().length < 10) return setError("La descripción debe tener al menos 10 caracteres.");

    const urls = form.imagenesUrls
      .split("\n")
      .map((x) => x.trim())
      .filter(Boolean);

    if (urls.length > 5) {
      return setError("El backend permite hasta 5 imágenes por reporte.");
    }

    try {
      setSaving(true);

      await api.crearReporte({
        categoriaId: Number(form.categoriaId),
        calle: form.calle.trim() || null,
        latitud: Number(form.latitud),
        longitud: Number(form.longitud),
        titulo: form.titulo.trim(),
        descripcion: form.descripcion.trim(),
        imagenesUrls: urls.length ? urls : null,
      });

      setSuccess("Reporte creado correctamente.");
      setForm({
        categoriaId: "",
        calle: "",
        latitud: "-34.6534",
        longitud: "-58.6198",
        titulo: "",
        descripcion: "",
        imagenesUrls: "",
      });
    } catch (err) {
      setError(err.message);
    } finally {
      setSaving(false);
    }
  }

  return (
    <form className="form" onSubmit={submit}>
      <div className="field">
        <label>Categoría *</label>
        <select name="categoriaId" value={form.categoriaId} onChange={change}>
          <option value="">Seleccioná una categoría</option>
          {categorias.map((categoria) => (
            <option key={categoria.id} value={categoria.id}>
              {categoria.nombre}
            </option>
          ))}
        </select>
      </div>

      <div className="field">
        <label>Ubicación / calle</label>
        <input
          name="calle"
          value={form.calle}
          onChange={change}
          placeholder="Calle y altura o referencia"
          maxLength={100}
        />
      </div>

      <div className="form__row">
        <div className="field">
          <label>Latitud *</label>
          <input name="latitud" type="number" step="any" value={form.latitud} onChange={change} />
        </div>

        <div className="field">
          <label>Longitud *</label>
          <input name="longitud" type="number" step="any" value={form.longitud} onChange={change} />
        </div>
      </div>

      <p className="hint">
        El backend recibe coordenadas decimales. Podés conectar después este
        formulario con un selector de mapa.
      </p>

      <div className="field">
        <label>Título *</label>
        <input
          name="titulo"
          value={form.titulo}
          onChange={change}
          placeholder="Ej.: Bache peligroso"
          minLength={3}
          maxLength={50}
        />
      </div>

      <div className="field">
        <label>Descripción *</label>
        <textarea
          name="descripcion"
          value={form.descripcion}
          onChange={change}
          placeholder="Describí el problema y cualquier dato relevante."
          minLength={10}
          maxLength={2000}
        />
      </div>

      <div className="field">
        <label>URLs de imágenes</label>
        <textarea
          name="imagenesUrls"
          value={form.imagenesUrls}
          onChange={change}
          placeholder={"Una URL por línea.\nhttps://..."}
        />
        <span className="hint">
          Tu endpoint actual recibe URLs, no archivos multipart.
        </span>
      </div>

      <label className="check">
        <input type="checkbox" required />
        <span>Acepto que estos datos sean utilizados para gestionar el reporte.</span>
      </label>

      <button className="btn btn--primary" disabled={saving}>
        {saving ? "Enviando..." : "Enviar reporte"}
      </button>

      {error && <p className="form__error">{error}</p>}
      {success && <p className="form__ok">{success}</p>}
    </form>
  );
}

/* =========================================================
   LOGIN
========================================================= */

function Login() {
  const { user, login } = useAuth();
  const navigate = useNavigate();

  const [email, setEmail] = useState("");
  const [contraseña, setContraseña] = useState("");
  const [error, setError] = useState("");
  const [saving, setSaving] = useState(false);

  if (user) {
    return <Navigate to="/panel" replace />;
  }

  async function submit(e) {
    e.preventDefault();
    setError("");

    try {
      setSaving(true);
      const profile = await login(email, contraseña);
      const role = normalizeRole(profile?.rol ?? profile?.Rol);
      navigate(role === "Ciudadano" ? "/mis-reportes" : "/panel");
    } catch (err) {
      setError(err.message);
    } finally {
      setSaving(false);
    }
  }

  return (
    <div className="auth-page">
      <form className="auth-card" onSubmit={submit}>
        <Link className="brand" to="/">
          <span className="brand__logo">M</span>
          <span className="brand__text">
            <strong>Municipio de Morón</strong>
            <small>Olimpeadas</small>
          </span>
        </Link>

        <div style={{ height: 25 }} />

        <h1>Iniciar sesión</h1>
        <p>Ingresá para crear reportes y consultar tu información.</p>

        <div className="field">
          <label>Email</label>
          <input
            type="email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            required
          />
        </div>

        <div className="field">
          <label>Contraseña</label>
          <input
            type="password"
            value={contraseña}
            onChange={(e) => setContraseña(e.target.value)}
            required
          />
        </div>

        <button className="btn btn--primary" style={{ width: "100%" }} disabled={saving}>
          {saving ? "Ingresando..." : "Ingresar"}
        </button>

        {error && <p className="form__error">{error}</p>}

        <div className="auth-actions">
          <Link to="/">Volver al inicio</Link>
          <Link to="/registro">Crear una cuenta</Link>
        </div>
      </form>
    </div>
  );
}

/* =========================================================
   REGISTRO
========================================================= */

function Registro() {
  const { register } = useAuth();
  const navigate = useNavigate();

  const [municipios, setMunicipios] = useState([]);
  const [form, setForm] = useState({
    municipioId: "",
    nombre: "",
    apellido: "",
    dni: "",
    telefono: "",
    email: "",
    contraseña: "",
    rol: "Ciudadano",
  });

  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");
  const [saving, setSaving] = useState(false);

  useEffect(() => {
    api.municipios().then(setMunicipios).catch((err) => setError(err.message));
  }, []);

  function change(e) {
    setForm((prev) => ({ ...prev, [e.target.name]: e.target.value }));
  }

  async function submit(e) {
    e.preventDefault();
    setError("");
    setSuccess("");

    try {
      setSaving(true);

      await register({
        ...form,
        municipioId: Number(form.municipioId),
      });

      setSuccess("Cuenta creada correctamente. Ahora podés iniciar sesión.");

      setTimeout(() => navigate("/login"), 900);
    } catch (err) {
      setError(err.message);
    } finally {
      setSaving(false);
    }
  }

  return (
    <div className="auth-page">
      <form className="auth-card" onSubmit={submit}>
        <Link className="brand" to="/">
          <span className="brand__logo">M</span>
          <span className="brand__text">
            <strong>Municipio de Morón</strong>
            <small>Registro ciudadano</small>
          </span>
        </Link>

        <div style={{ height: 25 }} />

        <h1>Crear cuenta</h1>
        <p>Completá los datos que requiere CrearUsuarioDTO.</p>

        <div className="form__row">
          <div className="field">
            <label>Nombre</label>
            <input name="nombre" value={form.nombre} onChange={change} required minLength={2} maxLength={20} />
          </div>

          <div className="field">
            <label>Apellido</label>
            <input name="apellido" value={form.apellido} onChange={change} required minLength={2} maxLength={20} />
          </div>
        </div>

        <div className="field">
          <label>DNI</label>
          <input name="dni" value={form.dni} onChange={change} required pattern="\\d{7,8}" />
        </div>

        <div className="field">
          <label>Teléfono</label>
          <input name="telefono" value={form.telefono} onChange={change} required minLength={6} maxLength={14} />
        </div>

        <div className="field">
          <label>Email</label>
          <input type="email" name="email" value={form.email} onChange={change} required />
        </div>

        <div className="field">
          <label>Contraseña</label>
          <input type="password" name="contraseña" value={form.contraseña} onChange={change} required minLength={8} maxLength={72} />
        </div>

        <div className="field">
          <label>Municipio</label>
          <select name="municipioId" value={form.municipioId} onChange={change} required>
            <option value="">Seleccioná tu municipio</option>
            {municipios.map((m) => (
              <option key={m.id} value={m.id}>
                {m.nombre}
              </option>
            ))}
          </select>
        </div>

        <button className="btn btn--primary" style={{ width: "100%" }} disabled={saving}>
          {saving ? "Creando..." : "Crear cuenta"}
        </button>

        {error && <p className="form__error">{error}</p>}
        {success && <p className="form__ok">{success}</p>}

        <div className="auth-actions">
          <Link to="/">Volver</Link>
          <Link to="/login">Ya tengo una cuenta</Link>
        </div>
      </form>
    </div>
  );
}

/* =========================================================
   CIUDADANO
========================================================= */

function MisReportes() {
  const [reportes, setReportes] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  async function load() {
    try {
      setLoading(true);
      setError("");
      setReportes(await api.misReportes());
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  }

  useEffect(() => {
    load();
  }, []);

  return (
    <>
      <Header />

      <main className="dashboard">
        <div className="container">
          <div className="dashboard-header">
            <div>
              <h1>Mis reportes</h1>
              <p>Consultá los reportes creados con tu cuenta.</p>
            </div>
            <Link className="btn btn--primary" to="/#reporte">
              Crear reporte
            </Link>
          </div>

          {error && <div className="alert alert--error">{error}</div>}

          {loading ? (
            <div className="loading">Cargando reportes...</div>
          ) : reportes.length === 0 ? (
            <div className="empty">Todavía no tenés reportes.</div>
          ) : (
            <div className="news__grid">
              {reportes.map((reporte) => (
                <ReporteCard key={reporte.id} reporte={reporte} />
              ))}
            </div>
          )}
        </div>
      </main>

      <Footer />
    </>
  );
}

function ReporteCard({ reporte }) {
  return (
    <article className="card">
      <div className="card__header">
        <StatusBadge estado={reporte.estado} />
        <small>#{reporte.id}</small>
      </div>

      <h3>{reporte.titulo}</h3>
      <p>{reporte.descripcion}</p>

      <div className="info-list">
        <div className="info-item">
          <small>Categoría</small>
          {reporte.categoriaNombre || "Sin categoría"}
        </div>

        <div className="info-item">
          <small>Ubicación</small>
          {reporte.direccionCalle || "Sin calle"}
        </div>
      </div>

      <div style={{ marginTop: 15 }}>
        <Link className="btn btn--secondary btn--sm" to={`/reportes/${reporte.id}`}>
          Ver detalle
        </Link>
      </div>
    </article>
  );
}

function ReporteDetalle() {
  const { id } = useParams();
  const [reporte, setReporte] = useState(null);
  const [historial, setHistorial] = useState([]);
  const [error, setError] = useState("");

  useEffect(() => {
    Promise.all([api.reporte(id), api.historial(id)])
      .then(([r, h]) => {
        setReporte(r);
        setHistorial(h || r.historialEstados || []);
      })
      .catch((err) => setError(err.message));
  }, [id]);

  if (error) {
    return (
      <>
        <Header />
        <main className="dashboard container">
          <div className="alert alert--error">{error}</div>
        </main>
      </>
    );
  }

  if (!reporte) {
    return (
      <>
        <Header />
        <div className="loading">Cargando reporte...</div>
      </>
    );
  }

  return (
    <>
      <Header />

      <main className="dashboard">
        <div className="container">
          <Link to="/mis-reportes" className="btn btn--secondary btn--sm">
            ← Volver
          </Link>

          <div style={{ height: 20 }} />

          <div className="dashboard-header">
            <div>
              <StatusBadge estado={reporte.estado} />
              <h1>{reporte.titulo}</h1>
              <p>Reporte #{reporte.id} · {formatDate(reporte.fechaCreacion)}</p>
            </div>
          </div>

          <div className="detail-grid">
            <div className="card">
              <h3>Información</h3>

              <div className="info-list">
                <div className="info-item">
                  <small>Categoría</small>
                  {reporte.categoriaNombre || "—"}
                </div>

                <div className="info-item">
                  <small>Descripción</small>
                  {reporte.descripcion}
                </div>

                <div className="info-item">
                  <small>Dirección</small>
                  {reporte.direccionCalle || "—"}
                </div>

                <div className="info-item">
                  <small>Coordenadas</small>
                  {reporte.latitud}, {reporte.longitud}
                </div>

                <div className="info-item">
                  <small>Empleado asignado</small>
                  {reporte.empleadoEncargadoNombre || "Sin asignar"}
                </div>
              </div>

              {reporte.imagenesUrls?.length > 0 && (
                <>
                  <h3 style={{ marginTop: 25 }}>Imágenes</h3>
                  <div className="image-grid">
                    {reporte.imagenesUrls.map((url) => (
                      <img key={url} src={url} alt="Imagen del reporte" />
                    ))}
                  </div>
                </>
              )}
            </div>

            <div className="card">
              <h3>Historial</h3>

              {historial.length === 0 ? (
                <p>No hay movimientos registrados.</p>
              ) : (
                <div className="timeline">
                  {historial.map((item) => (
                    <div className="timeline-item" key={item.id}>
                      <strong>{item.estadoNuevo}</strong>
                      <div>{item.comentario || "Sin comentario."}</div>
                      <small>
                        {item.usuarioNombre || "Usuario"} · {formatDate(item.fecha)}
                      </small>
                    </div>
                  ))}
                </div>
              )}
            </div>
          </div>
        </div>
      </main>

      <Footer />
    </>
  );
}

/* =========================================================
   PANEL EMPLEADO / ADMIN
========================================================= */

function Panel() {
  const { user } = useAuth();

  if (user?.rol === "Admin") return <AdminPanel />;
  return <EmpleadoPanel />;
}

function EmpleadoPanel() {
  const [reportes, setReportes] = useState([]);
  const [noticias, setNoticias] = useState([]);
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(true);

  async function load() {
    try {
      setLoading(true);
      const [reports, news] = await Promise.all([
        api.reportesAsignados(),
        api.noticias(),
      ]);
      setReportes(reports || []);
      setNoticias(news || []);
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  }

  useEffect(() => {
    load();
  }, []);

  return (
    <>
      <Header />

      <main className="dashboard">
        <div className="container">
          <div className="dashboard-header">
            <div>
              <h1>Panel de empleado</h1>
              <p>Gestioná los reportes asignados a tu usuario.</p>
            </div>
          </div>

          {error && <div className="alert alert--error">{error}</div>}

          <div className="dashboard-grid">
            <div className="dashboard-card">
              <strong>{reportes.length}</strong>
              <span>Reportes asignados</span>
            </div>
            <div className="dashboard-card">
              <strong>{reportes.filter(r => String(r.estado).toLowerCase() === "pendiente").length}</strong>
              <span>Pendientes</span>
            </div>
            <div className="dashboard-card">
              <strong>{reportes.filter(r => String(r.estado).toLowerCase() === "resuelto").length}</strong>
              <span>Resueltos</span>
            </div>
          </div>

          <section className="section">
            <span className="eyebrow">Gestión</span>
            <h2>Reportes asignados</h2>

            {loading ? (
              <div className="loading">Cargando...</div>
            ) : (
              <div className="news__grid">
                {reportes.map((r) => (
                  <EmpleadoReporteCard
                    key={r.id}
                    reporte={r}
                    onUpdated={load}
                  />
                ))}
              </div>
            )}
          </section>

          <EmpleadoNoticias />
        </div>
      </main>

      <Footer />
    </>
  );
}

function EmpleadoReporteCard({ reporte, onUpdated }) {
  const [estado, setEstado] = useState(String(reporte.estado));
  const [comentario, setComentario] = useState("");
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState("");

  async function updateStatus() {
    setSaving(true);
    setError("");

    try {
      await api.cambiarEstadoReporte(reporte.id, estado, comentario || null);
      setComentario("");
      await onUpdated();
    } catch (err) {
      setError(err.message);
    } finally {
      setSaving(false);
    }
  }

  return (
    <article className="card">
      <div className="card__header">
        <StatusBadge estado={reporte.estado} />
        <small>#{reporte.id}</small>
      </div>

      <h3>{reporte.titulo}</h3>
      <p>{reporte.descripcion}</p>

      <div className="field">
        <label>Nuevo estado</label>
        <select value={estado} onChange={(e) => setEstado(e.target.value)}>
          <option value="Pendiente">Pendiente</option>
          <option value="Revision">Revision</option>
          <option value="Resuelto">Resuelto</option>
          <option value="Rechazado">Rechazado</option>
        </select>
      </div>

      <div className="field">
        <label>Comentario</label>
        <textarea
          value={comentario}
          onChange={(e) => setComentario(e.target.value)}
          placeholder="Comentario opcional"
        />
      </div>

      <button className="btn btn--primary" disabled={saving} onClick={updateStatus}>
        {saving ? "Guardando..." : "Actualizar estado"}
      </button>

      {error && <p className="form__error">{error}</p>}
    </article>
  );
}

function EmpleadoNoticias() {
  const { user } = useAuth();
  const [titulo, setTitulo] = useState("");
  const [contenido, setContenido] = useState("");
  const [municipioId, setMunicipioId] = useState(user?.municipioId || "");
  const [reportesIds, setReportesIds] = useState("");
  const [message, setMessage] = useState("");

  async function submit(e) {
    e.preventDefault();
    setMessage("");

    try {
      await api.crearNoticia({
        municipioId: Number(municipioId),
        titulo,
        contenido,
        reportesIds: reportesIds
          .split(",")
          .map((x) => Number(x.trim()))
          .filter(Boolean),
      });

      setTitulo("");
      setContenido("");
      setReportesIds("");
      setMessage("Noticia creada correctamente.");
    } catch (err) {
      setMessage(err.message);
    }
  }

  return (
    <section className="section">
      <span className="eyebrow">Contenido</span>
      <h2>Crear noticia</h2>

      <form className="form" onSubmit={submit}>
        <div className="field">
          <label>Municipio ID</label>
          <input value={municipioId} onChange={(e) => setMunicipioId(e.target.value)} type="number" required />
        </div>

        <div className="field">
          <label>Título</label>
          <input value={titulo} onChange={(e) => setTitulo(e.target.value)} minLength={5} maxLength={150} required />
        </div>

        <div className="field">
          <label>Contenido</label>
          <textarea value={contenido} onChange={(e) => setContenido(e.target.value)} maxLength={150} />
        </div>

        <div className="field">
          <label>IDs de reportes vinculados</label>
          <input
            value={reportesIds}
            onChange={(e) => setReportesIds(e.target.value)}
            placeholder="12, 15, 20"
          />
        </div>

        <button className="btn btn--primary">Publicar noticia</button>

        {message && <p className="form__ok">{message}</p>}
      </form>
    </section>
  );
}

/* =========================================================
   ADMIN
========================================================= */

function AdminPanel() {
  const [tab, setTab] = useState("reportes");

  return (
    <>
      <Header />

      <main className="dashboard">
        <div className="container">
          <div className="dashboard-header">
            <div>
              <h1>Panel de administración</h1>
              <p>Administración de reportes, usuarios, noticias y categorías.</p>
            </div>
          </div>

          <div className="toolbar">
            <button className={`btn ${tab === "reportes" ? "btn--primary" : "btn--secondary"}`} onClick={() => setTab("reportes")}>
              Reportes
            </button>
            <button className={`btn ${tab === "usuarios" ? "btn--primary" : "btn--secondary"}`} onClick={() => setTab("usuarios")}>
              Usuarios
            </button>
            <button className={`btn ${tab === "categorias" ? "btn--primary" : "btn--secondary"}`} onClick={() => setTab("categorias")}>
              Categorías
            </button>
            <button className={`btn ${tab === "noticias" ? "btn--primary" : "btn--secondary"}`} onClick={() => setTab("noticias")}>
              Noticias
            </button>
            <button className={`btn ${tab === "alertas" ? "btn--primary" : "btn--secondary"}`} onClick={() => setTab("alertas")}>
              Alertas
            </button>
          </div>

          {tab === "reportes" && <AdminReportes />}
          {tab === "usuarios" && <AdminUsuarios />}
          {tab === "categorias" && <AdminCategorias />}
          {tab === "noticias" && <AdminNoticias />}
          {tab === "alertas" && <AdminAlertas />}
        </div>
      </main>

      <Footer />
    </>
  );
}

function AdminReportes() {
  const [reportes, setReportes] = useState([]);
  const [empleados, setEmpleados] = useState([]);
  const [filtro, setFiltro] = useState("");
  const [error, setError] = useState("");

  async function load() {
    try {
      const query = filtro ? `?estado=${encodeURIComponent(filtro)}` : "";
      const [r, e] = await Promise.all([
        api.reportes(query),
        api.usuarios("?rol=Empleado"),
      ]);

      setReportes(r || []);
      setEmpleados(e || []);
    } catch (err) {
      setError(err.message);
    }
  }

  useEffect(() => {
    load();
  }, [filtro]);

  async function assign(id, empleadoId) {
    if (!empleadoId) return;

    try {
      await api.asignarReporte(id, Number(empleadoId));
      await load();
    } catch (err) {
      setError(err.message);
    }
  }

  return (
    <section>
      {error && <div className="alert alert--error">{error}</div>}

      <div className="toolbar">
        <select value={filtro} onChange={(e) => setFiltro(e.target.value)}>
          <option value="">Todos los estados</option>
          <option value="Pendiente">Pendiente</option>
          <option value="Revision">Revision</option>
          <option value="Resuelto">Resuelto</option>
          <option value="Rechazado">Rechazado</option>
        </select>
      </div>

      <div className="table-wrap">
        <table className="table">
          <thead>
            <tr>
              <th>ID</th>
              <th>Título</th>
              <th>Categoría</th>
              <th>Estado</th>
              <th>Empleado</th>
            </tr>
          </thead>

          <tbody>
            {reportes.map((r) => (
              <tr key={r.id}>
                <td>#{r.id}</td>
                <td>{r.titulo}</td>
                <td>{r.categoriaNombre || "—"}</td>
                <td><StatusBadge estado={r.estado} /></td>
                <td>
                  <select
                    value={r.empleadoEncargadoId || ""}
                    onChange={(e) => assign(r.id, e.target.value)}
                  >
                    <option value="">Sin asignar</option>
                    {empleados.map((e) => (
                      <option key={e.id} value={e.id}>
                        {e.nombre} {e.apellido}
                      </option>
                    ))}
                  </select>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </section>
  );
}

function AdminUsuarios() {
  const [usuarios, setUsuarios] = useState([]);
  const [error, setError] = useState("");

  async function load() {
    try {
      setUsuarios(await api.usuarios());
    } catch (err) {
      setError(err.message);
    }
  }

  useEffect(() => {
    load();
  }, []);

  async function toggle(user) {
    try {
      await api.cambiarActivo(user.id, !user.activo);
      await load();
    } catch (err) {
      setError(err.message);
    }
  }

  return (
    <section>
      {error && <div className="alert alert--error">{error}</div>}

      <div className="table-wrap">
        <table className="table">
          <thead>
            <tr>
              <th>Usuario</th>
              <th>Email</th>
              <th>DNI</th>
              <th>Rol</th>
              <th>Activo</th>
              <th>Acción</th>
            </tr>
          </thead>

          <tbody>
            {usuarios.map((u) => (
              <tr key={u.id}>
                <td>{u.nombre} {u.apellido}</td>
                <td>{u.email}</td>
                <td>{u.dni}</td>
                <td>{normalizeRole(u.rol)}</td>
                <td>{u.activo ? "Sí" : "No"}</td>
                <td>
                  <button
                    className={`btn btn--sm ${u.activo ? "btn--danger" : "btn--success"}`}
                    onClick={() => toggle(u)}
                  >
                    {u.activo ? "Desactivar" : "Activar"}
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </section>
  );
}

function AdminCategorias() {
  const [categorias, setCategorias] = useState([]);
  const [nombre, setNombre] = useState("");
  const [message, setMessage] = useState("");

  async function load() {
    setCategorias(await api.categorias());
  }

  useEffect(() => {
    load().catch((err) => setMessage(err.message));
  }, []);

  async function submit(e) {
    e.preventDefault();

    try {
      await api.crearCategoria(nombre);
      setNombre("");
      setMessage("Categoría creada.");
      await load();
    } catch (err) {
      setMessage(err.message);
    }
  }

  return (
    <section>
      <form className="form" onSubmit={submit} style={{ marginBottom: 25 }}>
        <div className="field">
          <label>Nueva categoría</label>
          <input value={nombre} onChange={(e) => setNombre(e.target.value)} minLength={3} maxLength={30} required />
        </div>

        <button className="btn btn--primary">Crear categoría</button>

        {message && <p className="form__ok">{message}</p>}
      </form>

      <div className="news__grid">
        {categorias.map((c) => (
          <div className="card" key={c.id}>
            <strong>#{c.id}</strong>
            <h3>{c.nombre}</h3>
          </div>
        ))}
      </div>
    </section>
  );
}

function AdminNoticias() {
  const [noticias, setNoticias] = useState([]);
  const [error, setError] = useState("");

  useEffect(() => {
    api.noticias().then(setNoticias).catch((err) => setError(err.message));
  }, []);

  return (
    <section>
      {error && <div className="alert alert--error">{error}</div>}

      <div className="news__grid">
        {noticias.map((n) => (
          <article className="card" key={n.id}>
            <div className="card__header">
              <span className="badge badge--revision">#{n.id}</span>
              <small>{formatDate(n.fecha)}</small>
            </div>
            <h3>{n.titulo}</h3>
            <p>{n.contenido || "Sin contenido."}</p>
            <small>
              Reportes vinculados: {n.registrosVinculados?.length || 0}
            </small>
          </article>
        ))}
      </div>
    </section>
  );
}

function AdminAlertas() {
  const [alertas, setAlertas] = useState([]);
  const [error, setError] = useState("");

  async function load() {
    try {
      setAlertas(await api.alertas());
    } catch (err) {
      setError(err.message);
    }
  }

  useEffect(() => {
    load();
  }, []);

  async function changeState(id, estado) {
    try {
      await api.cambiarEstadoAlerta(id, estado);
      await load();
    } catch (err) {
      setError(err.message);
    }
  }

  return (
    <section>
      {error && <div className="alert alert--error">{error}</div>}

      <div className="table-wrap">
        <table className="table">
          <thead>
            <tr>
              <th>ID</th>
              <th>Tipo</th>
              <th>Fecha</th>
              <th>Estado</th>
              <th>Acción</th>
            </tr>
          </thead>

          <tbody>
            {alertas.map((a) => (
              <tr key={a.id}>
                <td>#{a.id}</td>
                <td>{a.tipo}</td>
                <td>{formatDate(a.fecha)}</td>
                <td><StatusBadge estado={a.estado} alerta /></td>
                <td>
                  <select
                    value={a.estado}
                    onChange={(e) => changeState(a.id, e.target.value)}
                  >
                    <option value="Enviada">Enviada</option>
                    <option value="Atendida">Atendida</option>
                    <option value="Cancelada">Cancelada</option>
                  </select>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </section>
  );
}

/* =========================================================
   NOTICIAS
========================================================= */

function NoticiaDetalle() {
  const { id } = useParams();
  const [noticia, setNoticia] = useState(null);
  const [error, setError] = useState("");

  useEffect(() => {
    api.noticia(id)
      .then(setNoticia)
      .catch((err) => setError(err.message));
  }, [id]);

  return (
    <>
      <Header />

      <main className="dashboard">
        <div className="container">
          {error && <div className="alert alert--error">{error}</div>}

          {!noticia ? (
            <div className="loading">Cargando noticia...</div>
          ) : (
            <article className="card">
              <span className="eyebrow">Noticias</span>
              <h1>{noticia.titulo}</h1>
              <p>{noticia.contenido}</p>
              <small>
                Publicada el {formatDate(noticia.fecha)}
              </small>

              {noticia.registrosVinculados?.length > 0 && (
                <>
                  <h3 style={{ marginTop: 30 }}>Reportes relacionados</h3>
                  <div className="news__grid">
                    {noticia.registrosVinculados.map((r) => (
                      <div className="card" key={r.id}>
                        <strong>Reporte #{r.reporteId}</strong>
                        <h3>{r.reporteTitulo || "Reporte"}</h3>
                        <p>{r.comentario || "Sin comentario."}</p>
                      </div>
                    ))}
                  </div>
                </>
              )}
            </article>
          )}
        </div>
      </main>

      <Footer />
    </>
  );
}

/* =========================================================
   EMERGENCIAS
========================================================= */

function EmergencyBar() {
  return (
    <section className="emerg" id="emergencias">
      <div className="topbar">
        <div className="container topbar__inner">
          <p>¿Hay riesgo inmediato para una persona o propiedad?</p>
          <a
            href="https://www.argentina.gob.ar/tema/emergencias"
            target="_blank"
            rel="noreferrer"
          >
            Emergencias · 911 · SAME 107 · Bomberos 100
          </a>
        </div>
      </div>
    </section>
  );
}

/* =========================================================
   PANIC BUTTON
========================================================= */

function BotonPanico() {
  const { user } = useAuth();
  const [tipo, setTipo] = useState("Policia");
  const [message, setMessage] = useState("");
  const [sending, setSending] = useState(false);

  async function send() {
    if (!navigator.geolocation) {
      setMessage("Tu navegador no permite obtener la ubicación.");
      return;
    }

    setSending(true);
    setMessage("");

    navigator.geolocation.getCurrentPosition(
      async (position) => {
        try {
          await api.crearAlerta({
            tipo,
            latitud: position.coords.latitude,
            longitud: position.coords.longitude,
          });

          setMessage("Alerta enviada correctamente.");
        } catch (err) {
          setMessage(err.message);
        } finally {
          setSending(false);
        }
      },
      () => {
        setMessage("No se pudo obtener tu ubicación.");
        setSending(false);
      }
    );
  }

  return (
    <>
      <Header />

      <main className="dashboard">
        <div className="container">
          <div className="card" style={{ maxWidth: 700, margin: "0 auto" }}>
            <span className="eyebrow">Emergencia</span>
            <h1>Botón de alerta</h1>
            <p>
              La API registra el tipo y las coordenadas obtenidas por el
              navegador. La alerta requiere autenticación.
            </p>

            <div className="field">
              <label>Tipo de emergencia</label>
              <select value={tipo} onChange={(e) => setTipo(e.target.value)}>
                <option value="Policia">Policía</option>
                <option value="Ambulancia">Ambulancia</option>
                <option value="Bombero">Bomberos</option>
              </select>
            </div>

            <button className="btn btn--danger" onClick={send} disabled={sending || !user}>
              {sending ? "Enviando..." : "Enviar alerta"}
            </button>

            {message && <p className="form__ok">{message}</p>}
          </div>
        </div>
      </main>

      <Footer />
    </>
  );
}

/* =========================================================
   STATUS / UTILIDADES
========================================================= */

function StatusBadge({ estado, alerta = false }) {
  const value = String(estado ?? "");

  let className = "badge ";

  if (alerta) {
    if (value === "Atendida") className += "badge--attended";
    else if (value === "Cancelada") className += "badge--cancelled";
    else className += "badge--sent";
  } else {
    if (value === "Resuelto") className += "badge--resolved";
    else if (value === "Rechazado") className += "badge--rejected";
    else if (value === "Revision") className += "badge--revision";
    else className += "badge--pending";
  }

  return <span className={className}>{value}</span>;
}

function formatDate(value) {
  if (!value) return "—";

  try {
    return new Intl.DateTimeFormat("es-AR", {
      dateStyle: "medium",
      timeStyle: "short",
    }).format(new Date(value));
  } catch {
    return String(value);
  }
}

/* =========================================================
   APP
========================================================= */

function App() {
  const [dark, setDark] = useState(() => localStorage.getItem("ol_dark") === "true");

  useEffect(() => {
    localStorage.setItem("ol_dark", String(dark));

    document.documentElement.style.setProperty("--bg", dark ? "#111827" : "#f5f7fa");
    document.documentElement.style.setProperty("--surface", dark ? "#18212c" : "#ffffff");
    document.documentElement.style.setProperty("--surface-2", dark ? "#243141" : "#eef2f6");
    document.documentElement.style.setProperty("--text", dark ? "#f3f4f6" : "#17202a");
    document.documentElement.style.setProperty("--muted", dark ? "#aeb8c2" : "#64748b");
    document.documentElement.style.setProperty("--border", dark ? "#344154" : "#dbe2ea");
  }, [dark]);

  return (
    <>
      <style>{styles}</style>

      <button
        onClick={() => setDark((v) => !v)}
        title="Cambiar tema"
        style={{
          position: "fixed",
          right: 18,
          bottom: 18,
          zIndex: 100,
          width: 44,
          height: 44,
          borderRadius: "50%",
          border: "1px solid var(--border)",
          background: "var(--surface)",
          color: "var(--text)",
          cursor: "pointer",
          boxShadow: "var(--shadow)",
        }}
      >
        {dark ? "☀" : "☾"}
      </button>

      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/login" element={<Login />} />
        <Route path="/registro" element={<Registro />} />
        <Route
          path="/mis-reportes"
          element={
            <ProtectedRoute roles={["Ciudadano"]}>
              <MisReportes />
            </ProtectedRoute>
          }
        />
        <Route
          path="/reportes/:id"
          element={
            <ProtectedRoute>
              <ReporteDetalle />
            </ProtectedRoute>
          }
        />
        <Route
          path="/panel"
          element={
            <ProtectedRoute roles={["Empleado", "Admin"]}>
              <Panel />
            </ProtectedRoute>
          }
        />
        <Route
          path="/boton-panico"
          element={
            <ProtectedRoute>
              <BotonPanico />
            </ProtectedRoute>
          }
        />
        <Route path="/noticias/:id" element={<NoticiaDetalle />} />
        <Route path="*" element={<Navigate to="/" replace />} />
      </Routes>
    </>
  );
}

export default function AppRoot() {
  return (
    <BrowserRouter>
      <AuthProvider>
        <App />
      </AuthProvider>
    </BrowserRouter>
  );
}
