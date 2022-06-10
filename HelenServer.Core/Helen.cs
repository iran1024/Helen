using System.Security.Cryptography;

namespace HelenServer.Core
{
    public static partial class Helen
    {
        public const string Delimiter = "#";

        public static IServiceProvider Services { get; set; } = null!;

        public static string NewGuid => Guid.NewGuid().ToString("N");

        public static string ToMD5(string value)
        {
            using var md5 = MD5.Create();

            var buffer = md5.ComputeHash(Encoding.UTF8.GetBytes(value));

            var builder = new StringBuilder();

            foreach (var b in buffer)
            {
                builder.Append(b.ToString("x2"));
            }

            return builder.ToString();
        }

        public static string ToMD5(Stream stream)
        {
            using var md5 = MD5.Create();

            var buffer = md5.ComputeHash(stream);

            var builder = new StringBuilder();

            foreach (var b in buffer)
            {
                builder.Append(b.ToString("x2"));
            }

            return builder.ToString();
        }

        public static bool TryFromBase64String(string base64, out string json)
        {
            var buffer = new Span<byte>(new byte[base64.Length]);

            var width = (base64.Length / 4 * 4) + (base64.Length % 4 == 0 ? 0 : 4);

            base64 = base64.PadRight(width, '=');

            if (Convert.TryFromBase64String(base64, buffer, out _))
            {
                json = Encoding.UTF8.GetString(Convert.FromBase64String(base64));

                return true;
            }
            else
            {
                json = string.Empty;

                return false;
            }
        }

        public static string Base64EncodeImage(Stream stream, string extension)
        {
            using var ms = new MemoryStream();

            var __buffer = new byte[1024];
            int r = -1;
            while ((r = stream.Read(__buffer, 0, __buffer.Length)) > 0)
            {
                ms.Write(__buffer, 0, r);
            }

            return $"data:image/{extension};base64,{Convert.ToBase64String(ms.GetBuffer())}";
        }

        public static T ToEnum<T>(this string enumName)
        {
            return (T)Enum.Parse(typeof(T), enumName);
        }

        public static string GetEnumName<T>(int value)
        {
            return Enum.GetName(typeof(T), value) ?? string.Empty;
        }

        public static string GenerateVerifyCode(int length)
        {
            var result = new StringBuilder();

            for (var i = 0; i < length; i++)
            {
                var r = new Random(Guid.NewGuid().GetHashCode());
                result.Append(r.Next(0, 10));
            }

            return result.ToString();
        }

        public static string[] ResolveCompositField(string value)
        {
            return value.Split(Delimiter);
        }

        public static string CompositField(string[] values)
        {
            return string.Join(Delimiter, values);
        }
    }
}