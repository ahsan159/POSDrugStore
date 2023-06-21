using System.Xml.Linq;
using System.Xml;
namespace persons
{
    public enum genderType
    {
        Male,
        Female,
        Other
    }
    public class person
    {
        private string lastName;
        private string firstName;
        private string midName;
        private DateTime dob;
        private DateTime added;
        private int age;
        public string address { set; get; }
        private string city;
        private string[] contact = new string[3];
        public string name { set; get; }
        public string contactString { set; get; }
        private genderType gender;
        private string picture;
        private string email;

        public person()
        {
            lastName = string.Empty;
            firstName = string.Empty;
            midName = string.Empty;
            added = DateTime.Now;
            name = string.Format("{0} {1} {2}",
            arg0: firstName,
            arg1: midName,
            arg2: lastName);
            gender = genderType.Other;
            address = string.Empty;
            contact[0] = string.Empty;
            contact[1] = string.Empty;
            //contact = new string[3];
            //contact[0] = null;
            dob = DateTime.MinValue;
            //gender = ;
            city = string.Empty;
            picture = string.Empty;
            email = string.Empty;
            contactString = string.Empty;
        }

        public person(string fName, string lName, string mName)
        {
            lastName = lName;
            firstName = fName;
            midName = mName;
            added = DateTime.Now;
            name = string.Format("{0} {1} {2}",
            arg0: firstName,
            arg1: midName,
            arg2: lastName);
            gender = genderType.Other;
            address = string.Empty;
            contact[0] = string.Empty;
            contact[1] = string.Empty;
            //contact = new string[3];
            //contact[0] = null;
            dob = DateTime.MinValue;
            //gender = ;
            city = string.Empty;
            picture = string.Empty;
            email = string.Empty;
            contactString = string.Empty;
        }

        public person(string fName, string lName, string mName, genderType _gender, string _address, string _city, string _phone, DateTime _DateofBirth, string _picture)
        {
            lastName = lName;
            firstName = fName;
            midName = mName;
            added = DateTime.Now;
            name = string.Format("{0} {1} {2}",
            arg0: firstName,
            arg1: midName,
            arg2: lastName);
            address = _address;
            //contact = new string[3];
            contact[0] = _phone;
            contact[1] = string.Empty;
            contactString = contact[0];
            dob = _DateofBirth;
            gender = _gender;
            city = _city;
            age = DateTime.Now.Year - dob.Year;// this age calculation needs improvement
            picture = _picture;
            email = string.Empty;
        }
        public string getAge()
        {
            age = DateTime.Now.Year - dob.Year;
            return age.ToString();
        }
        public string getName()
        {
            return name;
        }
        public void setAddress(string Address)
        {
            address = Address;
        }
        public string getAddress()
        {
            return address;
        }
        public string getContact0()
        {

            try { return contact[0]; }
            catch (Exception) { return string.Empty; }
        }
        public string getContact1()
        {

            try { return contact[1]; }
            catch (Exception) { return string.Empty; }
        }
        public void setContact0(string _c)
        {
            contact[0] = _c;
            if (contact[1].Length == 0 && contact[0].Length == 0)
            {
                contactString = string.Empty;
            }
            else if (contact[1].Length == 0 && contact[0].Length != 0)
            {
                contactString = contact[0];
            }
            else if (contact[0].Length == 0 && contact[1].Length != 0)
            {
                contactString = contact[1];
            }
            else
            {
                contactString = string.Empty;
            }
        }
        public void setContact1(string _c)
        {
            contact[1] = _c;
            contactString = contact[0] + "," + contact[1];
            if (contact[1].Length == 0 && contact[0].Length == 0)
            {
                contactString = string.Empty;
            }
            else if (contact[1].Length == 0 && contact[0].Length != 0)
            {
                contactString = contact[0];
            }
            else if (contact[0].Length == 0 && contact[1].Length != 0)
            {
                contactString = contact[1];
            }
            else
            {
                contactString = string.Empty;
            }
        }
        public void addContact(string _contact)
        {
            if (_contact.Length == 0)
            {
                contact[0] = string.Empty;
                contact[1] = string.Empty;
                return;
            }
            if (_contact.Equals(","))
            {
                contact[0] = string.Empty;
                contact[1] = string.Empty;
                return;
            }

            if (contact[0].Length == 0)
            {
                contact[0] = _contact;
            }
            else if (contact[1].Length == 0)
            {

                contact[1] = _contact;
            }
            // contruct string for contact
            contactString = string.Empty;
            if (contact[0] == string.Empty && contact[1] != string.Empty)
            {
                contactString = contact[1];
            }
            else if (contact[0] != string.Empty && contact[1] == string.Empty)
            {
                contactString = contact[0];
            }
            else
            {
                contactString = contact[0] + "," + contact[1];
            }
        }
        public void removeContact(string _contact)
        {
            if (contact[0].Equals(_contact))
            {
                contact[0] = contact[1];
                contact[1] = string.Empty;
            }
            else if (contact[1].Equals(_contact))
            {
                contact[1] = string.Empty;
            }
            // contruct string for contact
            contactString = string.Empty;
            if (contact[0] == string.Empty && contact[1] != string.Empty)
            {
                contactString = contact[1];
            }
            else if (contact[0] != string.Empty && contact[1] == string.Empty)
            {
                contactString = contact[0];
            }
            else
            {
                contactString = contact[0] + "," + contact[1];
            }
        }
        public string getContact()
        {
            // contruct string for contact
            contactString = string.Empty;
            if (contact[0] == string.Empty && contact[1] != string.Empty)
            {
                contactString = contact[1];
            }
            else if (contact[0] != string.Empty && contact[1] == string.Empty)
            {
                contactString = contact[0];
            }
            else
            {
                contactString = contact[0] + "," + contact[1];
            }
            return contactString;
        }
        public genderType getGender()
        {
            return gender;
        }
        public void setGender(genderType g)
        {
            gender = g;
        }

