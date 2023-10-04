using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
//using persons;


namespace person
{
    public class diary
    {
        List<person> personList = new List<person>();
        int currentPerson = 0;
        public diary()
        {

        }
        public person First()
        {
            currentPerson = 0;
            return personList[currentPerson];
        }
        public person Next()
        {
            currentPerson = currentPerson + 1;
            if (currentPerson >= personList.Count)
            {
                currentPerson = personList.Count - 1;
            }
            return personList[currentPerson];
        }
        public person Previous()
        {
            currentPerson = currentPerson - 1;
            if (currentPerson < 0)
            {
                currentPerson = 0;
            }
            return personList[currentPerson];
        }
        public person Last()
        {
            currentPerson = personList.Count - 1;
            return personList[currentPerson];
        }
        public void add(person _p)
        {
            personList.Add(_p);
        }
        public void remove(person _p)
        {
            personList.Remove(_p);
        }
        public void clearAll()
        {
            personList.Clear();
        }
        public List<person> getList()
        {
            return personList;
        }
        public XElement xml()
        {
            XElement ele = new XElement("phonebook");
            foreach (person p in personList)
            {
                ele.Add(p.xml());
            }
            return ele;
        }

        public void loadXml(string fileName)
        {
            XDocument doc = XDocument.Load(fileName);
            XElement? ele = doc.Element("phonebook");
            //Console.WriteLine(ele);
            foreach (XElement e in ele.Elements())
            {
                //Console.WriteLine("This is element");
                //Console.WriteLine(e);
                Console.Write("persons");
                loadpersonfromxml(e);
            }
        }
        private void loadpersonfromxml(XElement element)
        {
            string firstName = string.Empty;
            string lastName = string.Empty; ;
            string midName = string.Empty;
            string address = string.Empty;
            string city = string.Empty;
            string pic = string.Empty;
            genderType gt = genderType.Other;
            string contact0 = string.Empty;
            string contact1 = string.Empty;
            string email = string.Empty;
            DateTime added = DateTime.Now;
            DateTime dob = DateTime.Now;
            foreach (XElement e in element.Elements())
            {
                if (e.Name == "firstName")
                {
                    firstName = e.Value.Trim();
                }
                else if (e.Name == "lastName")
                {
                    lastName = e.Value.Trim();
                }
                else if (e.Name == "midName")
                {
                    midName = e.Value.Trim();
                }
                else if (e.Name == "address")
                {
                    address = e.Value;
                }
                else if (e.Name == "city")
                {
                    city = e.Value;
                }
                else if (e.Name == "email")
                {
                    email = e.Value;
                }
                else if (e.Name == "gender")
                {
                    if (e.Value == "Male")
                    {
                        gt = genderType.Male;
                    }
                    else if (e.Value == "Female")
                    {
                        gt = genderType.Female;
                    }
                }
                else if (e.Name == "added")
                {

                    try
                    {
                        added = DateTime.ParseExact(e.Value, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    }
                    catch (Exception)
                    {
                        added = DateTime.ParseExact(e.Value, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    }
                }
                else if (e.Name == "dateOfBirth")
                {
                    try
                    {
                        dob = DateTime.ParseExact(e.Value, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    }
                    catch (Exception)
                    {
                        dob = DateTime.ParseExact(e.Value, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    }
                }
                else if (e.Name == "contact")
                {
                    contact0 = e.Element("contact1").Value;
                    contact1 = e.Element("contact2").Value;
                }
            }
            person p = new person(firstName, lastName, midName, gt, address, city, string.Empty, dob, pic);
            p.setContact0(contact0);
            p.setContact1(contact1);
            p.setEmail(email);
            personList.Add(p);
        }
    }
}