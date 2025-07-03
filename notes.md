# Learning Notes

## Http Status Codes List

### **1xx - Bilgilendirme (Informational)**

```c#
100 Continue                 - İstek devam ediyor
101 Switching Protocols      - Protokol değiştiriliyor
```

### **2xx - Başarılı (Success)**

```c#
200 OK                       - İstek başarılı
201 Created                  - Yeni kaynak oluşturuldu
202 Accepted                 - İstek kabul edildi, işlem devam ediyor
204 No Content               - Başarılı ama döndürülecek veri yok
```

### **3xx - Yönlendirme (Redirection)**

```c#
301 Moved Permanently        - Kaynak kalıcı olarak taşındı
302 Found                    - Kaynak geçici olarak taşındı
304 Not Modified             - Kaynak değişmedi (cache kullan)
```

### **4xx - Client Hataları (Client Error)**

```c#
400 Bad Request              - Geçersiz istek
401 Unauthorized             - Kimlik doğrulama gerekli
403 Forbidden                - Erişim yasak
404 Not Found                - Kaynak bulunamadı
405 Method Not Allowed       - HTTP method'u desteklenmiyor
409 Conflict                 - Çakışma var (duplicate veri)
422 Unprocessable Entity     - Veri formatı doğru ama işlenemiyor
```

### **5xx - Server Hataları (Server Error)**

```c#
500 Internal Server Error    - Server'da beklenmeyen hata
501 Not Implemented          - Özellik henüz implement edilmedi
502 Bad Gateway              - Gateway hatası
503 Service Unavailable      - Servis geçici olarak kullanılamıyor
```

### **API'lerde En Çok Kullanılanlar (C# ASP.NET Core)**

```csharp
// 2xx - Başarılı
return Ok(data);                    // 200 OK
return Created("location", data);   // 201 Created
return NoContent();                 // 204 No Content

// 4xx - Client Hataları  
return BadRequest("mesaj");         // 400 Bad Request
return Unauthorized();              // 401 Unauthorized
return NotFound("mesaj");           // 404 Not Found
return Conflict("mesaj");           // 409 Conflict

// 5xx - Server Hataları
return StatusCode(500, "mesaj");    // 500 Internal Server Error
```

## ActionResult

- ASP.NET Core'da HTTP response'ları kontrol etmek için kullanılan bir wrapper'dır.

```c#
// SADECE DATA DÖNDÜRÜR
public Point GetPoint() 
{
    return new Point(); // ← Sadece Point döner, HTTP status yok
}

// DATA + HTTP STATUS DÖNDÜRÜR  
public ActionResult<Point> GetPoint()
{
    return Ok(new Point());     // ← 200 OK + Point
    return NotFound();          // ← 404 Not Found
    return BadRequest();        // ← 400 Bad Request
}
```

- Yaygın ActionResul Types

```c#
return Ok(data);              // 200 OK
return NotFound();            // 404 Not Found  
return BadRequest();          // 400 Bad Request
return Unauthorized();        // 401 Unauthorized
return Created("location", data); // 201 Created
return NoContent();   
```

## Dependency Injection (DI) Notes

## **DI Nedir?**

Class'ların ihtiyaç duyduğu dependencies'i (bağımlılıkları) constructor üzerinden dışarıdan almasıdır.

## **Projemizde DI Kullanımı**

### **1. Constructor Injection - Controller**

```csharp
public class NewApiController : ControllerBase
{
    // 🔸 5 farklı service interface'i inject ediyoruz
    private readonly IPointGetAllService _getAllService;
    private readonly IPointAddService _addService;
    private readonly IPointGetByIdService _getByIdService;
    private readonly IPointUpdateService _updateService;
    private readonly IPointDeleteService _deleteService;

    // 🔸 Constructor üzerinden dependencies alıyoruz
    public NewApiController(
        IPointGetAllService getAllService,
        IPointAddService addService,
        IPointGetByIdService getByIdService,
        IPointUpdateService updateService,
        IPointDeleteService deleteService)
    {
        _getAllService = getAllService;
        _addService = addService;
        // ... diğer atamalar
    }
}
```

### **2. DI Container Konfigürasyonu - Program.cs**

