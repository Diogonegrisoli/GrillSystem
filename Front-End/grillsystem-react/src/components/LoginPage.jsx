import { useState } from 'react'
import { http, setToken } from '../lib/api'
import Icon from './Icon'

export default function LoginPage({ onLogin }) {
  const [form, setForm] = useState({ email: '', senha: '' })
  const [showPassword, setShowPassword] = useState(false)
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState('')

  async function submit(event) {
    event.preventDefault()
    setLoading(true)
    setError('')
    try {
      const result = await http.post('/auth/login', form)
      setToken(result.token)
      await onLogin(result)
    } catch (err) {
      setError(err.message)
    } finally {
      setLoading(false)
    }
  }

  return (
    <main className="login-page">
      <section className="login-panel" aria-labelledby="login-title">
        <div className="brand-mark" aria-hidden="true">
          <span className="brand-crown">♛</span>
          <span className="brand-grill">▰</span>
        </div>
        <h1 id="login-title" className="login-logo">ImpérioSys</h1>
        <p className="login-subtitle">Gestão integrada para sua fábrica</p>
        <form className="login-form" onSubmit={submit}>
          <label>
            <span>E-mail</span>
            <input
              type="email" value={form.email} autoComplete="username" required
              placeholder="usuario@empresa.com"
              onChange={(e) => setForm({ ...form, email: e.target.value })}
            />
          </label>
          <label>
            <span>Senha</span>
            <div className="password-field">
              <input
                type={showPassword ? 'text' : 'password'} value={form.senha}
                autoComplete="current-password" required placeholder="Digite sua senha"
                onChange={(e) => setForm({ ...form, senha: e.target.value })}
              />
              <button type="button" onClick={() => setShowPassword(!showPassword)} aria-label={showPassword ? 'Ocultar senha' : 'Mostrar senha'}>
                <Icon name="eye" size={20} />
              </button>
            </div>
          </label>
          {error && <div className="form-error" role="alert">{error}</div>}
          <button className="login-button" disabled={loading}>
            <Icon name="logout" size={20} />
            {loading ? 'Entrando...' : 'Entrar'}
          </button>
        </form>
      </section>
      <section className="login-showcase" aria-hidden="true">
        <div className="showcase-overlay" />
        <div className="showcase-content">
          <span>Produção • Estoque • Vendas • Finanças</span>
          <strong>Controle completo para crescer com segurança.</strong>
        </div>
      </section>
    </main>
  )
}
