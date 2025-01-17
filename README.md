# Access Microsoft Graph on Behalf of a User

This document provides a guide to implementing delegated access for Microsoft Graph, allowing an application to act on behalf of a user. This is achieved using the OAuth 2.0 Authorization Code Grant Flow.

---

## Table of Contents

1. [Overview](#overview)  
2. [Prerequisites](#prerequisites)  
3. [Steps to Implement](#steps-to-implement) 
    - [Step 1: Request Authorization](#step-1-request-authorization)  
    - [Step 2: Request an Access Token](#step-2-request-an-access-token)  
    - [Step 3: Use the Access Token](#step-3-use-the-access-token)  
    - [Step 4: Renew the Access Token](#step-4-renew-the-access-token)  
4. [Using MSAL for Simplified Authentication](#using-msal-for-simplified-authentication)  
5. [Related Resources](#related-resources)  

---

## Overview

Microsoft Graph requires an access token to interact with its APIs securely. Tokens can represent:

- **Delegated access**: On behalf of a user.
- **App-only access**: Using the app's identity.

This document focuses on delegated access using the **OAuth 2.0 Authorization Code Flow**.

---

## Prerequisites

Before proceeding:

1. Understand the [Microsoft identity platform authentication and authorization basics](https://learn.microsoft.com/azure/active-directory/develop/authentication-vs-authorization).
2. Register your application in **Microsoft Entra ID**:
    - Save the `Application (Client) ID`.
    - Obtain a `Client Secret` or use a certificate/federated credential (for server-side apps only).
    - Configure a valid `Redirect URI`.
3. Familiarize yourself with [Microsoft Graph permissions and consent](https://learn.microsoft.com/azure/active-directory/develop/v2-permissions-and-consent).

---

## Steps to Implement

### Before starting: Add Variables to User Secrets

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

### Step 1: Request Authorization

Redirect the user to the `/authorize` endpoint to grant the app permission.

#### Example Request:

```http
GET https://login.microsoftonline.com/{tenant}/oauth2/v2.0/authorize
?client_id={APPLICATION_ID}
&response_type=code
&redirect_uri={ENCODED_REDIRECT_URI}
&response_mode=query
&scope=offline_access%20User.Read%20Mail.Read
&state={UNIQUE_STATE_VALUE}
```

#### Key Parameters:

- `tenant`: Defines who can sign in. Options: `common`, `organizations`, `consumers`, or a specific tenant ID.
- `client_id`: Your app's Application ID.
- `response_type`: Must be `code` for this flow.
- `redirect_uri`: The URI configured in app registration.
- `scope`: Permissions requested (e.g., `User.Read`, `Mail.Read`).
- `state`: A unique value for CSRF protection.

#### User Consent:

The user logs in and consents to the requested permissions. Upon success, an authorization code is returned in the query string of the `redirect_uri`.

---

### Step 2: Request an Access Token

Exchange the authorization code for an access token by calling the `/token` endpoint.

#### Example Request:

```
POST https://login.microsoftonline.com/{tenant}/oauth2/v2.0/token
Content-Type: application/x-www-form-urlencoded

client_id={APPLICATION_ID}
&scope=User.Read%20Mail.Read
&code={AUTHORIZATION_CODE}
&redirect_uri={ENCODED_REDIRECT_URI}
&grant_type=authorization_code
&client_secret={CLIENT_SECRET}
```

#### Key Parameters:

- `grant_type`: Must be `authorization_code`.
- `code`: The authorization code from Step 1.
- `client_secret`: Required for web apps.

#### Response:

The response contains the access token, refresh token, and expiry details:
```json
{
    "access_token": "eyJ0eXAiOiJKV1QiLCJhb...",
    "token_type": "Bearer",
    "expires_in": 3600,
    "refresh_token": "AwABAAAAvPM1KaPl...",
    "scope": "User.Read Mail.Read"
}
```

---

### Step 3: Use the Access Token

Attach the access token to the `Authorization` header as a Bearer token to make requests to Microsoft Graph.

#### Example Request:

```http
GET https://graph.microsoft.com/v1.0/me
Authorization: Bearer {ACCESS_TOKEN}
```

#### Response:

Returns user details:
```json
{
    "displayName": "John Doe",
    "mail": "john.doe@example.com",
    "id": "12345678-90ab-cdef-1234-567890abcdef"
}
```

---

### Step 4: Renew the Access Token

Access tokens are short-lived. Use the refresh token to request a new access token by calling the `/token` endpoint.

#### Example Request:

```
POST https://login.microsoftonline.com/{tenant}/oauth2/v2.0/token
Content-Type: application/x-www-form-urlencoded

client_id={APPLICATION_ID}
&grant_type=refresh_token
&refresh_token={REFRESH_TOKEN}
&client_secret={CLIENT_SECRET}
```

---

## Using MSAL for Simplified Authentication

Instead of manually handling tokens, use the **Microsoft Authentication Library (MSAL)** to:
- Handle authentication flows.
- Cache tokens securely.
- Manage refresh tokens.

Explore [MSAL libraries and samples](https://learn.microsoft.com/azure/active-directory/develop/msal-overview) to simplify integration.

---

## Related Resources

- [Microsoft Graph Tutorials](https://learn.microsoft.com/graph/tutorials)
- [Microsoft Identity Platform Code Samples](https://learn.microsoft.com/azure/active-directory/develop/sample-v2-code)
- [OAuth 2.0 Authorization Code Flow](https://learn.microsoft.com/azure/active-directory/develop/v2-oauth2-auth-code-flow)

--- 

For additional guidance, visit the official [Microsoft Graph documentation](https://learn.microsoft.com/graph).
