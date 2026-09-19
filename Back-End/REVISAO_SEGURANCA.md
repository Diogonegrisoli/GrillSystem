# Revisão de segurança e integridade

## Implementado

- ASP.NET Core Identity com `UserManager`, `SignInManager`, stores do Entity Framework e perfis.
- Autenticação JWT com validação de emissor, audiência, assinatura, expiração sem tolerância de relógio e `security stamp`.
- Bloqueio por 15 minutos após cinco falhas de login e rejeição de funcionário inativo.
- Perfis `Administrador`, `Gerente` e `Operador` incluídos como claims de papel no JWT.
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

Configure `DefaultConnection` e `JWT_KEY` e execute:

```powershell
dotnet tool restore
dotnet ef database update
```

Antes de aplicar em uma base existente, confirme que não existem e-mails duplicados nem mais de um usuário para o mesmo funcionário. A nova estrutura cria índices únicos para essas duas invariantes. A migração atribui `Administrador` ao usuário de menor ID e `Operador` aos demais.

Senhas antigas em texto puro deixam de ser aceitas. Elas devem ser redefinidas para um hash do Identity antes da entrada em produção.

## Riscos encontrados que exigem regra de negócio

1. Movimentações não atualizam automaticamente os saldos de matéria-prima/produto. Operações concorrentes podem produzir estoque inconsistente.
2. Totais de pedidos e contas são aceitos diretamente do cliente, sem recomposição pelos itens associados.
3. Datas relacionadas ainda não são validadas uniformemente em todos os DTOs (pedido, entrega, produção, pagamento e vencimento).
4. Vários serviços retornam HTTP 500 para recurso inexistente porque usam `Exception` genérica; devem migrar gradualmente para exceções tipadas/404.
5. Relações N:N não possuem índices compostos únicos, permitindo vínculos duplicados.
6. Listagens não possuem paginação e diversas consultas são rastreadas mesmo quando somente leitura.
7. Há avisos de nulabilidade em modelos/DTOs antigos; as propriedades precisam ser inicializadas ou marcadas conforme a nulabilidade real do domínio.
8. Não há refresh token, recuperação de senha por canal verificado, confirmação de e-mail nem segundo fator.

As regras de estoque, composição de preços e estados permitidos devem ser definidas antes de automatizar as correções, pois alteram o comportamento funcional do sistema.
