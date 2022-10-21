
using System.Diagnostics;
using Kopija.Modeli;
using Kopija.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using iTextSharp.text;
using iTextSharp.text.pdf;
using MySqlConnector;
using Kopija.ModeliPomocni;
using Document = iTextSharp.text.Document;
using Microsoft.CodeAnalysis;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SelectPdf;
using Microsoft.AspNetCore.Authorization;

namespace Kopija.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private UserManager<AppUser> UserMgr { get; }
        private SignInManager<AppUser> SignInMgr { get; }
        private RoleManager<AppRole> RoleMngr { get; } 
        private readonly IdentityAppContext _dropDownModel;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;
        protected readonly ICompositeViewEngine _compositeViewEngine;
        public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager, IdentityAppContext dropDownModel, IWebHostEnvironment hostingEnvironment, IConfiguration configuration,ICompositeViewEngine compositeViewEngine)
        {
            _logger = logger;
            UserMgr = userManager;
            SignInMgr = signInManager;
            RoleMngr = roleManager;
            _dropDownModel = dropDownModel;
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
            _compositeViewEngine = compositeViewEngine;
        }

        public static IConfiguration InitConfiguration()
        {
            var config = new ConfigurationBuilder()
          .AddJsonFile("appsettings.json")
           .AddEnvironmentVariables()
           .Build();
            return config;
        }

        public static string StringKonekcije()
        {

            return "Server=127.0.0.1;Port=3306;Database=persuproba;Uid=root;Pwd='';convert zero datetime=True;";
        }


        [HttpGet]
        public IActionResult Oprema()
        {

            IList<dostavljac> dostavljacLista = new List<dostavljac>();
            IList<oprema_kategorija> kategorijaLista = new List<oprema_kategorija>();

            if (HttpContext.Session.GetInt32("UserSektorId") == null)
            {

                return RedirectToAction("Login", "Account");
            }

            int SektorUser = (int)HttpContext.Session.GetInt32("UserSektorId");
            dostavljacLista = (from servis1 in _dropDownModel.dostavljac.Where(x => x.dostavljac_sektor_id == SektorUser) select servis1).ToList();


            var query = from Kategorija in _dropDownModel.oprema_kategorija

                        where Kategorija.oprema_kategorija_sektor_id == SektorUser//nemoj ovo zab
                        select new
                        {
                            Kategorija.oprema_kategorija_id,
                            Kategorija.oprema_kategorija_ime,


                        };


            foreach (var item in query)
            {
                oprema_kategorija kategorija = new oprema_kategorija();

                kategorija.oprema_kategorija_id = item.oprema_kategorija_id;
                kategorija.oprema_kategorija_ime = item.oprema_kategorija_ime;

                kategorijaLista.Add(kategorija);

            }

            ViewBag.dostavljac = dostavljacLista;
            ViewBag.kategorija = kategorijaLista;


            return View();
        }


        [HttpPost]
        public IActionResult Oprema(oprema model)
        {
            IList<dostavljac> dostavljacLista = new List<dostavljac>();
            IList<oprema_kategorija> kategorijaLista = new List<oprema_kategorija>();

            if (HttpContext.Session.GetInt32("UserSektorId") == null)
            {

                return RedirectToAction("Login", "Account");
            }

            int SektorUser = (int)HttpContext.Session.GetInt32("UserSektorId");
            dostavljacLista = (from servis1 in _dropDownModel.dostavljac.Where(x => x.dostavljac_sektor_id == SektorUser) select servis1).ToList();

            var query = from Kategorija in _dropDownModel.oprema_kategorija

                        where Kategorija.oprema_kategorija_sektor_id == SektorUser//nemoj ovo zab
                        select new
                        {
                            Kategorija.oprema_kategorija_id,
                            Kategorija.oprema_kategorija_ime,


                        };


            foreach (var item in query)
            {
                oprema_kategorija kategorija = new oprema_kategorija();

                kategorija.oprema_kategorija_id = item.oprema_kategorija_id;
                kategorija.oprema_kategorija_ime = item.oprema_kategorija_ime;

                kategorijaLista.Add(kategorija);

            }

            ViewBag.dostavljac = dostavljacLista;
            ViewBag.kategorija = kategorijaLista;

            var QRProvera = this._dropDownModel.oprema.Where(x => x.oprema_qr_kod == model.oprema_qr_kod).Count();
            if (QRProvera > 0)
            {

                ModelState.AddModelError("QR", "Izabrani QR kod vec postoji u sistemu!");
            }


            if (ModelState.IsValid)
            {
                



                try
                {

                    oprema model1 = new oprema
                    {
                        oprema_id = model.oprema_id,
                        oprema_model = model.oprema_model,
                        oprema_marka = model.oprema_marka,
                        oprema_kategorija_id = model.oprema_kategorija_id,
                        oprema_qr_kod = model.oprema_qr_kod,
                        oprema_datum_nabavke = model.oprema_datum_nabavke.Date,
                        oprema_cena = model.oprema_cena,
                        oprema_razlog_otpisa = model.oprema_razlog_otpisa,
                        oprema_garancija = model.oprema_garancija.Date,

                        oprema_dostavljac_id = model.oprema_dostavljac_id,
                        oprema_otpisano = model.oprema_otpisano,
                        oprema_stanje = model.oprema_stanje,
                        oprema_serijski_broj = model.oprema_serijski_broj,
                        oprema_napomena = model.oprema_napomena


                    };


                    this._dropDownModel.oprema.Add(model1);
                    _dropDownModel.SaveChanges();



                    ViewBag.Message = "Uspesno uneta oprema u sistem!";
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
        [Authorize(Roles = "Superadmin")]
        public IActionResult OpremaLista()
        {
            List<OpremaListaModel> OpremaListaKonacno = new List<OpremaListaModel>();

            

            MySqlConnection konekcija = new MySqlConnection(StringKonekcije());
            try
            {
                using (konekcija)
                {

                    konekcija.Open();
                    string upit = "SELECT oprema_id,oprema_marka,oprema_model,oprema_kategorija.oprema_kategorija_ime,\r\noprema_qr_kod,oprema_serijski_broj,oprema_datum_nabavke,oprema_cena,oprema_garancija,dostavljac.dostavljac_ime,sektor.sektor_ime,oprema_napomena FROM `oprema` \r\nINNER JOIN dostavljac on oprema_dostavljac_id = dostavljac.dostavljac_id\r\nINNER JOIN oprema_kategorija on oprema.oprema_kategorija_id = oprema_kategorija.oprema_kategorija_id\r\nINNER JOIN sektor on oprema_kategorija.oprema_kategorija_sektor_id = sektor.sektor_id\r\n\r\nWHERE oprema_otpisano = 0 and oprema_stanje = 0;";

                    MySqlCommand komanda = new MySqlCommand(upit, konekcija);

                    using (MySqlDataReader reader = komanda.ExecuteReader())
                    {
                        while (reader.Read())
                        {


                            OpremaListaModel model = new OpremaListaModel();


                            model.Id = Convert.ToInt32(reader["oprema_id"]);
                            model.Marka = Convert.ToString(reader["oprema_marka"]); ;
                            model.Model = Convert.ToString(reader["oprema_model"]); ;
                            model.KategorijaIme = Convert.ToString(reader["oprema_kategorija_ime"]); ;
                            model.QRKod = Convert.ToString(reader["oprema_qr_kod"]); ;
                            model.DatumNabavke = Convert.ToDateTime(reader["oprema_datum_nabavke"]); ;
                            model.Cena = Convert.ToString(reader["oprema_cena"]); ;
                            model.Garancija = Convert.ToDateTime(reader["oprema_garancija"]);
                            model.ImeDobavljaca = Convert.ToString(reader["dostavljac_ime"]);
                            model.Sektor = Convert.ToString(reader["sektor_ime"]);
                            model.SerijskiBroj = Convert.ToString(reader["oprema_serijski_broj"]);
                            model.Napomena = Convert.ToString(reader["oprema_napomena"]);



                            OpremaListaKonacno.Add(model);

                        }

                    }

                }
            }
            catch (Exception e)
            {
                ViewBag.Message = e.Message;
            }
            finally
            {

                konekcija.Close();
            }





            return View(OpremaListaKonacno);

        }
        

        [HttpGet]
        public IActionResult OpremaListaSektor()
        {
            List<OpremaListaModel> OpremaListaKonacno = new List<OpremaListaModel>();
            if (HttpContext.Session.GetInt32("UserSektorId") == null || HttpContext.Session.GetInt32("UserId") == null)
            {

                return RedirectToAction("Login", "Account");
            }

            int SektorUser = (int)HttpContext.Session.GetInt32("UserSektorId");


            MySqlConnection konekcija = new MySqlConnection(StringKonekcije());
            try
            {
                using (konekcija)
                {

                    konekcija.Open();
                    string upit = "SELECT oprema_id,oprema_marka,oprema_model,oprema_kategorija.oprema_kategorija_ime,oprema_qr_kod,oprema_serijski_broj,oprema_datum_nabavke,oprema_cena,oprema_garancija,dostavljac.dostavljac_ime,sektor.sektor_ime,oprema_napomena FROM `oprema` \r\nINNER JOIN dostavljac on oprema_dostavljac_id = dostavljac.dostavljac_id \r\nINNER JOIN oprema_kategorija on oprema.oprema_kategorija_id = oprema_kategorija.oprema_kategorija_id \r\nINNER JOIN sektor on oprema_kategorija.oprema_kategorija_sektor_id = sektor.sektor_id \r\nWHERE oprema_otpisano = 0 and oprema_stanje = 0 and oprema.oprema_kategorija_id in(SELECT oprema_kategorija.oprema_kategorija_id from oprema_kategorija WHERE oprema_kategorija.oprema_kategorija_sektor_id in (SELECT sektor.sektor_id FROM sektor WHERE sektor.sektor_id= "+SektorUser+") );";

                    MySqlCommand komanda = new MySqlCommand(upit, konekcija);

                    using (MySqlDataReader reader = komanda.ExecuteReader())
                    {
                        while (reader.Read())
                        {


                            OpremaListaModel model = new OpremaListaModel();


                            model.Id = Convert.ToInt32(reader["oprema_id"]);
                            model.Marka = Convert.ToString(reader["oprema_marka"]); ;
                            model.Model = Convert.ToString(reader["oprema_model"]); ;
                            model.KategorijaIme = Convert.ToString(reader["oprema_kategorija_ime"]); ;
                            model.QRKod = Convert.ToString(reader["oprema_qr_kod"]); ;
                            model.DatumNabavke = Convert.ToDateTime(reader["oprema_datum_nabavke"]); ;
                            model.Cena = Convert.ToString(reader["oprema_cena"]); ;
                            model.Garancija = Convert.ToDateTime(reader["oprema_garancija"]);
                            model.ImeDobavljaca = Convert.ToString(reader["dostavljac_ime"]);
                            model.Sektor = Convert.ToString(reader["sektor_ime"]);
                            model.SerijskiBroj = Convert.ToString(reader["oprema_serijski_broj"]);
                            model.Napomena = Convert.ToString(reader["oprema_napomena"]);

                            OpremaListaKonacno.Add(model);

                        }

                    }

                }
            }
            catch (Exception e)
            {
                ViewBag.Message = e.Message;
            }
            finally
            {

                konekcija.Close();
            }





            return View(OpremaListaKonacno);

        }


        [HttpGet]
        
        [Authorize(Roles = ("Superadmin"))]
        public IActionResult OpremaListaSvi()
        {
            List<OpremaListaSviModel> OpremaListaKonacno = new List<OpremaListaSviModel>();




            MySqlConnection konekcija = new MySqlConnection(StringKonekcije());
            try
            {
                using (konekcija)
                {

                    konekcija.Open();
                    string upit = "SELECT oprema_id,oprema_marka,oprema_model,oprema_kategorija.oprema_kategorija_ime,\r\noprema_qr_kod,oprema_serijski_broj,oprema_datum_nabavke,oprema_cena,oprema_garancija,dostavljac.dostavljac_ime,sektor.sektor_ime,oprema_napomena FROM `oprema` \r\nINNER JOIN dostavljac on oprema_dostavljac_id = dostavljac.dostavljac_id\r\nINNER JOIN oprema_kategorija on oprema.oprema_kategorija_id = oprema_kategorija.oprema_kategorija_id\r\nINNER JOIN sektor on oprema_kategorija.oprema_kategorija_sektor_id = sektor.sektor_id\r\n\r\nWHERE oprema_otpisano = 0;";

                    MySqlCommand komanda = new MySqlCommand(upit, konekcija);

                    using (MySqlDataReader reader = komanda.ExecuteReader())
                    {
                        while (reader.Read())
                        {


                            OpremaListaSviModel model = new OpremaListaSviModel();


                            model.Id = Convert.ToInt32(reader["oprema_id"]);
                            model.Marka = Convert.ToString(reader["oprema_marka"]); ;
                            model.Model = Convert.ToString(reader["oprema_model"]); ;
                            model.KategorijaIme = Convert.ToString(reader["oprema_kategorija_ime"]); ;
                            model.QRKod = Convert.ToString(reader["oprema_qr_kod"]); ;
                            model.DatumNabavke = Convert.ToDateTime(reader["oprema_datum_nabavke"]); ;
                            model.Cena = Convert.ToString(reader["oprema_cena"]); ;
                            model.Garancija = Convert.ToDateTime(reader["oprema_garancija"]);
                            model.ImeDobavljaca = Convert.ToString(reader["dostavljac_ime"]);
                            model.Sektor = Convert.ToString(reader["sektor_ime"]);
                            model.SerijskiBroj = Convert.ToString(reader["oprema_serijski_broj"]);
                            model.Napomena = Convert.ToString(reader["oprema_napomena"]);



                            OpremaListaKonacno.Add(model);

                        }

                    }

                }
            }
            catch (Exception e)
            {
                ViewBag.Message = e.Message;
            }
            finally
            {

                konekcija.Close();
            }

            return View(OpremaListaKonacno);

        }


        [HttpGet]
        public IActionResult Servis()
        {
            List<servis> servisLista = new List<servis>();
            List<OpremaServisDDModel> opremaDD = new List<OpremaServisDDModel>();
            List<OpremaServisDDModel> opremaDD1 = new List<OpremaServisDDModel>();

            

            if (HttpContext.Session.GetInt32("UserSektorId") == null || HttpContext.Session.GetInt32("UserId") == null)
            {

                return RedirectToAction("Login", "Account");
            }

            int SektorUser = (int)HttpContext.Session.GetInt32("UserSektorId");
            int idUser = (int)HttpContext.Session.GetInt32("UserId");
            //int servisId = 30; //id servisa u tabeli relokacija
            servisLista = (from servis1 in _dropDownModel.servis.Where(x => x.servis_sektor_id == SektorUser) select servis1).ToList();
            ViewBag.servis = servisLista;


            MySqlConnection konekcija = new MySqlConnection(StringKonekcije());



            try
            {
                using (konekcija)
                {

                    konekcija.Open();

                    var UpitZaProveruSektora = _dropDownModel.sektor.Where(x => x.sektor_id == SektorUser).FirstOrDefault();

                    if (UpitZaProveruSektora == null)
                    {
                        return RedirectToAction("Login", "Account");
                    }
                    string upit = "SELECT oprema.oprema_id,oprema.oprema_marka,oprema.oprema_model,oprema.oprema_qr_kod FROM oprema \r\n\r\nWHERE   oprema.oprema_otpisano = 0 and oprema.oprema_kategorija_id IN (SELECT oprema_kategorija.oprema_kategorija_id from oprema_kategorija WHERE oprema_kategorija.oprema_kategorija_sektor_id = " + SektorUser + ")";


                    if (UpitZaProveruSektora.sektor_bitan > 0)
                    {
                        upit = "SELECT oprema.oprema_id,oprema.oprema_marka,oprema.oprema_model,oprema.oprema_qr_kod \r\nFROM oprema \r\nWHERE oprema.oprema_otpisano = 0";
                    }


                    MySqlCommand komanda = new MySqlCommand(upit, konekcija);

                    using (MySqlDataReader reader = komanda.ExecuteReader())
                    {
                        while (reader.Read())
                        {


                            OpremaServisDDModel model1 = new OpremaServisDDModel();
                            model1.OpremaId = Convert.ToInt32(reader["oprema_id"]);
                            model1.Marka = Convert.ToString(reader["oprema_marka"]) + " " + Convert.ToString(reader["oprema_model"]) + " qr kod:" + Convert.ToString(reader["oprema_qr_kod"]);




                            opremaDD.Add(model1);

                        }
                    }

                    string upit1 = "SELECT relokacija_oprema_id  FROM relokacija  \r\n\r\nINNER JOIN aspnetusers    ON relokacija.relokacija_korisnik_do_koga_id = aspnetusers.Id \r\nINNER JOIN oprema  ON relokacija.relokacija_oprema_id = oprema.oprema_id \r\nINNER JOIN oprema_kategorija on oprema.oprema_kategorija_id = oprema_kategorija.oprema_kategorija_id\r\nINNER JOIN sektor on oprema_kategorija.oprema_kategorija_sektor_id = sektor.sektor_id\r\nWHERE relokacija_id in (SELECT MAX(relokacija_id) FROM relokacija GROUP BY relokacija_oprema_id)  and oprema.oprema_otpisano = 0 and oprema.oprema_stanje > 0 and aspnetusers.OtpisanRadnik = 0 and  relokacija_korisnik_do_koga_id = 30   GROUP BY relokacija_oprema_id;";

                    MySqlCommand komanda1 = new MySqlCommand(upit1, konekcija);

                    using (MySqlDataReader reader1 = komanda1.ExecuteReader())
                    {
                        while (reader1.Read())
                        {


                            OpremaServisDDModel model123 = new OpremaServisDDModel();
                            model123.OpremaId = Convert.ToInt32(reader1["relokacija_oprema_id"]);




                            opremaDD1.Add(model123);

                        }



                    }
                }
            }
            catch (Exception e)
            {
                ViewBag.Message = e.Message;
            }
            finally
            {

                konekcija.Close();
            }

            foreach (var item in opremaDD1.ToList())
            {
                foreach (var oprema in opremaDD.ToList())
                {
                    if (item.OpremaId == oprema.OpremaId)
                    {

                        opremaDD.Remove(oprema);
                    }
                }

            }


            ViewBag.oprema = opremaDD;





            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Servis(NaServisCreateModel model)
        {
            List<servis> servisLista = new List<servis>();
            List<OpremaServisDDModel> opremaDD = new List<OpremaServisDDModel>();
            List<OpremaServisDDModel> opremaDD1 = new List<OpremaServisDDModel>();
           

            if (HttpContext.Session.GetInt32("UserSektorId") == null || HttpContext.Session.GetInt32("UserId") == null)
            {

                return RedirectToAction("Login", "Account");
            }

            int SektorUser = (int)HttpContext.Session.GetInt32("UserSektorId");
            int idUser = (int)HttpContext.Session.GetInt32("UserId");

            //int servisId = 30; //id servisa u tabeli relokacija
            servisLista = (from servis1 in _dropDownModel.servis.Where(x=>x.servis_sektor_id == SektorUser) select servis1).ToList();
            ViewBag.servis = servisLista;


            MySqlConnection konekcija = new MySqlConnection("Server=127.0.0.1;Port=3306;Database=persuproba;Uid=root;Pwd='';convert zero datetime=True;");


            try
            {
                using (konekcija)
                {

                    konekcija.Open();

                    var UpitZaProveruSektora = _dropDownModel.sektor.Where(x => x.sektor_id == SektorUser).FirstOrDefault();

                    if (UpitZaProveruSektora == null)
                    {
                        return RedirectToAction("Login", "Account");
                    }
                    string upit = "SELECT oprema.oprema_id,oprema.oprema_marka,oprema.oprema_model,oprema.oprema_qr_kod FROM oprema \r\n\r\nWHERE   oprema.oprema_otpisano = 0 and oprema.oprema_kategorija_id IN (SELECT oprema_kategorija.oprema_kategorija_id from oprema_kategorija WHERE oprema_kategorija.oprema_kategorija_sektor_id = " + SektorUser + ")";


                    if (UpitZaProveruSektora.sektor_bitan > 0)
                    {
                        upit = "SELECT oprema.oprema_id,oprema.oprema_marka,oprema.oprema_model,oprema.oprema_qr_kod \r\nFROM oprema \r\nWHERE oprema.oprema_otpisano = 0";
                    }



                    MySqlCommand komanda = new MySqlCommand(upit, konekcija);

                    using (MySqlDataReader reader = komanda.ExecuteReader())
                    {
                        while (reader.Read())
                        {


                            OpremaServisDDModel model1 = new OpremaServisDDModel();
                            model1.OpremaId = Convert.ToInt32(reader["oprema_id"]);
                            model1.Marka = Convert.ToString(reader["oprema_marka"]) + " " + Convert.ToString(reader["oprema_model"]) + " qr kod:" + Convert.ToString(reader["oprema_qr_kod"]);




                            opremaDD.Add(model1);

                        }
                    }

                    string upit1 = "SELECT relokacija_oprema_id  FROM relokacija  \r\n\r\nINNER JOIN aspnetusers    ON relokacija.relokacija_korisnik_do_koga_id = aspnetusers.Id \r\nINNER JOIN oprema  ON relokacija.relokacija_oprema_id = oprema.oprema_id \r\nINNER JOIN oprema_kategorija on oprema.oprema_kategorija_id = oprema_kategorija.oprema_kategorija_id\r\nINNER JOIN sektor on oprema_kategorija.oprema_kategorija_sektor_id = sektor.sektor_id\r\nWHERE relokacija_id in (SELECT MAX(relokacija_id) FROM relokacija GROUP BY relokacija_oprema_id)  and oprema.oprema_otpisano = 0 and oprema.oprema_stanje > 0 and aspnetusers.OtpisanRadnik = 0 and  relokacija_korisnik_do_koga_id = 30   GROUP BY relokacija_oprema_id;";

                    MySqlCommand komanda1 = new MySqlCommand(upit1, konekcija);

                    using (MySqlDataReader reader1 = komanda1.ExecuteReader())
                    {
                        while (reader1.Read())
                        {


                            OpremaServisDDModel model123 = new OpremaServisDDModel();
                            model123.OpremaId = Convert.ToInt32(reader1["relokacija_oprema_id"]);


                            opremaDD1.Add(model123);

                        }



                    }
                }
            }
            catch (Exception e)
            {
                ViewBag.Message = e.Message;
            }
            finally
            {

                konekcija.Close();
            }


            foreach (var item in opremaDD1.ToList())
            {
                foreach (var oprema in opremaDD.ToList())
                {
                    if (item.OpremaId == oprema.OpremaId)
                    {

                        opremaDD.Remove(oprema);
                    }
                }

            }

            ViewBag.oprema = opremaDD;




            if (ModelState.IsValid)
            {
                using var transaction = _dropDownModel.Database.BeginTransaction();
                try
                {
                    int servis = 30;
                    na_servis model1 = new na_servis
                    {

                        na_servis_oprema_id = model.na_servis_oprema_id,
                        na_servis_datum = model.na_servis_datum.Date,
                        na_servis_servis_id = model.na_servis_servis_id,
                        na_servis_korisnik_id = idUser,
                        na_servis_napomena = model.na_servis_napomena,
                        na_servis_opis_kvara_pre = model.na_servis_opis_kvara_pre


                    };

                    //ladno prosla transakcija
                    this._dropDownModel.na_servis.Add(model1);



                    var commandText = "UPDATE oprema SET  oprema_stanje = 1  WHERE oprema_id = @oprema_id";
                    var updateOprema = new MySqlParameter("@oprema_id", model1.na_servis_oprema_id);

                    var upitRelokacijaNaServisText = "INSERT INTO relokacija(relokacija_korisnik_od_koga_id,relokacija_korisnik_do_koga_id,relokacija_datum,relokacija_oprema_id) VALUES(@relokacija_korisnik_od_koga_id,@relokacija_korisnik_do_koga_id,@relokacija_datum,@relokacija_oprema_id)";
                    var odKoga = new MySqlParameter("@relokacija_korisnik_od_koga_id", model1.na_servis_korisnik_id);
                    var doKoga = new MySqlParameter("@relokacija_korisnik_do_koga_id", servis);
                    var datum = new MySqlParameter("@relokacija_datum", model1.na_servis_datum.Date);
                    var oprema = new MySqlParameter("@relokacija_oprema_id", model1.na_servis_oprema_id);

                    _dropDownModel.Database.ExecuteSqlRaw(commandText, updateOprema);
                    _dropDownModel.Database.ExecuteSqlRaw(upitRelokacijaNaServisText, new[] { odKoga, doKoga, datum, oprema });
                    _dropDownModel.SaveChanges();

                    transaction.Commit();
                    ViewBag.Message = "Uspesno uneta oprema na servis!";
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
        public IActionResult ServisLista()
        {

            List<NaServisListaZaSektor> lista = new List<NaServisListaZaSektor>();

            if (HttpContext.Session.GetInt32("UserSektorId") == null)
            {

                return RedirectToAction("Login", "Account");
            }

            int SektorUser = (int)HttpContext.Session.GetInt32("UserSektorId");


            MySqlConnection konekcija = new MySqlConnection(StringKonekcije());
            try
            {
                using (konekcija)
                {

                    konekcija.Open();


                    var UpitZaProveruSektora = _dropDownModel.sektor.Where(x => x.sektor_id == SektorUser).FirstOrDefault();

                    if (UpitZaProveruSektora == null)
                    {
                        return RedirectToAction("Login", "Account");
                    }

                    string upit = "SELECT na_servis.na_servis_id,na_servis.na_servis_napomena,na_servis_opis_kvara_pre,aspnetusers.Ime,aspnetusers.Prezime,aspnetusers.UserName,oprema.oprema_marka,oprema.oprema_qr_kod,oprema.oprema_model,na_servis.na_servis_datum FROM `na_servis` INNER JOIN oprema ON na_servis.na_servis_oprema_id = oprema.oprema_id\r\nINNER JOIN aspnetusers ON na_servis.na_servis_korisnik_id = aspnetusers.Id where na_servis.na_servis_oprema_id IN (SELECT oprema.oprema_id from oprema WHERE oprema.oprema_kategorija_id IN (SELECT oprema_kategorija.oprema_kategorija_id from oprema_kategorija WHERE oprema_kategorija_sektor_id = " + SektorUser + " ) ) and na_servis_pokupljeno = 0 and oprema.oprema_otpisano = 0 and oprema.oprema_stanje > 0";


                    if (UpitZaProveruSektora.sektor_bitan > 0)
                    {
                        upit = "SELECT na_servis.na_servis_id,na_servis.na_servis_napomena,na_servis_opis_kvara_pre,aspnetusers.Ime,aspnetusers.Prezime,aspnetusers.UserName,oprema.oprema_marka,oprema.oprema_qr_kod,oprema.oprema_model,na_servis.na_servis_datum FROM `na_servis` INNER JOIN oprema ON na_servis.na_servis_oprema_id = oprema.oprema_id \r\nINNER JOIN aspnetusers ON na_servis.na_servis_korisnik_id = aspnetusers.Id where na_servis.na_servis_oprema_id IN (SELECT oprema.oprema_id from oprema WHERE oprema.oprema_kategorija_id IN (SELECT oprema_kategorija.oprema_kategorija_id from oprema_kategorija ) ) and na_servis_pokupljeno = 0 and oprema.oprema_otpisano = 0 and oprema.oprema_stanje > 0";
                    }





                    MySqlCommand komanda = new MySqlCommand(upit, konekcija);

                    using (MySqlDataReader reader = komanda.ExecuteReader())
                    {
                        while (reader.Read())
                        {


                            NaServisListaZaSektor model = new NaServisListaZaSektor();
                            model.Id = Convert.ToInt32(reader["na_servis_id"]);
                            model.Ime = Convert.ToString(reader["Ime"]);
                            model.Prezime = Convert.ToString(reader["Prezime"]);
                            model.Marka = Convert.ToString(reader["oprema_marka"]);
                            model.Model = Convert.ToString(reader["oprema_model"]);
                            model.Datum = Convert.ToDateTime(reader["na_servis_datum"]);
                            model.UserName = Convert.ToString(reader["UserName"]);
                            model.Napomena = Convert.ToString(reader["na_servis_napomena"]);
                            model.OpisKvaraPre = Convert.ToString(reader["na_servis_opis_kvara_pre"]);
                            model.QR = Convert.ToString(reader["oprema_qr_kod"]);



                            lista.Add(model);

                        }

                    }

                }
            }
            catch (Exception e)
            {
                ViewBag.Message = e.Message;
            }
            finally
            {

                konekcija.Close();
            }

            return View(lista);
        }



        [HttpGet]
        public IActionResult Pokupi(int? id)
        {
            if (id == 0)
            {
                RedirectToAction("ServisLista", "Home");

            }

            na_servis model = new na_servis();
            NaServisUpdate update = new NaServisUpdate();
            MySqlConnection konekcija = new MySqlConnection(StringKonekcije());
            try
            {
                using (konekcija)
                {

                    konekcija.Open();
                    string upit = "SELECT * FROM na_servis WHERE na_servis_id = " + id + "";

                    MySqlCommand komanda = new MySqlCommand(upit, konekcija);

                    using (MySqlDataReader reader = komanda.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            if (reader["na_servis_id"] != System.DBNull.Value)
                                model.na_servis_id = Convert.ToInt32(reader["na_servis_id"]);


                            if (reader["na_servis_datum"] != System.DBNull.Value)
                                model.na_servis_datum = Convert.ToDateTime(reader["na_servis_datum"]).Date;

                            if (reader["na_servis_korisnik_id"] != System.DBNull.Value)
                                model.na_servis_korisnik_id = Convert.ToInt32(reader["na_servis_korisnik_id"]);

                            if (reader["na_servis_oprema_id"] != System.DBNull.Value)
                                model.na_servis_oprema_id = Convert.ToInt32(reader["na_servis_oprema_id"]);

                            if (reader["na_servis_napomena"] != System.DBNull.Value)
                                model.na_servis_napomena = Convert.ToString(reader["na_servis_napomena"]);

                            if (reader["na_servis_pokupio_korisnik_id"] != System.DBNull.Value)
                                model.na_servis_pokupio_korisnik_id = Convert.ToInt32(reader["na_servis_pokupio_korisnik_id"]);


                            if (reader["na_servis_pokupljeno"] != System.DBNull.Value)
                                model.na_servis_pokupljeno = Convert.ToInt32(reader["na_servis_pokupljeno"]);

                            if (reader["na_servis_opis_kvara"] != System.DBNull.Value)
                                model.na_servis_opis_kvara = Convert.ToString(reader["na_servis_opis_kvara"]);

                            if (reader["na_servis_datum_pokupljeno"] != System.DBNull.Value)
                                model.na_servis_datum_pokupljeno = Convert.ToDateTime(reader["na_servis_datum_pokupljeno"]).Date;

                        }

                    }

                }
            }
            catch (Exception e)
            {
                ViewBag.Message = e.Message;
            }
            finally
            {

                konekcija.Close();
            }

            update.OpisKvara = model.na_servis_opis_kvara;
            update.PokupljenoDatum = model.na_servis_datum_pokupljeno;
            update.Napomena = model.na_servis_napomena_posle;


            return View(update);
        }


        [HttpPost]
        public IActionResult Pokupi(NaServisUpdate model, int id)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {

                return RedirectToAction("Login", "Account");
            }
            int IdUser = (int)HttpContext.Session.GetInt32("UserId");

            if (ModelState.IsValid)
            {

                using var transaction = _dropDownModel.Database.BeginTransaction();
                try
                {

                    var commandText = "UPDATE na_servis SET  na_servis_opis_kvara =@opisKvara,na_servis_datum_pokupljeno =@datum ,na_servis_pokupljeno = 1,na_servis_pokupio_korisnik_id = @User,na_servis_napomena_posle = @Napomena WHERE na_servis_id = @id";
                    var kvar = new MySqlParameter("@opisKvara", model.OpisKvara);
                    var datum = new MySqlParameter("@datum", model.PokupljenoDatum);
                    var user = new MySqlParameter("@User", IdUser);
                    var id1 = new MySqlParameter("@Id", id);
                    var napomena = new MySqlParameter("@napomena", model.Napomena);

                    _dropDownModel.Database.ExecuteSqlRaw(commandText, new[] { kvar, datum, user, id1, napomena });


                    int id123 = (int)_dropDownModel
      .na_servis
      .Where(u => u.na_servis_id == id)
      .Select(u => u.na_servis_oprema_id)
      .SingleOrDefault();

                    var sql = "UPDATE oprema SET  oprema_stanje = 0  WHERE oprema_id = @OpremaTrans";
                    var opremaID = new MySqlParameter("@OpremaTrans", id123);

                    _dropDownModel.Database.ExecuteSqlRaw(sql, opremaID);
                    _dropDownModel.SaveChanges();

                    transaction.Commit();
                    ViewBag.Message = "Uspesno ste pokupili opremu sa servisa!";
                }
                catch (Exception e)
                {
                    ViewBag.Message = e.Message;
                }
            }

            return View();
        }


        [Authorize(Roles = ("Superadmin"))]
        public IActionResult ServisListaSvi()
        {
            List<NaServisListaUkupno> lista = new List<NaServisListaUkupno>();



            MySqlConnection konekcija = new MySqlConnection(StringKonekcije());
            try
            {
                using (konekcija)
                {

                    konekcija.Open();
                    string upit = "SELECT na_servis.na_servis_id,na_servis.na_servis_napomena,na_servis_opis_kvara_pre,aspnetusers.Ime,aspnetusers.Prezime,aspnetusers.UserName,oprema.oprema_marka,oprema.oprema_qr_kod,oprema.oprema_model,na_servis.na_servis_datum,oprema_kategorija.oprema_kategorija_ime,sektor.sektor_ime\r\n\r\n\r\nFROM `na_servis`\r\nINNER JOIN oprema ON na_servis.na_servis_oprema_id = oprema.oprema_id\r\nINNER join oprema_kategorija ON oprema.oprema_kategorija_id = oprema_kategorija.oprema_kategorija_id\r\nINNER JOIN sektor on oprema_kategorija.oprema_kategorija_sektor_id = sektor.sektor_id\r\nINNER JOIN aspnetusers on na_servis_korisnik_id = aspnetusers.Id\r\n\r\nWHERE \r\nna_servis_pokupljeno = 0 and oprema.oprema_otpisano = 0 and oprema.oprema_stanje > 0";

                    MySqlCommand komanda = new MySqlCommand(upit, konekcija);

                    using (MySqlDataReader reader = komanda.ExecuteReader())
                    {
                        while (reader.Read())
                        {


                            NaServisListaUkupno model = new NaServisListaUkupno();
                            model.Id = Convert.ToInt32(reader["na_servis_id"]);
                            model.Ime = Convert.ToString(reader["Ime"]);
                            model.Prezime = Convert.ToString(reader["Prezime"]);
                            model.UserName = Convert.ToString(reader["UserName"]);
                            model.Marka = Convert.ToString(reader["oprema_marka"]);
                            model.Model = Convert.ToString(reader["oprema_model"]);
                            model.Datum = Convert.ToDateTime(reader["na_servis_datum"]);
                            model.Kategorija = Convert.ToString(reader["oprema_kategorija_ime"]);
                            model.Sektor = Convert.ToString(reader["sektor_ime"]);
                            model.Napomena = Convert.ToString(reader["na_servis_napomena"]);
                            model.OpisKvaraPre = Convert.ToString(reader["na_servis_opis_kvara_pre"]);
                            model.QR = Convert.ToString(reader["oprema_qr_kod"]);



                            lista.Add(model);

                        }

                    }

                }
            }
            catch (Exception e)
            {
                ViewBag.Message = e.Message;
            }
            finally
            {

                konekcija.Close();
            }



            return View(lista);
        }

        [Authorize(Roles = ("Superadmin"))]
        public IActionResult ServisListaIstorija()
        {
            List<ServisIstorijaModel> lista = new List<ServisIstorijaModel>();



            MySqlConnection konekcija = new MySqlConnection(StringKonekcije());
            try
            {
                using (konekcija)
                {

                    konekcija.Open();
                    //ovde mozda izbaciti oprema otpisano =0 i oprema stanje > 0,jer mozda bi bilo dobro celu istoriju imati
                    string upit = "SELECT na_servis.na_servis_id,na_servis.na_servis_napomena,na_servis.na_servis_datum_pokupljeno,na_servis.na_servis_napomena_posle,na_servis.na_servis_opis_kvara_pre,na_servis.na_servis_opis_kvara,servis.servis_ime,oprema.oprema_marka,oprema.oprema_qr_kod,oprema.oprema_model,na_servis.na_servis_datum,oprema_kategorija.oprema_kategorija_ime,sektor.sektor_ime,k1.UserName AS Pokupio,k2.Username AS Predao \r\nFROM `na_servis`\r\nINNER JOIN aspnetusers as k1 on na_servis.na_servis_pokupio_korisnik_id = k1.Id\r\nINNER JOIN servis ON na_servis.na_servis_servis_id = servis.servis_id \r\nINNER JOIN oprema ON na_servis.na_servis_oprema_id = oprema.oprema_id \r\nINNER join oprema_kategorija ON oprema.oprema_kategorija_id = oprema_kategorija.oprema_kategorija_id\r\nINNER JOIN sektor on oprema_kategorija.oprema_kategorija_sektor_id = sektor.sektor_id \r\nINNER JOIN aspnetusers as k2 on na_servis_korisnik_id = k2.Id \r\n\r\nWHERE na_servis_pokupljeno = 1 and oprema.oprema_otpisano = 0";

                    MySqlCommand komanda = new MySqlCommand(upit, konekcija);

                    using (MySqlDataReader reader = komanda.ExecuteReader())
                    {
                        while (reader.Read())
                        {


                            ServisIstorijaModel model = new ServisIstorijaModel();
                            model.Id = Convert.ToInt32(reader["na_servis_id"]);
                            model.UserName = Convert.ToString(reader["Predao"]);
                            model.Marka = Convert.ToString(reader["oprema_marka"]);
                            model.Model = Convert.ToString(reader["oprema_model"]);
                            model.Datum = Convert.ToDateTime(reader["na_servis_datum"]);
                            model.Kategorija = Convert.ToString(reader["oprema_kategorija_ime"]);
                            model.Sektor = Convert.ToString(reader["sektor_ime"]);
                            model.Napomena = Convert.ToString(reader["na_servis_napomena"]);
                            model.NapomenaPosle = Convert.ToString(reader["na_servis_napomena_posle"]);
                            model.OpisKvara = Convert.ToString(reader["na_servis_opis_kvara"]);
                            model.OpisKvarapre = Convert.ToString(reader["na_servis_opis_kvara_pre"]);
                            model.Datumpokupio = Convert.ToDateTime(reader["na_servis_datum_pokupljeno"]);
                            model.Servis = Convert.ToString(reader["servis_ime"]);
                            model.PokupioKorisnickoIme = Convert.ToString(reader["Pokupio"]);
                            model.QR = Convert.ToString(reader["oprema_qr_kod"]);



                            lista.Add(model);

                        }

                    }

                }
            }
            catch (Exception e)
            {
                ViewBag.Message = e.Message;
            }
            finally
            {

                konekcija.Close();
            }



            return View(lista);
        }



        public IActionResult Relokacija()
        {
            List<KorisnikRelokacijaModel> korisnikListaKonacno = new List<KorisnikRelokacijaModel>();
            IList<lokacija> lokacijaLista = new List<lokacija>();
            List<OpremaServisDDModel> opremaDD = new List<OpremaServisDDModel>();
            List<OpremaServisDDModel> opremaDD1 = new List<OpremaServisDDModel>();
            lokacijaLista = (from servis1 in _dropDownModel.lokacija select servis1).ToList();


            if (HttpContext.Session.GetInt32("UserSektorId") == null || HttpContext.Session.GetInt32("UserId") == null)
            {

                return RedirectToAction("Login", "Account");
            }

            int SektorUser = (int)HttpContext.Session.GetInt32("UserSektorId");
            int idUser = (int)HttpContext.Session.GetInt32("UserId");
            //int ServisId = 30;

            MySqlConnection konekcija = new MySqlConnection(StringKonekcije());


            try
            {
                using (konekcija)
                {

                    konekcija.Open();
                    var UpitZaProveruSektora = _dropDownModel.sektor.Where(x => x.sektor_id == SektorUser).FirstOrDefault();

                    if (UpitZaProveruSektora == null)
                    {
                        return RedirectToAction("Login", "Account");
                    }
                    string upit = "SELECT oprema.oprema_id,oprema.oprema_marka,oprema.oprema_model,oprema.oprema_qr_kod FROM oprema \r\nWHERE   oprema.oprema_otpisano = 0 and oprema.oprema_kategorija_id IN (SELECT oprema_kategorija.oprema_kategorija_id from oprema_kategorija WHERE oprema_kategorija.oprema_kategorija_sektor_id = "+SektorUser+")";


                    if (UpitZaProveruSektora.sektor_bitan > 0)
                    {
                        upit = "SELECT oprema.oprema_id,oprema.oprema_marka,oprema.oprema_model,oprema.oprema_qr_kod \r\nFROM oprema \r\nWHERE oprema.oprema_otpisano = 0";
                    }

                    MySqlCommand komanda = new MySqlCommand(upit, konekcija);

                    using (MySqlDataReader reader = komanda.ExecuteReader())
                    {
                        while (reader.Read())
                        {


                            OpremaServisDDModel model1 = new OpremaServisDDModel();
                            model1.OpremaId = Convert.ToInt32(reader["oprema_id"]);
                            model1.Marka = Convert.ToString(reader["oprema_marka"]) + " " + Convert.ToString(reader["oprema_model"]) + " qr kod:" + Convert.ToString(reader["oprema_qr_kod"]);




                            opremaDD.Add(model1);

                        }
                    }

                    string upit1 = "SELECT relokacija_oprema_id  FROM relokacija  \r\n\r\nINNER JOIN aspnetusers    ON relokacija.relokacija_korisnik_do_koga_id = aspnetusers.Id \r\nINNER JOIN oprema  ON relokacija.relokacija_oprema_id = oprema.oprema_id \r\nINNER JOIN oprema_kategorija on oprema.oprema_kategorija_id = oprema_kategorija.oprema_kategorija_id\r\nINNER JOIN sektor on oprema_kategorija.oprema_kategorija_sektor_id = sektor.sektor_id\r\nWHERE relokacija_id in (SELECT MAX(relokacija_id) FROM relokacija GROUP BY relokacija_oprema_id)  and oprema.oprema_otpisano = 0 and oprema.oprema_stanje > 0 and aspnetusers.OtpisanRadnik = 0 and  relokacija_korisnik_do_koga_id = 30   GROUP BY relokacija_oprema_id;";

                    MySqlCommand komanda1 = new MySqlCommand(upit1, konekcija);

                    using (MySqlDataReader reader1 = komanda1.ExecuteReader())
                    {
                        while (reader1.Read())
                        {


                            OpremaServisDDModel model123 = new OpremaServisDDModel();
                            model123.OpremaId = Convert.ToInt32(reader1["relokacija_oprema_id"]);

                            opremaDD1.Add(model123);

                        }

                    }
                }
            }
            catch (Exception e)
            {
                ViewBag.Message = e.Message;
            }
            finally
            {

                konekcija.Close();
            }

            foreach (var item in opremaDD1.ToList())
            {
                foreach (var oprema in opremaDD.ToList())
                {
                    if (item.OpremaId == oprema.OpremaId)
                    {

                        opremaDD.Remove(oprema);
                    }
                }

            }

            ViewBag.oprema = opremaDD;


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

                        };

            foreach (var item in query)
            {
                KorisnikRelokacijaModel model = new KorisnikRelokacijaModel();

                model.Ime = item.Ime + " " + item.Prezime + ", Korisnicko Ime: " + item.UserName;
                model.Id = item.Id;

                korisnikListaKonacno.Add(model);

            }
            ViewBag.Korisnik = korisnikListaKonacno;
            ViewBag.Lokacija = lokacijaLista;
            ViewBag.oprema = opremaDD;



            return View();
        }

        [HttpPost]
        public IActionResult Relokacija(RelokacijaModel model)
        {


            if (HttpContext.Session.GetInt32("UserSektorId") == null || HttpContext.Session.GetInt32("UserId") == null)
            {

                return RedirectToAction("Login", "Account");
            }
            List<OpremaServisDDModel> opremaDD = new List<OpremaServisDDModel>();
            List<OpremaServisDDModel> opremaDD1 = new List<OpremaServisDDModel>();
            int SektorUser = (int)HttpContext.Session.GetInt32("UserSektorId");
            int idUser = (int)HttpContext.Session.GetInt32("UserId");
            //int ServisId = 30;
            List<KorisnikRelokacijaModel> korisnikListaKonacno = new List<KorisnikRelokacijaModel>();
            IList<lokacija> lokacijaLista = new List<lokacija>();



            lokacijaLista = (from servis1 in _dropDownModel.lokacija select servis1).ToList();

            MySqlConnection konekcija = new MySqlConnection(StringKonekcije());


            try
            {
                using (konekcija)
                {

                    konekcija.Open();

                    var UpitZaProveruSektora = _dropDownModel.sektor.Where(x => x.sektor_id == SektorUser).FirstOrDefault();

                    if(UpitZaProveruSektora == null)
                    {
                        return RedirectToAction("Login", "Account");
                    }
                    string upit = "SELECT oprema.oprema_id,oprema.oprema_marka,oprema.oprema_model,oprema.oprema_qr_kod FROM oprema \r\nWHERE   oprema.oprema_otpisano = 0 and oprema.oprema_kategorija_id IN (SELECT oprema_kategorija.oprema_kategorija_id from oprema_kategorija WHERE oprema_kategorija.oprema_kategorija_sektor_id = "+SektorUser+")";


                    if (UpitZaProveruSektora.sektor_bitan > 0)
                    {
                         upit = "SELECT oprema.oprema_id,oprema.oprema_marka,oprema.oprema_model,oprema.oprema_qr_kod \r\nFROM oprema \r\nWHERE oprema.oprema_otpisano = 0";
                    }
                    
                    
                    





                    MySqlCommand komanda = new MySqlCommand(upit, konekcija);

                    using (MySqlDataReader reader = komanda.ExecuteReader())
                    {
                        while (reader.Read())
                        {


                            OpremaServisDDModel model1 = new OpremaServisDDModel();
                            model1.OpremaId = Convert.ToInt32(reader["oprema_id"]);
                            model1.Marka = Convert.ToString(reader["oprema_marka"]) + " " + Convert.ToString(reader["oprema_model"]) + " qr kod:" + Convert.ToString(reader["oprema_qr_kod"]);




                            opremaDD.Add(model1);

                        }
                    }

                    string upit1 = "SELECT relokacija_oprema_id  FROM relokacija  \r\n\r\nINNER JOIN aspnetusers    ON relokacija.relokacija_korisnik_do_koga_id = aspnetusers.Id \r\nINNER JOIN oprema  ON relokacija.relokacija_oprema_id = oprema.oprema_id \r\nINNER JOIN oprema_kategorija on oprema.oprema_kategorija_id = oprema_kategorija.oprema_kategorija_id\r\nINNER JOIN sektor on oprema_kategorija.oprema_kategorija_sektor_id = sektor.sektor_id\r\nWHERE relokacija_id in (SELECT MAX(relokacija_id) FROM relokacija GROUP BY relokacija_oprema_id)  and oprema.oprema_otpisano = 0 and oprema.oprema_stanje > 0 and aspnetusers.OtpisanRadnik = 0 and  relokacija_korisnik_do_koga_id = 30   GROUP BY relokacija_oprema_id;";

                    MySqlCommand komanda1 = new MySqlCommand(upit1, konekcija);

                    using (MySqlDataReader reader1 = komanda1.ExecuteReader())
                    {
                        while (reader1.Read())
                        {


                            OpremaServisDDModel model123 = new OpremaServisDDModel();
                            model123.OpremaId = Convert.ToInt32(reader1["relokacija_oprema_id"]);

                            opremaDD1.Add(model123);

                        }



                    }
                }


            }
            catch (Exception e)
            {
                ViewBag.Message = e.Message;
            }
            finally
            {

                konekcija.Close();
            }

            foreach (var item in opremaDD1.ToList())
            {
                foreach (var oprema in opremaDD.ToList())
                {
                    if (item.OpremaId == oprema.OpremaId)
                    {

                        opremaDD.Remove(oprema);
                    }
                }

            }




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

                        };

            foreach (var item in query)
            {
                KorisnikRelokacijaModel model1 = new KorisnikRelokacijaModel();

                model1.Ime = item.Ime + " " + item.Prezime + ", Korisnicko Ime: " + item.UserName;
                model1.Id = item.Id;

                korisnikListaKonacno.Add(model1);

            }
            ViewBag.Korisnik = korisnikListaKonacno;
            ViewBag.Lokacija = lokacijaLista;
            ViewBag.oprema = opremaDD;


            if (model.DoKoga == null && model.DokleLokacijaId == 0 && model.DokleId == 0)
            {

                ModelState.AddModelError("ErrorMessage", "Morate izabrati kome saljete opremu!");
                return View(model);
            }

            if (ModelState.IsValid)
            {
                if (model.DoKoga != null)
                {
                    using var transaction = _dropDownModel.Database.BeginTransaction();
                    try
                    {
                        var commandText = "UPDATE oprema SET  oprema_stanje = 1  WHERE oprema_id = @oprema_id";
                        var updateOprema = new MySqlParameter("@oprema_id", model.OpremaId);

                        var upitRelokacijaNaServisText = "INSERT INTO relokacija(relokacija_korisnik_od_koga_id,relokacija_do_koga,relokacija_datum,relokacija_oprema_id,relokacija_napomena) VALUES(@relokacija_korisnik_od_koga_id,@relokacija_korisnik_do_koga_id,@relokacija_datum,@relokacija_oprema_id,@Napomena)";
                        var odKoga = new MySqlParameter("@relokacija_korisnik_od_koga_id", idUser);
                        var doKoga = new MySqlParameter("@relokacija_korisnik_do_koga_id", model.DoKoga);
                        var datum = new MySqlParameter("@relokacija_datum", model.Datum.Date);
                        var oprema = new MySqlParameter("@relokacija_oprema_id", model.OpremaId);
                        var napomena = new MySqlParameter("@Napomena", model.Napomena);

                        _dropDownModel.Database.ExecuteSqlRaw(commandText, updateOprema);
                        _dropDownModel.Database.ExecuteSqlRaw(upitRelokacijaNaServisText, new[] { odKoga, doKoga, datum, oprema, napomena });
                        _dropDownModel.SaveChanges();

                        transaction.Commit();
                        ViewBag.Message = "Uspesno izvrsena relokacija!";
                        ModelState.Clear();
                    }
                    catch (Exception e)
                    {

                        ViewBag.Message = e.Message;
                    }

                    return View();

                }


                if (model.DokleLokacijaId == 0)
                {

                    using var transaction = _dropDownModel.Database.BeginTransaction();
                    try
                    {

                        var commandText = "UPDATE oprema SET  oprema_stanje = 1  WHERE oprema_id = @oprema_id";
                        var updateOprema = new MySqlParameter("@oprema_id", model.OpremaId);

                        var upitRelokacijaNaServisText = "INSERT INTO relokacija(relokacija_korisnik_od_koga_id,relokacija_korisnik_do_koga_id,relokacija_datum,relokacija_oprema_id,relokacija_napomena) VALUES(@relokacija_korisnik_od_koga_id,@relokacija_korisnik_do_koga_id,@relokacija_datum,@relokacija_oprema_id,@Napomena)";
                        var odKoga = new MySqlParameter("@relokacija_korisnik_od_koga_id", idUser);
                        var doKoga = new MySqlParameter("@relokacija_korisnik_do_koga_id", model.DokleId);
                        var datum = new MySqlParameter("@relokacija_datum", model.Datum.Date);
                        var oprema = new MySqlParameter("@relokacija_oprema_id", model.OpremaId);
                        var napomena = new MySqlParameter("@Napomena", model.Napomena);

                        _dropDownModel.Database.ExecuteSqlRaw(commandText, updateOprema);
                        _dropDownModel.Database.ExecuteSqlRaw(upitRelokacijaNaServisText, new[] { odKoga, doKoga, datum, oprema, napomena });
                        _dropDownModel.SaveChanges();

                        transaction.Commit();
                        ViewBag.Message = "Uspesno izvrsena relokacija!";
                        ModelState.Clear();
                    }
                    catch (Exception e)
                    {

                        ViewBag.Message = e.Message;
                    }


                    return View();

                }
                else
                {
                    using var transaction = _dropDownModel.Database.BeginTransaction();
                    try
                    {

                        var commandText = "UPDATE oprema SET  oprema_stanje = 1  WHERE oprema_id = @oprema_id";
                        var updateOprema = new MySqlParameter("@oprema_id", model.OpremaId);

                        var upitRelokacijaNaServisText = "INSERT INTO relokacija(relokacija_korisnik_od_koga_id,relokacija_do_lokacija_id,relokacija_datum,relokacija_oprema_id,relokacija_napomena) VALUES(@relokacija_korisnik_od_koga_id,@relokacija_korisnik_do_koga_id,@relokacija_datum,@relokacija_oprema_id,@Napomena)";
                        var odKoga = new MySqlParameter("@relokacija_korisnik_od_koga_id", idUser);
                        var doKoga = new MySqlParameter("@relokacija_korisnik_do_koga_id", model.DokleLokacijaId);
                        var datum = new MySqlParameter("@relokacija_datum", model.Datum.Date);
                        var oprema = new MySqlParameter("@relokacija_oprema_id", model.OpremaId);
                        var napomena = new MySqlParameter("@Napomena", model.Napomena);
                        _dropDownModel.Database.ExecuteSqlRaw(commandText, updateOprema);
                        _dropDownModel.Database.ExecuteSqlRaw(upitRelokacijaNaServisText, new[] { odKoga, doKoga, datum, oprema, napomena });
                        _dropDownModel.SaveChanges();

                        transaction.Commit();
                        ViewBag.Message = "Uspesno izvrsena relokacija!";
                        ModelState.Clear();
                    }
                    catch (Exception e)
                    {

                        ViewBag.Message = e.Message;
                    }
                    return View();
                }


            }


            return View();
        }


        [Authorize(Roles = ("Superadmin"))]
        public IActionResult RelokacijaListaSvi()
        {
            List<RelokacijaListaSvi> lista = new List<RelokacijaListaSvi>();



            MySqlConnection konekcija = new MySqlConnection(StringKonekcije());
            try
            {
                using (konekcija)
                {

                    konekcija.Open();
                    string upit = "SELECT relokacija_id,relokacija_napomena,relokacija_do_koga,oprema.oprema_serijski_broj,oprema.oprema_marka,oprema.oprema_model,oprema_qr_kod,aspnetusers.UserName,lokacija.lokacija_mesto,lokacija.lokacija_ime,lokacija.lokacija_adresa,sektor.sektor_ime,relokacija_datum,OdKoga.UserName as OdKoga \r\nFROM relokacija \r\nLEFT join  aspnetusers as OdKoga on relokacija_korisnik_od_koga_id = OdKoga.Id\r\nLEFT JOIN lokacija on relokacija_do_lokacija_id = lokacija.lokacija_id \r\nINNER join (SELECT * FROM oprema WHERE oprema.oprema_otpisano = 0 and oprema.oprema_stanje > 0 ) oprema on relokacija_oprema_id = oprema.oprema_id \r\n\r\nLEFT join (SELECT * FROM aspnetusers WHERE aspnetusers.OtpisanRadnik =0)  aspnetusers on relokacija_korisnik_do_koga_id = aspnetusers.Id\r\nINNER JOIN oprema_kategorija on oprema.oprema_kategorija_id = oprema_kategorija.oprema_kategorija_id \r\nINNER join sektor on oprema_kategorija.oprema_kategorija_sektor_id = sektor.sektor_id \r\nWHERE relokacija_id in ( SELECT MAX(relokacija_id) from relokacija GROUP BY relokacija_oprema_id);";

                    MySqlCommand komanda = new MySqlCommand(upit, konekcija);

                    using (MySqlDataReader reader = komanda.ExecuteReader())
                    {
                        while (reader.Read())
                        {


                            RelokacijaListaSvi model = new RelokacijaListaSvi();
                            model.Id = Convert.ToInt32(reader["relokacija_id"]);
                            model.UserName = Convert.ToString(reader["UserName"]);
                            model.Marka = Convert.ToString(reader["oprema_marka"]);
                            model.Model = Convert.ToString(reader["oprema_model"]);
                            model.Datum = Convert.ToDateTime(reader["relokacija_datum"]);
                            model.Sektor = Convert.ToString(reader["sektor_ime"]);
                            model.Qr = Convert.ToString(reader["oprema_qr_kod"]);
                            model.LokacijaIme = Convert.ToString(reader["lokacija_ime"]);
                            model.Adresa = Convert.ToString(reader["lokacija_adresa"]);
                            model.Napomena = Convert.ToString(reader["relokacija_napomena"]);
                            model.DoKoga = Convert.ToString(reader["relokacija_do_koga"]);
                            model.SerijskiBroj = Convert.ToString(reader["oprema_serijski_broj"]);
                            model.Mesto = Convert.ToString(reader["lokacija_mesto"]);
                            model.OdKoga = Convert.ToString(reader["OdKoga"]);

                            if (reader["UserName"] == DBNull.Value && reader["lokacija_ime"] == DBNull.Value && reader["relokacija_do_koga"] == DBNull.Value)
                            {
                                continue; //preskoci svu opremu koja nije ni u lokalu ni kod korisnika

                            }


                            lista.Add(model);

                        }

                    }

                }
            }
            catch (Exception e)
            {
                ViewBag.Message = e.Message;
            }
            finally
            {

                konekcija.Close();
            }



            return View(lista);
        }

        [Authorize(Roles = ("Superadmin,Admin"))]
        public IActionResult RelokacijaLista()
        {

            List<RelokacijaListaSvi> lista = new List<RelokacijaListaSvi>();

            if (HttpContext.Session.GetInt32("UserSektorId") == null)
            {
                return RedirectToAction("Login", "Account");

            }

            int SektorUser = (int)HttpContext.Session.GetInt32("UserSektorId");



            MySqlConnection konekcija = new MySqlConnection(StringKonekcije());
            try
            {
                using (konekcija)
                {

                    konekcija.Open();
                    string upit = "SELECT relokacija_id,relokacija_napomena,relokacija_do_koga,oprema.oprema_serijski_broj,oprema.oprema_marka,oprema.oprema_model,oprema_qr_kod,aspnetusers.UserName,lokacija.lokacija_mesto,lokacija.lokacija_ime,lokacija.lokacija_adresa,sektor.sektor_ime,relokacija_datum, OdKoga.UserName as OdKoga \r\nFROM relokacija \r\nLEFT JOIN lokacija on relokacija_do_lokacija_id = lokacija.lokacija_id \r\nINNER join (SELECT * FROM oprema WHERE oprema.oprema_otpisano = 0 and oprema.oprema_stanje > 0 ) oprema on relokacija_oprema_id = oprema.oprema_id \r\nLEFT join (SELECT * FROM aspnetusers WHERE aspnetusers.OtpisanRadnik =0)  aspnetusers on relokacija_korisnik_do_koga_id = aspnetusers.Id\r\nLEFT join  aspnetusers as OdKoga on relokacija_korisnik_od_koga_id = OdKoga.Id\r\nINNER JOIN oprema_kategorija on oprema.oprema_kategorija_id = oprema_kategorija.oprema_kategorija_id \r\nINNER join sektor on oprema_kategorija.oprema_kategorija_sektor_id = sektor.sektor_id \r\nWHERE relokacija_id in ( SELECT MAX(relokacija_id) from relokacija GROUP BY relokacija_oprema_id) and sektor.sektor_id = "+SektorUser+"";

                    MySqlCommand komanda = new MySqlCommand(upit, konekcija);

                    using (MySqlDataReader reader = komanda.ExecuteReader())
                    {
                        while (reader.Read())
                        {


                            RelokacijaListaSvi model = new RelokacijaListaSvi();
                            model.Id = Convert.ToInt32(reader["relokacija_id"]);
                            model.UserName = Convert.ToString(reader["UserName"]);
                            model.Marka = Convert.ToString(reader["oprema_marka"]);
                            model.Model = Convert.ToString(reader["oprema_model"]);
                            model.Datum = Convert.ToDateTime(reader["relokacija_datum"]);
                            model.Qr = Convert.ToString(reader["oprema_qr_kod"]);
                            model.LokacijaIme = Convert.ToString(reader["lokacija_ime"]);
                            model.Adresa = Convert.ToString(reader["lokacija_adresa"]);
                            model.Napomena = Convert.ToString(reader["relokacija_napomena"]);
                            model.DoKoga = Convert.ToString(reader["relokacija_do_koga"]);
                            model.SerijskiBroj = Convert.ToString(reader["oprema_serijski_broj"]);
                            model.Mesto = Convert.ToString(reader["lokacija_mesto"]);
                            model.OdKoga = Convert.ToString(reader["OdKoga"]);

                            if (reader["UserName"] == DBNull.Value && reader["lokacija_ime"] == DBNull.Value && reader["relokacija_do_koga"] == DBNull.Value)
                            {
                                continue; //preskoci svu opremu koja nije ni u lokalu ni kod korisnika,regulisati kada se obrise korisnik da se sva oprema koju je imao stavi na stanje automatski

                            }


                            lista.Add(model);

                        }

                    }

                }
            }
            catch (Exception e)
            {
                ViewBag.Message = e.Message;
            }
            finally
            {

                konekcija.Close();
            }



            return View(lista);

        }

        //trebalo bi da uzima odredjeni id i da vraca tabelu 
        [HttpGet]
        public IActionResult OpremaIstorija(int id)
        {
            if (id == 0 || id < 0)
            {
                RedirectToAction("OpremaLista", "Home");

            }

            List<OpremaListaIstorijaModel> listaOprema = new List<OpremaListaIstorijaModel>();
            List<OpremaListaIstorijaModel> listaOpremaSortirano = new List<OpremaListaIstorijaModel>(); 
            MySqlConnection konekcija = new MySqlConnection("Server=127.0.0.1;Port=3306;Database=persuproba;Uid=root;Pwd='';convert zero datetime=True;");
            try
            {
                using (konekcija)
                {

                    konekcija.Open();
                    string upit = "SELECT relokacija_id,oprema.oprema_marka,oprema.oprema_model,relokacija_napomena,relokacija_do_koga,aspnetusers.UserName,lokacija.lokacija_ime,lokacija.lokacija_mesto,lokacija.lokacija_adresa,relokacija_datum \r\nFROM relokacija \r\nLEFT JOIN lokacija on relokacija_do_lokacija_id = lokacija.lokacija_id \r\nINNER join (SELECT * FROM oprema WHERE oprema.oprema_otpisano = 0 ) oprema on relokacija_oprema_id = oprema.oprema_id \r\nLEFT join (SELECT * FROM aspnetusers WHERE aspnetusers.OtpisanRadnik = 0)  aspnetusers on relokacija_korisnik_do_koga_id = aspnetusers.Id \r\nINNER JOIN oprema_kategorija on oprema.oprema_kategorija_id = oprema_kategorija.oprema_kategorija_id \r\nINNER join sektor on oprema_kategorija.oprema_kategorija_sektor_id = sektor.sektor_id \r\nWHERE oprema.oprema_id = " + id + ";";

                    MySqlCommand komanda = new MySqlCommand(upit, konekcija);

                    using (MySqlDataReader reader = komanda.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            

                            OpremaListaIstorijaModel model = new OpremaListaIstorijaModel();
                            model.Id = Convert.ToInt32(reader["relokacija_id"]);
                            model.Marka = Convert.ToString(reader["oprema_marka"]);
                            model.Model = Convert.ToString(reader["oprema_model"]);
                            model.Napomena = Convert.ToString(reader["relokacija_napomena"]);
                            model.DoKoga = Convert.ToString(reader["relokacija_do_koga"]);
                            model.UserName = Convert.ToString(reader["UserName"]);
                            model.LokacijaIme = Convert.ToString(reader["lokacija_ime"]);
                            model.LokacijaAdresa = Convert.ToString(reader["lokacija_adresa"]);
                            model.Datum = Convert.ToDateTime(reader["relokacija_datum"]).Date;
                            //model.OdKoga = ;
                            model.LokacijaMesto = Convert.ToString(reader["lokacija_mesto"]); ;

                            listaOprema.Add(model);
                            

                        }

                    }

                }
            }
            catch (Exception e)
            {
                ViewBag.Message = e.Message;
            }
            finally
            {

                konekcija.Close();
            }

            listaOpremaSortirano = listaOprema.OrderByDescending(o => o.Id).ToList();

            try
            {
                Document doc = new Document(iTextSharp.text.PageSize.LETTER, 0f, 0f, 10f, 10f);

                PdfWriter wri = PdfWriter.GetInstance(doc, new FileStream("OpremaIstorijaLista.pdf", FileMode.Create));
                try
                {

                    doc.Open();
                    Type type = typeof(OpremaListaIstorijaModel);
                    int NumberOfRecords = type.GetProperties().Length;

                    PdfPTable table = new PdfPTable(NumberOfRecords);


                    table.AddCell("Id");
                    table.AddCell("Model opreme:");
                    table.AddCell("Marka opreme:");
                    table.AddCell("Napomena:");
                    table.AddCell("Kod:");
                    table.AddCell("Kod Korisnika:");
                    table.AddCell("Na Lokaciji");
                    table.AddCell("Adresa objekta:");
                    table.AddCell("Mesto:");
                    table.AddCell("Datum:");



                    foreach (var item in listaOpremaSortirano)
                    {

                        table.AddCell(item.Id.ToString());
                        table.AddCell(item.Marka);
                        table.AddCell(item.Model);
                        table.AddCell(item.Napomena);
                        table.AddCell(item.DoKoga);
                        table.AddCell(item.UserName);
                        table.AddCell(item.LokacijaIme);
                        table.AddCell(item.LokacijaAdresa);
                        table.AddCell(item.LokacijaMesto);
                        table.AddCell(item.Datum.Date.ToShortDateString());
                    }

                    doc.Add(table);

                    //doc.Close();
                }
                catch (Exception e)
                {

                    throw new Exception("Greska,pokusajte ponovo!.", e);
                }
                finally
                {

                    doc.Close();
                }
            }
            catch (Exception)
            {

                throw new Exception("Greska,pokusajte ponovo!.");
            }


            return View(listaOpremaSortirano);
        }

        public IActionResult OpremaIstorijaPDF()
        {

            string physicalPath = "OpremaIstorijaLista.pdf"; 
            byte[] pdfBytes = System.IO.File.ReadAllBytes(physicalPath);

            if (pdfBytes == null)
            {
                RedirectToAction("OpremaListaSvi", "Home");

            }

            MemoryStream ms = new MemoryStream(pdfBytes);

            if (ms == null)
            {
                RedirectToAction("OpremaListaSvi", "Home");

            }

            return new FileStreamResult(ms, "application/pdf");
        }
        [AllowAnonymous]
        public IActionResult Index() 
        {


            
            return View();
        }

        [HttpGet]
        public IActionResult Otpad(int? id)
        {

            if (id == null)
            {

                return RedirectToAction("KorisnikLista");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Otpad(OtpadModel model, int? id)
        {

            if (id == null)
            {

                return RedirectToAction("KorisnikLista");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var sql = "UPDATE oprema SET  oprema_otpisano = 1,oprema_razlog_otpisa = @Razlog  WHERE oprema_id = @OpremaTrans";
                    var opremaID = new MySqlParameter("@OpremaTrans", id);
                    var Razlog = new MySqlParameter("@Razlog", model.RazlogOtpada);

                    _dropDownModel.Database.ExecuteSqlRaw(sql, new[] { opremaID, Razlog });
                    _dropDownModel.SaveChanges();
                    ViewBag.Message = "Uspesno obrisana oprema!";
                }
                catch (Exception)
                {
                    ViewBag.Message = "Neuspesno obrisana oprema,pokusajte ponovo!";
                }
            }



            return View();
        }

        [Authorize(Roles = ("Superadmin"))]
        public IActionResult OtpadLista()
        {
            List<OtpadListaModel> OpremaListaOtpad = new List<OtpadListaModel>();


            var query = from Oprema in _dropDownModel.oprema
                        join Dobavljac in _dropDownModel.dostavljac on Oprema.oprema_dostavljac_id equals Dobavljac.dostavljac_id
                        join Kategorija in _dropDownModel.oprema_kategorija on Oprema.oprema_kategorija_id equals Kategorija.oprema_kategorija_id
                        join Sektor in _dropDownModel.sektor on Kategorija.oprema_kategorija_sektor_id equals Sektor.sektor_id
                        where Oprema.oprema_otpisano == 1 //nemoj ovo zaboraviti
                        select new
                        {
                            Oprema.oprema_id,
                            Oprema.oprema_marka,
                            Oprema.oprema_model,
                            Kategorija.oprema_kategorija_ime,
                            Oprema.oprema_qr_kod,
                            Oprema.oprema_cena,
                            Dobavljac.dostavljac_ime,
                            Sektor.sektor_ime,
                            Oprema.oprema_razlog_otpisa,

                        };

            foreach (var item in query)
            {
                OtpadListaModel model = new OtpadListaModel();

                model.Id = item.oprema_id;
                model.Marka = item.oprema_marka;
                model.Model = item.oprema_model;
                model.KategorijaIme = item.oprema_kategorija_ime;
                model.QRKod = item.oprema_qr_kod;
                model.Cena = item.oprema_cena;
                model.ImeDobavljaca = item.dostavljac_ime;
                model.Sektor = item.sektor_ime;
                model.Razlog = item.oprema_razlog_otpisa;

                OpremaListaOtpad.Add(model);
            }


            return View(OpremaListaOtpad);


        }

        [HttpGet] 
        public async Task<IActionResult> Razduzenje(int id)
        {
            if (id == 0 || id < 0)
            {
                RedirectToAction("RelokacijaLista", "Home");

            }


            RazduzenjeModel model = new RazduzenjeModel();
            MySqlConnection konekcija = new MySqlConnection(StringKonekcije());
            try
            {
                using (konekcija)
                {

                    konekcija.Open();
                    string upit = "SELECT relokacija_oprema_id,oprema.oprema_marka,oprema.oprema_model,oprema.oprema_qr_kod,oprema.oprema_serijski_broj,relokacija_napomena,relokacija_do_koga,aspnetusers.UserName,lokacija.lokacija_ime,lokacija.lokacija_mesto,lokacija.lokacija_adresa \r\nFROM relokacija \r\nLEFT JOIN lokacija on relokacija_do_lokacija_id = lokacija.lokacija_id \r\nINNER join (SELECT * FROM oprema WHERE oprema.oprema_otpisano = 0 ) oprema on relokacija_oprema_id = oprema.oprema_id \r\nLEFT join (SELECT * FROM aspnetusers WHERE aspnetusers.OtpisanRadnik = 0)  aspnetusers on relokacija_korisnik_do_koga_id = aspnetusers.Id \r\n\r\nWHERE relokacija.relokacija_id = " + id + " limit 1;";

                    MySqlCommand komanda = new MySqlCommand(upit, konekcija);

                    using (MySqlDataReader reader = komanda.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            model.Marka = Convert.ToString(reader["oprema_marka"]);
                            model.Model = Convert.ToString(reader["oprema_model"]);
                            model.SerijskiBroj = Convert.ToString(reader["oprema_serijski_broj"]);
                            model.Qr = Convert.ToString(reader["oprema_qr_kod"]);
                            model.OpremaID = Convert.ToInt32(reader["relokacija_oprema_id"]);
                            model.Napomena = Convert.ToString(reader["relokacija_napomena"]);
                            model.KorisnikImePrezime = Convert.ToString(reader["relokacija_do_koga"]);
                            model.KorisnickoIme = Convert.ToString(reader["UserName"]);
                            model.ObjekatIme = Convert.ToString(reader["lokacija_ime"]);
                            model.ObjekatAdresa = Convert.ToString(reader["lokacija_adresa"]);
                            model.ObjekatMesto = Convert.ToString(reader["lokacija_mesto"]);
                        }
                    }

                }
            }
            catch (Exception e)
            {
                ViewBag.Message = e.Message;
            }
            finally
            {

                konekcija.Close();
            }

            TempData["lokacijaIme"] = model.ObjekatIme;
            TempData["Adresa"] = model.ObjekatAdresa;
            TempData["Mesto"] = model.ObjekatMesto;
            TempData["Datum"] = DateTime.Now.Date;
            TempData["Marka"] = model.Marka;
            TempData["Model"] = model.Model;
            TempData["KorisnickoIme"] = model.KorisnickoIme;
            TempData["ImePrezime"] = model.KorisnikImePrezime;
            TempData["Napomena"] = model.Napomena;
            TempData["Qr"] = model.Qr;
            TempData["SerijskiBroj"] = model.SerijskiBroj;





            using (var stringWriter = new StringWriter())
            {
                var viewResult = _compositeViewEngine.FindView(ControllerContext, "Razduzenje", false);

                if (viewResult.View == null)
                {
                    throw new ArgumentNullException($"'Views/Home/Razduzenje.cshtml' does not match any available view");
                }

                var viewDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary());

                var viewContext = new ViewContext(
                    ControllerContext,
                    viewResult.View,
                    viewDictionary,
                    TempData,
                    stringWriter,
                    new HtmlHelperOptions()
                );

                await viewResult.View.RenderAsync(viewContext);

                var htmlToPdf = new HtmlToPdf(1000, 1414);
                htmlToPdf.Options.DrawBackground = true;

                var pdf = htmlToPdf.ConvertHtmlString(stringWriter.ToString());
                var pdfBytes = pdf.Save();

                using (var streamWriter = new StreamWriter(@"Razduzenje.pdf"))
                {
                    await streamWriter.BaseStream.WriteAsync(pdfBytes, 0, pdfBytes.Length);
                }

                //UBaci jos samo da se oprema stavi na stanje = 0 i tjt 
                //model.OpremaId

                try
                {
                    var sql = "UPDATE oprema SET  oprema_stanje = 0  WHERE oprema_id = @OpremaTrans";
                    var opremaID = new MySqlParameter("@OpremaTrans", model.OpremaID);
                   

                    _dropDownModel.Database.ExecuteSqlRaw(sql, new[] { opremaID });
                    _dropDownModel.SaveChanges();
                   
                }
                catch (Exception)
                {
                   return RedirectToAction("RelokacijaLista","Home");
                }



                return File(pdfBytes, "application/pdf");
            }

        }


       
        public async Task<IActionResult> Revers(int id)
        {


            if (id == 0 || id < 0)
            {
                RedirectToAction("RelokacijaLista", "Home");

            }
            ReversModel model = new ReversModel();
            MySqlConnection konekcija = new MySqlConnection(StringKonekcije());
            try
            {
                using (konekcija)
                {

                    konekcija.Open();
                    string upit = "SELECT oprema.oprema_marka,oprema.oprema_model,oprema.oprema_qr_kod,oprema.oprema_serijski_broj,relokacija_napomena,relokacija_do_koga,aspnetusers.UserName,lokacija.lokacija_ime,lokacija.lokacija_mesto,lokacija.lokacija_adresa,relokacija_datum \r\nFROM relokacija \r\nLEFT JOIN lokacija on relokacija_do_lokacija_id = lokacija.lokacija_id \r\nINNER join (SELECT * FROM oprema WHERE oprema.oprema_otpisano = 0 ) oprema on relokacija_oprema_id = oprema.oprema_id \r\nLEFT join (SELECT * FROM aspnetusers WHERE aspnetusers.OtpisanRadnik = 0)  aspnetusers on relokacija_korisnik_do_koga_id = aspnetusers.Id \r\n\r\nWHERE relokacija.relokacija_id = " + id + " limit 1;";

                    MySqlCommand komanda = new MySqlCommand(upit, konekcija);

                    using (MySqlDataReader reader = komanda.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            model.Marka = Convert.ToString(reader["oprema_marka"]);
                            model.Model = Convert.ToString(reader["oprema_model"]);
                            model.SerijskiBroj = Convert.ToString(reader["oprema_serijski_broj"]);
                            model.Qr = Convert.ToString(reader["oprema_qr_kod"]);
                            model.Napomena = Convert.ToString(reader["relokacija_napomena"]);
                            model.KorisnikImePrezime = Convert.ToString(reader["relokacija_do_koga"]);
                            model.KorisnickoIme = Convert.ToString(reader["UserName"]);
                            model.LokacijaIme = Convert.ToString(reader["lokacija_ime"]);
                            model.LokacijaAdresa = Convert.ToString(reader["lokacija_adresa"]);
                            model.Datum = Convert.ToDateTime(reader["relokacija_datum"]).Date;
                            model.LokacijaMesto = Convert.ToString(reader["lokacija_mesto"]);

                        }

                    }

                }
            }
            catch (Exception e)
            {
                ViewBag.Message = e.Message;
            }
            finally
            {

                konekcija.Close();
            }

          
            TempData["lokacijaIme"] = model.LokacijaIme;
            TempData["Adresa"] = model.LokacijaAdresa;
            TempData["Mesto"] = model.LokacijaMesto;
            TempData["Datum"] = model.Datum.Date.ToShortDateString();
            TempData["Marka"] = model.Marka;
            TempData["Model"] = model.Model;
            TempData["KorisnickoIme"] = model.KorisnickoIme;
            TempData["ImePrezime"] = model.KorisnikImePrezime;
            TempData["Napomena"] = model.Napomena;
            TempData["Qr"] = model.Qr;
            TempData["SerijskiBroj"] = model.SerijskiBroj;





            using (var stringWriter = new StringWriter())
            {
                var viewResult = _compositeViewEngine.FindView(ControllerContext, "Revers", false);

                if (viewResult.View == null)
                {
                    throw new ArgumentNullException($"'Views/Home/Revers.cshtml' does not match any available view");
                }

                var viewDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary());

                var viewContext = new ViewContext(
                    ControllerContext,
                    viewResult.View,
                    viewDictionary,
                    TempData,
                    stringWriter,
                    new HtmlHelperOptions()
                );

                await viewResult.View.RenderAsync(viewContext);

                var htmlToPdf = new HtmlToPdf(1000, 1414);
                htmlToPdf.Options.DrawBackground = true;

                var pdf = htmlToPdf.ConvertHtmlString(stringWriter.ToString());
                var pdfBytes = pdf.Save();

                //using (var streamWriter = new StreamWriter(@"Revers.pdf"))
                //{
                //    await streamWriter.BaseStream.WriteAsync(pdfBytes, 0, pdfBytes.Length);
                //}



                return File(pdfBytes, "application/pdf");

            }
        }


        [HttpGet]
        public IActionResult LokacijaSaOpremom(int id)  
        {
            if (id <= 0)
            {
                RedirectToAction("LokacijaLista", "Account");

            }

            List<LokacijaSaOpremom> LokacijaSaOpremom = new List<LokacijaSaOpremom>();

            MySqlConnection konekcija = new MySqlConnection(StringKonekcije());
            try
            {
                using (konekcija)
                {

                    konekcija.Open();
                    string upit = "SELECT relokacija_napomena,oprema.oprema_serijski_broj,oprema.oprema_marka,oprema.oprema_model,oprema_qr_kod,lokacija.lokacija_mesto,lokacija.lokacija_ime,lokacija.lokacija_adresa,relokacija_datum\r\nFROM relokacija \r\nLEFT JOIN lokacija on relokacija_do_lokacija_id = lokacija.lokacija_id \r\nINNER join (SELECT * FROM oprema WHERE oprema.oprema_otpisano = 0 and oprema.oprema_stanje > 0 ) oprema on relokacija_oprema_id = oprema.oprema_id \r\nINNER JOIN oprema_kategorija on oprema.oprema_kategorija_id = oprema_kategorija.oprema_kategorija_id \r\nINNER join sektor on oprema_kategorija.oprema_kategorija_sektor_id = sektor.sektor_id \r\nWHERE relokacija_id in ( SELECT MAX(relokacija_id) from relokacija GROUP BY relokacija_oprema_id) and relokacija_do_lokacija_id = "+id+";";

                    MySqlCommand komanda = new MySqlCommand(upit, konekcija);

                    using (MySqlDataReader reader = komanda.ExecuteReader())
                    {
                        while (reader.Read())
                        {


                            LokacijaSaOpremom model = new LokacijaSaOpremom();
                            
                            model.Marka = Convert.ToString(reader["oprema_marka"]);
                            model.Model = Convert.ToString(reader["oprema_model"]);
                            model.SerijskiBroj = Convert.ToString(reader["oprema_serijski_broj"]);
                            model.Napomena = Convert.ToString(reader["relokacija_napomena"]);
                            model.Qr = Convert.ToString(reader["oprema_qr_kod"]);
                            model.LokacijaIme = Convert.ToString(reader["lokacija_ime"]);
                            model.LokacijaAdresa = Convert.ToString(reader["lokacija_adresa"]);
                            model.Datum = Convert.ToDateTime(reader["relokacija_datum"]).Date;
                            model.LokacijaMesto = Convert.ToString(reader["lokacija_mesto"]); ;

                            LokacijaSaOpremom.Add(model);


                        }

                    }

                }
            }
            catch (Exception e)
            {
                ViewBag.Message = e.Message;
            }
            finally
            {

                konekcija.Close();
            }

            try
            {
                Document doc = new Document(iTextSharp.text.PageSize.LETTER, 0f, 0f, 10f, 10f);

                PdfWriter wri = PdfWriter.GetInstance(doc, new FileStream("LokacijaSaOpremom.pdf", FileMode.Create));
                try
                {

                    doc.Open();
                    Type type = typeof(LokacijaSaOpremom);
                    int NumberOfRecords = type.GetProperties().Length;

                    PdfPTable table = new PdfPTable(NumberOfRecords);

                    Font titleFont = FontFactory.GetFont("Arial", 20);
                    Paragraph title;
                    title = new Paragraph("KOMPLETAN REVERS ZA RADNJU", titleFont);
                    title.Alignment = Element.ALIGN_CENTER;

                    doc.Add(title);
                    doc.Add(new Paragraph("   "));

                    table.AddCell("Na Lokaciji:");
                    table.AddCell("Adresa objekta:");
                    table.AddCell("Mesto:");
                    table.AddCell("Qr kod:");
                    table.AddCell("Model opreme:");
                    table.AddCell("Marka opreme:");
                    table.AddCell("Serijski Broj:");
                    table.AddCell("Datum relokacije:");
                    table.AddCell("Napomena:");
                    
                    
                    



                    foreach (var item in LokacijaSaOpremom)
                    {

                        table.AddCell(item.LokacijaIme);
                        table.AddCell(item.LokacijaAdresa);
                        table.AddCell(item.LokacijaMesto);
                        table.AddCell(item.Qr);
                        table.AddCell(item.Model);
                        table.AddCell(item.Marka);
                        table.AddCell(item.SerijskiBroj);
                        table.AddCell(item.Datum.Date.ToShortDateString());
                        table.AddCell(item.Napomena);
                        
                        
                        
                    }

                    doc.Add(table);

                    doc.Add(new Paragraph("                  Potpis:"));
                    doc.Add(new Paragraph("   "));
                    Paragraph copyright = new Paragraph("______________                                                                                         ______________");
                    PdfPTable footerTbl = new PdfPTable(1);
                    footerTbl.TotalWidth = 300;
                    PdfPCell cell = new PdfPCell(copyright);
                    cell.Border = 0;
                    footerTbl.AddCell(cell);
                    footerTbl.WriteSelectedRows(0, -1, 30, 30, wri.DirectContent);
                    doc.Add(footerTbl);
                    doc.Add(new Paragraph("                  Za BB Trade                                                                                                 Za Radnju "));

                    if (LokacijaSaOpremom.Count <= 0)
                    {
                        ModelState.AddModelError("Error","Ne postoji nijedna oprema u ovom objektu!");

                    }

                    doc.Close();
                }
                catch (Exception e)
                {

                    throw new Exception("Greska,pokusajte ponovo!.", e);
                }
                finally
                {

                    doc.Close();
                }
            }
            catch (Exception)
            {

                throw new Exception("Greska,pokusajte ponovo!.");
            }

            return View(LokacijaSaOpremom);
        }

        public IActionResult LokacijaSaOpremomPDF()
        {

            string physicalPath = "LokacijaSaOpremom.pdf"; // ovo radi na kompu ali moram nekako staviti da vadi iz projekta,tj kad bude na serveru
            byte[] pdfBytes = System.IO.File.ReadAllBytes(physicalPath);

            if (pdfBytes == null)
            {
                RedirectToAction("OpremaListaSvi", "Home");

            }

            MemoryStream ms = new MemoryStream(pdfBytes);

            if (ms == null)
            {
                RedirectToAction("OpremaListaSvi", "Home");

            }

            return new FileStreamResult(ms, "application/pdf");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}