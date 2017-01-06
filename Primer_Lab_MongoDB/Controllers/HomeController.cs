using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;


namespace Controllers
{
    public class HomeController : Controller
    {

        Models.Connection conn = new Models.Connection();

        public ActionResult Index()
        {
            //Models.Connection conn = new Models.Connection();
            //List<Models.Person> _list = conn.getDocsWithModel();

            //conn.modifyDoc(new Models.Person() { Age = "25", Name = "kevin a. rojas vargas", Guid = "214c3fec-33ce-4334-a201-3b515641fbad" });
            //conn.deleteDoc("214c3fec-33ce-4334-a201-3b515641fbad");
            return View();
        }

        public string getDocs()
        {
            try
            {
                return new JavaScriptSerializer().Serialize(conn.getDocsWithModel());
            }
            catch (Exception ex) { return ex.Message; }
        }

        public string addDoc(string pName, string pAge)
        {
            try
            {
                conn.addDoc(new Models.Person() { Age = pAge, Name = pName, Guid = Guid.NewGuid().ToString() });
                return "";
            }
            catch (Exception ex) { return ex.Message; }
        }

        public string modifyDoc(string pName, string pAge, string pGuid)
        {
            try
            {
                conn.modifyDoc(new Models.Person() { Age = pAge, Name = pName, Guid = pGuid });
                return "";
            }
            catch (Exception ex) { return ex.Message; }
        }

        public string deleteDoc(string pGuid)
        {
            try
            {
                conn.deleteDoc(pGuid);
                return "";
            }
            catch (Exception ex) { return ex.Message; }
        }

        public string findDoc(string pGuid)
        {
            try
            {
                if (pGuid.Equals(""))
                    return "The information to search doesn't exist.";
                Models.Person result = conn.findDoc(pGuid);
                if(result == null)
                    return "Can't find the document.";
                return new JavaScriptSerializer().Serialize(result);
            }
            catch (Exception ex) { return ex.Message; }
        }

    }
}
