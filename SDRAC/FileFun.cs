using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDRAC
{
    class FileFun
    {

        public string CreateFile(string namebeg, string path)
        {
             string name = path;     

                int count = 0;
            do
            {
                name = path;
                name += namebeg;
                name += DateTime.Now.Hour.ToString() + ".";
                name += DateTime.Now.Minute.ToString() + ".";
                name += DateTime.Now.Second.ToString() + "_";
                name += count.ToString();
                name += ".txt";
                count++;
            }
            while (System.IO.File.Exists(name));

                System.IO.File.WriteAllText(name, null);           

            return name;
        }
        public string CreateTempFile(string namebeg, string ApEP, string ApPN)
        {
            string path = CheckPath(ApEP,ApPN);

            string name = path;
            name += namebeg;        
            name += DateTime.Now.Hour.ToString() + ".";
            name += DateTime.Now.Minute.ToString() + ".";
            name += DateTime.Now.Second.ToString();
            name += ".txt";

            int count = 0;
            while (System.IO.File.Exists(name))
            {
                name = path;
                name += namebeg;
                name += DateTime.Now.Hour.ToString() + ".";
                name += DateTime.Now.Minute.ToString() + ".";
                name += DateTime.Now.Second.ToString() + "_";
                name += count.ToString();
                name += ".txt";
                count++;
            }

            System.IO.File.WriteAllText(name, null);

            return name;

        }

        public string CheckPath(string ApEP, string ApPN)
        {     
            string path=null;
            try
            {
                path = ApEP.Substring(0, (ApEP.Length - (ApPN + ".exe").Length));
            }
            catch (Exception){ }

            return path;
        }

        public void CopyFromOneFileToAnother(string From, string To)
        {
            try
            {
                using (StreamReader reader = new StreamReader(From))
                {
                    using (StreamWriter writer = new StreamWriter(To))
                    {
                        while (reader.Peek() != -1)
                        {
                            writer.WriteLine(reader.ReadLine());

                        }
                    }
                }
            }
            catch (Exception ex)
            {}
        }
    }
}
