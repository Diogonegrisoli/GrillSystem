import { useState } from 'react'
import { navigation } from '../config/modules'
import Icon from './Icon'

function allowed(item, roles) {
  if (item.role) return roles.includes(item.role)
  if (item.roles) return item.roles.some((role) => roles.includes(role))
  return true
}

export default function Shell({ route, user, onNavigate, onLogout, children }) {
  const [open, setOpen] = useState(false)
  const roles = user?.perfis || []
  return (
    <div className="app-shell">
      <aside className={`sidebar ${open ? 'sidebar-open' : ''}`}>
        <button className="sidebar-brand" onClick={() => onNavigate('dashboard')} aria-label="Ir para visão geral">
          <span>♛</span><small>IS</small>
        </button>
        <button className="sidebar-toggle" onClick={() => setOpen(!open)} aria-label="Alternar menu">
          <Icon name="menu" />
        </button>
        <nav>
          {navigation.filter((item) => allowed(item, roles)).map((item) => (
            <button key={item.key} className={route === item.key ? 'active' : ''} onClick={() => { onNavigate(item.key); setOpen(false) }} title={item.label}>
              <Icon name={item.icon} />
              <span>{item.label}</span>
            </button>
          ))}
        </nav>
        <button className="logout-button" onClick={onLogout} title="Sair">
          <Icon name="logout" /><span>Sair</span>
        </button>
      </aside>
      <div className="app-content">
        <header className="topbar">
          <button className="mobile-menu" onClick={() => setOpen(!open)}><Icon name="menu" /></button>
          <div className="topbar-user">
            <span><strong>{user?.nomeFuncionario || 'Usuário'}</strong><small>{roles.join(' • ')}</small></span>
            <div className="avatar">{(user?.nomeFuncionario || 'U').slice(0, 1).toUpperCase()}</div>
          </div>
        </header>
        <main className="page-content">{children}</main>
      </div>
      {open && <button className="sidebar-backdrop" onClick={() => setOpen(false)} aria-label="Fechar menu" />}
    </div>
  )
}
