#  Proje

Bu proje, görevlerin kolayca takip edilmesini sağlayan bir TaskBoard uygulamasıdır.
Proje modern yazılım mimarisine uygun olarak frontend ve backend katmanlarına ayrılmıştır.
Kullanıcı arayüzünde semantik HTML5 standartlarına tam uyum hedeflenmiştir.
Kullanıcılar yeni görev başlığı girerek sisteme yeni görev ekleyebilirler.
Mevcut görevler liste görünümü üzerinden takip edilebilir durumdadır.
Gereksiz div yapıları yerine header, main, section ve footer gibi anlamsal etiketler kullanılmıştır.
Form elemanlarında erişilebilirlik için label ve input bağlantıları kurulmuştur.
İlerleyen aşamalarda CSS ile stil eklenecek ve JavaScript ile dinamik hale getirilecektir.
Backend tarafında C# entegrasyonu yapılarak verilerin kalıcı olması sağlanacaktır.
Bu repository, projenin 1. gün temel ortam ve arayüz iskeletini içermektedir.


Gün 1: Ortam Kurulumu ve Proje İskeleti
Stajın ilk gününde geliştirme ortamı yapılandırıldı. .NET SDK, VS Code ve temel C# geliştirme eklentileri kuruldu. Git versiyon kontrol sistemi başlatılarak GitHub deposu bağlandı. Projenin backend ve frontend temel dizin mimarisi oluşturularak ilk commit atıldı.

Gün 2: HTML İskeleti ve Semantik Web Standartları
Görev panosu (TaskBoard) arayüzü için semantik HTML5 standartlarına uygun sayfa yapısı kurgulandı. Başlıklar, görev listeleme alanları ve form elemanları yerleştirildi. Sayfa erişilebilirliği temel düzeyde sağlandı ve form etiketleri düzenlendi.

Gün 3: CSS Temelleri ve Temel Sayfa Düzeni
Hazırlanan HTML iskeletine modern CSS stilleri uygulandı. Tipografi, renk paleti ve kart görünümleri tanımlandı. Temel Flexbox kuralları kullanılarak görev kartlarının ve form alanlarının hizalanması sağlandı.

Gün 4: Responsive Tasarım ve CSS Grid
Arayüz mobil, tablet ve masaüstü ekran boyutlarına uyumlu hale getirildi. CSS Grid ve Medya Sorguları (Media Queries) kullanılarak duyarlı bir pano tasarımı elde edildi. Ekran genişliğine göre kart yerleşimleri optimize edildi.

Gün 5: JavaScript Temelleri ve DOM Manipülasyonu
Vanilla JavaScript kullanılarak statik arayüze dinamizm kazandırıldı. Sayfa üzerindeki form dinleyicileri (event listeners) yazıldı ve JavaScript ile dinamik olarak HTML kartları üretildi (createElement, appendChild).

Gün 6: Dinamik Görev Yönetimi ve LocalStorage
Eklenen görevlerin sayfa yenilendiğinde kaybolmaması için tarayıcının localStorage API'si entegre edildi. Görev ekleme, listeleme ve yerel hafızaya kaydetme döngüsü kuruldu; veriler JSON formatında saklandı ve geri okundu.

Gün 7: UI İyileştirmeleri, Filtreleme ve Arama
Kullanıcı deneyimini artırmak amacıyla görevler arasında metin bazlı arama ve öncelik derecesine göre filtreleme özellikleri geliştirildi. Arayüze anlık sonuç getiren arama kutusu ve filtre dropdown'ları eklendi.

Gün 8: C# Temelleri ve Konsol Uygulaması
Backend tarafına geçiş yapılarak C# dili ile temel nesne yönelimli programlama (OOP) yapıları incelendi. Görev modelini temsil eden sınıflar (classes) ve konsol üzerinden çalışan temel menü mimarisi kuruldu.

Gün 9: ASP.NET Core Web API Giriş
ASP.NET Core Web API projesi ayağa kaldırıldı. RESTful mimari prensiplerine giriş yapılarak controller yapısı, route mekanizmaları ve HTTP metotları (GET, POST) yapılandırıldı.

Gün 10: In-Memory Veri Yönetimi ve Mock Servisler
Veritabanı entegrasyonu öncesinde controller operasyonlarını desteklemek amacıyla bellek içi (In-Memory) liste yapıları ve mock veri servisleri yazıldı. Temel CRUD işlemleri simüle edildi.

Gün 11: Dependency Injection ve Servis Yaşam Döngüleri
ASP.NET Core'un yerleşik Dependency Injection (DI) konteyneri incelendi. Transient, Scoped ve Singleton servis yaşam döngüleri deneyimlendi. Bağımlılıkların gevşek bağlı (loosely coupled) hale getirilmesi sağlandı.

