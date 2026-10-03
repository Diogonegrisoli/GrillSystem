import { useCallback, useEffect, useMemo, useState } from 'react'
import { http, paged } from '../lib/api'
import Icon from './Icon'

const money = new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' })
const number = new Intl.NumberFormat('pt-BR', { maximumFractionDigits: 3 })

function getInitial(fields) {
  return Object.fromEntries(fields.map((f) => [f.name, f.type === 'checkbox' ? false : (f.default ?? '')]))
}

function normalizeForForm(record, fields) {
  const data = {}
  for (const f of fields) {
    let value = record?.[f.name]
    if (f.name === 'perfil' && Array.isArray(record?.perfis)) value = record.perfis[0]
    if (f.type === 'date' && value) value = String(value).slice(0, 10)
    if (f.type === 'datetime-local' && value) value = String(value).slice(0, 16)
    data[f.name] = value ?? (f.type === 'checkbox' ? false : '')
  }
  return data
}

function normalizePayload(form, fields) {
  const payload = {}
  for (const f of fields) {
    const value = form[f.name]
    if (f.type === 'number' || f.type === 'lookup') payload[f.name] = value === '' ? 0 : Number(value)
    else if (f.type === 'select' && f.options?.some((o) => typeof o.value === 'number')) payload[f.name] = value === '' ? null : Number(value)
    else if (f.type === 'datetime-local') payload[f.name] = value ? new Date(value).toISOString() : null
    else payload[f.name] = value
  }
  return payload
}

function documentMask(value) {
  const digits = String(value || '').replace(/\D/g, '')
  if (digits.length === 11) return digits.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/, '$1.$2.$3-$4')
  if (digits.length === 14) return digits.replace(/(\d{2})(\d{3})(\d{3})(\d{4})(\d{2})/, '$1.$2.$3/$4-$5')
  return value || '—'
}

function formatValue(value, column) {
  if (value === null || value === undefined || value === '') return '—'
  if (column.format === 'currency') return money.format(value)
  if (column.format === 'decimal') return number.format(value)
  if (column.format === 'document') return documentMask(value)
  if (column.format === 'date') return new Date(`${String(value).slice(0, 10)}T12:00:00`).toLocaleDateString('pt-BR')
  if (column.format === 'datetime') return new Date(value).toLocaleString('pt-BR')
  if (column.format === 'array') return Array.isArray(value) ? value.join(', ') : value
  if (column.format === 'blocked') return <span className={`status-badge ${value ? 'danger' : 'success'}`}>{value ? 'Bloqueado' : 'Liberado'}</span>
  if (column.format === 'active') return <span className={`status-badge ${Number(value) === 0 ? 'success' : 'muted'}`}>{Number(value) === 0 ? 'Ativo' : 'Inativo'}</span>
  if (column.options) {
    const label = column.options[Number(value)] ?? value
    return column.format === 'status' ? <span className={`status-badge status-${Number(value)}`}>{label}</span> : label
  }
  return String(value)
}

function Field({ config, value, onChange, lookupOptions, disabled = false }) {
  const id = `field-${config.name}`
  if (config.type === 'textarea') {
    return <label className="form-field wide" htmlFor={id}><span>{config.label}{config.required && ' *'}</span><textarea id={id} value={value} disabled={disabled} required={config.required} onChange={(e) => onChange(e.target.value)} /></label>
  }
  if (config.type === 'checkbox') {
    return <label className="check-field"><input id={id} type="checkbox" checked={Boolean(value)} disabled={disabled} onChange={(e) => onChange(e.target.checked)} /><span>{config.label}</span></label>
  }
  if (config.type === 'select' || config.type === 'lookup') {
    const options = config.type === 'lookup' ? lookupOptions : config.options
    return (
      <label className="form-field" htmlFor={id}>
        <span>{config.label}{config.required && ' *'}</span>
        <select id={id} value={value} disabled={disabled} required={config.required} onChange={(e) => onChange(e.target.value)}>
          <option value="">Selecione</option>
          {(options || []).map((option) => <option key={option.value} value={option.value}>{option.label}</option>)}
        </select>
      </label>
    )
  }
  return (
    <label className="form-field" htmlFor={id}>
      <span>{config.label}{config.required && ' *'}</span>
      <input id={id} type={config.type} value={value} disabled={disabled} required={config.required} min={config.min} max={config.max} step={config.step} onChange={(e) => onChange(e.target.value)} />
    </label>
  )
}

