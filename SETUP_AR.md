# تعليمات التشغيل السريع - CarNest API

## 📋 الخطوات المطلوبة

### 1️⃣ تحديث Connection String
افتح ملف `Presentation/appsettings.json` وعدل السطر التالي:

```json
"MostafaDB": "Server=YOUR_SERVER_NAME;Database=CarNest;Trusted_Connection=True;TrustServerCertificate=True"
```

غير `YOUR_SERVER_NAME` إلى اسم SQL Server الخاص بك.

### 2️⃣ تشغيل Migration
افتح SQL Server Management Studio وشغل السكريبت:
```
DAL/Migrations/Manual_RemoveCarImagesAddImageUrls.sql
```

### 3️⃣ تشغيل المشروع
```bash
dotnet run --project Presentation
```

أو افتح المشروع في Visual Studio واضغط F5

## 🔑 الأدوار والصلاحيات

### Admin (المدير)
- ✅ عرض كل السيارات
- ✅ إضافة سيارات جديدة مع صور
- ✅ تعديل أي سيارة
- ✅ حذف أي سيارة

### Vendor (البائع)
- ✅ عرض سياراته فقط
- ✅ إضافة سيارات جديدة مع صور
- ✅ تعديل سياراته فقط
- ✅ حذف سياراته فقط

### Customer (العميل)
- ✅ عرض كل السيارات
- ✅ إضافة سيارات للمفضلة
- ✅ عرض المفضلة
- ❌ لا يمكنه إضافة/تعديل/حذف سيارات

## 📡 أهم الـ Endpoints

### التسجيل والدخول
```
POST /api/account/register    # تسجيل مستخدم جديد
POST /api/account/login        # تسجيل الدخول والحصول على Token
```

### السيارات
```
GET    /api/car              # عرض كل السيارات
GET    /api/car/{id}         # عرض تفاصيل سيارة
POST   /api/car              # إضافة سيارة مع صور
PUT    /api/car/{id}         # تعديل سيارة
DELETE /api/car/{id}         # حذف سيارة
```

### المفضلة
```
GET    /api/favorite         # عرض المفضلة
POST   /api/favorite         # إضافة للمفضلة
DELETE /api/favorite/{id}    # حذف من المفضلة
```

## 🖼️ رفع الصور

الصور يتم رفعها كـ `multipart/form-data`:

```javascript
const formData = new FormData();
formData.append('Year', 2024);
formData.append('Price', 25000);
formData.append('MakeId', 1);
formData.append('ModelId', 5);
formData.append('BodyTypeId', 2);
formData.append('FuelId', 1);
formData.append('LocId', 3);

// إضافة الصور
imageFiles.forEach(file => formData.append('images', file));

// رفع البيانات
fetch('/api/car', {
    method: 'POST',
    headers: { 'Authorization': 'Bearer ' + token },
    body: formData
});
```

## 📝 ملاحظات مهمة

1. **الصور**: يتم حفظها في `wwwroot/images/cars/`
2. **حجم الصورة**: أقصى حجم 5MB
3. **أنواع الصور المسموحة**: jpg, jpeg, png, gif, webp
4. **التوكن**: يجب إرسال JWT Token في الـ Authorization header

## 🔧 اختبار الـ API

بعد تشغيل المشروع، افتح:
```
https://localhost:PORT/swagger
```

سيفتح لك Swagger UI لاختبار كل الـ Endpoints.

## ❓ مشاكل شائعة

### خطأ في الاتصال بقاعدة البيانات
- تأكد من تشغيل SQL Server
- تأكد من صحة Connection String
- تأكد من تشغيل Migration Script

### خطأ 401 Unauthorized
- تأكد من تسجيل الدخول والحصول على Token
- تأكد من إرسال Token في Authorization header

### خطأ 403 Forbidden
- تأكد من أن الدور (Role) لديه الصلاحية المطلوبة
- Vendor لا يمكنه تعديل سيارات البائعين الآخرين

## 📞 للدعم

راجع ملف [README.md](README.md) للتفاصيل الكاملة.

---

**مبروك! المشروع جاهز للاستخدام** 🎉
