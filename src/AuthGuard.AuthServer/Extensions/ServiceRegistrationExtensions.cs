using Microsoft.EntityFrameworkCore;

namespace AuthGuard.AuthServer.Extensions
{
    public static class ServiceRegistrationExtensions
    {
        public static void RegisterOpenIddict(this WebApplicationBuilder builder)
        {
            builder.Services.AddOpenIddict()
            //OpenIddict core/çekirdek yapılandırmaları gerçekleştiriliyor.
            .AddCore(options =>
            {
                //Entity Framework Core kullanılacağı bildiriliyor.
                options.UseEntityFrameworkCore()
                       //Kullanılacak context nesnesi bildiriliyor.
                       .UseDbContext<AuthGuardDbContext>();
            })
            //OpenIddict server yapılandırmaları gerçekleştiriliyor.
            .AddServer(options =>
            {
                //Token talebinde bulunulacak endpoint'i set ediyoruz.
                options.SetTokenEndpointUris("/auth/token");
                //Akış türü olarak Client Credentials Flow'u etkinleştiriyoruz.
                options.AllowClientCredentialsFlow();
                //Signing ve encryption sertifikalarını ekliyoruz.
                options.AddEphemeralEncryptionKey()
                       .AddEphemeralSigningKey()
                       //Normalde OpenIddict üretilecek token'ı güvenlik amacıyla şifreli bir şekilde bizlere sunmaktadır.
                       //Haliyle jwt.io sayfasında bu token'ı çözümleyip görmek istediğimizde şifresinden dolayı
                       //incelemede bulunamayız. Bu DisableAccessTokenEncryption özelliği sayesinde üretilen access token'ın
                       //şifrelenmesini iptal ediyoruz.
                       .DisableAccessTokenEncryption();
                //OpenIddict Server servislerini IoC Container'a ekliyoruz.
                options.UseAspNetCore()
                        .DisableTransportSecurityRequirement()
                       //EnableTokenEndpointPassthrough : OpenID Connect request'lerinin OpenIddict tarafından işlenmesi için gerekli konfigürasyonu sağlar.
                       .EnableTokenEndpointPassthrough();
                //Yetkileri(scope) belirliyoruz.
                options.RegisterScopes("read", "write");
            });
        }

        public static void RegisterDbContext(this WebApplicationBuilder builder)
        {
            //OpenIddict'i SQL Server'ı kullanacak şekilde yapılandırıyoruz.
            builder.Services.AddDbContext<AuthGuardDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("SQLServer"));
                //OpenIddict tarafından ihtiyaç duyulan Entity sınıflarını kaydediyoruz.
                options.UseOpenIddict();
            });
        }
    }
}