        public void setDOB(DateTime _dt)
        {
            dob = _dt;
            int y = (DateTime.Now.Year - dob.Year);
            int z = (DateTime.Now.Month > dob.Month) && (DateTime.Now.Day > dob.Day) ? 0 : 1;
            //age = a.Duration;
            age = y - z;
        }

        public DateTime getDOB()
        {
            return dob;
        }
        public void setEmail(string s)
        {
            email = s;
        }

        public string getEmail()
        {
            return email;
        }
        public string getCity()
        {
            return city;
        }
        public void setCity(string _city)
        {
            city = _city;
        }

        public override string ToString()
        {
            string s = string.Empty;
            s = firstName + "," + lastName + "," + midName + "," + gender + "," + age + "," + address + "," + city + "," + contact[0] + "," + contact[1];
            return s;
        }
        public string JSON()
        {
            string s = string.Empty;
            s = "{" + Environment.NewLine +
                "firstName: \"" + firstName + "\"" + Environment.NewLine +
                "lastName: \"" + lastName + "\"" + Environment.NewLine +
                "middleName: \"" + midName + "\"" + Environment.NewLine +
                "age: \"" + age + "\"" + Environment.NewLine +
                "gender: \"" + gender + "\"" + Environment.NewLine +
                "address: \"" + address + "\"" + Environment.NewLine +
                "city: \"" + city + "\"" + Environment.NewLine +
                "contact0: \"" + contact[0] + "\"" + Environment.NewLine +
                "contact1: \"" + contact[1] + "\"" + Environment.NewLine +
                "email: \"" + email + "\"" + Environment.NewLine +
                "dateAdded: \"" + added.ToString("dd/MM/yyyy") + "\"" + Environment.NewLine +
                "DOB: \"" + dob.ToString("dd/MM/yyyy") + "\"" + Environment.NewLine +
                "}";
            return s;
        }
        public XElement xml()
        {
            XElement ele = new XElement("person");

            XElement XfName = new XElement("firstName");
            XfName.Value = firstName;
            ele.Add(XfName);

            XElement XlName = new XElement("lastName");
            XlName.Value = lastName;
            ele.Add(XlName);

            XElement XmName = new XElement("midName");
            XmName.Value = midName;
            ele.Add(XmName);

            XElement Xaddress = new XElement("address");
            Xaddress.Value = address;
            ele.Add(Xaddress);

            XElement Xcity = new XElement("city");
            Xcity.Value = city;
            ele.Add(Xcity);

            XElement XdateAdded = new XElement("added");
            XdateAdded.Value = added.ToString("dd/MM/yyyy");
            ele.Add(XdateAdded);

            XElement Xdob = new XElement("dateOfBirth");
            Xdob.Value = dob.ToString("dd/MM/yyyy");
            ele.Add(Xdob);

            XElement Xgender = new XElement("gender");
            Xgender.Value = gender.ToString();
            ele.Add(Xgender);

            XElement Xemail = new XElement("email");
            Xemail.Value = email;
            ele.Add(Xemail);

            XElement Xcontact = new XElement("contact");
            XElement xc1 = new XElement("contact1");
            xc1.Value = contact[0];
            Xcontact.Add(xc1);
            XElement xc2 = new XElement("contact2");
            xc2.Value = contact[1];
            Xcontact.Add(xc2);
            ele.Add(Xcontact);

            return ele;
        }
    }
}