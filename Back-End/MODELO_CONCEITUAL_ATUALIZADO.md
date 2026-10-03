# Adaptação ao modelo conceitual atualizado

O modelo do diagrama foi incorporado à API mantendo os campos antigos necessários para compatibilidade com registros e clientes da API existentes. Não executei as migrações em um banco de dados real.

## Correspondência com o diagrama

| Conceito | Implementação |
| --- | --- |
| Pessoa física / jurídica | `PessoaFisica` e `PessoaJuridica`, cada uma com relação 1:1 com `Cliente`. O `Tipo` do cliente determina qual subtipo é criado. |
| Endereço | `Endereco` com vínculos N:N a cliente, fornecedor e funcionário. |
| Usuário / perfil | Identity já fornece `Usuarios`, `Perfis` e `UsuariosPerfis` (além das tabelas auxiliares de autenticação). |
| Contas parceladas | `ContaPagarParcelada` e `ContaReceberParcelada`, uma ou mais por conta, com vencimento, valor e estado próprios. |
| Caixa | `Caixa` e `MovimentacaoCaixa`; uma liquidação vincula uma parcela e atualiza o saldo em uma transação. |
| Estoque | Movimentações distinguem origem manual, compra ou produção; produção registra o identificador da ordem. |

Os textos antigos `Cliente.Endereco` e `Fornecedor.Endereco` e os campos `Cliente.Nome` e `Cliente.CpfCnpj` permanecem por compatibilidade. Endereços antigos não são convertidos automaticamente em registros estruturados; cadastre e vincule os novos endereços conforme necessário. Cada conta antiga recebe uma parcela equivalente durante a migração. Não há movimentação de caixa retroativa para pagamentos antigos.

## Rotas novas

- `GET/POST /endereco`, `GET/PUT/DELETE /endereco/{id}`.
- `POST/DELETE /endereco/{id}/vinculos` com `tipoTitular` (`0` cliente, `1` fornecedor, `2` funcionário) e `titularId`.
- `GET/POST /caixa`, `GET /caixa/{id}`, `GET /caixa/{id}/movimentacoes`, `POST /caixa/{id}/fechar`.
- `POST /caixa/{id}/liquidar-parcela` com `parcelaId`, `tipo` (`0` entrada/conta a receber, `1` saída/conta a pagar) e `descricao`.
- `GET /conta-pagar/{id}` e `GET /conta-receber/{id}` agora incluem as parcelas.

As rotas de caixa exigem perfil Administrador, Gerente ou Financeiro. Liquidação direta nos campos `DataPagamento`/`DataRecebimento` das contas não é aceita; use o caixa. A liquidação é integral por parcela (sem pagamento parcial). O caixa de saída precisa estar aberto e ter saldo suficiente.

## Aplicar ao banco

1. Faça backup do banco e confira documentos de clientes já cadastrados (tipo, CPF/CNPJ, unicidade). A migração preserva os valores existentes e usa a data de execução como data de cadastro dos clientes antigos.
2. Configure `.env`/conexão como já feito no projeto.
3. Revise as migrações `ModeloConceitualAtualizado` e `AjustesModeloConceitual` e execute, na pasta `Back-End`, `dotnet ef database update` em uma cópia do banco primeiro.
4. Valide clientes PF/PJ, parcelas históricas, saldos e novos vínculos antes de aplicar em produção.

O front-end deverá enviar os campos novos dos pedidos (`parcelas`, `formaPagamento` e, na venda, `desconto`/`funcionarioId`) e consultar as parcelas antes de liquidá-las. Os valores padrão preservam pedidos antigos com uma parcela.
