using Kopija.Modeli;
using Kopija.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Kopija.ModeliPomocni;
using Microsoft.AspNetCore.Authorization;

namespace Kopija.Controllers
{
    [Authorize(Roles =("Superadmin"))]
    public class AccountController : Controller
    {
        private UserManager<AppUser> UserMgr { get; }
        private SignInManager<AppUser> SignInMgr { get; }
        private RoleManager<AppRole> RoleMngr { get; } // ovo sam individualno ubacio da znas 
        private readonly IdentityAppContext _dropDownModel;
        private readonly ILogger<AccountController> _logger;
        private readonly IWebHostEnvironment _hostingEnvironment;



        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager, IdentityAppContext dropDownModel, ILogger<AccountController> logger, IWebHostEnvironment hostingEnvironment)
        {
            UserMgr = userManager;
            SignInMgr = signInManager;
            RoleMngr = roleManager;
            _dropDownModel = dropDownModel;
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
        }

       


        [AllowAnonymous]
        public IActionResult Login()
        {

            return View();

        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await SignInMgr.PasswordSignInAsync(model.UserName, model.Password, false, false);
                
                if (result.Succeeded)
                {
                    var idUsera = await UserMgr.FindByNameAsync(model.UserName);


                    var user = await UserMgr.FindByIdAsync(idUsera.Id.ToString());
                    var role = await UserMgr.GetRolesAsync(user);

                    string rola = role.First(); 

                    
                    

                    TempData["user_id"] = user.Id; 

                    //ovako nekako stavljam u sesiju podatke
                    HttpContext.Session.SetInt32("UserId", user.Id);
                    HttpContext.Session.SetInt32("UserLokacijaId", user.LokacijaId);
                    HttpContext.Session.SetInt32("UserSektorId", user.SektorId);
                    HttpContext.Session.SetString("UserRola", rola);


                    return RedirectToAction("Index", "Home");

                }
                else
                {
                   
                    ModelState.AddModelError("Error", "Podaci koji su uneti nisu ispravni,pokusajte ponovo!");
                }
            }
            else
            {
                ModelState.AddModelError("Error", "Podaci koji su uneti nisu ispravni,pokusajte ponovo!");

            }

