using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MVC_Identity.Models.Context;
using MVC_Identity.Models.Entity;

var builder = WebApplication.CreateBuilder(args);

//Configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");//veri taban� �ekmek i�in

// Add services to the container.
builder.Services.AddControllersWithViews();//Bu metot, controller'lar�n ve view'lerin eklenmesi i�in gerekli olan MVC hizmetlerini i�eren bir dizi �n ayar� yapar. Controller'lar, uygulaman�n istekleri nas�l i�leyece�ini belirlerken, view'ler, kullan�c� arabirimini olu�turmak i�in kullan�l�r.

//AddDbContext
builder.Services
    .AddDbContext<ProjectContext>(options => options.UseSqlServer(connectionString));


//IdentityService
builder.Services
    .AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<ProjectContext>()
    .AddDefaultTokenProviders() //bir emaili konfirme edebilmek i�in token olu�turkulmas� gerekmektedir. Bu token olu�turucu (provider) usermanager servis i�erisinde tan�ml� olmas� gerekmektedir.
    ;

//Identity Password Customize
// �zelliklerini de�i�tirerek �ifre gereksinimlerini �zelle�tirmektedir. �zellikle �ifre politikalar�n� belirlemek i�in kullan�l�r. �ifre politikalar�, kullan�c�lar�n olu�turduklar� �ifrelerde hangi �zelliklerin gereklilik oldu�unu belirler.
builder.Services.Configure<IdentityOptions>(x =>
{
    x.Password.RequireDigit = false;
    x.Password.RequiredLength = 6;
    x.Password.RequireLowercase = false;
    x.Password.RequireUppercase = false;
    x.Password.RequireNonAlphanumeric = false;
    
});

//Cookie
//kimlik do�rulama �erezi (authentication cookie) i�in ayarlar� yap�land�rmak i�in kullan�l�r. Bu �erez, kullan�c�n�n oturumunu (session) ve kimlik do�rulama durumunu tutar.
builder.Services.ConfigureApplicationCookie(x =>
{
    x.Cookie = new CookieBuilder
    {
        Name = "Yzl3171Cookie"
    };

    x.LoginPath = new PathString("/Account/Login");//Kullan�c� oturumu gerektiren bir istek yap�ld���nda y�nlendirilecek olan yol (path) belirtilir. 
    x.SlidingExpiration = true;// kullan�c� her istekte oturum s�resi uzat�l�r.
    x.ExpireTimeSpan = TimeSpan.FromMinutes(1);// Oturum �erezinin ne kadar s�reyle ge�erli olaca��n� belirler. (1dk)


});
//HTTP isteklerini i�leyen ve isteklerin nas�l y�nlendirilece�ini belirleyen HTTP istek boru hatt�n� yap�land�rmak i�in kullan�l�r.
var app = builder.Build();//boru hatt� yap�land�r�l�r

if (!app.Environment.IsDevelopment())//uygulaman�n geli�tirme ortam�nda olup olmad���n� kontrol eder.
{
    app.UseExceptionHandler("/Home/Error");// E�er bir istek s�ras�nda bir hata olu�ursa, "/Home/Error" yolundaki bir controller'a y�nlendirilir. 
    
    app.UseHsts();//g�venlik a��s�ndan 
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


app.UseAuthentication(); //kimlik y�netimi
app.UseAuthorization(); //oturum y�netimi



app.UseEndpoints(endpoint =>
{

    //Administrator Area Route
    //Bu b�l�m, farkl� alanlarda (�rne�in, y�netici paneli gibi) �al��an controller'lar� belirtmek i�in kullan�l�r. {area:exists} deseni, istenen alan�n varl���n� kontrol eder ve e�er varsa, o alandaki controller'lar� belirtilen varsay�lan action ve parametrelerle �a��r�r.
    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllerRoute(
          name: "areas",
          pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
        );
    });


    //Default Route
    //Bu k�s�m, genel istekler i�in kullan�lan varsay�lan bir route (yol) belirtir. E�er bir istek belirtilen bir alanla ili�kilendirilemiyorsa veya alan belirtilmemi�se, bu route kullan�l�r. Bu, HomeController'daki Index action'�n� varsay�lan olarak �a��r�r.
    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllerRoute(
          name: "default",
          pattern: "{controller=Home}/{action=Index}/{id?}"
        );
    });


});

app.Run();
