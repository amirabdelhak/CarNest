# 🚀 دليل رفع CarNest API على Somee (Monster)

## لماذا Somee؟
- ✅ **Hosting مجاني** لـ ASP.NET Core
- ✅ **SQL Server Database مجانية**
- ✅ **Live URL** - الفرونت إند يستخدمه مباشرة
- ✅ **No Setup** - الفرونت إند مش محتاج ينزل حاجة

---

## 📋 الخطوات الكاملة

### 1️⃣ إنشاء حساب على Somee

1. اذهب إلى: https://somee.com
2. اضغط على **Sign Up** أو **Create Account**
3. املأ البيانات:
   - Username (هيكون جزء من الـ URL)
   - Email
   - Password
4. فعّل الحساب من الإيميل

### 2️⃣ إنشاء قاعدة بيانات SQL Server

1. بعد تسجيل الدخول، اذهب إلى **Control Panel**
2. اضغط على **Databases** → **MS SQL**
3. اضغط على **Create Database**
4. سجّل المعلومات دي (مهمة جداً):
   ```
   Server: [YourUsername].mssql.somee.com
   Database: [YourUsername]_CarNest
   Username: [YourUsername]_CarNest
   Password: [كلمة السر اللي هتختارها]
   ```

### 3️⃣ تحديث Connection String

افتح `Presentation/appsettings.json` وأضف:

```json
{
  "ConnectionStrings": {
    "SomeeDB": "Server=[YourUsername].mssql.somee.com;Database=[YourUsername]_CarNest;User Id=[YourUsername]_CarNest;Password=[YourPassword];TrustServerCertificate=True;MultipleActiveResultSets=true"
  }
}
```

وعدّل في `Program.cs` السطر ده:
```csharp
// غيّر من:
options.UseSqlServer(builder.Configuration.GetConnectionString("MostafaDB"))

// إلى:
options.UseSqlServer(
    builder.Configuration.GetConnectionString("SomeeDB") 
    ?? builder.Configuration.GetConnectionString("MostafaDB")
)
```

### 4️⃣ رفع Database Schema

**الطريقة الأولى: SQL Server Management Studio (SSMS)**

1. افتح SSMS
2. اتصل بـ Somee Database:
   - Server: `[YourUsername].mssql.somee.com`
   - Authentication: SQL Server Authentication
   - Username: `[YourUsername]_CarNest`
   - Password: `[YourPassword]`

3. شغّل الـ Migration Script:
   ```sql
   -- افتح الملف ده وشغّله
   DAL/Migrations/Manual_RemoveCarImagesAddImageUrls.sql
   ```

4. شغّل الـ Schema Script (إذا كان موجود):
   - أو استخدم `dotnet ef database update` محلياً ثم انسخ الـ schema

**الطريقة الثانية: استخدام Somee phpMyAdmin**

1. من Control Panel → **Databases** → **MS SQL**
2. اضغط على **Manage** جنب الداتابيز
3. استخدم Query Editor لتشغيل الـ Scripts

### 5️⃣ Build المشروع للنشر

```bash
cd d:\CarNestRepo
dotnet publish Presentation -c Release -o publish
```

✅ **تم بالفعل!** الملفات موجودة في `d:\CarNestRepo\publish`

### 6️⃣ رفع الملفات على Somee

**الطريقة الأولى: FTP (الأسهل)**

1. من Somee Control Panel، اذهب إلى **File Manager** أو **FTP Details**
2. سجّل معلومات FTP:
   ```
   Host: ftp://[YourUsername].somee.com
   Username: [YourUsername]
   Password: [YourPassword]
   ```

3. استخدم FileZilla أو أي FTP Client:
   - نزّل FileZilla من: https://filezilla-project.org
   - اتصل بالـ FTP
   - ارفع محتويات مجلد `publish` إلى `/wwwroot/`

**الطريقة الثانية: File Manager من الموقع**

1. من Control Panel → **File Manager**
2. اذهب إلى `/wwwroot/`
3. ارفع الملفات من مجلد `publish`
4. تأكد من رفع:
   - كل الـ DLL files
   - `appsettings.json`
   - `web.config` (إذا موجود)
   - مجلد `wwwroot` (للصور)