            return View();


        }
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {

            await SignInMgr.SignOutAsync();
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");


        }

        [HttpGet]
        public IActionResult CreateRole()
        {


            return View();

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRole(RoleModel model)
        {


            if (ModelState.IsValid)
            {
                AppRole identityRole = new AppRole
                {
                    Name = model.Ime

                };

                IdentityResult result = await RoleMngr.CreateAsync(identityRole);

                if (result.Succeeded)
                {

                    return RedirectToAction("Index", "Home");
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

            }


            return View(model);


        }

        [HttpGet]
        public IActionResult DodajKorisnika()
        {
            
            IList<sektor> sektorLista = new List<sektor>();
            IList<lokacija> lokacijaLista = new List<lokacija>();
            List<aspnetroles> roleLista = new List<aspnetroles>();

            roleLista = (from servis1 in _dropDownModel.aspnetroles select servis1).ToList();
            lokacijaLista = (from servis1 in _dropDownModel.lokacija select servis1).ToList();
            sektorLista = (from servis1 in _dropDownModel.sektor select servis1).ToList();


            ViewData["sektor"] = sektorLista;
            ViewData["lokacija"] = lokacijaLista;

           
            ViewBag.sektor = sektorLista;
            ViewBag.pristup = roleLista;
            ViewBag.lokacija = lokacijaLista;

            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DodajKorisnika(KorisnikModel model)
        {

            if (ModelState.IsValid)
            {

                List<sektor> sektorLista = new List<sektor>();
                List<lokacija> lokacijaLista = new List<lokacija>();
                List<aspnetroles> roleLista = new List<aspnetroles>();

                roleLista = (from servis1 in _dropDownModel.aspnetroles select servis1).ToList();
                lokacijaLista = (from servis1 in _dropDownModel.lokacija select servis1).ToList();
                sektorLista = (from servis1 in _dropDownModel.sektor select servis1).ToList();

                ViewBag.sektor = sektorLista;
                ViewBag.pristup = roleLista;
                ViewBag.lokacija = lokacijaLista;

                try
                {
                    AppUser user = await UserMgr.FindByNameAsync(model.KorisnickoIme);
                    ViewBag.Message = "Korisnik  sa tim korisnickim imenom vec postoji!";
                    if (user == null)
                    {
                        user = new AppUser();

                        user.UserName = model.KorisnickoIme;
                        user.BrojTelefona = model.BrojTelefona;
                        user.Ime = model.Ime;
                        user.Prezime = model.Prezime;
                        user.RadnoMesto = model.RadnoMesto;
                        user.SektorId = model.SektorId;
                        user.LokacijaId = model.LokacijaId;
                        user.OtpisanRadnik = model.OtpisanRadnik;

                        IdentityResult result = await UserMgr.CreateAsync(user, model.Sifra);
                        if (result.Succeeded)
                        {
                            IdentityResult dodajRolu = await UserMgr.AddToRoleAsync(user, model.NivoPristupa);

                            if (dodajRolu.Succeeded)
                            {
                                ViewBag.Message = "Korisnik je kreiran!";
                                ModelState.Clear();
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Message = ex.Message;
                }

               
            }


           return View();

        }

        [HttpGet]
        public ActionResult KorisnikLista()
        {
            List<KorisnikListaModel> korisnikListaKonacno = new List<KorisnikListaModel>();

            var query = from Korisnik in _dropDownModel.aspnetusers
                        join Sektor in _dropDownModel.sektor on Korisnik.SektorId equals Sektor.sektor_id
                        join Lokacija in _dropDownModel.lokacija on Korisnik.LokacijaId equals Lokacija.lokacija_id
                        where Korisnik.OtpisanRadnik == 0 //nemoj ovo zab
                        select new
                        {
                            Korisnik.Id,
                            Korisnik.UserName,
                            Korisnik.Ime,
                            Korisnik.Prezime,
                            Korisnik.BrojTelefona,
                            Korisnik.RadnoMesto,
                            Sektor.sektor_ime,
                            Lokacija.lokacija_ime,
                            Lokacija.lokacija_adresa
                        };

            foreach (var item in query)
            {
                KorisnikListaModel model = new KorisnikListaModel();

                model.Ime = item.Ime;
                model.Prezime = item.Prezime;
                model.BrojTelefona = item.BrojTelefona;
                model.RadnoMesto = item.RadnoMesto;
                model.Adresa = item.lokacija_adresa;
                model.LokacijaIme = item.lokacija_ime;
                model.Sektor = item.sektor_ime;
                model.KorisnickoIme = item.UserName;
                model.Id = item.Id;

                korisnikListaKonacno.Add(model);
                
            }


            return View(korisnikListaKonacno);
        }
 
        [HttpGet]
        public async Task<IActionResult> EditUser(int? id)
        {
            var user = await UserMgr.FindByIdAsync(id.ToString());

            if(user == null)
            {

                ViewBag.Message = "ne postoji korisnik!";
                return View("Not Found!");
            }

            var model = new EdituserViewModel()
            {
                Id = user.Id,
                Ime = user.Ime,
                Prezime = user.Prezime,
                BrojTelefona = user.BrojTelefona,
                RadnoMesto = user.RadnoMesto,
                UserName = user.UserName,

            };


            return View(model); 
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EdituserViewModel model)
        {
             var user = await UserMgr.FindByIdAsync(model.Id.ToString());
          // var user = await UserMgr.FindByNameAsync(model.UserName);
            if (user == null)
            {

                ViewBag.Message = "ne postoji korisnik!";
                return View("EditUser");
            }

            else
            {

                try
                {
                    user.Ime = model.Ime;
                    user.Prezime = model.Prezime;
                    user.BrojTelefona = model.BrojTelefona;
                    user.RadnoMesto = model.RadnoMesto;
                    user.UserName = model.UserName;


                    var proveraUser = await UserMgr.FindByNameAsync(user.UserName);

                    if (proveraUser != null && proveraUser != user)
                    {
                        ModelState.AddModelError("Provera", "Korisnicko ime vec postoji!");
                        return View(model);
                    }



                    var result = await UserMgr.UpdateAsync(user);
                    _dropDownModel.SaveChanges();
                    if (result.Succeeded)
                    {


                        ViewBag.Message = "Uspesno promenjeni podaci o korisniku!!";


                        //return RedirectToAction("KorisnikLista");

                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("Provera", error.Description);
                    }
                }
                catch (Exception)
                {

                    ViewBag.Message = "Uspesno promenjeni podaci o korisniku!!"; ;
                }
                

                return View(model);

            }
        }


        [HttpGet]
        public IActionResult ChangePassword(int? id)
        {

            if (id == null)
            {

               return RedirectToAction("KorisnikLista");
            }



            return View() ;
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ResetPasswordModel model,int? id)
        {

            if (id == null)
            {

                return RedirectToAction("KorisnikLista");
            }

            var user = await UserMgr.FindByIdAsync(id.ToString());

            if(user == null)
            {
                return View("Korisnik nije pronadjen!");

            }


           
                var token = await UserMgr.GeneratePasswordResetTokenAsync(user);


                var reset = await UserMgr.ResetPasswordAsync(user, token, model.Password);

            if (reset.Succeeded)
            {

                ViewBag.Message = "Uspesno promenjena sifra siso mala!";
            }
            


                foreach (var item in reset.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            
  

            return View();
        }


        [HttpGet]
        public IActionResult Dostavljac()
        {

            IList<sektor> sektorLista = new List<sektor>();
            sektorLista = (from servis1 in _dropDownModel.sektor select servis1).ToList();
            ViewBag.sektor = sektorLista;

            return View(); // ne moze  ovako izgleda da cu morati praviti partial view
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Dostavljac(dostavljac model) 
        {

            IList<sektor> sektorLista = new List<sektor>();
            sektorLista = (from servis1 in _dropDownModel.sektor select servis1).ToList();
            ViewBag.sektor = sektorLista;
            if (!ModelState.IsValid)
            {
                ViewBag.Message = "Neuspesno unet dostavljac!";
                return View();

            }

            try
            {
                
               
                this._dropDownModel.dostavljac.Add(model);
                _dropDownModel.SaveChanges();

               

                ViewBag.Message = "Uspesno unet dostavljac!";
                ModelState.Clear();
            }
            catch (Exception e)
            {

                ViewBag.Message = e.Message;
            }


            return View();
        }
       



        


        [HttpGet]
        public ActionResult DostavljacLista()
        {
            IList<SektorZaDobavljaceSuperadmin> dostavljaclista = new List<SektorZaDobavljaceSuperadmin>();

            var nesto = (from dostavljac in _dropDownModel.dostavljac
                         join sektor in _dropDownModel.sektor on dostavljac.dostavljac_sektor_id equals sektor.sektor_id
                         select new
                         {
                             dostavljac.dostavljac_ime,
                             dostavljac.dostavljac_pib,
                             dostavljac.dostavljac_adresa,
                             dostavljac.dostavljac_kontakt_osoba_broj,
                             dostavljac.dostavljac_kontakt_osoba_broj1,
                             dostavljac.dostavljac_kontakt_osoba,
                             sektor.sektor_ime
                         }).ToList();

            foreach (var item in nesto)
            {
                SektorZaDobavljaceSuperadmin model = new SektorZaDobavljaceSuperadmin();
                model.dostavljac_ime = item.dostavljac_ime;
                model.dostavljac_pib = item.dostavljac_pib;
                model.dostavljac_adresa = item.dostavljac_adresa;
                model.dostavljac_kontakt_osoba_broj = item.dostavljac_kontakt_osoba_broj;
                model.dostavljac_kontakt_osoba_broj1 = item.dostavljac_kontakt_osoba_broj1;
                model.dostavljac_kontakt_osoba = item.dostavljac_kontakt_osoba;
                model.dostavljac_sektor_id = item.sektor_ime;
                dostavljaclista.Add(model);
            }


            return View(dostavljaclista); 
        }

        

            

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Servis(servis model)

        {

            IList<sektor> sektorLista = new List<sektor>();
            sektorLista = (from servis1 in _dropDownModel.sektor select servis1).ToList();
            ViewBag.sektor = sektorLista;

            if (ModelState.IsValid)
            {


                try
                {


                    this._dropDownModel.servis.Add(model);
                    _dropDownModel.SaveChanges();



                    ViewBag.Message = "Uspesno unet servis!";
                    ModelState.Clear();
                }
                catch (Exception e)
                {

                    ViewBag.Message = e.Message;

                }

            }

            return View();
        }

        [HttpGet]
        public ActionResult Servis()
        {
            IList<sektor> sektorLista = new List<sektor>();
            sektorLista = (from servis1 in _dropDownModel.sektor select servis1).ToList();
            ViewBag.sektor = sektorLista;

            return View();
        }

        [HttpGet]
        public ActionResult ServisLista()
            {

            IList<SektorListaViewSuperAdmin> sektorListaKonacno = new List<SektorListaViewSuperAdmin>();

            var nesto = (from servis in _dropDownModel.servis
                        join sektor in _dropDownModel.sektor on servis.servis_sektor_id equals sektor.sektor_id
                        select new {
                            servis.servis_ime,
                            servis.servis_pib,
                            servis.servis_adresa,
                            servis.servis_kontakt_osoba_broj,
                            servis.servis_kontakt_osoba_broj1,
                            servis.servis_kontakt_osoba,
                            sektor.sektor_ime
                        }).ToList();

            foreach (var item in nesto)
            {
                SektorListaViewSuperAdmin model = new SektorListaViewSuperAdmin();
                model.servis_ime = item.servis_ime;
                model.servis_pib = item.servis_pib;
                model.servis_adresa = item.servis_adresa;
                model.servis_kontakt_osoba_broj = item.servis_kontakt_osoba_broj;
                model.servis_kontakt_osoba_broj1 = item.servis_kontakt_osoba_broj1;
                model.servis_kontakt_osoba = item.servis_kontakt_osoba;
                model.servis_sektor_id = item.sektor_ime;
                sektorListaKonacno.Add(model);
            }

            return View(sektorListaKonacno);
             }


        [HttpGet]
            public ActionResult LokacijaLista()
            {
            IList<lokacija> lokacijaLista = new List<lokacija>();
            lokacijaLista = (from servis1 in _dropDownModel.lokacija select servis1).ToList();


            return View(lokacijaLista);
            }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Lokacija(lokacija model)
        {
            if (ModelState.IsValid)
            { 

                try
                {


                    this._dropDownModel.lokacija.Add(model);
                    _dropDownModel.SaveChanges();



                    ViewBag.Message = "Uspesno uneta lokacija!";
                    ModelState.Clear();
                }
                catch (Exception e)
                {

                    ViewBag.Message = e.Message;
                }

            }


            return View();
        }

        [HttpGet]
        public ActionResult Lokacija()
        {



            return View();
        }


        [HttpGet]
            public ActionResult Sektor()
        {



            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Sektor(SektorView modelView)
        {
            sektor model = new sektor();

            if (ModelState.IsValid)
            {
                

                if(modelView.sektor_bitan == false)
                {
                    model.sektor_ime = modelView.sektor_ime;
                    model.sektor_bitan = 0;

                }else
                {
                    model.sektor_ime = modelView.sektor_ime;
                    model.sektor_bitan = 1;
                }

                try
                {
                    this._dropDownModel.sektor.Add(model);
                    _dropDownModel.SaveChanges();



                    ViewBag.Message = "Uspesno unet sektor!";
                    ModelState.Clear();
                }
                catch (Exception e)
                {

                    ViewBag.Message = e.Message;
                }
            }

            return View();
        }

        [HttpGet]
        public ActionResult SektorLista()
        {

            IList<sektor> sektorLista = new List<sektor>();
            sektorLista = (from servis1 in _dropDownModel.sektor select servis1).ToList();

            
            return View(sektorLista);
        }


        public ActionResult KategorijaLista()
        {
            IList<OpremaKategorijaListaModel> kategorijaLista = new List<OpremaKategorijaListaModel>();


            var query = from Kategorija in _dropDownModel.oprema_kategorija
                        join Sektor in _dropDownModel.sektor on Kategorija.oprema_kategorija_sektor_id equals Sektor.sektor_id
                        
                        select new
                        {
                            Kategorija.oprema_kategorija_ime,
                            Sektor.sektor_ime 
                        };

            foreach (var item in query)
            {
                OpremaKategorijaListaModel model = new OpremaKategorijaListaModel();

                model.oprema_kategorija_ime = item.oprema_kategorija_ime;
                model.oprema_kategorija_sektor_iime = item.sektor_ime;
             
                kategorijaLista.Add(model);
            }


            return View(kategorijaLista);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Kategorija(oprema_kategorija model)
        {

            if (ModelState.IsValid)
            {
                List<sektor> sektorLista = new List<sektor>();
                sektorLista = (from servis1 in _dropDownModel.sektor select servis1).ToList();
                ViewBag.sektor = sektorLista;
                try
                {
                    

                    this._dropDownModel.oprema_kategorija.Add(model);
                    _dropDownModel.SaveChanges();



                    ViewBag.Message = "Uspesno uneta kategorija!";
                    ModelState.Clear();
                }
                catch (Exception e)
                {

                    ViewBag.Message = e.Message;
                }
               

            }

            return View();
        }
        public ActionResult Kategorija()
        {
            List<sektor> sektorLista = new List<sektor>();
            sektorLista = (from servis1 in _dropDownModel.sektor select servis1).ToList();
            ViewBag.sektor = sektorLista;

            return View();
        }
        [AllowAnonymous]
        public ActionResult AccessDenied()
        {


            return RedirectToAction("Login","Account");
        }


    }
}


