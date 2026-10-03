const activeOptions = [
  { value: 0, label: 'Ativo' },
  { value: 1, label: 'Inativo' },
]

export const unitOptions = [
  'Grama', 'Quilograma', 'Tonelada', 'Centímetro', 'Centímetro quadrado',
  'Centímetro cúbico', 'Metro', 'Metro quadrado', 'Metro cúbico', 'Mililitro',
  'Litro', 'Unidade', 'Caixa', 'Pacote',
].map((label, value) => ({ value, label }))

const paymentOptions = [
  { value: 0, label: 'Débito' }, { value: 1, label: 'Crédito' },
  { value: 2, label: 'Pix' }, { value: 3, label: 'Dinheiro' },
]
const receivePaymentOptions = [
  { value: 0, label: 'Pix' }, { value: 1, label: 'Dinheiro' },
  { value: 2, label: 'Crédito' }, { value: 3, label: 'Débito' },
  { value: 4, label: 'Boleto' },
]

const field = (name, label, type = 'text', extra = {}) => ({ name, label, type, ...extra })
const statusColumn = { key: 'situacao', label: 'Situação', format: 'active' }

export const modules = {
  clientes: {
    title: 'Consultar Clientes', singular: 'Cliente', endpoint: '/cliente', icon: 'users',
    searchKeys: ['nome', 'cpfCnpj', 'email'],
    columns: [
      { key: 'id', label: 'Código' }, { key: 'nome', label: 'Nome' },
      { key: 'cpfCnpj', label: 'CPF/CNPJ', format: 'document' },
      { key: 'tipo', label: 'Tipo', options: ['Física', 'Jurídica'] },
      { key: 'telefone', label: 'Telefone' }, statusColumn,
    ],
    fields: [
      field('tipo', 'Tipo de pessoa', 'select', { required: true, options: [{ value: 0, label: 'Pessoa física' }, { value: 1, label: 'Pessoa jurídica' }] }),
      field('nome', 'Nome / razão social', 'text', { required: true }),
      field('razaoSocial', 'Razão social'), field('nomeFantasia', 'Nome fantasia'),
      field('cpfCnpj', 'CPF / CNPJ', 'text', { required: true }),
      field('dataNascimento', 'Data de nascimento', 'date'),
      field('telefone', 'Telefone', 'tel', { required: true }), field('celular', 'Celular', 'tel'),
      field('email', 'E-mail', 'email'), field('endereco', 'Endereço legado'),
      field('observacoes', 'Observações', 'textarea'),
    ],
    editFields: [
      field('nome', 'Nome / razão social', 'text', { required: true }),
      field('nomeFantasia', 'Nome fantasia'), field('dataNascimento', 'Data de nascimento', 'date'),
      field('telefone', 'Telefone', 'tel', { required: true }), field('celular', 'Celular', 'tel'),
      field('email', 'E-mail', 'email'), field('endereco', 'Endereço legado'),
      field('situacao', 'Situação', 'select', { options: activeOptions }),
      field('observacoes', 'Observações', 'textarea'),
    ],
  },
  fornecedores: {
    title: 'Consultar Fornecedores', singular: 'Fornecedor', endpoint: '/fornecedor', icon: 'handshake',
    searchKeys: ['razaoSocial', 'nomeFantasia', 'cnpj'],
    columns: [
      { key: 'id', label: 'Código' }, { key: 'razaoSocial', label: 'Razão social' },
      { key: 'nomeFantasia', label: 'Nome fantasia' }, { key: 'cnpj', label: 'CNPJ', format: 'document' },
      { key: 'celular', label: 'Celular' }, statusColumn,
    ],
    fields: [
      field('razaoSocial', 'Razão social', 'text', { required: true }),
      field('nomeFantasia', 'Nome fantasia', 'text', { required: true }),
      field('cnpj', 'CNPJ', 'text', { required: true, createOnly: true }),
      field('email', 'E-mail', 'email', { required: true }), field('celular', 'Celular', 'tel'),
      field('contrato', 'Contrato'), field('endereco', 'Endereço legado'),
      field('situacao', 'Situação', 'select', { options: activeOptions }),
    ],
  },
  funcionarios: {
    title: 'Consultar Funcionários', singular: 'Funcionário', endpoint: '/funcionario', icon: 'badge',
    searchKeys: ['nome', 'cpf'],
    columns: [
      { key: 'id', label: 'Código' }, { key: 'nome', label: 'Nome' },
      { key: 'cpf', label: 'CPF', format: 'document' }, { key: 'status', label: 'Situação', format: 'active' },
    ],
    fields: [field('nome', 'Nome completo', 'text', { required: true }), field('cpf', 'CPF', 'text', { required: true })],
  },
  usuarios: {
    title: 'Consultar Usuários', singular: 'Usuário', endpoint: '/usuario', icon: 'shield', adminOnly: true,
    searchKeys: ['email', 'nomeFuncionario'],
    columns: [
      { key: 'id', label: 'Código' }, { key: 'nomeFuncionario', label: 'Funcionário' },
      { key: 'email', label: 'Login' }, { key: 'perfis', label: 'Perfil', format: 'array' },
      { key: 'bloqueado', label: 'Acesso', format: 'blocked' },
    ],
    fields: [
      field('email', 'E-mail', 'email', { required: true }), field('senha', 'Senha', 'password', { required: true }),
      field('funcionarioId', 'Funcionário', 'lookup', { endpoint: '/funcionario', optionLabel: 'nome', required: true, createOnly: true }),
      field('perfil', 'Perfil', 'select', { required: true, options: ['Administrador', 'Gerente', 'Operador', 'Mestre de Produção', 'Financeiro', 'Vendedor', 'Comprador'].map(value => ({ value, label: value })) }),
    ],
    editFields: [
      field('email', 'E-mail', 'email', { required: true }), field('senha', 'Nova senha', 'password'),
      field('perfil', 'Perfil', 'select', { required: true, options: ['Administrador', 'Gerente', 'Operador', 'Mestre de Produção', 'Financeiro', 'Vendedor', 'Comprador'].map(value => ({ value, label: value })) }),
      field('bloqueado', 'Bloqueado', 'checkbox'),
    ],
  },
  materias: {
    title: 'Consultar Matérias-primas', singular: 'Matéria-prima', endpoint: '/materia-prima', icon: 'materials',
    searchKeys: ['codigo', 'descricao'],
    columns: [
      { key: 'codigo', label: 'Código' }, { key: 'descricao', label: 'Descrição' },
      { key: 'quantidade', label: 'Estoque', format: 'decimal' },
      { key: 'quantidadeMinima', label: 'Mínimo', format: 'decimal' },
      { key: 'unidadeMedida', label: 'Unidade', options: unitOptions.map(x => x.label) }, statusColumn,
    ],
    fields: [
      field('codigo', 'Código', 'text', { required: true }), field('descricao', 'Descrição', 'text', { required: true }),
      field('quantidadeMinima', 'Quantidade mínima', 'number', { required: true, step: '0.001' }),
      field('unidadeMedida', 'Unidade de medida', 'select', { required: true, options: unitOptions }),
      field('situacao', 'Situação', 'select', { options: activeOptions }), field('observacoes', 'Observações', 'textarea'),
    ],
  },
  produtos: {
    title: 'Consultar Produtos', singular: 'Produto', endpoint: '/produto', icon: 'box',
    searchKeys: ['codigo', 'descricao'],
    columns: [
      { key: 'codigo', label: 'Código' }, { key: 'descricao', label: 'Descrição' },
      { key: 'preco', label: 'Preço', format: 'currency' }, { key: 'quantidade', label: 'Estoque' },
      { key: 'estoqueMinimo', label: 'Mínimo' }, statusColumn,
    ],
    fields: [
      field('codigo', 'Código', 'text', { required: true }), field('descricao', 'Descrição', 'text', { required: true }),
      field('preco', 'Preço unitário', 'number', { required: true, step: '0.01' }),
      field('estoqueMinimo', 'Estoque mínimo', 'number', { required: true }),
      field('unidadeMedida', 'Unidade de medida', 'select', { required: true, options: unitOptions }),
      field('situacao', 'Situação', 'select', { options: activeOptions }),
    ],
  },
  composicoes: {
    title: 'Composição dos Produtos', singular: 'Item da composição', endpoint: '/produto-materia-prima', icon: 'materials',
    searchKeys: ['id', 'produtoId', 'materiaPrimaId'],
    columns: [
      { key: 'id', label: 'Código' }, { key: 'produtoId', label: 'Produto' },
      { key: 'materiaPrimaId', label: 'Matéria-prima' },
      { key: 'quantidadeNecessaria', label: 'Quantidade necessária', format: 'decimal' },
    ],
    fields: [
      field('produtoId', 'Produto', 'lookup', { endpoint: '/produto', optionLabel: 'descricao', required: true }),
      field('materiaPrimaId', 'Matéria-prima', 'lookup', { endpoint: '/materia-prima', optionLabel: 'descricao', required: true }),
      field('quantidadeNecessaria', 'Quantidade necessária por produto', 'number', { required: true, min: '0.001', step: '0.001' }),
    ],
  },
  compras: {
    title: 'Pedidos de Compra', singular: 'Pedido de compra', endpoint: '/pedido-compra', icon: 'cart',
    searchKeys: ['id', 'fornecedorId'],
    columns: [
      { key: 'id', label: 'Código' }, { key: 'dataPedido', label: 'Pedido', format: 'date' },
      { key: 'dataEntrega', label: 'Entrega', format: 'date' }, { key: 'fornecedorId', label: 'Fornecedor' },
      { key: 'valorTotal', label: 'Total', format: 'currency' },
      { key: 'status', label: 'Situação', options: ['Entregue', 'Em andamento', 'Solicitado', 'Pendente'], format: 'status' },
    ],
    fields: [
      field('dataPedido', 'Data do pedido', 'date', { required: true }), field('dataEntrega', 'Data de entrega', 'date'),
      field('fornecedorId', 'Fornecedor', 'lookup', { endpoint: '/fornecedor', optionLabel: 'nomeFantasia', required: true }),
      field('funcionarioId', 'Funcionário', 'lookup', { endpoint: '/funcionario', optionLabel: 'nome', required: true }),
      field('formaPagamento', 'Forma de pagamento', 'select', { options: paymentOptions }),
      field('parcelas', 'Parcelas', 'number', { required: true, min: 1, max: 120 }),
    ],
    editExtra: [field('status', 'Situação', 'select', { options: [{ value: 3, label: 'Pendente' }, { value: 2, label: 'Solicitado' }, { value: 1, label: 'Em andamento' }, { value: 0, label: 'Entregue' }], required: true })],
  },
  vendas: {
    title: 'Consultar Vendas', singular: 'Pedido de venda', endpoint: '/pedido-venda', icon: 'sale',
    searchKeys: ['id', 'clienteId'],
    columns: [
      { key: 'id', label: 'Código' }, { key: 'dataPedido', label: 'Pedido', format: 'date' },
      { key: 'dataEntrega', label: 'Entrega', format: 'date' }, { key: 'clienteId', label: 'Cliente' },
      { key: 'valorTotal', label: 'Total', format: 'currency' },
      { key: 'status', label: 'Situação', options: ['Pendente', 'Em produção', 'Enviado', 'Entregue', 'Cancelado'], format: 'status' },
    ],
    fields: [
      field('dataPedido', 'Data do pedido', 'date', { required: true }), field('dataEntrega', 'Data de entrega', 'date', { required: true }),
      field('clienteId', 'Cliente', 'lookup', { endpoint: '/cliente', optionLabel: 'nome', required: true }),
      field('funcionarioId', 'Vendedor', 'lookup', { endpoint: '/funcionario', optionLabel: 'nome' }),
      field('formaPagamento', 'Forma de pagamento', 'select', { options: receivePaymentOptions }),
      field('parcelas', 'Parcelas', 'number', { required: true, min: 1, max: 120 }),
      field('desconto', 'Desconto', 'number', { step: '0.01' }),
    ],
    editExtra: [field('status', 'Situação', 'select', { options: ['Pendente', 'Em produção', 'Enviado', 'Entregue', 'Cancelado'].map((label, value) => ({ value, label })), required: true })],
  },
  itensVenda: {
    title: 'Itens dos Pedidos de Venda', singular: 'Item de venda', endpoint: '/produto-pedido-venda', icon: 'sale',
    searchKeys: ['id', 'pedidoVendaId', 'produtoId'],
    columns: [
      { key: 'id', label: 'Código' }, { key: 'pedidoVendaId', label: 'Pedido' },
      { key: 'produtoId', label: 'Produto' }, { key: 'quantidade', label: 'Quantidade' },
      { key: 'precoUnitario', label: 'Preço unitário', format: 'currency' },
    ],
    fields: [
      field('pedidoVendaId', 'Pedido de venda', 'lookup', { endpoint: '/pedido-venda', optionLabel: 'id', required: true }),
      field('produtoId', 'Produto', 'lookup', { endpoint: '/produto', optionLabel: 'descricao', required: true }),
      field('quantidade', 'Quantidade', 'number', { required: true, min: 1 }),
    ],
  },
  itensCompra: {
    title: 'Itens dos Pedidos de Compra', singular: 'Item de compra', endpoint: '/pedido-compra-materia-prima', icon: 'cart',
    searchKeys: ['id', 'pedidoCompraId', 'materiaPrimaId'],
    columns: [
      { key: 'id', label: 'Código' }, { key: 'pedidoCompraId', label: 'Pedido' },
      { key: 'materiaPrimaId', label: 'Matéria-prima' }, { key: 'quantidade', label: 'Quantidade', format: 'decimal' },
      { key: 'custoUnitario', label: 'Custo unitário', format: 'currency' },
    ],
    fields: [
      field('pedidoCompraId', 'Pedido de compra', 'lookup', { endpoint: '/pedido-compra', optionLabel: 'id', required: true }),
      field('materiaPrimaId', 'Matéria-prima', 'lookup', { endpoint: '/materia-prima', optionLabel: 'descricao', required: true }),
      field('quantidade', 'Quantidade', 'number', { required: true, min: '0.001', step: '0.001' }),
      field('custoUnitario', 'Custo unitário', 'number', { required: true, min: '0.01', step: '0.01' }),
    ],
  },
  producao: {
    title: 'Ordens de Produção', singular: 'Ordem de produção', endpoint: '/ordem-producao', icon: 'factory',
    searchKeys: ['id'],
    columns: [
      { key: 'id', label: 'Código' }, { key: 'dataInicio', label: 'Início', format: 'date' },
      { key: 'dataFim', label: 'Fim', format: 'date' }, { key: 'quantidade', label: 'Quantidade' },
      { key: 'tipo', label: 'Tipo', options: ['Puxada', 'Empurrada'] },
      { key: 'status', label: 'Situação', options: ['Pendente', 'Em andamento', 'Finalizada'], format: 'status' },
    ],
    fields: [
      field('quantidade', 'Quantidade', 'number', { required: true, min: 1 }),
      field('dataInicio', 'Data inicial', 'date', { required: true }),
      field('tipo', 'Tipo da ordem', 'select', { required: true, options: [{ value: 0, label: 'Puxada' }, { value: 1, label: 'Empurrada' }] }),
    ],
    editExtra: [field('status', 'Situação', 'select', { options: ['Pendente', 'Em andamento'].map((label, value) => ({ value, label })), required: true })],
    actions: [{
      label: 'Finalizar ordem', icon: 'save', path: (record) => `/ordem-producao/${record.id}/finalizar`,
      visible: (record) => Number(record.status) === 1,
      confirm: 'A finalização consumirá as matérias-primas da composição e adicionará a quantidade produzida ao estoque. Esta operação não pode ser desfeita.',
      success: 'Ordem finalizada e estoques atualizados com sucesso.',
    }],
  },
  itensProducao: {
    title: 'Produtos das Ordens', singular: 'Produto da ordem', endpoint: '/produto-ordem-producao', icon: 'factory',
    searchKeys: ['id', 'ordemProducaoId', 'produtoId'],
    columns: [
      { key: 'id', label: 'Código' }, { key: 'ordemProducaoId', label: 'Ordem' },
      { key: 'produtoId', label: 'Produto' },
    ],
    fields: [
      field('ordemProducaoId', 'Ordem de produção', 'lookup', { endpoint: '/ordem-producao', optionLabel: 'id', required: true }),
      field('produtoId', 'Produto', 'lookup', { endpoint: '/produto', optionLabel: 'descricao', required: true }),
    ],
  },
  estoque: {
    title: 'Movimentações de Estoque', singular: 'Movimentação', endpoint: '/movimentacao-estoque', icon: 'stock',
    searchKeys: ['referencia', 'materiaPrimaId'],
    columns: [
      { key: 'id', label: 'Código' }, { key: 'data', label: 'Data', format: 'date' },
      { key: 'tipo', label: 'Tipo', options: ['', 'Entrada', 'Saída'], format: 'status' },
      { key: 'materiaPrimaId', label: 'Matéria-prima' }, { key: 'quantidade', label: 'Quantidade', format: 'decimal' },
      { key: 'referencia', label: 'Referência' },
    ],
    fields: [
      field('tipo', 'Tipo de movimentação', 'select', { required: true, options: [{ value: 1, label: 'Entrada' }, { value: 2, label: 'Saída' }] }),
      field('materiaPrimaId', 'Matéria-prima', 'lookup', { endpoint: '/materia-prima', optionLabel: 'descricao', required: true }),
      field('quantidade', 'Quantidade', 'number', { step: '0.001', required: true }),
      field('custoUnitario', 'Custo unitário', 'number', { step: '0.01', required: true }),
      field('data', 'Data', 'date', { required: true }), field('referencia', 'Referência', 'text', { required: true }),
      field('observacoes', 'Observações', 'textarea'),
    ],
  },
  pagar: {
    title: 'Controle de Contas a Pagar', singular: 'Conta a pagar', endpoint: '/conta-pagar', icon: 'payable',
    searchKeys: ['id', 'pedidoCompraId'],
    columns: [
      { key: 'id', label: 'Lançamento' }, { key: 'pedidoCompraId', label: 'Compra' },
      { key: 'dataEmissao', label: 'Emissão', format: 'date' }, { key: 'dataVencimento', label: 'Vencimento', format: 'date' },
      { key: 'valor', label: 'Valor', format: 'currency' },
      { key: 'status', label: 'Situação', options: ['Pago', 'Pendente', 'Cancelado'], format: 'status' },
    ],
    fields: [
      field('pedidoCompraId', 'Pedido de compra', 'lookup', { endpoint: '/pedido-compra', optionLabel: 'id', required: true }),
      field('tipoPagamento', 'Tipo de pagamento', 'select', { options: paymentOptions, required: true }),
      field('dataEmissao', 'Data de emissão', 'datetime-local', { required: true, createOnly: true }),
      field('dataVencimento', 'Primeiro vencimento', 'datetime-local', { required: true }),
      field('observacao', 'Observação', 'textarea'),
    ],
  },
  receber: {
    title: 'Contas a Receber', singular: 'Conta a receber', endpoint: '/conta-receber', icon: 'receivable',
    searchKeys: ['id', 'pedidoVendaId'],
    columns: [
      { key: 'id', label: 'Lançamento' }, { key: 'pedidoVendaId', label: 'Venda' },
      { key: 'dataEmissao', label: 'Emissão', format: 'date' }, { key: 'dataVencimento', label: 'Vencimento', format: 'date' },
      { key: 'valor', label: 'Valor', format: 'currency' },
      { key: 'statusPagamento', label: 'Situação', options: ['Pago', 'Pendente', 'Atrasado', 'Cancelado'], format: 'status' },
    ],
    fields: [
      field('pedidoVendaId', 'Pedido de venda', 'lookup', { endpoint: '/pedido-venda', optionLabel: 'id', required: true }),
      field('tipoPagamento', 'Tipo de pagamento', 'select', { options: receivePaymentOptions, required: true }),
      field('dataVencimento', 'Primeiro vencimento', 'date', { required: true }), field('observacao', 'Observação', 'textarea'),
    ],
  },
  enderecos: {
    title: 'Endereços', singular: 'Endereço', endpoint: '/endereco', icon: 'pin',
    searchKeys: ['logradouro', 'cidade', 'cep'],
    columns: [
      { key: 'id', label: 'Código' }, { key: 'logradouro', label: 'Logradouro' }, { key: 'numero', label: 'Número' },
      { key: 'bairro', label: 'Bairro' }, { key: 'cidade', label: 'Cidade' }, { key: 'estado', label: 'UF' },
    ],
    fields: [
      field('cep', 'CEP', 'text', { required: true }), field('logradouro', 'Logradouro', 'text', { required: true }),
      field('numero', 'Número', 'text', { required: true }), field('bairro', 'Bairro', 'text', { required: true }),
      field('cidade', 'Cidade', 'text', { required: true }), field('estado', 'UF', 'text', { required: true }),
    ],
  },
  caixa: {
    title: 'Controle de Caixa', singular: 'Caixa', endpoint: '/caixa', icon: 'cash', financeOnly: true,
    searchKeys: ['id', 'descricao'], canEdit: false, canDelete: false,
    columns: [
      { key: 'id', label: 'Código' }, { key: 'descricao', label: 'Descrição' },
      { key: 'tipo', label: 'Tipo', options: ['Físico', 'Bancário'] },
      { key: 'dataAbertura', label: 'Abertura', format: 'datetime' },
      { key: 'saldoInicial', label: 'Saldo inicial', format: 'currency' },
      { key: 'saldoFinal', label: 'Saldo atual', format: 'currency' },
      { key: 'situacao', label: 'Situação', options: ['Aberto', 'Fechado'], format: 'status' },
    ],
    fields: [
      field('descricao', 'Descrição', 'text', { required: true }),
      field('tipo', 'Tipo de caixa', 'select', { options: [{ value: 0, label: 'Físico' }, { value: 1, label: 'Bancário' }], required: true }),
      field('saldoInicial', 'Saldo inicial', 'number', { step: '0.01', required: true }),
    ],
    actions: [{
      label: 'Fechar caixa', icon: 'save', path: (record) => `/caixa/${record.id}/fechar`,
      visible: (record) => Number(record.situacao) === 0,
      confirm: 'O caixa será fechado e não aceitará novas liquidações.',
      success: 'Caixa fechado com sucesso.',
    }],
  },
  categorias: {
    title: 'Categorias Financeiras', singular: 'Categoria', endpoint: '/categoria-financeira', icon: 'cash', financeOnly: true,
    searchKeys: ['id', 'nome'],
    columns: [
      { key: 'id', label: 'Código' }, { key: 'nome', label: 'Nome' },
      { key: 'tipo', label: 'Tipo', options: ['Receita', 'Despesa'], format: 'status' },
    ],
    fields: [
      field('nome', 'Nome', 'text', { required: true }),
      field('tipo', 'Tipo', 'select', { required: true, options: [{ value: 0, label: 'Receita' }, { value: 1, label: 'Despesa' }] }),
    ],
  },
  lancamentos: {
    title: 'Lançamentos Financeiros', singular: 'Lançamento', endpoint: '/lancamento', icon: 'receivable', financeOnly: true,
    searchKeys: ['id', 'descricao', 'categoriaFinanceiraId'],
    columns: [
      { key: 'id', label: 'Código' }, { key: 'data', label: 'Data', format: 'date' },
      { key: 'descricao', label: 'Descrição' }, { key: 'valor', label: 'Valor', format: 'currency' },
      { key: 'categoriaFinanceiraId', label: 'Categoria' },
    ],
    fields: [
      field('data', 'Data', 'date', { required: true }),
      field('valor', 'Valor', 'number', { required: true, min: '0.01', step: '0.01' }),
      field('categoriaFinanceiraId', 'Categoria', 'lookup', { endpoint: '/categoria-financeira', optionLabel: 'nome', required: true }),
      field('funcionarioId', 'Funcionário', 'lookup', { endpoint: '/funcionario', optionLabel: 'nome', required: true }),
      field('descricao', 'Descrição', 'textarea', { required: true }),
    ],
  },
}

