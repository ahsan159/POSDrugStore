using System.Xml;
using System.Xml.Linq;
namespace drugs
{
    enum drugType
    {
        Syrup = 0,
        Tablet,
        Aerosol,
        Injection
    };
    public class drugs
    {
        public string name { get; set; }
        public string formula { get; set; }
        public string manufacturer { get; set; }
        public string supplier { get; set; }
        public DateTime expiry { get; set; }
        public DateTime entry { get; set; }
        public double cost { get; set; }
        public double purchaseCost { get; set; }
        public double quantity { set; get; }

        public drugs()
        {
            name = string.Empty;
            formula = string.Empty;
            manufacturer = string.Empty;
            supplier = string.Empty;
            expiry = DateTime.MinValue;
            entry = DateTime.MinValue;
            cost = -1;
            purchaseCost = -1;
            quantity = -1;
        }
        public drugs(string _name, string _manufacturer, double _cost, double _quantity)
        {            
            formula = string.Empty;            
            supplier = string.Empty;
            expiry = DateTime.MinValue;            
            cost = -1;
            purchaseCost = -1;
            name = _name;
            manufacturer = _manufacturer;
            cost = _cost;
            quantity = _quantity;
            entry = DateTime.Now;
            supplier = string.Empty;            
        }
        public drugs(string _name, string _manufacturer, double _cost, double _quantity, string _expiry)
        {
            purchaseCost = -1;            
            name = _name;
            formula = string.Empty;
            supplier = string.Empty;
            manufacturer = _manufacturer;
            cost = _cost;
            quantity = _quantity;
            entry = DateTime.Now;
            setExpiry(_expiry);
        }
        public drugs(string _name, string _manufacturer, double _cost, double _quantity, DateTime _expiry)
        {            
            formula = string.Empty;         
            supplier = string.Empty;                        
            purchaseCost = -1;            
            name = _name;
            manufacturer = _manufacturer;
            cost = _cost;
            quantity = _quantity;
            entry = DateTime.Now;
            setExpiry(_expiry);
        }

        public void setExpiry(DateTime _time)
        {
            expiry = _time;
        }
        public void setExpiry(string _s)
        {
            if (_s.Contains('-'))
            {
                expiry = DateTime.ParseExact(_s, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
            }
            else if (_s.Contains('/'))
            {
                expiry = DateTime.ParseExact(_s, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            }
        }
        public string getExpiry()
        {
            return expiry.ToString("dd-MM-yyyy");
        }
        public XElement xml()
        {
            XElement ele = new XElement("drug");
            XElement xn = new XElement("name");
            xn.Value = this.name;            
            XElement xm = new XElement("manufacturer");
            xm.Value = this.manufacturer;
            XElement xs = new XElement("supplier");
            xs.Value = supplier;
            XElement xf = new XElement("formula");
            xf.Value = formula;
            XElement xexp = new XElement("expiry");
            xexp.Value = expiry.ToString("dd/MM/yyyy");
            XElement xent = new XElement("entry");
            xent.Value = entry.ToString("dd/MM/yyyy");
            XElement xp = new XElement("purchase");
            xp.Value = purchaseCost.ToString();
            XElement xc = new XElement("price");
            xc.Value = cost.ToString();
            XElement xq = new XElement("inventory");
            xq.Value = quantity.ToString();

            ele.Add(xn);
            ele.Add(xm);
            ele.Add(xs);
            ele.Add(xf);
            ele.Add(xexp);
            ele.Add(xent);
            ele.Add(xp);
            ele.Add(xc);
            ele.Add(xq);
            
            return ele;            
        }
    }
}