function RecordDialog({ config, record, mode, onClose, onSaved }) {
  const isEdit = mode === 'edit'
  const isView = mode === 'view'
  const baseFields = isEdit ? (config.editFields || config.fields) : config.fields
  const fields = useMemo(() => [...baseFields.filter((f) => !(isEdit && f.createOnly)), ...(isEdit ? (config.editExtra || []) : [])], [baseFields, config.editExtra, isEdit])
  const initialForm = useMemo(() => record ? normalizeForForm(record, fields) : getInitial(fields), [fields, record])
  const [form, setForm] = useState(initialForm)
  const [lookups, setLookups] = useState({})
  const [error, setError] = useState('')
  const [saving, setSaving] = useState(false)

  useEffect(() => {
    let active = true
    Promise.all(fields.filter((f) => f.type === 'lookup').map(async (f) => {
      try {
        const result = await paged(f.endpoint, 1, 100)
        return [f.name, (result.itens || []).map((item) => ({ value: item.id, label: String(item[f.optionLabel] ?? item.id) }))]
      } catch { return [f.name, []] }
    })).then((pairs) => { if (active) setLookups(Object.fromEntries(pairs)) })
    return () => { active = false }
  }, [fields])

  async function submit(event) {
    event.preventDefault()
    setSaving(true)
    setError('')
    try {
      const payload = normalizePayload(form, fields)
      if (isEdit) await http.put(`${config.endpoint}/${record.id}`, payload)
      else await http.post(config.endpoint, payload)
      onSaved(`${config.singular} ${isEdit ? 'atualizado' : 'cadastrado'} com sucesso.`)
    } catch (err) {
      setError(err.message)
    } finally { setSaving(false) }
  }

  return (
    <div className="modal-backdrop" role="presentation" onMouseDown={(e) => e.target === e.currentTarget && onClose()}>
      <section className="modal" role="dialog" aria-modal="true" aria-labelledby="modal-title">
        <header><div><small>{isView ? 'Consulta' : isEdit ? 'Edição' : 'Novo cadastro'}</small><h2 id="modal-title">{isView ? `Visualizar ${config.singular}` : isEdit ? `Editar ${config.singular}` : `Cadastrar ${config.singular}`}</h2></div><button className="icon-button" onClick={onClose} aria-label="Fechar"><Icon name="close" /></button></header>
        <form onSubmit={submit}>
          <div className="form-grid">
            {fields.map((f) => <Field key={f.name} config={f} value={form[f.name]} disabled={isView} lookupOptions={lookups[f.name]} onChange={(value) => setForm({ ...form, [f.name]: value })} />)}
          </div>
          {error && <div className="form-error" role="alert">{error}</div>}
          <footer>
            {!isView && <button type="button" className="button secondary" onClick={() => setForm(initialForm)}>Restaurar</button>}
            <button type="button" className="button secondary" onClick={onClose}>{isView ? 'Fechar' : 'Cancelar'}</button>
            {!isView && <button className="button primary" disabled={saving}><Icon name="save" size={18} />{saving ? 'Salvando...' : 'Salvar'}</button>}
          </footer>
        </form>
      </section>
    </div>
  )
}

