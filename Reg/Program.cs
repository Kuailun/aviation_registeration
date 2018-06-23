using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.IO;

namespace Reg
{
    class Program
    {
        public static FileStream fileStream;
        public static StreamWriter streamWriter;
        public static string airline = "ZYB Lily Jet";
        public static int  count = 0;
        public struct data_struct
        {
            public string reg;
            public string manufacturer;
            public string aircraft;
            public string type;
            public string entire_type;
            public string delivered;
            public string status;
            public string prev;
            public string airline;
            public string web;
        }
        static void Main(string[] args)
        {
            string path_in = "G:\\SoftwareWorkspace\\C#\\aviation_registeration\\Reg\\document\\reg.txt";
            fileStream = new FileStream("G:\\SoftwareWorkspace\\C#\\aviation_registeration\\Reg\\document\\regg.txt", FileMode.Create);
            streamWriter = new StreamWriter(fileStream, Encoding.Default);
            
            using (StreamReader sr = new StreamReader(path_in, Encoding.Default))
            {
                string line;
                int state = 0;
                data_struct data = new data_struct();
                data.airline = airline;
                
                while ((line=sr.ReadLine())!=null)
                {
                    count++;
                    if(line.IndexOf("<a href=")!=-1)
                    {
                        if(line.IndexOf("/airframe")!=-1)
                        {
                            string[] reg = line.Split(' ');
                            data.reg = reg[10];
                        }
                        else if(line.IndexOf("/production-list")!=-1)
                        {
                            string[] s1 = line.Split('>');
                            string[] s2 = s1[1].Split('<');
                            string[] s3 = s2[0].Split(' ');
                            data.manufacturer = s3[0];
                            data.entire_type = s3[1];
                            string[] s4 = s3[1].Split('-');
                            data.type = s4[0];
                            string[] s5 = s1[2].Split(' ');
                            s5 = s5.Where(s => !string.IsNullOrEmpty(s)).ToArray();
                            int r = 0;
                            if(s5.Length<3)
                            {
                                data.status = s5[0] + " " + s5[1];
                            }
                            else
                            {
                                if (int.TryParse(s5[2], out r))
                                {
                                    data.delivered = s5[2] + "/" + GetMonth(s5[1]) + "/" + GetDay(s5[0]);
                                    data.status = s5[3];
                                }
                                else if(int.TryParse(s5[1],out r))
                                {
                                    data.delivered = s5[1] + "/" + GetMonth(s5[0])+"/"+GetDay("1.");
                                    data.status = s5[2];
                                }
                            }
                        }
                        else if(line.IndexOf("/correction")!=-1)
                        {
                            Write(data);
                            data = new data_struct();
                            data.airline = airline;
                        }
                        /*switch(state)
                        {
                            case 0:
                                state++;
                                break;
                            case 1:
                                string[] reg = line.Split(' ');
                                data.reg = reg[10];
                                string[] sstr = reg[3].Split('/');
                                data.aircraft = sstr[3];
                                state++;
                                break;
                            case 2:
                                string[] str = line.Split(' ');
                                data.entire_type = str[4].Replace("</a>","");

                                string[] manufacture = str[3].Split('/');
                                data.manufacturer = manufacture[2];

                                int r=0;
                                if(int.TryParse(str[9],out r))
                                {
                                    data.delivered = str[9] + "/" + GetMonth(str[8]) + "/" + GetDay(str[7]);
                                    data.status = str[12];
                                }
                                else
                                {
                                    data.delivered = str[10] + "/" + GetMonth(str[9]) + "/" + GetDay(str[8]);
                                    data.status = str[13];
                                }
                                
                                state++;
                                break;
                            case 3:
                                //string[] str = line.Split(' ');                                
                                state=0;
                                Write(data);
                                data = new data_struct();
                                data.airline = airline;
                                break;
                        }*/
                    }
                }
                streamWriter.Close();
                fileStream.Close();
            }
        }
        static public string GetMonth(string str)
        {
            switch(str)
            {
                case "Jan":
                    return "1";
                case "Feb":
                    return "2";
                case "Mar":
                    return "3";
                case "Apr":
                    return "4";
                case "May":
                    return "5";
                case "Jun":
                    return "6";
                case "Jul":
                    return "7";
                case "Aug":
                    return "8";
                case "Sep":
                    return "9";
                case "Oct":
                    return "10";
                case "Nov":
                    return "11";
                case "Dec":
                    return "12";
                default:
                    return "";
            }
        }
        static public string GetDay(string str)
        {
            return str.Substring(0, str.Length - 1);
        }
        static public void Write(data_struct d)
        {
            streamWriter.Write(d.reg+"\t"+d.manufacturer+"\t"+d.aircraft+"\t"+d.type+"\t"+d.entire_type+"  \t\t"+d.delivered+"  \t"+d.status+"\t"+d.airline+ "\r\n");
            streamWriter.Flush();
            //streamWriter.Close();
            //fileStream.Close();
        }
    }
}
