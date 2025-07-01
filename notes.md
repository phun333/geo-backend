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