export default function ResourcePage({ config, notify }) {
  const [result, setResult] = useState({ itens: [], pagina: 1, totalPaginas: 1, totalItens: 0 })
  const [query, setQuery] = useState('')
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')
  const [dialog, setDialog] = useState(null)
  const [deleteRecord, setDeleteRecord] = useState(null)
  const [pendingAction, setPendingAction] = useState(null)

  const load = useCallback(async (page = 1) => {
    setLoading(true); setError('')
    try { setResult(await paged(config.endpoint, page, 20)) }
    catch (err) { setError(err.message) }
    finally { setLoading(false) }
  }, [config.endpoint])

  // A listagem pertence a uma fonte externa e deve ser carregada ao trocar de módulo.
  // oxlint-disable-next-line react/set-state-in-effect
  useEffect(() => { load() }, [load])

  const rows = useMemo(() => {
    const text = query.trim().toLowerCase()
    if (!text) return result.itens || []
    return (result.itens || []).filter((item) => config.searchKeys.some((key) => String(item[key] ?? '').toLowerCase().includes(text)))
  }, [config.searchKeys, query, result.itens])

  async function remove() {
    try {
      await http.delete(`${config.endpoint}/${deleteRecord.id}`)
      setDeleteRecord(null); notify(`${config.singular} excluído com sucesso.`); load(result.pagina)
    } catch (err) { notify(err.message, 'error'); setDeleteRecord(null) }
  }

  async function runAction() {
    const { action, record } = pendingAction
    try {
      await http.post(action.path(record), action.body?.(record))
      setPendingAction(null); notify(action.success); load(result.pagina)
    } catch (err) { notify(err.message, 'error'); setPendingAction(null) }
  }

  return (
    <>
      <header className="page-heading"><div><span>{config.singular}</span><h1>{config.title}</h1></div><Icon name={config.icon} size={58} /></header>
      <section className="workspace-card">
        <div className="toolbar">
          <label className="search-box"><Icon name="search" size={20} /><input placeholder={`Pesquisar ${config.singular.toLowerCase()}...`} value={query} onChange={(e) => setQuery(e.target.value)} /></label>
          <button className="button secondary" onClick={() => load(result.pagina)}><Icon name="search" size={18} />Buscar</button>
          <button className="button primary" onClick={() => setDialog({ mode: 'create' })}><Icon name="plus" size={20} />Novo</button>
        </div>
        {error && <div className="empty-state error-state"><strong>Não foi possível carregar os dados</strong><span>{error}</span><button className="button secondary" onClick={() => load()}>Tentar novamente</button></div>}
        {!error && <div className="table-wrap">
          <table>
            <thead><tr>{config.columns.map((column) => <th key={column.key}>{column.label}</th>)}<th>Opções</th></tr></thead>
            <tbody>
              {loading && <tr><td colSpan={config.columns.length + 1}><div className="loading-row">Carregando...</div></td></tr>}
              {!loading && rows.length === 0 && <tr><td colSpan={config.columns.length + 1}><div className="loading-row">Nenhum registro encontrado.</div></td></tr>}
              {!loading && rows.map((item) => <tr key={item.id}>
                {config.columns.map((column) => <td key={column.key}>{formatValue(item[column.key], column)}</td>)}
                <td><div className="row-actions">
                  {(config.actions || []).filter((action) => !action.visible || action.visible(item)).map((action) => <button key={action.label} className="action" onClick={() => setPendingAction({ action, record: item })} title={action.label}><Icon name={action.icon || 'save'} size={17} /></button>)}
                  {config.canEdit !== false && <button className="action edit" onClick={() => setDialog({ mode: 'edit', record: item })} title="Editar"><Icon name="edit" size={17} /></button>}
                  <button className="action view" onClick={() => setDialog({ mode: 'view', record: item })} title="Visualizar"><Icon name="eye" size={17} /></button>
                  {config.canDelete !== false && <button className="action delete" onClick={() => setDeleteRecord(item)} title="Excluir"><Icon name="trash" size={17} /></button>}
                </div></td>
              </tr>)}
            </tbody>
          </table>
        </div>}
        <footer className="pagination"><span>{result.totalItens || 0} registros</span><div><button disabled={result.pagina <= 1} onClick={() => load(result.pagina - 1)}>Anterior</button><span>{result.pagina || 1} de {result.totalPaginas || 1}</span><button disabled={result.pagina >= result.totalPaginas} onClick={() => load(result.pagina + 1)}>Próxima</button></div></footer>
      </section>
      {dialog && <RecordDialog config={config} mode={dialog.mode} record={dialog.record} onClose={() => setDialog(null)} onSaved={(message) => { setDialog(null); notify(message); load(result.pagina) }} />}
      {deleteRecord && <div className="modal-backdrop"><section className="confirm-dialog" role="alertdialog"><div className="danger-icon"><Icon name="trash" /></div><h2>Excluir {config.singular.toLowerCase()}?</h2><p>Esta ação só será concluída se não existirem registros relacionados.</p><div><button className="button secondary" onClick={() => setDeleteRecord(null)}>Cancelar</button><button className="button danger" onClick={remove}>Excluir</button></div></section></div>}
      {pendingAction && <div className="modal-backdrop"><section className="confirm-dialog" role="alertdialog"><div className="danger-icon neutral"><Icon name={pendingAction.action.icon || 'save'} /></div><h2>{pendingAction.action.label}?</h2><p>{pendingAction.action.confirm}</p><div><button className="button secondary" onClick={() => setPendingAction(null)}>Cancelar</button><button className="button primary" onClick={runAction}>Confirmar</button></div></section></div>}
    </>
  )
}
