# ImpérioSys — Front-end

Interface web do GrillSystem, desenvolvida com React e Vite e integrada aos contratos atuais da API ASP.NET Core.

## Executar em desenvolvimento

1. Inicie a API na pasta `Back-End`:

   ```powershell
   dotnet run
   ```

2. Em outro terminal, inicie o front-end:

   ```powershell
   npm install
   npm run dev
   ```

O Vite encaminha automaticamente as chamadas iniciadas por `/api` para `http://localhost:5225`, porta definida no `launchSettings.json` da API.

## Configuração da API

O arquivo `.env.example` documenta a variável `VITE_API_URL`. Durante o desenvolvimento, o valor `/api` usa o proxy do Vite. Em produção, defina a URL pública da API ou configure o servidor para encaminhar `/api` para o back-end.

## Validação

```powershell
npm run lint
npm run build
```

O token JWT é mantido no navegador e enviado automaticamente no cabeçalho `Authorization`. Respostas `401` encerram a sessão; permissões continuam sendo validadas pela API.
