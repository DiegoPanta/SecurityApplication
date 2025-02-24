# SecurityApplication

## Visão geral
O SecurityApplication é um aplicativo baseado em .NET que se concentra em funcionalidades relacionadas à segurança.

## Recursos
- Mecanismos de autenticação de segurança
- Manuseio de token JWT
- Controle de acesso do usuário
- Definições de configuração armazenadas em `dotnet user-secrets set "JwtSettings:Key"`

## Estrutura do projeto
```
SecurityApplication/
├── SecurityApplication.sln  # Solution file
├── .vs/                     # Visual Studio configuration files
├── Presentation/            # Source code directory (if available)
```

## Requisitos
- .NET SDK (recomenda-se a versão mais recente)
- Visual Studio (para desenvolvimento)
- ASP.NET Core

## Instruções de configuração
1. Clone o repositório.
2. Abra o arquivo `SecurityApplication.sln` no Visual Studio.
3. Restaure as dependências:
   ```sh
   dotnet restore
   ```
4. Compile e execute o projeto:
   ```sh
   dotnet run
   ```

## Considerações sobre segurança
- Certifique-se de que o arquivo `secretkey` não esteja exposto no controle de versão.
- Use variáveis de ambiente para configurações confidenciais.
- Implemente mecanismos adequados de autenticação e autorização.

