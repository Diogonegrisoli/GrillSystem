# Revisão de segurança e integridade

## Implementado

- ASP.NET Core Identity com `UserManager`, `SignInManager`, stores do Entity Framework e perfis.
- Autenticação JWT com validação de emissor, audiência, assinatura, expiração sem tolerância de relógio e `security stamp`.
- Bloqueio por 15 minutos após três falhas de login e rejeição de funcionário inativo.
- Perfis `Administrador`, `Gerente`, `Operador`, `Mestre de Produção`, `Financeiro`, `Vendedor` e `Comprador` disponíveis como claims de papel no JWT.
- Política global que exige autenticação, política administrativa para usuários e política de gestão para exclusões.
- Respostas de usuário por DTO, sem serializar hash, stamps ou dados internos do Identity.
- Primeiro cadastro como administrador; os cadastros seguintes exigem administrador.
- Proteção contra exclusão do próprio usuário e contra remoção do último administrador.
- Tratamento centralizado de exceções com Problem Details, `traceId` e ocultação de detalhes internos em erros 500.
- Migração `IdentityEAutorizacao`, preservando a coluna `SenhaHash` e adaptando registros existentes.
- Correções em CPF/CNPJ, quantidade de item de venda, quantidade de matéria-prima, data de recebimento e DTO de fornecedor.

## Matriz de autorização

| Operação | Operador | Gerente | Administrador |
|---|---:|---:|---:|
| Login e consulta de dados de negócio | Sim | Sim | Sim |
| Inclusão e alteração de dados de negócio | Sim | Sim | Sim |
| Exclusão de dados de negócio | Não | Sim | Sim |
| Listar/criar/alterar/excluir usuários | Não | Não | Sim |

O endpoint `POST /usuario` é anônimo somente enquanto não existe nenhum usuário. O primeiro usuário sempre recebe o perfil `Administrador`, independentemente do perfil enviado.

## Aplicação da migração

Configure `DefaultConnection`, `JWT_KEY` e, se necessário, `MYSQL_VERSION` e execute:

```powershell
dotnet tool restore
dotnet ef database update
```

Antes de aplicar em uma base existente, confirme que não existem e-mails, CPF/CNPJ ou códigos duplicados; vínculos repetidos entre as tabelas associativas; mais de um usuário para o mesmo funcionário; ou mais de uma conta para o mesmo pedido. A migração faz uma verificação de duplicidade antes das alterações estruturais, e os novos índices únicos rejeitam essas inconsistências em vez de escolher silenciosamente qual registro manter.

A migração `IntegridadeRegrasNegocio` preserva os dados existentes e recompõe o preço unitário dos itens de venda, o total dos pedidos de venda e das contas a receber. Como não é possível deduzir com segurança informações que antes não eram armazenadas, cadastros antigos devem receber manualmente:

- a quantidade de cada matéria-prima da ficha técnica do produto;
- a quantidade e o custo unitário dos itens de pedidos de compra.

Enquanto esses valores estiverem zerados, a API impede a finalização da produção e o recebimento da compra. Isso evita alterar o estoque com uma suposição incorreta.

Senhas antigas em texto puro deixam de ser aceitas. Elas devem ser redefinidas para um hash do Identity antes da entrada em produção.

## Correções das regras de negócio

1. **Estoque e concorrência:** entradas e saídas atualizam o saldo por operação atômica. Uma saída só ocorre se houver saldo. O recebimento de uma compra aumenta areia, cimento, tijolo, barra de ferro e demais matérias-primas; a finalização da produção consome a ficha técnica e aumenta o produto acabado; o envio da venda baixa o produto acabado. Todas as operações compostas usam transação e bloqueiam processamento duplicado concorrente.
2. **Totais calculados no servidor:** pedidos começam com total zero. O total da venda é `quantidade × preço capturado no item`; o total da compra é `quantidade × custo unitário`. Contas a receber e a pagar usam e acompanham o total do pedido. Esses valores não são mais aceitos do cliente.
3. **Datas:** data obrigatória, data não futura e ordem cronológica são validadas para pedidos, entrega/recebimento, produção, lançamento, vencimento, pagamento e recebimento.
4. **Erros HTTP:** recurso ausente retorna 404, conflito/concorrência retorna 409, regra de negócio retorna 422 e dados inválidos retornam 400. Detalhes internos do banco não são expostos ao cliente.
5. **Integridade relacional:** índices únicos impedem CPF/CNPJ, códigos, contas por pedido e vínculos N:N duplicados. Exclusões que apagariam histórico comercial, financeiro ou de estoque foram restringidas.
6. **Consultas:** todas as listagens usam paginação (`pagina`, `tamanhoPagina`, máximo 100) e consultas somente de leitura usam `AsNoTracking`.
7. **Nulabilidade:** modelos e DTOs refletem os campos obrigatórios e opcionais do domínio; a compilação não apresenta avisos de nulabilidade.
8. **Estados permitidos:** compras seguem `Pendente → Solicitado → EmAndamento → Entregue`; vendas seguem `Pendente → EmProducao → Enviado → Entregue`, com cancelamento apenas antes do envio; produção só pode finalizar a partir de `EmAndamento`. A API exige itens e ficha técnica válidos antes de avançar, e itens/dados estruturais ficam imutáveis após a etapa permitida.

## Risco de autenticação ainda pendente

Refresh token, recuperação de senha por canal verificado, confirmação de e-mail e segundo fator não foram implementados nesta alteração. Essa parte exige definir o canal de envio (por exemplo SMTP, Microsoft 365 ou outro provedor), a validade dos tokens e quais perfis terão segundo fator obrigatório. Implementar sem essa definição criaria um fluxo incompleto ou inseguro.
