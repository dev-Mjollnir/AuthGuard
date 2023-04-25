# AuthGuard

Bu projede business servislerimizi koruyan bir mekanizma geli�tirmeyi ama�lad�k. K�t�phane olarak OpenIddict tercih ettik.

## AuthGuard.AuthServer

Bu proje servislerimizi kullanacak client'i ve servislerimizi koruyacak token�n �retilmesi i�in geli�tirilmi�tir.

- Endpointler \
`/client` [POST]-> ClientId, ClientSecret, DisplayName alanlar�yla bir post iste�i atarak bir client olu�turuyoruz\
�rnek Model-> {"ClientId": "employee-service","ClientSecret": "P0NCxuJCtvYo2CI1S7hfZRwKr5beSHTX","DisplayName ": "Employee Service"}\
`/auth/token"` [POST]-> client_id, client_secret, grant_type, scope alanlar�yla "application/x-www-form-urlencoded" format�nda bir post iste�i atarak bir token olu�turuyoruz.
�rnek Model-> {"client_id": "employee-service", "client_secret": "P0NCxuJCtvYo2CI1S7hfZRwKr5beSHTX", "grant_type": "client_credentials", "scope":"read write delete"}

## AutGuard.API
Bu proje business kodlar�m�z� ger�ekle�tirmek �zere geli�tirilmi�tir.

- Endpointler \
T�m endpointler i�in Headerda 'Bearer {token}' �eklinde token bilgisi gerekmektedir.\
`/api/employee/{id}` [GET] -> �al��an bilgilerimizi getirir.\
`/api/employee"` [POST]-> �al��an�m�z� kaydediyoruz.
�rnek Model-> {"name": "Mert", "surname": "Sava�", "age": 25}\
`/api/employee"` [PUT]-> �al��an�m�z�n bilgilerini g�ncelliyoruz.
�rnek Model-> �rnek Model-> {"id":1, "name": "Mert", "surname": "Sava�", "age": 24}\
`/api/employee/{id}"` [DELETE]-> �al��an�m�z� siliyoruz.

Token ile g�venli�i ge�tikten sonra servislerimizi bu token�n i�indeki claimslerde yer alan policy bilgileri ile korumaya �al��t�k. Bu policiyler read, write, delete olmak �zere 3 adettir. Token �retilirken bu policy claimleri belirtilmektedir. E�er client yarat�l�rken scope olarak delete almam��sa daha sonra delete i�lemi yapamaz.