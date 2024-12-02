# GraphAPIDelegatedFlow

## Add Variables to User Secrets

To keep sensitive information out of your code, you can use [User Secrets](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets). Follow these steps to set up user secrets for your project:

1. Right-click the project in Solution Explorer and select **Manage User Secrets**. This will create a `secrets.json` file in the `%APPDATA%\Microsoft\UserSecrets\<user_secrets_id>\` directory.

2. Add the following JSON to the `secrets.json` file:

    ```json
    {
      "ClientID": "[Guid]",
      "TenantID": "[Guid]",
      "ClientSecret": "[string]",
      "Scopes": "user.read mail.send offline_access",
      "CallBackUrl": "https://localhost:7009/msgraphapi/Callback"
    }
    ```

3. In your `Startup.cs` or `Program.cs`, add the following code to load the secrets:

    ```csharp
    var builder = WebApplication.CreateBuilder(args);

    builder.Configuration.AddUserSecrets<Program>();

    var configuration = builder.Configuration;

    var clientId = configuration["ClientID"];
    var tenantId = configuration["TenantID"];
    var clientSecret = configuration["ClientSecret"];
    var scopes = configuration["Scopes"];
    var callBackUrl = configuration["CallBackUrl"];
    ```

By following these steps, you can securely manage sensitive information in your application.