export const navigation = [
  { key: 'dashboard', label: 'Visão geral', icon: 'home' },
  { key: 'clientes', label: 'Clientes', icon: 'users' },
  { key: 'fornecedores', label: 'Fornecedores', icon: 'handshake' },
  { key: 'funcionarios', label: 'Funcionários', icon: 'badge' },
  { key: 'usuarios', label: 'Usuários', icon: 'shield', role: 'Administrador' },
  { key: 'materias', label: 'Matérias-primas', icon: 'materials' },
  { key: 'produtos', label: 'Produtos', icon: 'box' },
  { key: 'composicoes', label: 'Composição', icon: 'materials' },
  { key: 'compras', label: 'Compras', icon: 'cart' },
  { key: 'itensCompra', label: 'Itens de compra', icon: 'cart' },
  { key: 'vendas', label: 'Vendas', icon: 'sale' },
  { key: 'itensVenda', label: 'Itens de venda', icon: 'sale' },
  { key: 'producao', label: 'Produção', icon: 'factory' },
  { key: 'itensProducao', label: 'Produtos das ordens', icon: 'factory' },
  { key: 'estoque', label: 'Estoque', icon: 'stock' },
  { key: 'caixa', label: 'Caixa', icon: 'cash', roles: ['Administrador', 'Gerente', 'Financeiro'] },
  { key: 'pagar', label: 'Contas a pagar', icon: 'payable' },
  { key: 'receber', label: 'Contas a receber', icon: 'receivable' },
  { key: 'categorias', label: 'Categorias financeiras', icon: 'cash', roles: ['Administrador', 'Gerente', 'Financeiro'] },
  { key: 'lancamentos', label: 'Lançamentos', icon: 'receivable', roles: ['Administrador', 'Gerente', 'Financeiro'] },
  { key: 'enderecos', label: 'Endereços', icon: 'pin' },
]
