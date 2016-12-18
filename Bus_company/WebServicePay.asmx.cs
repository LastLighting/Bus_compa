using Bus_company.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace Bus_company
{
    /// <summary>
    /// Summary description for WebServicePay
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WebServicePay : System.Web.Services.WebService
    {
        CardsEntities cards = new CardsEntities();
        [WebMethod]
        public string operation(int card, int money)
        {
            if(cards.Cards.FirstOrDefault(c => c.Card1 == card) != null)
            {
                var item = cards.Cards.FirstOrDefault(c => c.Card1 == card);
                if (item.money < money)
                {
                    return "Недостаточно средств";
                }else
                {
                    item.money = item.money - money;
                    cards.Entry(item).State = EntityState.Modified;
                    cards.SaveChanges();
                    return "Оплата произведена";
                }
            }
            else
            {
                return "Такой карточки нет";
            }
            
        }
    }
}
