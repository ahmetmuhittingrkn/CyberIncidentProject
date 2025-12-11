# 🚀 WPF Frontend Projesini Nasıl Çalıştırırım?

## ✅ Backend Hazır - Frontend'i Çalıştıralım!

Backend zaten `localhost:8080` üzerinde çalışıyor ✓  
Şimdi frontend'i çalıştırma zamanı!

---

## 🎯 YÖNTEMler (En Kolaydan Zora)

---

## 📝 YÖNTEM 1: PowerShell Script (EN KOLAY) ⭐

### Adım 1: Dosya Gezgini'nde klasörü açın
```
C:\Users\agurk\Desktop\CyberIncidentFrontend
```

### Adım 2: Üstteki adres çubuğuna yazın:
```
powershell
```

### Adım 3: Açılan pencerede yazın:
```powershell
.\build.ps1
```

### Adım 4: Sorulara cevap verin:
- Backend bağlantısı kontrolü → `y` (yes)
- Uygulamayı çalıştır → `y` (yes)

**✅ UYGULAMA AÇILACAK!**

---

## 💻 YÖNTEM 2: Visual Studio 2022 (Eğer Varsa)

### Adım 1: Visual Studio 2022'yi açın

### Adım 2: "Open a project or solution"

### Adım 3: Şu dosyayı seçin:
```
C:\Users\agurk\Desktop\CyberIncidentFrontend\CyberIncidentWPF.csproj
```

### Adım 4: F5 tuşuna basın veya yeşil ▶ butona tıklayın

**✅ UYGULAMA AÇILACAK!**

---

## 📟 YÖNTEM 3: Komut Satırı (CMD/PowerShell)

### Adım 1: Win + R tuşlarına basın

### Adım 2: Yazın: `cmd` ve Enter

### Adım 3: Klasöre gidin:
```cmd
cd C:\Users\agurk\Desktop\CyberIncidentFrontend
```

### Adım 4: NuGet paketlerini yükleyin:
```cmd
dotnet restore
```

### Adım 5: Projeyi derleyin:
```cmd
dotnet build
```

### Adım 6: Uygulamayı çalıştırın:
```cmd
dotnet run
```

**✅ UYGULAMA AÇILACAK!**

---

## 📂 YÖNTEM 4: Visual Studio Code

### Adım 1: VS Code'u açın

### Adım 2: File → Open Folder

### Adım 3: Klasörü seçin:
```
C:\Users\agurk\Desktop\CyberIncidentFrontend
```

### Adım 4: Terminal açın (Ctrl + `)

### Adım 5: Yazın:
```bash
dotnet run
```

**✅ UYGULAMA AÇILACAK!**

---

## 🎯 HIZLI BAŞLATMA (Tek Satır)

PowerShell açın ve şunu yazın:

```powershell
cd C:\Users\agurk\Desktop\CyberIncidentFrontend; dotnet run
```

---

## 🔍 İlk Çalıştırmada Neler Olur?

1. **.NET paketleri indirilir** (ilk seferde biraz sürer)
2. **Proje derlenir** (~10-20 saniye)
3. **WPF penceresi açılır** (Ana uygulama)
4. **Incident List** otomatik yüklenir

---

## 🖥️ Uygulama Açıldığında Ne Göreceksiniz?

```
┌────────────────────────────────────────────────┐
│ 🛡️ Cyber Security Incident Platform           │
├──────────────┬─────────────────────────────────┤
│ Sol Menü:    │  Ana Ekran:                     │
│              │                                  │
│ 📊 Incident  │  ┌─────────────────────────┐   │
│    List      │  │ Tüm incidentler         │   │
│              │  │ burada görünecek        │   │
│ ➕ Create    │  │                         │   │
│    Incident  │  │ DataGrid ile liste      │   │
│              │  └─────────────────────────┘   │
│ 📈 Analytics │                                  │
│    Dashboard │  Filtreler, arama, butonlar     │
│              │                                  │
└──────────────┴─────────────────────────────────┘
```

---

## ✅ Backend Bağlantı Kontrolü

### Backend çalışıyor mu kontrol edin:

Tarayıcıda açın:
```
http://localhost:8080/api/incidents
```

Veya PowerShell'de:
```powershell
curl http://localhost:8080/api/incidents
```

**✅ JSON verisi görünüyorsa backend hazır!**

---

## 🎮 İlk Test - Uygulama Açıldıktan Sonra

### 1. Incident List'i Görün
- Sol menüden "📊 Incident List" zaten açık olacak
- Backend'deki tüm incidentler listelenir

