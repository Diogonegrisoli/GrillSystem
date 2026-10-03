import { useCallback, useEffect, useState } from 'react'
import './App.css'
import DashboardPage from './components/DashboardPage'
import LoginPage from './components/LoginPage'
import ResourcePage from './components/ResourcePage'
import Shell from './components/Shell'
import { modules } from './config/modules'
import { getToken, http, setToken } from './lib/api'

function currentRoute() {
  return window.location.hash.replace(/^#\/?/, '') || 'dashboard'
}

export default function App() {
  const [session, setSession] = useState({ loading: Boolean(getToken()), user: null })
  const [route, setRoute] = useState(currentRoute())
  const [toast, setToast] = useState(null)

  const logout = useCallback(() => {
    setToken(null)
    setSession({ loading: false, user: null })
    window.location.hash = ''
  }, [])

  const loadUser = useCallback(async () => {
    if (!getToken()) { setSession({ loading: false, user: null }); return }
    try {
      const user = await http.get('/auth/me')
      setSession({ loading: false, user })
    } catch {
      logout()
    }
  }, [logout])

  // A sessão armazenada precisa ser validada na API quando a aplicação inicia.
  // oxlint-disable-next-line react/set-state-in-effect
  useEffect(() => { loadUser() }, [loadUser])
  useEffect(() => {
    const change = () => setRoute(currentRoute())
    const unauthorized = () => logout()
    window.addEventListener('hashchange', change)
    window.addEventListener('imperiosys:unauthorized', unauthorized)
    return () => { window.removeEventListener('hashchange', change); window.removeEventListener('imperiosys:unauthorized', unauthorized) }
  }, [logout])

  function navigate(next) {
    window.location.hash = `/${next}`
    setRoute(next)
  }

  function notify(message, type = 'success') {
    setToast({ message, type })
    window.clearTimeout(notify.timer)
    notify.timer = window.setTimeout(() => setToast(null), 4500)
  }

  if (session.loading) return <div className="app-loading"><span className="spinner" /><strong>Carregando ImpérioSys...</strong></div>
  if (!session.user) return <LoginPage onLogin={loadUser} />

  const config = modules[route]
  return (
    <Shell route={route} user={session.user} onNavigate={navigate} onLogout={logout}>
      {route === 'dashboard' && <DashboardPage user={session.user} onNavigate={navigate} />}
      {config && <ResourcePage config={config} notify={notify} />}
      {!config && route !== 'dashboard' && <section className="empty-state"><strong>Página não encontrada</strong><button className="button primary" onClick={() => navigate('dashboard')}>Voltar à visão geral</button></section>}
      {toast && <div className={`toast ${toast.type}`} role="status">{toast.message}<button onClick={() => setToast(null)}>×</button></div>}
    </Shell>
  )
}
