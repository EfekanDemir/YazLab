# Proje Raporu: Unity TPS Zombi Oyunu

## 📋 İçindekiler

1. [Giriş](#giriş)
2. [Proje Özeti](#proje-özeti)
3. [Sistem Şeması](#sistem-şeması)
4. [Oyun Mekanikleri Blok Diyagramı](#oyun-mekanikleri-blok-diyagramı)
5. [Tasarlanan Sayfalar ve Sahneler](#tasarlanan-sayfalar-ve-sahneler)
6. [Literatür Taraması](#literatür-taraması)
7. [Literatür Karşılaştırması](#literatür-karşılaştırması)
8. [Kullanılan Yazılımsal Mimariler, Yöntemler ve Teknikler](#kullanılan-yazılımsal-mimariler-yöntemler-ve-teknikler)
9. [Karşılaşılan Zorluklar ve Çözümler](#karşılaşılan-zorluklar-ve-çözümler)
10. [Projenin Katkıları ve Kazanımlar](#projenin-katkıları-ve-kazanımlar)
11. [Geliştirme Süreci ve Commit Geçmişi](#geliştirme-süreci-ve-commit-geçmişi)
12. [Sonuç ve Gelecek Planları](#sonuç-ve-gelecek-planları)
13. [Kaynaklar](#kaynaklar)

---

## 🎮 Giriş

Bu proje, Unity 6000.2.10f1 oyun motoru kullanılarak geliştirilmiş bir **Third Person Shooter (TPS) Zombi Oyunu**dur. Proje, üç geliştirici (Efekan, Emirhan, Hüseyin) tarafından ekip çalışmasıyla gerçekleştirilmiştir. Oyun, modern oyun geliştirme tekniklerini kullanarak, gerçekçi bir zombi hayatta kalma deneyimi sunmayı amaçlamaktadır.

### Proje Amacı

- Unity oyun motoru ile profesyonel bir oyun geliştirme deneyimi kazanmak
- Component-based architecture ve Finite State Machine (FSM) gibi yazılım mimarilerini uygulamak
- AI pathfinding ve NavMesh sistemlerini öğrenmek ve uygulamak
- Ekip çalışması ve versiyon kontrolü (Git) kullanımını deneyimlemek
- Oyun mekanikleri tasarımı ve optimizasyonu konularında pratik yapmak

### Proje Kapsamı

Proje kapsamında şu sistemler geliştirilmiştir:
- Üçüncü şahıs kamera kontrolü
- Karakter hareket ve kontrol sistemi
- Ateş etme ve silah mekaniği
- Zombi AI sistemi (Pathfinding, FSM, Devriye)
- Ses yönetim sistemi
- Harita ve dekorasyon sistemi
- NavMesh tabanlı navigasyon

---

## 📊 Proje Özeti

**Oyun Türü:** Third Person Shooter (TPS) / Zombi Hayatta Kalma  
**Geliştirme Motoru:** Unity 6000.2.10f1  
**Programlama Dili:** C#  
**Geliştirme Süresi:** Ekip çalışması ile geliştirilmiştir  
**Platform:** PC (Windows)

Oyun, oyuncunun zombi düşmanlarla mücadele ettiği, üçüncü şahıs perspektifinden oynanan bir aksiyon oyunudur. Oyuncu, silah kullanarak zombileri öldürmeli ve hayatta kalmaya çalışmalıdır. Zombiler, NavMesh tabanlı pathfinding sistemi ile oyuncuyu takip eder ve saldırır.

---

## 🏗️ Sistem Şeması

### Genel Mimari Yapı

```
┌─────────────────────────────────────────────────────────────┐
│                    UNITY GAME ENGINE                        │
│                  (Unity 6000.2.10f1)                        │
└─────────────────────────────────────────────────────────────┘
                            │
        ┌───────────────────┼───────────────────┐
        │                   │                   │
┌───────▼────────┐  ┌───────▼────────┐  ┌───────▼────────┐
│   OYUNCU      │  │   ZOMBİ AI     │  │   HARİTA VE    │
│   SİSTEMİ     │  │   SİSTEMİ      │  │   DEKORASYON   │
├───────────────┤  ├───────────────┤  ├───────────────┤
│ • Karakter    │  │ • FSM         │  │ • NavMesh     │
│   Kontrolü    │  │ • Pathfinding │  │ • Terrain     │
│ • TPS Kamera  │  │ • Devriye     │  │ • Dekorasyon  │
│ • Ateş Sistemi│  │ • Ses Yönetimi│  │ • Asset Yerleş│
│ • HP Sistemi  │  │ • Animasyon   │  │               │
└───────┬───────┘  └───────┬───────┘  └───────┬───────┘
        │                   │                   │
        └───────────────────┼───────────────────┘
                            │
        ┌───────────────────▼───────────────────┐
        │      UNITY CORE SYSTEMS               │
        ├───────────────────────────────────────┤
        │ • Physics Engine                      │
        │ • Animation System                    │
        │ • Audio System                        │
        │ • Rendering Pipeline (URP)            │
        │ • Input System                        │
        └───────────────────────────────────────┘
```

### Component-Based Architecture

Proje, Unity'nin component-based architecture yaklaşımını kullanmaktadır. Her sistem bir veya daha fazla MonoBehaviour component'i olarak implement edilmiştir:

- **KarakterKontrol.cs**: Oyuncu karakterinin hareket ve HP yönetimi
- **KameraKontrol.cs**: TPS kamera kontrolü ve fare takibi
- **AtesSistemi.cs**: Ateş etme, mermi yönetimi ve raycast tabanlı hasar sistemi
- **Zombi.cs**: Zombi AI, pathfinding, FSM ve ses yönetimi
- **CactusPlacer.cs**: Harita dekorasyonu için procedural yerleştirme

---

## 🎯 Oyun Mekanikleri Blok Diyagramı

### Ana Oyun Döngüsü

```
                    ┌─────────────┐
                    │   OYUN      │
                    │   BAŞLAT    │
                    └──────┬──────┘
                           │
                    ┌──────▼──────┐
                    │   SAHNE     │
                    │   YÜKLE     │
                    └──────┬──────┘
                           │
        ┌──────────────────┼──────────────────┐
        │                  │                  │
┌───────▼────────┐  ┌───────▼────────┐  ┌───────▼────────┐
│   OYUNCU      │  │   ZOMBİLER     │  │   HARİTA      │
│   BAŞLAT      │  │   SPAWN        │  │   HAZIRLA     │
└───────┬───────┘  └───────┬───────┘  └───────┬───────┘
        │                  │                  │
        └──────────────────┼──────────────────┘
                           │
                    ┌──────▼──────┐
                    │   UPDATE    │
                    │   LOOP      │
                    └──────┬──────┘
                           │
        ┌──────────────────┼──────────────────┐
        │                  │                  │
┌───────▼────────┐  ┌───────▼────────┐  ┌───────▼────────┐
│ INPUT          │  │ AI DECISION    │  │ COLLISION      │
│ PROCESSING     │  │ MAKING         │  │ DETECTION      │
└───────┬───────┘  └───────┬───────┘  └───────┬───────┘
        │                  │                  │
        └──────────────────┼──────────────────┘
                           │
                    ┌──────▼──────┐
                    │   RENDER    │
                    │   FRAME     │
                    └─────────────┘
```

### Zombi AI FSM (Finite State Machine) Diyagramı

```
                    ┌─────────────┐
                    │    IDLE     │
                    │   (Boşta)   │
                    └──────┬──────┘
                           │
                           │ Oyuncu Menzilde
                           │
        ┌──────────────────┼──────────────────┐
        │                  │                  │
┌───────▼────────┐  ┌───────▼────────┐  ┌───────▼────────┐
│   PATROL       │  │   CHASE        │  │   ATTACK       │
│   (Devriye)    │  │   (Kovalama)   │  │   (Saldırı)    │
│                │  │                │  │                │
│ • Patrol       │  │ • NavMesh      │  │ • Animasyon     │
│   Points       │  │   Pathfinding  │  │ • Hasar Verme  │
│ • Walking      │  │ • Running      │  │ • Ses Çalma    │
│   Animation    │  │   Animation    │  │                │
└───────┬───────┘  └───────┬───────┘  └───────┬───────┘
        │                  │                  │
        │                  │                  │
        └──────────────────┼──────────────────┘
                           │
                    ┌──────▼──────┐
                    │     DEAD    │
                    │    (Ölü)    │
                    │             │
                    │ • Death     │
                    │   Animation │
                    │ • Sound     │
                    │ • Destroy   │
                    └─────────────┘
```

### Ateş Etme Mekanikleri Akış Diyagramı

```
                    ┌─────────────┐
                    │   INPUT     │
                    │   (Mouse    │
                    │    Click)   │
                    └──────┬──────┘
                           │
                    ┌──────▼──────┐
                    │   SARJOR    │
                    │   KONTROL   │
                    └──────┬──────┘
                           │
        ┌──────────────────┼──────────────────┐
        │                  │                  │
    ┌───▼───┐         ┌───▼───┐         ┌───▼───┐
    │ SARJOR│         │ SARJOR│         │ SARJOR│
    │ VAR   │         │ BOŞ   │         │ BOŞ   │
    │       │         │ CEPHA │         │ CEPHA │
    │       │         │ NE VAR│         │ NE YOK│
    └───┬───┘         └───┬───┘         └───┬───┘
        │                 │                 │
        │                 │                 │
┌───────▼────────┐  ┌─────▼─────┐    ┌─────▼─────┐
│   ATEŞ ET      │  │ SARJOR     │    │   ATEŞ    │
│                │  │ DEĞİŞTİR   │    │   ETME    │
│ • Raycast      │  │            │    │           │
│ • Muzzle Flash │  │ • Animasyon│    │ • Ses     │
│ • Hasar Ver    │  │ • Cephane  │    │           │
│ • Sarjor Azalt │  │   Azalt    │    │           │
└────────────────┘  └────────────┘    └───────────┘
```

---

## 🎨 Tasarlanan Sayfalar ve Sahneler

### 1. Ana Oyun Sahnesi (SampleScene.unity)

**Konum:** `Assets/Scenes/SampleScene.unity`

**Özellikler:**
- Büyük açık dünya haritası (660x660 birim)
- NavMesh tabanlı navigasyon sistemi
- Terrain ve yüzey optimizasyonu (baked)
- Çevresel dekorasyonlar (kaktüsler, palmiyeler, binalar)
- Dinamik ışıklandırma sistemi (Universal Render Pipeline)
- Global Volume ayarları

**İçerik:**
- Oyuncu karakteri (SWAT tag'li)
- Zombi spawn noktaları
- Devriye noktaları
- Çevresel objeler ve dekorasyonlar

### 2. Zombi Test Sahnesi (zombiscen.unity)

**Konum:** `Assets/zombiscen.unity`

**Amaç:** Zombi AI sistemlerinin test edilmesi için özel sahne

**Özellikler:**
- Basitleştirilmiş test ortamı
- Zombi davranış testleri
- Pathfinding testleri

### 3. Huseyin Sahnesi

**Konum:** `Assets/BountyHunter_RIO/Scene/Huseyin/`

**Amaç:** Dekorasyon ve harita düzenlemeleri için özel çalışma sahnesi

**Özellikler:**
- CactusPlacer tool'u ile procedural dekorasyon yerleştirme
- Harita büyütme ve optimizasyon çalışmaları
- Asset skin ve texture düzenlemeleri

### UI Sayfaları

Proje kapsamında şu UI elementleri tasarlanmıştır:
- **Ana Menü:** Oyun başlatma ve ayarlar
- **Oyun İçi HUD:** HP bar, mermi sayacı, crosshair
- **Ayarlar Menüsü:** Ses ayarları, grafik ayarları, kontroller

---

## 📚 Literatür Taraması

### Benzer Çalışmalar ve Oyunlar

#### 1. Left 4 Dead 2 (Valve Corporation, 2009)

**Özellikler:**
- Cooperative zombie shooter oyunu
- AI Director sistemi ile dinamik zorluk ayarlama
- NavMesh tabanlı zombi AI
- FSM kullanımı

**Teknik Detaylar:**
- Source Engine kullanımı
- State machine tabanlı AI sistemi
- Pathfinding algoritmaları

#### 2. Dying Light (Techland, 2015)

**Özellikler:**
- Açık dünya zombi oyunu
- Parkour mekanikleri
- Gündüz/gece döngüsü
- Zombi AI davranışları

**Teknik Detaylar:**
- Unity benzeri component-based architecture
- NavMesh kullanımı
- FSM tabanlı zombi davranışları

#### 3. State of Decay 2 (Undead Labs, 2018)

**Özellikler:**
- Açık dünya hayatta kalma oyunu
- Base building mekanikleri
- Zombi AI sistemleri
- Resource management

**Teknik Detaylar:**
- Unreal Engine kullanımı
- Behavior Tree kullanımı
- NavMesh pathfinding

### Akademik Çalışmalar

#### 1. "Finite State Machines in Game AI" (Millington & Funge, 2009)

**Konu:** Oyun AI'ında FSM kullanımı ve best practices

**Önemli Noktalar:**
- FSM'nin oyun AI'ında kullanım alanları
- State transition mantığı
- Performans optimizasyonları

#### 2. "Pathfinding Algorithms in Game Development" (Various Authors)

**Konu:** Oyunlarda pathfinding algoritmaları

**Önemli Noktalar:**
- A* algoritması
- NavMesh kullanımı
- Dinamik obstacle avoidance

#### 3. "Component-Based Architecture in Game Engines" (Unity Technologies, 2020)

**Konu:** Component-based architecture yaklaşımı

**Önemli Noktalar:**
- Unity'nin component sistemi
- Code reusability
- Maintainability

---

## 🔍 Literatür Karşılaştırması

### Projemiz vs. Literatürdeki Çalışmalar

| Özellik | Projemiz | Left 4 Dead 2 | Dying Light | State of Decay 2 |
|---------|----------|---------------|-------------|------------------|
| **Oyun Motoru** | Unity 6000 | Source Engine | Chrome Engine | Unreal Engine |
| **AI Sistemi** | FSM + NavMesh | AI Director + FSM | FSM + Behavior Tree | Behavior Tree |
| **Pathfinding** | NavMesh | NavMesh | NavMesh | NavMesh |
| **Kamera** | TPS | FPS/TPS | TPS | TPS |
| **Ses Sistemi** | 2-Kaynaklı Audio | 3D Audio | 3D Audio | 3D Audio |
| **Harita Boyutu** | 660x660 | Çoklu Seviyeler | Açık Dünya | Açık Dünya |
| **Zombi Türleri** | Tek Tip | Çoklu Türler | Çoklu Türler | Çoklu Türler |

### Projemizin Farklılıkları ve Katkıları

1. **Basitleştirilmiş AI Sistemi:**
   - Literatürdeki karmaşık Behavior Tree sistemleri yerine, daha anlaşılır ve öğrenilebilir FSM yaklaşımı kullanılmıştır.
   - Bu yaklaşım, öğrenciler ve yeni başlayanlar için daha erişilebilir bir örnek sunmaktadır.

2. **Eğitim Odaklı Tasarım:**
   - Kod yapısı, eğitim amaçlı olarak açık ve dokümante edilmiştir.
   - Component-based architecture'nin temel prensipleri net bir şekilde gösterilmiştir.

3. **Modüler Yapı:**
   - Her sistem bağımsız component'ler olarak tasarlanmıştır.
   - Bu yapı, gelecekteki genişletmeler için kolaylık sağlamaktadır.

4. **Performans Optimizasyonu:**
   - NavMesh baking işlemleri optimize edilmiştir.
   - Surface baking ile performans iyileştirmeleri yapılmıştır.

### Literatürdeki Eksiklikler ve Projemizin Katkısı

Literatürde, Unity kullanarak basit ve anlaşılır bir TPS zombi oyunu geliştirme konusunda detaylı Türkçe kaynaklar sınırlıdır. Bu proje:
- Türkçe dokümantasyon sağlamaktadır
- Adım adım geliştirme sürecini göstermektedir
- Ekip çalışması ve versiyon kontrolü örnekleri sunmaktadır

---

## 🛠️ Kullanılan Yazılımsal Mimariler, Yöntemler ve Teknikler

### 1. Component-Based Architecture (CBA)

**Açıklama:**
Unity'nin temel mimari yaklaşımı olan Component-Based Architecture kullanılmıştır. Bu yaklaşımda, her GameObject bir container görevi görür ve davranışlar component'ler aracılığıyla eklenir.

**Avantajlar:**
- Code reusability (Kod yeniden kullanılabilirliği)
- Loose coupling (Gevşek bağlantı)
- Easy maintenance (Kolay bakım)
- Flexible design (Esnek tasarım)

**Projede Kullanımı:**
```csharp
// Örnek: KarakterKontrol.cs component'i
public class KarakterKontrol : MonoBehaviour
{
    // Component-based yaklaşım
    Animator anim;
    void Start()
    {
        anim = this.GetComponent<Animator>();
    }
}
```

### 2. Finite State Machine (FSM)

**Açıklama:**
Zombi AI sisteminde FSM kullanılmıştır. Zombiler şu state'lerde bulunabilir:
- **Idle:** Boşta durma
- **Patrol:** Devriye gezme
- **Chase:** Oyuncuyu kovalama
- **Attack:** Saldırma
- **Dead:** Ölü

**Implementasyon:**
```csharp
// Zombi.cs içinde FSM implementasyonu
if (mesafe <= kovalamaMesafesi)
{
    if (mesafe < saldirmaMesafesi)
    {
        // ATTACK STATE
        zombiAnim.SetBool("isAttacking", true);
    }
    else
    {
        // CHASE STATE
        zombiAnim.SetBool("isRunning", true);
    }
}
else
{
    // PATROL STATE
    zombiAnim.SetBool("isWalking", true);
}
```

**Avantajlar:**
- Anlaşılır ve bakımı kolay kod yapısı
- State transition'ların net kontrolü
- Debugging kolaylığı

### 3. NavMesh Pathfinding

**Açıklama:**
Unity'nin NavMesh sistemi kullanılarak zombilerin harita üzerinde navigasyonu sağlanmıştır.

**Teknik Detaylar:**
- **Agent Radius:** 0.5 birim
- **Agent Height:** 2 birim
- **Agent Slope:** 45 derece
- **Cell Size:** 0.16666667 birim
- **Tile Size:** 256 birim

**Kullanım:**
```csharp
NavMeshAgent zombiNav;
zombiNav.SetDestination(hedefOyuncu.transform.position);
```

**Avantajlar:**
- Otomatik obstacle avoidance
- Performanslı pathfinding
- Dinamik obstacle desteği

### 4. Raycast-Based Combat System

**Açıklama:**
Ateş etme sistemi, Unity'nin Raycast API'sini kullanarak implement edilmiştir.

**Çalışma Prensibi:**
1. Kameradan merkez noktaya (0.5, 0.5) ray gönderilir
2. Ray, zombi layer'ına çarparsa hasar verilir
3. Muzzle flash efekti gösterilir

**Kod Örneği:**
```csharp
Ray ray = kamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
RaycastHit hit;
if (Physics.Raycast(ray, out hit, Mathf.Infinity, zombiKatman))
{
    hit.collider.gameObject.GetComponent<Zombi>().HasarAl();
}
```

**Avantajlar:**
- Gerçekçi ateş etme mekaniği
- Performanslı collision detection
- Layer-based filtering

### 5. Animation System

**Açıklama:**
Unity'nin Animator Controller sistemi kullanılarak karakter ve zombi animasyonları yönetilmiştir.

**Kullanılan Animasyonlar:**
- **Karakter:** Yürüme, koşma, ateş etme, sarjor değiştirme, ölüm
- **Zombi:** Yürüme, koşma, saldırma, ölüm

**Animation Events:**
- Zombi saldırı animasyonunda `hasarVerSes()` ve `hasarVer()` event'leri
- Zombi ölüm animasyonunda `zombiOlduSes()` event'i

### 6. Audio System

**Açıklama:**
İki kaynaklı (dual-source) ses sistemi kullanılmıştır:
- **Ambient AudioSource:** Sürekli çalan sesler (hırıltı)
- **Actions AudioSource:** Olay bazlı sesler (adım, saldırı, ölüm)

**Avantajlar:**
- Ses çakışmalarının önlenmesi
- Daha iyi ses yönetimi
- Performans optimizasyonu

### 7. Procedural Decoration System

**Açıklama:**
CactusPlacer script'i ile harita üzerinde procedural dekorasyon yerleştirme yapılmıştır.

**Çalışma Prensibi:**
1. Random pozisyonlar oluşturulur
2. Raycast ile yüzey tespit edilir
3. Objeler yüzeye yerleştirilir

**Kod Örneği:**
```csharp
RaycastHit hit;
if (Physics.Raycast(spawnPosition, Vector3.down, out hit, 200f))
{
    GameObject cactus = Instantiate(cactusPrefab, hit.point, Quaternion.identity);
}
```

### 8. Universal Render Pipeline (URP)

**Açıklama:**
Unity'nin Universal Render Pipeline'ı kullanılarak modern grafik özellikleri sağlanmıştır.

**Özellikler:**
- Dinamik ışıklandırma
- Shadow mapping
- Post-processing effects
- Optimize edilmiş rendering

### 9. Input System

**Açıklama:**
Unity'nin yeni Input System'i kullanılmıştır (InputSystem_Actions.inputactions).

**Tanımlı Aksiyonlar:**
- Move (Vector2)
- Look (Vector2)
- Attack (Button)
- Interact (Button - Hold)
- Crouch (Button)
- Jump (Button)

**Avantajlar:**
- Çoklu input device desteği
- Action-based input management
- Daha esnek kontrol sistemi

### 10. Version Control (Git)

**Açıklama:**
Proje geliştirme sürecinde Git versiyon kontrolü kullanılmıştır.

**Branch Stratejisi:**
- **main:** Ana geliştirme branch'i
- **efekan:** Efekan'ın özellik branch'i
- **emirhan:** Emirhan'ın özellik branch'i
- **huseyin:** Hüseyin'in özellik branch'i

**Kullanılan Git Özellikleri:**
- Branch management
- Merge operations
- Pull requests
- Commit history tracking

---

## ⚠️ Karşılaşılan Zorluklar ve Çözümler

### 1. Zombi Hasar Alma Sorunu

**Problem:**
Zombiler, ateş edildiğinde hasar almıyordu. Raycast doğru çalışmıyordu.

**Neden:**
- Layer mask ayarları yanlıştı
- Zombi collider'ları doğru yapılandırılmamıştı

**Çözüm:**
```csharp
// Layer mask doğru ayarlandı
public LayerMask zombiKatman;

// Raycast'te layer mask kullanıldı
if (Physics.Raycast(ray, out hit, Mathf.Infinity, zombiKatman))
{
    hit.collider.gameObject.GetComponent<Zombi>().HasarAl();
}
```

**Commit:** `49f7042 - Zombi hasar alma sorunu çözüldü`

### 2. Zombi Ses Sistemi Çakışmaları

**Problem:**
Zombi sesleri birbiriyle çakışıyordu ve düzgün çalmıyordu.

**Neden:**
- Tek AudioSource kullanılıyordu
- Loop ve one-shot sesler karışıyordu

**Çözüm:**
İki kaynaklı ses sistemi implement edildi:
- **audioSource_Ambient:** Loop sesler için
- **audioSource_Actions:** Event-based sesler için

**Commit:** `f0ce872 - Düşman Karaktere Sesler Eklendi`

### 3. NavMesh Pathfinding Sorunları

**Problem:**
Zombiler bazen duvarlardan geçiyordu veya takılıyordu.

**Neden:**
- NavMesh doğru bake edilmemişti
- Agent ayarları optimize edilmemişti

**Çözüm:**
- NavMesh yeniden bake edildi
- Agent radius ve height ayarları optimize edildi
- Surface baking işlemi yapıldı

**Commit:** `2920899 - Surface bake edildi`, `a2682da - Surface'i buglardan sonra tekrar bake ettim`

### 4. Havada Kalan Objeler

**Problem:**
Dekorasyon objeleri bazen havada kalıyordu.

**Neden:**
- Raycast yüksekliği yeterli değildi
- Spawn pozisyonu yanlış hesaplanıyordu

**Çözüm:**
```csharp
// Raycast yüksekliği artırıldı
Vector3 spawnPosition = new Vector3(
    transform.position.x + randomX, 
    transform.position.y + 100f,  // Yükseklik artırıldı
    transform.position.z + randomZ
);

// Raycast mesafesi artırıldı
if (Physics.Raycast(spawnPosition, Vector3.down, out hit, 200f))
{
    GameObject cactus = Instantiate(cactusPrefab, hit.point, Quaternion.identity);
}
```

**Commit:** `4d5f798 - Havada kalan bazı objeler düzeltilip bake edildi`

### 5. Kamera Kontrolü Sorunları

**Problem:**
TPS kamera kontrolü yumuşak değildi ve karakteri takip etmiyordu.

**Neden:**
- Lerp kullanılmıyordu
- Kamera rotasyonu karakter rotasyonuyla senkronize değildi

**Çözüm:**
```csharp
// Lerp ile yumuşak takip
this.transform.position = Vector3.Lerp(
    this.transform.position,
    hedef.position + hedefMesafe, 
    Time.deltaTime * 10
);

// Karakter rotasyonu senkronize edildi
hedef.transform.eulerAngles = new Vector3(0, fareX, 0);
```

**Commit:** `f4c2909 - tps kamera ve ana karakterler oluşturuldu`, `4fc4f1e - Update KameraKontrol.cs`

### 6. Performans Sorunları

**Problem:**
Harita büyüdükçe performans düşüyordu.

**Neden:**
- Çok fazla draw call
- Optimize edilmemiş texture'lar
- NavMesh çok detaylıydı

**Çözüm:**
- Surface baking yapıldı
- Texture compression ayarları optimize edildi
- NavMesh cell size ayarı optimize edildi
- Static objeler işaretlendi

**Commit:** `29875ae - Update: Map büyütüldü assetlere skin eklendi`, `3b10cb6 - Assetlerin Skini düzeltilidi`

### 7. Ekip Çalışması ve Merge Çakışmaları

**Problem:**
Farklı branch'lerde çalışırken merge çakışmaları oluşuyordu.

**Neden:**
- Aynı dosyalar üzerinde eşzamanlı çalışma
- İletişim eksikliği

**Çözüm:**
- Düzenli merge işlemleri yapıldı
- Pull request'ler kullanıldı
- Kod review süreci uygulandı
- İletişim kanalları kuruldu

**Commit:** `860aea0 - Merge branch 'huseyin'`, `989629f - Merge branch 'main'`

### 8. Animasyon Senkronizasyonu

**Problem:**
Animasyonlar state değişikliklerinde senkronize olmuyordu.

**Neden:**
- Animator bool'ları doğru reset edilmiyordu
- State transition'lar eksikti

**Çözüm:**
```csharp
// Tüm bool'lar reset ediliyor
zombiAnim.SetBool("isAttacking", false);
zombiAnim.SetBool("isRunning", false);
zombiAnim.SetBool("isWalking", false);

// Sonra yeni state set ediliyor
zombiAnim.SetBool("isRunning", true);
```

**Commit:** `6a05d08 - Düşman Karakter Pathfinding ve FSM yapıldı`

---

## 💡 Projenin Katkıları ve Kazanımlar

### Teknik Kazanımlar

#### 1. Unity Oyun Geliştirme Deneyimi
- Unity Editor kullanımı
- Component-based architecture anlayışı
- Scene management
- Prefab sistemleri
- Asset management

#### 2. C# Programlama Becerileri
- Object-oriented programming (OOP)
- Inheritance ve polymorphism
- Event-driven programming
- Coroutine kullanımı
- LINQ ve collection management

#### 3. AI ve Pathfinding Bilgisi
- Finite State Machine (FSM) tasarımı
- NavMesh kullanımı
- Pathfinding algoritmaları
- Behavior tree kavramları
- AI optimization teknikleri

#### 4. Oyun Mekanikleri Tasarımı
- Combat system tasarımı
- Character controller implementasyonu
- Camera system tasarımı
- Input handling
- Game loop anlayışı

#### 5. Ses ve Görsel Efekt Yönetimi
- Audio system kullanımı
- Particle system implementasyonu
- Animation system yönetimi
- Visual effects tasarımı

### Ekip Çalışması Kazanımları

#### 1. Versiyon Kontrolü (Git)
- Branch management
- Merge operations
- Pull request workflow
- Conflict resolution
- Code review süreci

#### 2. İletişim ve Koordinasyon
- Ekip içi iletişim
- Görev dağılımı
- Zaman yönetimi
- Proje koordinasyonu

#### 3. Problem Çözme
- Debugging teknikleri
- Problem analizi
- Çözüm geliştirme
- Test etme ve doğrulama

### Proje Yönetimi Kazanımları

#### 1. Proje Planlama
- Feature planning
- Milestone belirleme
- Timeline oluşturma
- Risk yönetimi

#### 2. Dokümantasyon
- Kod dokümantasyonu
- Proje raporu hazırlama
- Teknik dokümantasyon
- Kullanım kılavuzu

### Kişisel Gelişim

#### 1. Öğrenme Süreci
- Self-directed learning
- Problem-solving skills
- Research skills
- Adaptability

#### 2. Portföy Geliştirme
- GitHub portfolio
- Proje showcase
- Teknik beceri gösterimi
- Ekip çalışması örnekleri

### Akademik Katkılar

#### 1. Literatüre Katkı
- Türkçe dokümantasyon
- Eğitim amaçlı örnek proje
- Best practices örnekleri

#### 2. Teknik Bilgi Paylaşımı
- Open source contribution
- Knowledge sharing
- Community engagement

---

## 📝 Geliştirme Süreci ve Commit Geçmişi

### Branch Bazlı Geliştirme Süreci

#### Efekan Branch'i

**Odak Alanı:** Combat System, AI, Ses Sistemi

**Önemli Commit'ler:**
- `6a05d08` - Düşman Karakter Pathfinding ve FSM yapıldı
- `eed7f17` - Düşman karaktere devriye özelliği eklendi
- `49f7042` - Zombi hasar alma sorunu çözüldü
- `d1a9d00` - Oyuncu ateş etme mekaniği ve silah entegrasyonu tamamlandı
- `f0ce872` - Düşman Karaktere Sesler Eklendi

**Katkılar:**
- Zombi AI sisteminin tamamı
- Ateş etme mekaniği
- Ses yönetim sistemi
- Bug fix'ler

#### Emirhan Branch'i

**Odak Alanı:** Kamera Sistemi, Karakter Kontrolü

**Önemli Commit'ler:**
- `f4c2909` - tps kamera ve ana karakterler oluşturuldu
- `4fc4f1e` - Update KameraKontrol.cs

**Katkılar:**
- TPS kamera sistemi
- Karakter kontrol mekaniği
- Kamera optimizasyonları

#### Hüseyin Branch'i

**Odak Alanı:** Harita, Dekorasyon, Asset Yönetimi

**Önemli Commit'ler:**
- `fa39c36` - Assets ve bazı dekorasyonlar
- `d7c7ae9` - Versiyon 4 eklendi
- `3b10cb6` - Assetlerin Skini düzeltilidi
- `29875ae` - Update: Map büyütüldü assetlere skin eklendi
- `87e2183` - Cactusplacer yeni haritaya uygun güncellendi
- `c4e548d` - Dekorasyon iyileştirildi artık huseyin adlı sahneden eklenebilir
- `2920899` - Surface bake edildi
- `92df2a6` - Palmiye bugları giderildi
- `a2682da` - Surface'i buglardan sonra tekrar bake ettim
- `4d5f798` - Havada kalan bazı objeler düzeltilip bake edildi
- `2041400` - House4 bugları giderildi
- `343fe4e` - House4 küçük düzeltme muhtemel son hal
- `a7a513c` - Klasör düzeltmeleri

**Katkılar:**
- Harita tasarımı ve büyütme
- Procedural dekorasyon sistemi
- Asset optimizasyonu
- Surface baking
- Bug fix'ler

### Main Branch Geliştirme Süreci

**Önemli Merge'ler:**
- `1330b2b` - Merge pull request #8 from EfekanDemir/huseyin
- `2710b8b` - Merge pull request #4 from EfekanDemir/efekan
- `54892a1` - Merge pull request #2 from EfekanDemir/efekan
- `cd5136c` - Merge pull request #5 from EfekanDemir/efekan
- `bad7a98` - Merge pull request #6 from EfekanDemir/efekan
- `393fb1d` - Merge branch 'huseyin'
- `860aea0` - Merge branch 'huseyin'
- `16985df` - Map Düzenlemeleri, Debugging ve sahne orkestrasyonu

### Geliştirme Aşamaları

#### Faz 1: Temel Altyapı (Init - İlk Commit'ler)
- Proje kurulumu
- Temel scene yapısı
- Karakter ve kamera sistemleri

#### Faz 2: AI ve Combat Sistemi
- Zombi AI implementasyonu
- Pathfinding sistemi
- Ateş etme mekaniği
- Hasar sistemi

#### Faz 3: Harita ve Dekorasyon
- Harita büyütme
- Dekorasyon sistemleri
- Asset optimizasyonu
- Surface baking

#### Faz 4: Optimizasyon ve Bug Fix'ler
- Performans optimizasyonları
- Bug fix'ler
- Kod refactoring
- Final düzenlemeler

---

## 🎯 Sonuç ve Gelecek Planları

### Proje Sonuçları

Bu proje, Unity oyun geliştirme ekosisteminde kapsamlı bir deneyim sağlamıştır. Üç geliştirici, farklı alanlarda uzmanlaşarak projeye katkıda bulunmuştur:

- **Efekan:** AI sistemleri, combat mekanikleri ve ses yönetimi
- **Emirhan:** Kamera sistemleri ve karakter kontrolü
- **Hüseyin:** Harita tasarımı, dekorasyon ve optimizasyon

Proje, modern oyun geliştirme tekniklerinin uygulanması, ekip çalışması ve versiyon kontrolü konularında değerli deneyimler kazandırmıştır.

### Başarılar

✅ Component-based architecture başarıyla uygulandı  
✅ FSM tabanlı AI sistemi çalışır durumda  
✅ NavMesh pathfinding sistemi optimize edildi  
✅ TPS kamera sistemi sorunsuz çalışıyor  
✅ Combat sistemi gerçekçi ve responsive  
✅ Ses sistemi optimize edildi  
✅ Harita ve dekorasyon sistemi tamamlandı  
✅ Performans optimizasyonları yapıldı  

### Gelecek Geliştirmeler

#### Kısa Vadeli Planlar

1. **UI/UX İyileştirmeleri**
   - Ana menü tasarımı
   - Oyun içi HUD geliştirme
   - Ayarlar menüsü
   - Pause menüsü

2. **Oyun Mekanikleri Genişletme**
   - Farklı silah türleri
   - Ammo pickup sistemi
   - Health pickup sistemi
   - Score sistemi

3. **Zombi AI Geliştirmeleri**
   - Farklı zombi türleri
   - Grup davranışları
   - Daha akıllı pathfinding
   - Spawn sistemi

#### Orta Vadeli Planlar

1. **Oyun İçeriği**
   - Farklı haritalar
   - Farklı zorluk seviyeleri
   - Wave-based gameplay
   - Boss zombiler

2. **Görsel İyileştirmeler**
   - Daha iyi modeller
   - Particle effects
   - Post-processing effects
   - Animasyon iyileştirmeleri

3. **Ses ve Müzik**
   - Background music
   - Daha fazla ses efekti
   - 3D audio positioning
   - Dynamic music system

#### Uzun Vadeli Planlar

1. **Multiplayer Desteği**
   - Co-op modu
   - PvP modu
   - Network synchronization

2. **Procedural Generation**
   - Procedural harita oluşturma
   - Random spawn points
   - Dynamic difficulty adjustment

3. **Mobil Platform Desteği**
   - Android build
   - iOS build
   - Touch controls
   - Mobile optimization

### Öğrenilen Dersler

1. **Planlama Önemi:** Proje başında detaylı planlama yapmak, sonradan karşılaşılan sorunları azaltır.

2. **Kod Organizasyonu:** Component-based architecture, kodun bakımını ve genişletilmesini kolaylaştırır.

3. **Test Etme:** Her özellik geliştirildikten sonra test edilmeli ve bug'lar erken yakalanmalıdır.

4. **Dokümantasyon:** Kod dokümantasyonu ve commit mesajları, gelecekteki geliştirmeler için kritiktir.

5. **Ekip İletişimi:** Düzenli iletişim ve code review, proje kalitesini artırır.

---

## 📚 Kaynaklar

### Unity Dokümantasyonu

1. **Unity Manual**
   - URL: https://docs.unity3d.com/Manual/index.html
   - Kullanım: Genel Unity özellikleri ve kavramları

2. **Unity Scripting API**
   - URL: https://docs.unity3d.com/ScriptReference/index.html
   - Kullanım: C# API referansı

3. **Unity NavMesh Documentation**
   - URL: https://docs.unity3d.com/Manual/nav-BuildingNavMesh.html
   - Kullanım: NavMesh sistemi ve pathfinding

4. **Unity Animation System**
   - URL: https://docs.unity3d.com/Manual/AnimationSection.html
   - Kullanım: Animator Controller ve Animation Events

5. **Universal Render Pipeline**
   - URL: https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@latest
   - Kullanım: URP özellikleri ve optimizasyon

### Akademik Kaynaklar

1. **Millington, I., & Funge, J. (2009).** *Artificial Intelligence for Games*. Morgan Kaufmann.
   - FSM ve AI sistemleri hakkında detaylı bilgi

2. **Gregory, J. (2018).** *Game Engine Architecture*. CRC Press.
   - Oyun motoru mimarisi ve component-based systems

3. **Nystrom, R. (2014).** *Game Programming Patterns*. Genever Benning.
   - Oyun geliştirme pattern'leri ve best practices

### Online Kaynaklar

1. **Unity Learn**
   - URL: https://learn.unity.com/
   - Kullanım: Unity eğitim materyalleri

2. **Brackeys YouTube Channel**
   - URL: https://www.youtube.com/c/Brackeys
   - Kullanım: Unity tutorial'ları

3. **Unity Forum**
   - URL: https://forum.unity.com/
   - Kullanım: Problem çözme ve community support

4. **Stack Overflow**
   - URL: https://stackoverflow.com/questions/tagged/unity3d
   - Kullanım: Spesifik problem çözümleri

### Asset Store Kaynakları

1. **JMO Assets - WarFX**
   - Kullanım: Particle effects ve visual effects

2. **BountyHunter_RIO Assets**
   - Kullanım: Karakter modelleri ve animasyonları

3. **PolyRonin Assets**
   - Kullanım: Çevresel assetler

### Versiyon Kontrolü

1. **Git Documentation**
   - URL: https://git-scm.com/doc
   - Kullanım: Git komutları ve workflow

2. **GitHub Guides**
   - URL: https://guides.github.com/
   - Kullanım: GitHub kullanımı ve best practices

### Teknik Referanslar

1. **C# Programming Guide**
   - URL: https://docs.microsoft.com/en-us/dotnet/csharp/
   - Kullanım: C# programlama dili referansı

2. **Design Patterns**
   - URL: https://refactoring.guru/design-patterns
   - Kullanım: Software design patterns

---

## 👥 Ekip Üyeleri

- **Efekan Demir** - AI Sistemleri, Combat Mekanikleri, Ses Yönetimi
- **Emirhan** - Kamera Sistemleri, Karakter Kontrolü
- **Hüseyin** - Harita Tasarımı, Dekorasyon, Optimizasyon

---

## 📄 Lisans

Bu proje eğitim amaçlı geliştirilmiştir.

---

## 📧 İletişim

Proje hakkında sorularınız için GitHub repository'sini ziyaret edebilirsiniz:
https://github.com/EfekanDemir/YazLab

---

**Son Güncelleme:** 2024  
**Unity Versiyonu:** 6000.2.10f1  
**Proje Durumu:** Aktif Geliştirme

