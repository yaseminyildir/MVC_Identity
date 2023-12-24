using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MVC_Identity.Models.Context;
using MVC_Identity.Models.Entity;

var builder = WebApplication.CreateBuilder(args);

//Configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");//veri tabaný çekmek için

// Add services to the container.
builder.Services.AddControllersWithViews();//Bu metot, controller'larýn ve view'lerin eklenmesi için gerekli olan MVC hizmetlerini içeren bir dizi ön ayarý yapar. Controller'lar, uygulamanýn istekleri nasýl iþleyeceðini belirlerken, view'ler, kullanýcý arabirimini oluþturmak için kullanýlýr.

//AddDbContext
builder.Services
    .AddDbContext<ProjectContext>(options => options.UseSqlServer(connectionString));


//IdentityService
builder.Services
    .AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<ProjectContext>()
    .AddDefaultTokenProviders() //bir emaili konfirme edebilmek için token oluþturkulmasý gerekmektedir. Bu token oluþturucu (provider) usermanager servis içerisinde tanýmlý olmasý gerekmektedir.
    ;

//Identity Password Customize
// özelliklerini deðiþtirerek þifre gereksinimlerini özelleþtirmektedir. Özellikle þifre politikalarýný belirlemek için kullanýlýr. Þifre politikalarý, kullanýcýlarýn oluþturduklarý þifrelerde hangi özelliklerin gereklilik olduðunu belirler.
builder.Services.Configure<IdentityOptions>(x =>
{
    x.Password.RequireDigit = false;
    x.Password.RequiredLength = 6;
    x.Password.RequireLowercase = false;
    x.Password.RequireUppercase = false;
    x.Password.RequireNonAlphanumeric = false;
    
});

//Cookie
//kimlik doðrulama çerezi (authentication cookie) için ayarlarý yapýlandýrmak için kullanýlýr. Bu çerez, kullanýcýnýn oturumunu (session) ve kimlik doðrulama durumunu tutar.
builder.Services.ConfigureApplicationCookie(x =>
{
    x.Cookie = new CookieBuilder
    {
        Name = "Yzl3171Cookie"
    };

    x.LoginPath = new PathString("/Account/Login");//Kullanýcý oturumu gerektiren bir istek yapýldýðýnda yönlendirilecek olan yol (path) belirtilir. 
    x.SlidingExpiration = true;// kullanýcý her istekte oturum süresi uzatýlýr.
    x.ExpireTimeSpan = TimeSpan.FromMinutes(1);// Oturum çerezinin ne kadar süreyle geçerli olacaðýný belirler. (1dk)


});
//HTTP isteklerini iþleyen ve isteklerin nasýl yönlendirileceðini belirleyen HTTP istek boru hattýný yapýlandýrmak için kullanýlýr.
var app = builder.Build();//boru hattý yapýlandýrýlýr

if (!app.Environment.IsDevelopment())//uygulamanýn geliþtirme ortamýnda olup olmadýðýný kontrol eder.
{
    app.UseExceptionHandler("/Home/Error");// Eðer bir istek sýrasýnda bir hata oluþursa, "/Home/Error" yolundaki bir controller'a yönlendirilir. 
    
    app.UseHsts();//güvenlik açýsýndan 
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


app.UseAuthentication(); //kimlik yönetimi
app.UseAuthorization(); //oturum yönetimi



app.UseEndpoints(endpoint =>
{

    //Administrator Area Route
    //Bu bölüm, farklý alanlarda (örneðin, yönetici paneli gibi) çalýþan controller'larý belirtmek için kullanýlýr. {area:exists} deseni, istenen alanýn varlýðýný kontrol eder ve eðer varsa, o alandaki controller'larý belirtilen varsayýlan action ve parametrelerle çaðýrýr.
    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllerRoute(
          name: "areas",
          pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
        );
    });


    //Default Route
    //Bu kýsým, genel istekler için kullanýlan varsayýlan bir route (yol) belirtir. Eðer bir istek belirtilen bir alanla iliþkilendirilemiyorsa veya alan belirtilmemiþse, bu route kullanýlýr. Bu, HomeController'daki Index action'ýný varsayýlan olarak çaðýrýr.
    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllerRoute(
          name: "default",
          pattern: "{controller=Home}/{action=Index}/{id?}"
        );
    });


});

app.Run();