```csharp
// 🔸 Repository'yi Singleton olarak kaydet (tek instance)
builder.Services.AddSingleton<IPointRepository, PointRepository>();

// 🔸 Service'leri Scoped olarak kaydet (request başına yeni instance)
builder.Services.AddScoped<IPointGetAllService, PointGetAllService>();
builder.Services.AddScoped<IPointAddService, PointAddService>();
builder.Services.AddScoped<IPointGetByIdService, PointGetByIdService>();
builder.Services.AddScoped<IPointUpdateService, PointUpdateService>();
builder.Services.AddScoped<IPointDeleteService, PointDeleteService>();
```

### **3. Service Layer'da DI**

```csharp
// 🔸 Her service kendi constructor'ında IPointRepository inject eder
public class PointAddService : IPointAddService
{
    private readonly IPointRepository _pointRepository;

    public PointAddService(IPointRepository pointRepository)
    {
        _pointRepository = pointRepository;
    }
}
```

## **Neden DI Kullanıyoruz?**

### **✅ Avantajlar:**

- **Loose Coupling**: Interface'lere bağımlılık, concrete class'lara değil
- **Testability**: Mock service'ler inject edebiliriz
- **Singleton Pattern**: Repository tek instance (aynı data)
- **Clean Code**: Constructor'da dependencies açık
- **Flexibility**: Farklı implementation'lar switch edilebilir

### **❌ DI Olmasaydı:**

```csharp
// Kötü yaklaşım
public class NewApiController
{
    private readonly PointRepository _repo = new PointRepository();
    private readonly PointAddService _service = new PointAddService(_repo);
    
    // Her controller farklı repository instance'ı = farklı data
    // Test edilemez, sıkı bağlılık
}
```

## **DI Lifecycle Types**

| Type | Açıklama | Projemizde Kullanım |
|------|----------|---------------------|
| **Singleton** | Uygulama boyunca tek instance | `IPointRepository` |
| **Scoped** | HTTP request başına yeni instance | Service'ler |
| **Transient** | Her çağrıda yeni instance | Kullanmıyoruz |

## **DI Akışı**

```c#
1. Program.cs → DI Container'a kayıt
2. Controller → 5 service interface'i inject eder
3. Her Service → IPointRepository inject eder
4. Repository → Singleton olarak tek instance paylaşılır
```

## **Önemli Not**
>
> DI sayesinde tüm service'ler aynı repository instance'ını kullanır. Bu yüzden eklenen point'ler listede görünür. DI olmasaydı her service farklı repository instance'ı kullanır, data kaybolurdu.

## CRUD API Controller - Error Handling Notes

## **Error Handling Strategy**

### **1. Validation Errors (User-Friendly)**

```csharp
catch (ArgumentException ex)
catch (ArgumentNullException ex)
{
    return BadRequest(new ApiResponse<Point>
    {
        IsSuccess = false,
        Message = ex.Message  // ✅ Safe to show user
    });
}
```

### **2. System Errors (Security)**

```csharp
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");  // ✅ Log for developers
    return StatusCode(500, new ApiResponse<Point>
    {
        IsSuccess = false,
        Message = "Generic error message"  // ✅ Hide system details
    });
}
```

## **HTTP Status Codes**

| Scenario | Status Code | Method |
|----------|-------------|--------|
| Success | 200 | `Ok(response)` |
| Created | 201 | `CreatedAtAction(...)` |
| Validation Error | 400 | `BadRequest(response)` |
| Not Found | 404 | `NotFound(response)` |
| System Error | 500 | `StatusCode(500, response)` |

## **Service Response Pattern**

### **Success Response:**

```csharp
if (!response.IsSuccess)
{
    return BadRequest(response);  // Service handles business logic
}
return Ok(response);
```

### **ID Validation Logic:**

```csharp
return response.Message!.Contains("greater than 0") 
    ? BadRequest(response)   // Invalid ID
    : NotFound(response);    // Valid ID but not found
```

## **Key Principles**

- **Validation Errors** → Show `ex.Message` (user can fix)
- **System Errors** → Show generic message (security)
- **Logging** → Always log actual errors for debugging
- **Consistency** → Same error handling pattern across all endpoints