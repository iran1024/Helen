using System.Web;

namespace HelenServer.Core;

public static partial class OperationExtensions
{
    public static OperationUser GetUser(this HttpContext context)
    {
        var headers = context.Request.Headers;

        var authorization = headers["X-Forwarded-Authorization"];

        if (Helen.TryFromBase64String(authorization.ToString(), out string json))
        {
            return GetUser(json);
        }
        else
        {
            return OperationUser.None;
        }
    }

    public static OperationUser GetUser(this ServerCallContext context)
    {
        var userId = context.RequestHeaders.GetEntry(nameof(OperationUser.UserId));

        var userName = context.RequestHeaders.GetEntry(nameof(OperationUser.Username));

        var roles = context.RequestHeaders.GetEntry(nameof(OperationUser.Roles));

        return new OperationUser(
            userId?.Value ?? string.Empty,
            userName?.Value ?? string.Empty,
            roles?.Value?.Split(Helen.Delimiter) ?? Array.Empty<string>(),
            !string.IsNullOrWhiteSpace(userId?.Value));
    }

    public static Metadata ToMetadata(this OperationUser user)
    {
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        var metadata = new Metadata
        {
            { nameof(user.UserId), user.UserId },
            { nameof(user.Username), HttpUtility.UrlEncode(user.Username) },
            { nameof(user.Roles), string.Join(Helen.Delimiter, user.Roles) }
        };

        return metadata;
    }

    public static Metadata ToMetadata(this IHeaderDictionary dic)
    {
        var metadata = new Metadata();

        var user = OperationUser.None;

        metadata.Add(nameof(user.UserId), dic[nameof(user.UserId)].Count > 0 ? dic[nameof(user.UserId)] : string.Empty);
        metadata.Add(nameof(user.Username), dic[nameof(user.Username)].Count > 0 ? dic[nameof(user.Username)] : string.Empty);
        metadata.Add(nameof(user.Roles), dic[nameof(user.Roles)].Count > 0 ? dic[nameof(user.Roles)] : string.Empty);

        return metadata;
    }

    public static Operation<TParameter> GetOperation<TParameter>(this HttpContext context, TParameter model)
    {
        return new Operation<TParameter>(context.GetUser()) { Parameter = model };
    }

    public static Operation<TKey, TParameter> GetOperation<TKey, TParameter>(
        this HttpContext context, TKey key, TParameter model)
    {
        return new Operation<TKey, TParameter>(context.GetUser(), key, model);
    }

    public static Operation<T> GetOperation<T>(this ServerCallContext context, T value)
    {
        return new Operation<T>(context.GetUser(), value);
    }

    public static PageOperation<TParameter> GetPageOperation<TParameter>(
        this HttpContext context, TParameter model, int? pageNum, int? pageSize)
    {
        return new PageOperation<TParameter>(context.GetUser(), pageNum, pageSize)
        {
            Parameter = model
        };
    }

    private static OperationUser GetUser(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            return OperationUser.None;
        }

        var element = JsonDocument.Parse(json).RootElement;

        string uid = string.Empty;
        string uname = string.Empty;
        string[]? roles = Array.Empty<string>();

        if (element.TryGetProperty("sub", out var uidElement))
        {
            uid = uidElement.GetString()!;
        }

        if (element.TryGetProperty("name", out var unameElement))
        {
            uname = unameElement.GetString()!;
        }

        if (element.TryGetProperty("role", out var roleElement))
        {
            if (roleElement.ValueKind == JsonValueKind.Array)
            {
                foreach (var role in roleElement.EnumerateArray())
                {
                    roles = roles.Append(role.GetString()!).ToArray();
                }
            }
            else if (roleElement.ValueKind == JsonValueKind.String)
            {
                roles = roles.Append(roleElement.GetString()!).ToArray();
            }
        }


        return new OperationUser(uid, uname, roles, true);
    }
}