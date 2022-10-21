
using Kopija.Modeli;
using Kopija.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Authorization;
using Kopija.ModeliPomocni;
using MySqlConnector;

namespace Kopija.Controllers
{
    [Authorize(Roles = ("Admin,Superadmin"))]
    public class AdminController : Controller
    {

        private UserManager<AppUser> UserMgr { get; }
        private SignInManager<AppUser> SignInMgr { get; }
        private RoleManager<AppRole> RoleMngr { get; } // ovo sam individualno ubacio da znas 
        private readonly IdentityAppContext _dropDownModel;
        private readonly ILogger<AccountController> _logger;
        private readonly IWebHostEnvironment _hostingEnvironment;


        public AdminController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager, IdentityAppContext dropDownModel, ILogger<AccountController> logger, IWebHostEnvironment hostingEnvironment)
        {
            UserMgr = userManager;
            SignInMgr = signInManager;
            RoleMngr = roleManager;
            _dropDownModel = dropDownModel;
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
        }
        public static string StringKonekcije()
        {

            return "Server=127.0.0.1;Port=3306;Database=persuproba;Uid=root;Pwd='';convert zero datetime=True;";
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Korisnik(KorisnikModel model)
        {
            if (ModelState.IsValid)
            {


                List<lokacija> lokacijaLista = new List<lokacija>();
                lokacijaLista = (from servis1 in _dropDownModel.lokacija select servis1).ToList();
                ViewBag.lokacija = lokacijaLista;

                if (HttpContext.Session.GetInt32("UserSektorId") == null)
                {

                    return RedirectToAction("Login", "Account");
                }

                ViewBag.UserSektorId = HttpContext.Session.GetInt32("UserSektorId");
                //ViewBag.UserRole = HttpContext.Session.GetString("UserRola");
                ViewBag.UserRole = "Korisnik";


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


        public IActionResult Korisnik()
        {
            List<lokacija> lokacijaLista = new List<lokacija>();
            lokacijaLista = (from servis1 in _dropDownModel.lokacija select servis1).ToList();
            ViewBag.lokacija = lokacijaLista;
            if (HttpContext.Session.GetInt32("UserSektorId") == null)
            {

                return RedirectToAction("Login", "Account");
            }

            var sektor = Convert.ToInt32(HttpContext.Session.GetInt32("UserSektorId"));
            ViewBag.UserSektorId = sektor;
            ViewBag.UserRole = "Korisnik";


            return View();
        }

        public IActionResult KorisnikLista()
        {
            if (HttpContext.Session.GetInt32("UserSektorId") == null)
            {

                return RedirectToAction("Login", "Account");
            }

            var sektor = Convert.ToInt32(HttpContext.Session.GetInt32("UserSektorId"));
            ViewBag.UserSektorId = sektor;
            ViewBag.UserRole = "Korisnik";


            //nesto sesija posle neg vremena zeza,kao da se brise pa mora da se uloguje korisnik opet i tako to

            List<KorisnikListaModel> korisnikListaKonacno = new List<KorisnikListaModel>();

            var query = from Korisnik in _dropDownModel.aspnetusers
                        join Sektor in _dropDownModel.sektor on Korisnik.SektorId equals Sektor.sektor_id
                        join Lokacija in _dropDownModel.lokacija on Korisnik.LokacijaId equals Lokacija.lokacija_id
                        where Korisnik.OtpisanRadnik == 0 && Sektor.sektor_id == sektor //nemoj ovo zab
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
                            Lokacija.lokacija_adresa,
                            Lokacija.lokacija_mesto
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
                model.Mesto = item.lokacija_mesto;

                korisnikListaKonacno.Add(model);
            }


            return View(korisnikListaKonacno);



        }

        public ActionResult Kategorija()
        {
            if (HttpContext.Session.GetInt32("UserSektorId") == null)
            {

                return RedirectToAction("Login", "Account");
            }
            var sektor = Convert.ToInt32(HttpContext.Session.GetInt32("UserSektorId"));
            ViewBag.sektor = sektor;

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Kategorija(oprema_kategorija model)
        {
            if (HttpContext.Session.GetInt32("UserSektorId") == null)
            {

                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {


                var sektor = Convert.ToInt32(HttpContext.Session.GetInt32("UserSektorId"));
                ViewBag.sektor = sektor;
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

        public ActionResult KategorijaLista()
        {
            if (HttpContext.Session.GetInt32("UserSektorId") == null)
            {

                return RedirectToAction("Login", "Account");
            }

            var sektor = Convert.ToInt32(HttpContext.Session.GetInt32("UserSektorId"));
            ViewBag.sektor = sektor;

            IList<OpremaKategorijaListaModel> kategorijaLista = new List<OpremaKategorijaListaModel>();


            var query = from Kategorija in _dropDownModel.oprema_kategorija
                        join Sektor in _dropDownModel.sektor on Kategorija.oprema_kategorija_sektor_id equals Sektor.sektor_id
                        where Sektor.sektor_id == sektor
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

        public ActionResult Servis()
        {
            if (HttpContext.Session.GetInt32("UserSektorId") == null)
            {

                return RedirectToAction("Login", "Account");
            }
            var sektor = Convert.ToInt32(HttpContext.Session.GetInt32("UserSektorId"));
            ViewBag.sektor = sektor;

            return View();
        }
        [HttpPost]
        public ActionResult Servis(servis model)
        {
            if (HttpContext.Session.GetInt32("UserSektorId") == null)
            {

                return RedirectToAction("Login", "Account");
            }
            var sektor = Convert.ToInt32(HttpContext.Session.GetInt32("UserSektorId"));
            ViewBag.sektor = sektor;

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
        public ActionResult ServisLista()
        {
            if (HttpContext.Session.GetInt32("UserSektorId") == null)
            {

                return RedirectToAction("Login", "Account");
            }
            int sektor = (int)HttpContext.Session.GetInt32("UserSektorId");

            IList<servis> servisLista = new List<servis>();
            servisLista = (from servis1 in _dropDownModel.servis.Where(x => x.servis_sektor_id == sektor ) select servis1).ToList();


            return View(servisLista);

        }
        [HttpGet]
        public ActionResult Dostavljac()
        {
            if (HttpContext.Session.GetInt32("UserSektorId") == null)
            {

                return RedirectToAction("Login", "Account");
            }
            int sektor = (int)HttpContext.Session.GetInt32("UserSektorId");

            ViewBag.sektor = sektor;

            return View();

        }
        [HttpPost]
        public ActionResult Dostavljac(dostavljac model)
        {
            if (HttpContext.Session.GetInt32("UserSektorId") == null)
            {

                return RedirectToAction("Login", "Account");
            }
            int sektor = (int)HttpContext.Session.GetInt32("UserSektorId");

            ViewBag.sektor = sektor;

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
      
            public ActionResult DostavljacLista()
            {
            if (HttpContext.Session.GetInt32("UserSektorId") == null)
            {

                return RedirectToAction("Login", "Account");
            }
            int sektor = (int)HttpContext.Session.GetInt32("UserSektorId");

            IList<dostavljac> dostavljaclista = new List<dostavljac>();
            dostavljaclista = (from servis1 in _dropDownModel.dostavljac.Where(x => x.dostavljac_sektor_id == sektor) select servis1).ToList();


            return View(dostavljaclista);

        }
        

             public IActionResult RadnjaRevers()
               {
            IList<lokacija> lokacijaLista = new List<lokacija>();
            lokacijaLista = (from servis1 in _dropDownModel.lokacija select servis1).ToList();


            return View(lokacijaLista);

                }



        public IActionResult Revers(int id)
        {
            if (id == 0 || id < 0)
            {
                RedirectToAction("RadnjaRevers", "Admin");

            }

            {
                if (HttpContext.Session.GetInt32("UserSektorId") == null)
                {

                    return RedirectToAction("Login", "Account");
                }
                int sektor = (int)HttpContext.Session.GetInt32("UserSektorId");

                List<LokacijaSaOpremom> LokacijaSaOpremom = new List<LokacijaSaOpremom>();

                MySqlConnection konekcija = new MySqlConnection(StringKonekcije());
                try
                {
                    using (konekcija)
                    {

                        konekcija.Open();
                        string upit = "SELECT relokacija_napomena,oprema.oprema_serijski_broj,oprema.oprema_marka,oprema.oprema_model,oprema_qr_kod,lokacija.lokacija_mesto,lokacija.lokacija_ime,lokacija.lokacija_adresa,relokacija_datum \r\nFROM relokacija \r\nLEFT JOIN lokacija on relokacija_do_lokacija_id = lokacija.lokacija_id \r\nINNER join (SELECT * FROM oprema WHERE oprema.oprema_otpisano = 0 and oprema.oprema_stanje > 0 ) oprema on relokacija_oprema_id = oprema.oprema_id \r\nINNER JOIN oprema_kategorija on oprema.oprema_kategorija_id = oprema_kategorija.oprema_kategorija_id \r\nINNER join sektor on oprema_kategorija.oprema_kategorija_sektor_id = sektor.sektor_id \r\nWHERE relokacija_id in ( SELECT MAX(relokacija_id) from relokacija GROUP BY relokacija_oprema_id) and relokacija_do_lokacija_id = 4 and sektor.sektor_id = " + sektor + " ;";

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
                        title = new Paragraph("REVERS ZA RADNJU", titleFont);
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
                        table.AddCell("Datum:");
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
                            ModelState.AddModelError("Error", "Ne postoji nijedna oprema u ovom objektu!");

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
        }

        public IActionResult LokacijaSaOpremomPDF()
        {

            string physicalPath = "LokacijaSaOpremom.pdf"; 
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

        public IActionResult Index()
            {
                return View();
            }
        
    }
} 
