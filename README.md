# AuthGuard

Bu projede business servislerimizi koruyan bir mekanizma geliþtirmeyi amaçladýk. Kütüphane olarak OpenIddict tercih ettik.

## AuthGuard.AuthServer

Bu proje servislerimizi kullanacak client'i ve servislerimizi koruyacak tokenýn üretilmesi için geliþtirilmiþtir.

- Endpointler \
`/client` [POST]-> ClientId, ClientSecret, DisplayName alanlarýyla bir post isteði atarak bir client oluþturuyoruz\
Örnek Model-> {"ClientId": "employee-service","ClientSecret": "P0NCxuJCtvYo2CI1S7hfZRwKr5beSHTX","DisplayName ": "Employee Service"}\
`/auth/token"` [POST]-> client_id, client_secret, grant_type, scope alanlarýyla "application/x-www-form-urlencoded" formatýnda bir post isteði atarak bir token oluþturuyoruz.
Örnek Model-> {"client_id": "employee-service", "client_secret": "P0NCxuJCtvYo2CI1S7hfZRwKr5beSHTX", "grant_type": "client_credentials", "scope":"read write delete"}

## AutGuard.API
Bu proje business kodlarýmýzý gerçekleþtirmek üzere geliþtirilmiþtir.

- Endpointler \
Tüm endpointler için Headerda 'Bearer {token}' þeklinde token bilgisi gerekmektedir.\
`/api/employee/{id}` [GET] -> Çalýþan bilgilerimizi getirir.\
`/api/employee"` [POST]-> Çalýþanýmýzý kaydediyoruz.
Örnek Model-> {"name": "Mert", "surname": "Savaþ", "age": 25}\
`/api/employee"` [PUT]-> Çalýþanýmýzýn bilgilerini güncelliyoruz.
Örnek Model-> Örnek Model-> {"id":1, "name": "Mert", "surname": "Savaþ", "age": 24}\
`/api/employee/{id}"` [DELETE]-> Çalýþanýmýzý siliyoruz.

Token ile güvenliði geçtikten sonra servislerimizi bu tokenýn içindeki claimslerde yer alan policy bilgileri ile korumaya çalýþtýk. Bu policiyler read, write, delete olmak üzere 3 adettir. Token üretilirken bu policy claimleri belirtilmektedir. Eðer client yaratýlýrken scope olarak delete almamýþsa daha sonra delete iþlemi yapamaz.