### 2. Yeni Incident Oluşturun
- Sol menüden "➕ Create Incident"
- Formu doldurun:
  - **Title**: "Test Phishing Email"
  - **Description**: "Şüpheli bir e-posta geldi"
  - **Type**: PHISHING
  - **Severity**: MEDIUM
  - **Date**: Bugünün tarihi
  - **Reporter ID**: 1
- "Create Incident" butonuna tıklayın
- ✅ Başarılı mesajı göreceksiniz!

### 3. Analytics'i Görün
- Sol menüden "📈 Analytics Dashboard"
- İstatistikleri ve grafikleri görün

---

## ❗ Sorun Giderme

### Hata: ".NET SDK bulunamadı"
**Çözüm**: .NET 6 SDK'yı yükleyin
```
https://dotnet.microsoft.com/download/dotnet/6.0
```

### Hata: "Backend'e bağlanılamıyor"
**Çözüm**: Backend'in çalıştığından emin olun
```powershell
# Backend'i test edin
curl http://localhost:8080/api/incidents
```

### Hata: "Port zaten kullanılıyor"
**Çözüm**: Başka bir WPF uygulaması çalışıyor olabilir. Kapatın ve tekrar deneyin.

### Pencere açılmıyor
**Çözüm**: 
```powershell
# Temiz derleme
dotnet clean
dotnet build
dotnet run
```

---

## 📸 Ekran Görüntüleri

### Ana Pencere:
```
┌─────────────────────────────────────────────────────┐
│ Cyber Incident Reporting & Analysis Platform       │
├────────────┬────────────────────────────────────────┤
│            │  Incident Management                   │
│  🛡️ Cyber  │  ┌──────────────────────────────────┐ │
│  Security  │  │ Filters: [Type▼] [Severity▼]    │ │
│            │  │         [Date] [Search...]        │ │
│ ────────   │  └──────────────────────────────────┘ │
│            │                                        │
│ 📊 List    │  ╔══════════════════════════════════╗ │
│ ➕ Create  │  ║ ID │ Title │ Type │ Severity   ║ │
│ 📈 Analytics│  ╠══════════════════════════════════╣ │
│            │  ║ 1  │ Phish │ PHISH│ HIGH       ║ │
│            │  ║ 2  │ Malware│MALW │ CRITICAL  ║ │
│ ────────   │  ╚══════════════════════════════════╝ │
│ ● Connected│                                        │
│ localhost  │  [View] [Update Status] [Delete]      │
│            │                                        │
└────────────┴────────────────────────────────────────┘
```

---

## 🎯 Hızlı Komut Kartı

| Yapılacak İş | Komut |
|--------------|-------|
| Projeyi çalıştır | `dotnet run` |
| Projeyi derle | `dotnet build` |
| Paketleri yükle | `dotnet restore` |
| Temizle | `dotnet clean` |
| Yayınla (Release) | `dotnet publish -c Release` |

---

## 🔄 Uygulamayı Yeniden Başlatma

Uygulamayı kapatıp tekrar çalıştırmak için:

1. **Pencereyi kapatın** (X butonuna tıklayın)
2. **Komutu tekrar çalıştırın**: `dotnet run`

---

## 💡 Öneriler

### Geliştirme için:
- **Visual Studio 2022** kullanın (en iyi WPF deneyimi)
- **Hot Reload** özelliği ile anlık değişiklikleri görün

### Sadece test için:
- **PowerShell script** (`build.ps1`) en hızlısı
- Veya direkt `dotnet run` komutu

### İlk kez çalıştırıyorsanız:
- **QUICKSTART.md** dosyasını okuyun
- **TEST_SCENARIOS.md** ile tüm özellikleri test edin

---

## 📞 Yardım

Sorun yaşarsanız:

1. Backend çalışıyor mu kontrol edin
2. .NET 6 SDK yüklü mü kontrol edin: `dotnet --version`
3. Port 8080 kullanılabilir mi kontrol edin

---

## ✅ Başarı Kriterleri

Uygulama başarıyla çalışıyorsa:

✅ WPF penceresi açıldı  
✅ Sol menü görünüyor  
✅ Incident List yüklendi  
✅ Backend'den veri geldi  
✅ Yeşil "Backend Connected" göstergesi var  

---

**🎉 Başarılar! Artık projeyi test edebilirsiniz!**

Sorularınız için: README.md ve TEST_SCENARIOS.md dosyalarına bakın.

