import { useEffect, useState } from 'react'
import { paged } from '../lib/api'
import Icon from './Icon'

export default function DashboardPage({ user, onNavigate }) {
  const [stats, setStats] = useState({ producao: 0, vendas: 0, estoque: 0, receber: 0 })
  const [materials, setMaterials] = useState([])

  useEffect(() => {
    let active = true
    Promise.allSettled([
      paged('/ordem-producao', 1, 100), paged('/pedido-venda', 1, 100),
      paged('/materia-prima', 1, 100), paged('/conta-receber', 1, 100),
    ]).then((all) => {
      if (!active) return
      const value = (index) => all[index].status === 'fulfilled' ? all[index].value : { itens: [], totalItens: 0 }
      const raw = value(2).itens || []
      setStats({
        producao: (value(0).itens || []).filter((x) => x.status !== 2).length,
        vendas: value(1).totalItens || 0,
        estoque: raw.filter((x) => Number(x.quantidade) <= Number(x.quantidadeMinima)).length,
        receber: (value(3).itens || []).filter((x) => x.statusPagamento !== 0).reduce((sum, x) => sum + Number(x.valor || 0), 0),
      })
      setMaterials(raw.filter((x) => Number(x.quantidade) <= Number(x.quantidadeMinima)).slice(0, 5))
    })
    return () => { active = false }
  }, [])

  const cards = [
    { label: 'Ordens em aberto', value: stats.producao, icon: 'factory', route: 'producao', tone: 'purple' },
    { label: 'Pedidos de venda', value: stats.vendas, icon: 'sale', route: 'vendas', tone: 'blue' },
    { label: 'Itens abaixo do mínimo', value: stats.estoque, icon: 'stock', route: 'materias', tone: 'orange' },
    { label: 'Total a receber', value: stats.receber.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' }), icon: 'receivable', route: 'receber', tone: 'green' },
  ]

  return (
    <>
      <header className="dashboard-heading"><div><small>Visão geral</small><h1>Seja bem-vindo ao ImpérioSys, {user?.nomeFuncionario?.split(' ')[0] || 'usuário'}</h1><p>Acompanhe os pontos principais da operação.</p></div></header>
      <section className="metric-grid">
        {cards.map((card) => <button className={`metric-card ${card.tone}`} key={card.label} onClick={() => onNavigate(card.route)}><span className="metric-icon"><Icon name={card.icon} /></span><span><small>{card.label}</small><strong>{card.value}</strong><em>Ver detalhes</em></span></button>)}
      </section>
      <section className="dashboard-grid">
        <article className="dashboard-card production-chart">
          <header><div><small>Produção</small><h2>Visão geral de produção</h2></div><button onClick={() => onNavigate('producao')}>Ver ordens</button></header>
          <div className="bars" aria-label="Representação visual da produção mensal">
            {[36, 52, 70, 38, 62, 84, 66, 54, 73, 65, 86, 48].map((height, i) => <span key={i}><i style={{ height: `${height}%` }} /><small>{['Jan','Fev','Mar','Abr','Mai','Jun','Jul','Ago','Set','Out','Nov','Dez'][i]}</small></span>)}
          </div>
        </article>
        <article className="dashboard-card donut-card"><small>Proporções de fabricação</small><h2>Situação das ordens</h2><div className="donut"><span>{stats.producao}</span></div><p>Ordens que precisam de acompanhamento</p></article>
        <article className="dashboard-card alerts-card"><header><div><small>Estoque</small><h2>Materiais que exigem atenção</h2></div><button onClick={() => onNavigate('materias')}>Abrir estoque</button></header>{materials.length === 0 ? <p className="ok-message">Nenhum material abaixo do estoque mínimo.</p> : <ul>{materials.map((item) => <li key={item.id}><span><strong>{item.descricao}</strong><small>{item.codigo}</small></span><span className="status-badge danger">{item.quantidade} / mín. {item.quantidadeMinima}</span></li>)}</ul>}</article>
      </section>
    </>
  )
}