### 7️⃣ إنشاء/تعديل web.config

إذا لم يكن موجود، أنشئ ملف `web.config` في مجلد `publish`:

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <location path="." inheritInChildApplications="false">
    <system.webServer>
      <handlers>
        <add name="aspNetCore" path="*" verb="*" modules="AspNetCoreModuleV2" resourceType="Unspecified" />
      </handlers>
      <aspNetCore processPath="dotnet" 
                  arguments=".\Presentation.dll" 
                  stdoutLogEnabled="false" 
                  stdoutLogFile=".\logs\stdout" 
                  hostingModel="inprocess" />
    </system.webServer>
  </location>
</configuration>
```

### 8️⃣ اختبار الـ API

بعد الرفع، الـ API هيكون متاح على:
```
http://[YourUsername].somee.com/api/car
http://[YourUsername].somee.com/swagger
```

جرّب:
```bash
# اختبار الـ API
curl http://[YourUsername].somee.com/api/car

# أو افتح في المتصفح
http://[YourUsername].somee.com/swagger
```

---

## 🔧 حل المشاكل الشائعة

### مشكلة: 500 Internal Server Error

**الحل:**
1. تأكد من رفع كل الـ DLL files
2. تأكد من `web.config` صحيح
3. تأكد من Connection String صحيح
4. شوف الـ logs في Somee Control Panel

### مشكلة: Database Connection Failed

**الحل:**
1. تأكد من Connection String صحيح
2. تأكد من تشغيل Migration Scripts
3. جرّب الاتصال بالداتابيز من SSMS أولاً

### مشكلة: Images لا تظهر

**الحل:**
1. تأكد من رفع مجلد `wwwroot/images/cars`
2. تأكد من الـ permissions على المجلد
3. تأكد من `app.UseStaticFiles()` في Program.cs

### مشكلة: CORS Errors

**الحل:**
تأكد من إضافة domain الفرونت إند في CORS:
```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder
            .WithOrigins("http://frontend-domain.com")
            .AllowAnyMethod()
            .AllowAnyHeader());
});
```

---

## 📝 Checklist قبل الرفع

- [ ] تحديث Connection String في appsettings.json
- [ ] Build المشروع: `dotnet publish -c Release`
- [ ] إنشاء Database على Somee
- [ ] رفع Migration Scripts على Database
- [ ] رفع ملفات publish على FTP
- [ ] إنشاء/تعديل web.config
- [ ] اختبار الـ API من المتصفح
- [ ] اختبار Swagger
- [ ] اختبار رفع الصور

---

## 🎯 بعد الرفع - للفرونت إند

أعطي الفرونت إند ديفولبر:

1. **Base URL**:
   ```
   https://[YourUsername].somee.com
   ```

2. **Swagger Documentation**:
   ```
   https://[YourUsername].somee.com/swagger
   ```

3. **Sample Endpoints**:
   ```
   GET  https://[YourUsername].somee.com/api/car
   POST https://[YourUsername].somee.com/api/account/login
   GET  https://[YourUsername].somee.com/api/favorite
   ```

---

## 💡 نصائح مهمة

1. **Somee Free Plan Limitations**:
   - Database: 20 MB
   - Bandwidth: محدود
   - قد يتم إيقاف الموقع إذا لم يُستخدم لفترة

2. **Keep Alive**:
   - استخدم خدمة مثل UptimeRobot لإبقاء الموقع نشط
   - أو اطلب من الفرونت إند عمل ping كل 5 دقائق

3. **Backup**:
   - احتفظ بنسخة من الداتابيز
   - احتفظ بنسخة من الملفات المرفوعة

---

## 🚀 بدائل Somee (إذا احتجت)

1. **Azure App Service** (Free Tier)
   - أفضل أداء
   - 60 دقيقة CPU يومياً

2. **Heroku** (مع Container)
   - سهل الاستخدام
   - Free tier محدود

3. **Railway.app**
   - سهل جداً
   - $5 شهرياً

---

**بالتوفيق! 🎉**

إذا واجهت أي مشكلة، اتصل بي وسأساعدك.
