# Proje Raporu: Unity TPS Zombi Oyunu

## 📋 İçindekiler

1. [Giriş](#giriş)
2. [Sistem Şeması](#sistem-şeması)
3. [Oyun Mekanikleri Blok Diyagramı](#oyun-mekanikleri-blok-diyagramı)
4. [Tasarlanan Sayfalar](#tasarlanan-sayfalar)
5. [Literatür Taraması ve Karşılaştırma](#literatür-taraması-ve-karşılaştırma)
6. [Kullanılan Yazılımsal Mimariler, Yöntemler ve Teknikler](#kullanılan-yazılımsal-mimariler-yöntemler-ve-teknikler)
7. [Karşılaşılan Zorluklar ve Çözümler](#karşılaşılan-zorluklar-ve-çözümler)
8. [Projenin Katkıları](#projenin-katkıları)

---

## 🎮 Giriş

Bu proje, Unity 6000.2.10f1 oyun motoru kullanılarak geliştirilmiş bir **Third Person Shooter (TPS) Zombi Oyunu**dur. Proje, üç geliştirici (Efekan, Emirhan, Hüseyin) tarafından ekip çalışmasıyla gerçekleştirilmiştir.

**Oyun Türü:** Third Person Shooter (TPS) / Zombi Hayatta Kalma  
**Geliştirme Motoru:** Unity 6000.2.10f1  
**Programlama Dili:** C#  
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

Proje, Unity'nin component-based architecture yaklaşımını kullanmaktadır:

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
        └──────────────────┼──────────────────┘
                           │
                    ┌──────▼──────┐
                    │     DEAD    │
                    │    (Ölü)    │
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
┌───────▼────────┐  ┌─────▼─────┐    ┌─────▼─────┐
│   ATEŞ ET      │  │ SARJOR     │    │   ATEŞ    │
│                │  │ DEĞİŞTİR   │    │   ETME    │
│ • Raycast      │  │            │    │           │
│ • Hasar Ver    │  │ • Animasyon│    │ • Ses     │
│ • Sarjor Azalt │  │ • Cephane  │    │           │
└────────────────┘  └────────────┘    └───────────┘
```

---

## 🎨 Tasarlanan Sayfalar

### 1. Ana Sahne (MainScene.unity)

**Konum:** `Assets/BountyHunter_RIO/Scene/SON/MainScene.unity`

**Özellikler:**
- Bütün sistemlerin birleştiği ana sahne
- Tüm oyun mekaniklerinin entegre edildiği sahne
- Oyunun tam versiyonunu içeren sahne

**Amaç:** Tüm oyun sistemlerinin (oyuncu, zombiler, harita, UI vb.) bir arada çalıştığı ana oyun sahnesi

### 2. SWAT Karakter Sahnesi (BountyHunterSample.unity)

**Konum:** `Assets/BountyHunter_RIO/Scene/SON/BountyHunterSample.unity`

**Özellikler:**
- Ana oyun karakteri olan SWAT'ın bulunduğu sahne
- Karakter kontrolü ve hareket mekaniklerinin test edildiği sahne
- TPS kamera sisteminin test edildiği sahne

**Amaç:** SWAT karakterinin ve karakter kontrol sistemlerinin test edilmesi ve geliştirilmesi için özel sahne

### 3. Harita Sahnesi (MapScene.unity)

**Konum:** `Assets/BountyHunter_RIO/Scene/SON/MapScene.unity`

**Özellikler:**
- Oyun haritasının görüntülendiği sahne
- Harita tasarımı ve düzenlemelerinin yapıldığı sahne
- Çevresel dekorasyonların yerleştirildiği sahne

**Amaç:** Oyun haritasının tasarımı, düzenlenmesi ve görselleştirilmesi için özel sahne

### 4. Zombi Test Sahnesi (Zombi.unity)

**Konum:** `Assets/BountyHunter_RIO/Scene/SON/Zombi.unity`

**Özellikler:**
- Sadece zombilerin bulunduğu sahne
- Devriye noktalarının yerleştirildiği sahne
- Zombi AI sistemlerinin test edildiği sahne
- Pathfinding ve FSM davranışlarının test edildiği sahne

**Amaç:** Zombi AI sistemlerinin, pathfinding mekaniklerinin ve devriye sisteminin test edilmesi ve debug edilmesi için özel test ortamı

### 5. Menü Sahnesi (GameOverSahnesi.unity)

**Konum:** `Assets/BountyHunter_RIO/Scene/SON/GameOverSahnesi.unity`

**Özellikler:**
- Oyun menüsünün bulunduğu sahne
- Menü navigasyonu ve UI elementleri
- Oyun başlatma ve ayarlar erişimi

**Amaç:** Oyuncunun oyuna başlamadan önce karşılaştığı menü ekranı ve oyun ayarlarının yönetildiği sahne

---

## 📚 Literatür Taraması ve Karşılaştırma

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
- Zombi AI davranışları

**Teknik Detaylar:**
- Unity benzeri component-based architecture
- NavMesh kullanımı
- FSM tabanlı zombi davranışları

#### 3. State of Decay 2 (Undead Labs, 2018)

**Özellikler:**
- Açık dünya hayatta kalma oyunu
- Zombi AI sistemleri
- Resource management

**Teknik Detaylar:**
- Unreal Engine kullanımı
- Behavior Tree kullanımı
- NavMesh pathfinding

### Literatür Karşılaştırması

| Özellik | Projemiz | Left 4 Dead 2 | Dying Light | State of Decay 2 |
|---------|----------|---------------|-------------|------------------|
| **Oyun Motoru** | Unity 6000 | Source Engine | Chrome Engine | Unreal Engine |
| **AI Sistemi** | FSM + NavMesh | AI Director + FSM | FSM + Behavior Tree | Behavior Tree |
| **Pathfinding** | NavMesh | NavMesh | NavMesh | NavMesh |
| **Kamera** | TPS | FPS/TPS | TPS | TPS |
| **Ses Sistemi** | 2-Kaynaklı Audio | 3D Audio | 3D Audio | 3D Audio |
| **Harita Boyutu** | 660x660 | Çoklu Seviyeler | Açık Dünya | Açık Dünya |

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

4. **Türkçe Dokümantasyon:**
   - Literatürde, Unity kullanarak basit ve anlaşılır bir TPS zombi oyunu geliştirme konusunda detaylı Türkçe kaynaklar sınırlıdır.
   - Bu proje Türkçe dokümantasyon sağlamaktadır.

---

## 🛠️ Kullanılan Yazılımsal Mimariler, Yöntemler ve Teknikler

### 1. Component-Based Architecture (CBA)

**Açıklama:**
Unity'nin temel mimari yaklaşımı olan Component-Based Architecture kullanılmıştır. Her GameObject bir container görevi görür ve davranışlar component'ler aracılığıyla eklenir.

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

### 5. Animation System

**Açıklama:**
Unity'nin Animator Controller sistemi kullanılarak karakter ve zombi animasyonları yönetilmiştir.

**Kullanılan Animasyonlar:**
- **Karakter:** Yürüme, koşma, ateş etme, sarjor değiştirme, zıplama, çömelme, nişan alma
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

## 💡 Projenin Katkıları

### Teknik Kazanımlar

1. **Unity Oyun Geliştirme Deneyimi**
   - Unity Editor kullanımı
   - Component-based architecture anlayışı
   - Scene management
   - Prefab sistemleri
   - Asset management

2. **C# Programlama Becerileri**
   - Object-oriented programming (OOP)
   - Inheritance ve polymorphism
   - Event-driven programming
   - Coroutine kullanımı

3. **AI ve Pathfinding Bilgisi**
   - Finite State Machine (FSM) tasarımı
   - NavMesh kullanımı
   - Pathfinding algoritmaları
   - AI optimization teknikleri

4. **Oyun Mekanikleri Tasarımı**
   - Combat system tasarımı
   - Character controller implementasyonu
   - Camera system tasarımı
   - Input handling
   - Game loop anlayışı

5. **Ses ve Görsel Efekt Yönetimi**
   - Audio system kullanımı
   - Particle system implementasyonu
   - Animation system yönetimi

### Ekip Çalışması Kazanımları

1. **Versiyon Kontrolü (Git)**
   - Branch management
   - Merge operations
   - Pull request workflow
   - Conflict resolution
   - Code review süreci

2. **İletişim ve Koordinasyon**
   - Ekip içi iletişim
   - Görev dağılımı
   - Zaman yönetimi
   - Proje koordinasyonu

3. **Problem Çözme**
   - Debugging teknikleri
   - Problem analizi
   - Çözüm geliştirme
   - Test etme ve doğrulama

### Proje Yönetimi Kazanımları

1. **Proje Planlama**
   - Feature planning
   - Milestone belirleme
   - Timeline oluşturma
   - Risk yönetimi

2. **Dokümantasyon**
   - Kod dokümantasyonu
   - Proje raporu hazırlama
   - Teknik dokümantasyon

### Kişisel Gelişim

1. **Öğrenme Süreci**
   - Self-directed learning
   - Problem-solving skills
   - Research skills
   - Adaptability

2. **Portföy Geliştirme**
   - GitHub portfolio
   - Proje showcase
   - Teknik beceri gösterimi
   - Ekip çalışması örnekleri

### Akademik Katkılar

1. **Literatüre Katkı**
   - Türkçe dokümantasyon
   - Eğitim amaçlı örnek proje
   - Best practices örnekleri

2. **Teknik Bilgi Paylaşımı**
   - Open source contribution
   - Knowledge sharing
   - Community engagement

---

## 👥 Ekip Üyeleri

- **Efekan Demir** - AI Sistemleri, Combat Mekanikleri, Ses Yönetimi
- **Emirhan** - Kamera Sistemleri, Karakter Kontrolü
- **Hüseyin** - Harita Tasarımı, Dekorasyon, Optimizasyon

---

**Son Güncelleme:** 2024  
**Unity Versiyonu:** 6000.2.10f1  
**Proje Durumu:** Tamamlandı
