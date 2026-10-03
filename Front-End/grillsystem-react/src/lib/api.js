const TOKEN_KEY = 'imperiosys.token'
const API_BASE = (import.meta.env.VITE_API_URL || '/api').replace(/\/$/, '')

export function getToken() {
  return localStorage.getItem(TOKEN_KEY)
}

export function setToken(token) {
  if (token) localStorage.setItem(TOKEN_KEY, token)
  else localStorage.removeItem(TOKEN_KEY)
}

function extractMessage(payload, status) {
  if (payload?.mensagem) return payload.mensagem
  if (payload?.detail) return payload.detail
  if (payload?.title && !payload?.errors) return payload.title
  if (payload?.errors) return Object.values(payload.errors).flat().join(' ')
  return `Não foi possível concluir a operação (${status}).`
}

export async function api(path, options = {}) {
  const token = getToken()
  const response = await fetch(`${API_BASE}${path}`, {
    ...options,
    headers: {
      ...(options.body ? { 'Content-Type': 'application/json' } : {}),
      ...(token ? { Authorization: `Bearer ${token}` } : {}),
      ...options.headers,
    },
  })

  const contentType = response.headers.get('content-type') || ''
  const payload = contentType.includes('application/json')
    ? await response.json()
    : null

  if (!response.ok) {
    if (response.status === 401 && token) {
      setToken(null)
      window.dispatchEvent(new Event('imperiosys:unauthorized'))
    }
    throw new Error(extractMessage(payload, response.status))
  }

  return payload
}

export const http = {
  get: (path) => api(path),
  post: (path, body) => api(path, { method: 'POST', body: JSON.stringify(body) }),
  put: (path, body) => api(path, { method: 'PUT', body: JSON.stringify(body) }),
  delete: (path, body) => api(path, {
    method: 'DELETE',
    ...(body ? { body: JSON.stringify(body) } : {}),
  }),
}

export async function paged(endpoint, page = 1, pageSize = 20) {
  const join = endpoint.includes('?') ? '&' : '?'
  return http.get(`${endpoint}${join}Pagina=${page}&TamanhoPagina=${pageSize}`)
}