Gün 12: Asenkron Programlama (async/await)
I/O işlemlerinde sunucu kaynaklarını verimli kullanmak adına controller ve servis katmanındaki tüm operasyonlar asenkron (async/await) yapıya dönüştürüldü. Task yapıları standartlaştırıldı.

Gün 13: Veritabanı Şeması Tasarımı ve SQLite
Kalıcı veri yönetimi için SQLite veritabanı seçildi. Görev panosunun gereksinim duyduğu tablo şemaları, birincil anahtarlar (Primary Key), zorunlu alanlar ve indeks yapıları belirlendi.

Gün 14: Entity Framework Core ve Seed Verileri
SQLite veritabanı TaskBoardDbContext aracılığıyla Entity Framework Core'a bağlandı. Code-First yaklaşımıyla veritabanı tabloları oluşturuldu. Başlangıçta tohum (seed) veriler veritabanına yazıldı ve /api/tasks üzerinden JSON formatında doğrulandı.

Gün 15: CRUD Operasyonları, Service Katmanı ve DTO Mimarisi
Controller içerisindeki doğrudan veritabanı erişimi kaldırılarak temiz backend mimarisine geçildi. ITaskService ve TaskService katmanı kurularak AddScoped ile kaydedildi. Entity nesnelerinin dış dünyaya sızmaması için TaskResponse, CreateTaskDto ve UpdateTaskDto yapıları oluşturuldu. Tüm CRUD fonksiyonları yazıldı; bulunamayan kayıtlar için 404 NotFound, başarılı silmeler için 204 NoContent kodları bağlandı.

Gün 16: Frontend API Entegrasyonu ve Gerçek CRUD Ekranı
Frontend ile Backend API tam entegre hale getirildi. Merkezi apiClient.js modülü üzerinden fetch istekleri yapılandırılarak GET, POST ve DELETE operasyonları arayüze bağlandı. Asenkron işlemler esnasında butonlar kilitlenerek mükerrer istekler önlendi; silme işlemi öncesinde onay penceresi (confirm) ve olası hatalarda form altı uyarı mesajı eklendi.

Gün 17: Basit Kimlik Doğrulama, Yetki ve Kullanıcı Deneyimi
Cookie Authentication altyapısı kurularak Admin ve User rollerine sahip demo kullanıcı sistemi entegre edildi. Kullanıcı bazlı claim yapıları oluşturuldu. API tarafında DELETE endpoint'i [Authorize(Roles = "Admin")] ile koruma altına alındı. Frontend tarafında giriş/çıkış akışı sağlandı, normal kullanıcılar için silme butonu arayüzde gizlendi ve yetkisiz silme girişimlerinde anlaşılır hata mesajı gösterildi.

Gün 18: Hata Yönetimi, Loglama, Validasyon ve Güvenli Kod
TaskService içinde guard clause kontrolleri yapılarak geçersiz girdiler ArgumentException ile kesildi ve başlık verileri trim işlemine tabi tutuldu. Kritik CRUD adımları ILogger ile parametreli olarak loglandı. Beklenen hatalar için 400/404 durum kodları tanımlandı; beklenmeyen hatalarda stack trace sızmasını önleyen global exception middleware eklendi ve frontend validasyonu API hatalarından ayrıldı.

Gün 19: Test, Refactoring, Deployment Hazırlığı ve Dokümantasyon
Projeye TaskBoard.Tests adında xUnit test projesi eklendi. In-Memory veritabanı kullanılarak boş başlık kontrolü, görev kaydı, var olmayan ID silinmesi, tüm kayıtların listelenmesi ve güncellenmesi senaryolarını kapsayan 5 adet birim testi yazıldı ve dotnet test ile 5/5 başarıyla doğrulandı. TaskService içindeki uzun fonksiyonlar private yardımcı metotlara bölünerek refactor edildi. Canlıya hazırlık adımı olarak hassas veri içermeyen appsettings.Example.json şablonu oluşturuldu ve dotnet publish komutu ile Release derlemesi alındı.

Gün 20: Final Özellikler, Demo, Kod Review ve Geri Bildirim
Stajın final gününde TaskBoard projesine sunucu taraflı arama, öncelik ve durum filtreleme ile sayfalama (pagination) altyapısı eklendi. TaskQuery ve PagedResult<T> sınıfları oluşturularak GET /api/tasks endpoint'i dinamik hale getirildi. Frontend tarafında arama kutusuna sunucuya yapılan gereksiz istekleri engellemek amacıyla 300 ms debounce gecikmesi eklendi. Sayfa boyutu (5, 10, 20) ve sıralama seçenekleri entegre edildi, boş arama sonuçları için özel durum mesajları sağlandı. Proje kod review kontrol listesinden geçirilerek xUnit birim testleri (5/5) ve dotnet publish çıktısı ile staj başarıyla tamamlandı.